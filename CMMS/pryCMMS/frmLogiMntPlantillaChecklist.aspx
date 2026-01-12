<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmLogiMntPlantillaChecklist.aspx.vb" Inherits="pryCMMS.frmLogiMntPlantillaChecklist" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- ===============================================-->
    <!--    Stylesheets-->
    <!-- ===============================================-->
    <link href="assets/css/theme-rtl.min.css" rel="stylesheet" id="style-rtl">
    <link href="assets/css/theme.min.css" rel="stylesheet" id="style-default">
    <link href="assets/css/user-rtl.min.css" rel="stylesheet" id="user-style-rtl">
    <link href="assets/css/user.min.css" rel="stylesheet" id="user-style-default">

    <script>
        var isRTL = JSON.parse(localStorage.getItem('isRTL'));
        if (isRTL) {
            var linkDefault = document.getElementById('style-default');
            var userLinkDefault = document.getElementById('user-style-default');
            linkDefault.setAttribute('disabled', true);
            userLinkDefault.setAttribute('disabled', true);
            document.querySelector('html').setAttribute('dir', 'rtl');
        } else {
            var linkRTL = document.getElementById('style-rtl');
            var userLinkRTL = document.getElementById('user-style-rtl');
            linkRTL.setAttribute('disabled', true);
            userLinkRTL.setAttribute('disabled', true);
        }
    </script>
    <style type="text/css">
        #BarraHTML {
            position: sticky;
            z-index: 100;
            top: 65px;
        }

            #BarraHTML ul {
                list-style-type: none;
            }

            #BarraHTML li {
                display: inline;
                text-align: center;
                margin: 0 0 0 0;
            }

                #BarraHTML li a {
                    padding: 2px 7px 2px 7px;
                    text-decoration: none;
                }

                    #BarraHTML li a:hover {
                        background-color: #333333;
                        color: #ffffff;
                    }

        #texto {
            padding: 60px 0 0 0;
        }
    </style>
    <script type="text/javascript" language="javascript">

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cuerpo" runat="server">
    <link href="Content/CuadroDialogo.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jsUpdateProgress.js" type="text/javascript"></script>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel ID="updpnlContent" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnNuevo" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnEditar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarDuplicarPlantillaChecklist" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnAtras" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:HiddenField ID="hfdOperacion" runat="server" />
            <asp:HiddenField ID="hfdFocusObjeto" runat="server" />
            <asp:HiddenField ID="hfdEstado" runat="server" />
            <asp:HiddenField ID="hfdFechaTransaccion" runat="server" />
            <asp:HiddenField ID="hfdIdNroDoc" runat="server" />
            <asp:Panel ID="pnlCabecera" runat="server">
                <div class="card mb-3">
                    <div class="card-header">
                        <div class="row flex-between-end">
                            <div class="col-auto align-self-center">
                                <div class="nav nav-pills gap-2" role="tablist">
                                    <h5 class="mb-0" data-anchor="data-anchor">Listado de Plantillas de CheckList</h5>
                                    <a class="btn btn-falcon-default btn-sm ml-auto float-right" href="frmBienvenida.aspx" aria-expanded="true">
                                        <span class="fas fa-home"></span><span class="nav-link-text ps-1">Inicio</span>
                                    </a>
                                </div>
                            </div>
                            <div class="col-auto ms-auto">
                                <div class="nav gap-2" role="tablist">
                                    <asp:LinkButton ID="btnNuevo" runat="server" OnClick="btnNuevo_Click" CssClass="btn btn-success btn-sm ml-auto float-right" Text="<span class='fas fa-plus'></span> Nuevo" ToolTip="Nueva Platilla CheckList" />
                                    <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-primary btn-sm ml-auto float-right" Text="<span class='fas fa-pencil-alt'></span> Editar" ToolTip="Editar Platilla CheckList" />
                                    <asp:LinkButton ID="btnDuplicar" runat="server" CssClass="btn btn-secondary btn-sm ml-auto float-right" Text="<span class='fas fa-pencil-alt'></span> Duplicar" ToolTip="Duplicar Platilla CheckList" />
                                    <div class="dropdown font-sans-serif btn-reveal-trigger">
                                        <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="kanbanColumn1" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                                        <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="kanbanColumn1">
                                            <asp:LinkButton ID="lnkbtnImprimirPlantilla" runat="server" ToolTip="Imprimir la plantilla según el tipo de mantenimiento" Text="<span class='fas fa-file me-2'></span>Ver Plantilla" CssClass="nav-link nav-link-card-details" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="row bg-100 p-2">
                            <div class="col-lg-6 col-sm-6">
                                <span class="fas fa-filter"></span>
                                <label class="text-black-50 col-form-label fs--2 m-1">Filtrar por:</label>
                                <div class="input-group">
                                    <asp:DropDownList ID="cboFiltroCheckList" runat="server" CssClass="form-select form-select-sm js-choice" TabIndex="1">
                                        <asp:ListItem>SELECCIONE EL FILTRO</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-6">
                                <span class="far fa-address-card"></span>
                                <label class="text-black-50 col-form-label fs--2 m-1">Buscar por:</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtBuscarCheckList" runat="server" CssClass="form-control form-control-sm" TabIndex="2"
                                        Style="text-transform: uppercase" placeholder="Ingrese Busqueda"></asp:TextBox>
                                    &nbsp;
							        <asp:ImageButton ID="imgbtnBuscarCheckList" runat="server" Height="18px" TabIndex="3" CssClass="mt-1"
                                        ImageUrl="~/Imagenes/buscar.png" ToolTip="Buscar Registro" Width="21px" />
                                </div>
                            </div>
                        </div>
                        <div class="kanban-items-container scrollbar">
                            <div class="row">
                                <div class="col-md-12 mt-3">
                                    <div class="table-responsive">
                                        <asp:GridView ID="grdLista" runat="server" AutoGenerateColumns="False" TabIndex="4" DataKeyNames="IdTipoMantenimiento"
                                            GridLines="None" CssClass="table table-responsive-sm table-hover small" AllowPaging="True"
                                            EmptyDataText="No hay registros a visualizar" PageSize="14" OnRowCommand="grdLista_RowCommand">
                                            <PagerStyle CssClass="pgr" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <AlternatingRowStyle CssClass="alt" />
                                            <Columns>
                                                <asp:BoundField DataField="IdTipoMantenimiento" HeaderText="Id.Mnt." HeaderStyle-CssClass="bg-200 text-900" />
                                                <asp:BoundField DataField="IdNumero" HeaderText="Correlativo" HeaderStyle-CssClass="bg-200 text-900" />
                                                <asp:BoundField DataField="IdCatalogo" HeaderText="IdCatalogo" HeaderStyle-CssClass="bg-200 text-900" />
                                                <asp:BoundField DataField="IdJerarquiaCatalogo" HeaderText="IdJerarquia" HeaderStyle-CssClass="bg-200 text-900" />
                                                <asp:BoundField DataField="FormatoArchivo" HeaderText="Id. Doc." HeaderStyle-CssClass="bg-200 text-900" />
                                                <asp:BoundField DataField="Descripcion" HeaderText="Plantilla" HeaderStyle-CssClass="bg-200 text-900" />
                                                <asp:BoundField DataField="DescripcionTipoMantenimiento" HeaderText="Tipo Mantenimiento" HeaderStyle-CssClass="bg-200 text-900" />
                                                <asp:BoundField DataField="DescripcionCatalogo" HeaderText="Tipo equipo" HeaderStyle-CssClass="bg-200 text-900" />
                                                <asp:TemplateField HeaderText="Estado" ItemStyle-Width="120px" HeaderStyle-CssClass="bg-200 text-900">
                                                    <ItemTemplate>
                                                        <div class="bootstrap-switch bootstrap-switch-on bootstrap bootstrap-switch-animate bootstrap-switch-mini ">
                                                            <div class="bootstrap-switch-container">
                                                                <asp:LinkButton ID="lnkEstadoDetalleOn" runat="server" CommandName="Activar" CommandArgument='<%# Eval("IdTipoMantenimiento") & "," & Eval("IdNumero") %>' CssClass="bootstrap-switch-handle-on bootstrap-switch-primary" Visible='<%# If(Eval("Estado") = "True", "True", "False") %>'>Activado</asp:LinkButton>
                                                                <span class="bootstrap-switch-label">&nbsp;</span>
                                                                <asp:LinkButton ID="lnkEstadoDetalleOff" runat="server" CommandName="Desactivar" CommandArgument='<%# Eval("IdTipoMantenimiento") & "," & Eval("IdNumero") %>' CssClass="bootstrap-switch-handle-off bootstrap-switch-danger" Visible='<%# If(Eval("Estado") = "True", "False", "True") %>'>Anulado</asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="thead-dark" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:LinkButton ID="lnk_mostrarPanelMensajeDocumentoValidacion" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMensajeDocumentoValidacion_ModalPopupExtender"
                runat="server" BackgroundCssClass="FondoAplicacion"
                CancelControlID="imgbtnCancelarDocumentoValidacionImagen" DropShadow="False"
                DynamicServicePath="" Enabled="True" PopupControlID="pnlMensajeDocumentoValidacion"
                TargetControlID="lnk_mostrarPanelMensajeDocumentoValidacion">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlMensajeDocumentoValidacion" runat="server" CssClass="container">
                <div class="modal-dialog modal-sm">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title" id="myModalLabelMensajeDocumentoValidacion">Mensaje Informativo</h4>
                                    <asp:Button ID="imgbtnCancelarDocumentoValidacionImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <div class="form-group row pt-2">
                                        <span class="col-sm-12 col-form-label-sm text-center text-900">Se va a proceder a crear una nueva plantilla, pero le informamos que ya existe una con el tipo de mantenimiento </span>
                                        <h3 class="col-sm-12 text-center">
                                            <asp:Label ID="lblDescripcionTipoMantenimientoMensajeDocumentoValidacion" runat="server" Text="00000" CssClass="text-info"></asp:Label></h3>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnSiMensajeDocumentoValidacion" runat="server" Text="Si" CssClass="btn btn-outline-facebook btn-sm d-block" ToolTip="Si desea generar Plantilla" />
                                    &nbsp;&nbsp;<asp:Button ID="btnNoMensajeDocumentoValidacion" runat="server" Text="No" CssClass="btn btn-outline-google-plus btn-sm d-block" ToolTip="No desea generar la Plantilla" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <br />

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updpnnlEquipo" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnNuevo" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnEditar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnDuplicar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="cboTipoActivo" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="cboCatalogo" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnAtras" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkbtnNuevoActividadCatalogoComponente" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkbtnEditarActividadCatalogoComponente" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkbtnImprimirPlantilla" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarDuplicarPlantillaChecklist" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSiMensajeDocumentoValidacion" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="pnlEquipo" runat="server">
                <div id="BarraHTML" class="container-fluid">
                    <div class="row">
                        <div class="col-auto align-self-center mt-2 mb-2">
                            <div class="nav nav-pills gap-2" role="tablist">
                                <asp:LinkButton ID="btnAtras" runat="server" CssClass="btn btn-falcon-default btn-sm ml-auto float-right" Text="<span class='fas fa-step-backward'></span> Atrás" ToolTip="Ir hacia atrás" />
                            </div>
                        </div>
                        <div class="col-auto ms-auto">
                            <div class="nav nav-pills gap-2" role="tablist">
                                <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-success btn-sm ml-auto float-right" Text="<span class='fas fa-plus'></span> Guardar" ToolTip="Guardar Registro" />
                            </div>
                        </div>
                    </div>
                </div>
                <asp:HiddenField ID="hfdOperacionDetalle" runat="server" />
                <div class="card mb-3">
                    <div class="card-body">
                        <div class="row mt-3">
                            <div class="col-12 mb-0 bg-100">
                                <h6 class="mt-2 mb-1">DATOS DE LA PLANTILLA</h6>
                                <hr class="bg-300 m-0" />
                            </div>
                        </div>
                        <div class="row mt-1">
                            <div class="col-lg-4 col-md-6 mt-2">
                                <span class="far fa-address-card"></span>
                                <label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Tipo de Mantenimiento:</label>
                                <div class="input-group">
                                    <asp:DropDownList ID="cboTipoMantenimiento" runat="server" AppendDataBoundItems="True"
                                        AutoPostBack="True" CssClass="form-select form-select-sm js-choice"
                                        Style="text-transform: uppercase" TabIndex="13">
                                        <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                    </asp:DropDownList>
                                    <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                                        <div class="dropdown font-sans-serif btn-reveal-trigger">
                                            <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesTipoMantenimiento" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                                            <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesTipoMantenimiento">
                                                <asp:LinkButton ID="lnkbtnNuevoTipoMantenimiento" runat="server" ToolTip="Crear un tipo de mantenimiento" Text="<span class='fas fa-file me-2'></span>Nuevo" CssClass="nav-link nav-link-card-details" />
                                                <asp:LinkButton ID="lnkbtnEditarTipoMantenimiento" runat="server" ToolTip="Editar un tipo de mantenimiento" Text="<span class='fas fa-file-signature me-2'></span>Editar" CssClass="nav-link nav-link-card-details" />
                                            </div>
                                        </div>
                                    </div>
                                    <asp:LinkButton ID="lnk_mostrarPanelMantenimientoTipoMantenimiento" runat="server"></asp:LinkButton>
                                    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMantenimientoTipoMantenimiento_ModalPopupExtender"
                                        runat="server" BackgroundCssClass="FondoAplicacion"
                                        CancelControlID="btnCancelarMantenimientoTipoMantenimiento"
                                        DynamicServicePath="" Enabled="True" PopupControlID="pnlIngresarMantenimientoTipoMantenimiento"
                                        TargetControlID="lnk_mostrarPanelMantenimientoTipoMantenimiento">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="pnlIngresarMantenimientoTipoMantenimiento" runat="server" CssClass="container">
                                        <div class="modal-dialog modal-lg">
                                            <div class="shadow rounded">
                                                <div class="modal-dialog-scrollable">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <h4 class="modal-title" id="myModalLabelMantenimientoTipoMantenimiento">Datos del Tipo de Mantenimiento</h4>
                                                            <asp:Button ID="imgbtnCancelarMantenimientoTipoMantenimientoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                        </div>
                                                        <div class="modal-body">
                                                            <div class="form-group">
                                                                <div class="row mt-1">
                                                                    <div class="col-lg-12 col-sm-12">
                                                                        <div class="row">
                                                                            <div class="col-12 mb-0 bg-100 d-none d-md-block">
                                                                                <h6 class="mt-2 mb-1">DATOS PRINCIPALES</h6>
                                                                                <hr class="bg-300 m-0" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-lg-2 col-md-4 col-sm-12 mt-2">
                                                                                <span class="fas fa-shipping-fast"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Código:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtIdMantenimientoTipoMantenimiento" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="23"
                                                                                        Style="text-transform: uppercase" placeholder="Código del Tipo de Mantenimiento"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="rfvIdMantenimientoTipoMantenimiento" runat="server"
                                                                                        ControlToValidate="txtIdMantenimientoTipoMantenimiento" EnableClientScript="False"
                                                                                        ErrorMessage="Ingrese el código del tipo de mantenimiento" Font-Size="10px" ForeColor="Red"
                                                                                        ValidationGroup="vgrpValidarMantenimientoTipoMantenimiento">(*)</asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-6 col-md-4 col-sm-12 mt-2">
                                                                                <span class="fas fa-shipping-fast"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Descripción:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtDescripcionMantenimientoTipoMantenimiento" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="23"
                                                                                        Style="text-transform: uppercase" placeholder="Descripción Tipo de Mantenimiento"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="rfvDescripcionMantenimientoTipoMantenimiento" runat="server"
                                                                                        ControlToValidate="txtDescripcionMantenimientoTipoMantenimiento" EnableClientScript="False"
                                                                                        ErrorMessage="Ingrese la descripción del tipo de mantenimiento" Font-Size="10px" ForeColor="Red"
                                                                                        ValidationGroup="vgrpValidarMantenimientoTipoMantenimiento">(*)</asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-4 col-md-4 col-sm-12 mt-2">
                                                                                <span class="fas fa-chalkboard-teacher"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Descripción Abreviada:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtDescripcionAbreviadaMantenimientoTipoMantenimiento" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="24"
                                                                                        Style="text-transform: uppercase" placeholder="Descripción Abreviada Tipo de Mantenimiento"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="rfvDescripcionAbreviadaMantenimientoTipoMantenimiento" runat="server"
                                                                                        ControlToValidate="txtDescripcionAbreviadaMantenimientoTipoMantenimiento" EnableClientScript="False"
                                                                                        ErrorMessage="Ingrese la descripción abreviada del tipo de mantenimiento" Font-Size="10px"
                                                                                        ForeColor="Red" ValidationGroup="vgrpValidarMantenimientoTipoMantenimiento">(*)</asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-lg-2 col-md-4 col-sm-4 mt-2">
                                                                                <span class="fas fa-shipping-fast"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Meses:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtMesesDesdeContratoMantenimientoTipoMantenimiento" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="25"
                                                                                        Style="text-transform: uppercase" placeholder="Meses Mantenimiento del Tipo de Mantenimiento"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="modal-footer">
                                                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarMantenimientoTipoMantenimiento" />
                                                            <div class="col-md-4 text-end p-2">
                                                                <asp:Button ID="btnAceptarMantenimientoTipoMantenimiento" runat="server" ValidationGroup="vgrpValidarMantenimientoTipoMantenimiento" OnClick="btnAceptarMantenimientoTipoMantenimiento_Click" OnClientClick="deshabilitar(this.id)"
                                                                    ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                                &nbsp;
                                                        <asp:Button ID="btnCancelarMantenimientoTipoMantenimiento" runat="server"
                                                            ToolTip="Cancelar Registro" TabIndex="73" Text="Cancelar" CssClass="btn btn-outline-google-plus btn-sm" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 mt-2">
                                <span class="far fa-clone"></span>
                                <label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Tipo Activo:</label>
                                <div class="input-group">
                                    <asp:DropDownList ID="cboTipoActivo" runat="server" AppendDataBoundItems="True"
                                        AutoPostBack="True" CssClass="form-select form-select-sm js-choice"
                                        Style="text-transform: uppercase" TabIndex="13">
                                        <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 mt-2">
                                <span class="far fa-clone"></span>
                                <label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Catalogo Principal:</label>
                                <div class="input-group">
                                    <asp:DropDownList ID="cboCatalogo" runat="server" AppendDataBoundItems="True"
                                        AutoPostBack="True" CssClass="form-select form-select-sm js-choice"
                                        Style="text-transform: uppercase" TabIndex="14">
                                        <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCatalogo" runat="server"
                                        ControlToValidate="cboCatalogo" Display="Static" EnableClientScript="False"
                                        ErrorMessage="Ingrese el catalogo" Font-Size="10px" ForeColor="Red"
                                        InitialValue="SELECCIONE DATO" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-lg-8 col-md-12 mt-2">
                                <span class="far fa-clone"></span>
                                <label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Descripción Plantilla:</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtDescripcionPlantilla" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="23"
                                        Style="text-transform: uppercase" placeholder="Descripción de la Plantilla"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDescripcionPlantilla" runat="server"
                                        ControlToValidate="txtDescripcionPlantilla" EnableClientScript="False"
                                        ErrorMessage="Ingrese la descripción de la plantilla del tipo de mantenimiento" Font-Size="10px" ForeColor="Red"
                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-12 mt-2">
                                <span class="far fa-clone"></span>
                                <label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Código de Documento:</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtFormatoArchivo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="24"
                                        Style="text-transform: uppercase" placeholder="Formato Archivo"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                        ControlToValidate="txtFormatoArchivo" EnableClientScript="False"
                                        ErrorMessage="Ingrese el formato del archivo de la plantilla del tipo de mantenimiento" Font-Size="10px" ForeColor="Red"
                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <div class="position-fixed bottom-0 end-0 p-3 z-index-2">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidar" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updpnlComponentes" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnNuevo" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnEditar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="cboTipoActivo" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="cboCatalogo" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="grdCatalogoComponente" EventName="PageIndexChanging" />
            <asp:AsyncPostBackTrigger ControlID="btnAdicionarActividadCatalogoComponente" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnAtras" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkbtnNuevoActividadCatalogoComponente" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkbtnEditarActividadCatalogoComponente" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSiMensajeDocumentoValidacion" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="pnlComponentes" runat="server">
                <div class="card mb-3">
                    <div class="card-body">
                        <div class="row justify-content-between">
                            <div class="col-lg-8 col-md-8 col-sm-7">
                                <h5 class="mb-1" data-anchor="data-anchor">Asignación de Actividades por Componente</h5>
                            </div>
                        </div>
                        <div>
                            <div class="row mt-1">
                                <div class="col-md-6 col-sm-12">
                                    <div class="card mb-3">
                                        <div class="card-header">
                                            <div class="row">
                                                <div class="col-12">
                                                    <h6 class="modal-title">Listado de Componentes del Catálogo</h6>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body">
                                            <asp:UpdatePanel ID="updpnlCatalogoComponente" runat="server" UpdateMode="Conditional">
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdCatalogoComponente" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <div id="divGridCatalogoComponente" style="overflow: auto; height: 240px; align: left;" class="scrollbar">
                                                        <asp:GridView ID="grdCatalogoComponente" runat="server" AutoGenerateColumns="False" TabIndex="20"
                                                            GridLines="None" CssClass="table table-responsive-sm table-striped table-hover small" AllowPaging="True"
                                                            EmptyDataText="No hay registros a visualizar" PageSize="14">
                                                            <PagerStyle CssClass="pgr" />
                                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:CommandField ShowSelectButton="True" />
                                                                <asp:BoundField DataField="IdCatalogo" HeaderText="Código" />
                                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                                                                <asp:BoundField DataField="IdTipoActivo" HeaderText="Tipo Activo" />
                                                                <asp:BoundField DataField="IdSistemaFuncional" HeaderText="Sistema Funcional" />
                                                                <asp:BoundField DataField="DescripcionAbreviada" HeaderText="Descripción Abreviada" />
                                                            </Columns>
                                                            <HeaderStyle CssClass="thead-dark" />
                                                        </asp:GridView>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-12 bg-200">
                                    <div class="row">
                                        <div class="col-8">
                                            <h6 class="modal-title">Actividades Asignadas por Componente</h6>
                                        </div>
                                        <div class="col-3">
                                            <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                                                <div class="dropdown font-sans-serif btn-reveal-trigger">
                                                    <span class="fas fa-cog"></span>
                                                    <label class="text-black-50 col-form-label fs--2 m-1">Opciones:</label>
                                                    <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesComponentesCatalogo" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                                                    <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesComponentesCatalogo">
                                                        <asp:LinkButton ID="lnkbtnNuevoActividadCatalogoComponente" runat="server" ToolTip="Crear una actividad" Text="<span class='fas fa-file me-2'></span>Nuevo" CssClass="nav-link nav-link-card-details" />
                                                        <asp:LinkButton ID="lnkbtnEditarActividadCatalogoComponente" runat="server" ToolTip="Editar una actividad" Text="<span class='fas fa-file-signature me-2'></span>Editar" CssClass="nav-link nav-link-card-details" />
                                                        <asp:LinkButton ID="lnkbtnVerActividadCatalogoComponente" runat="server" ToolTip="Ver todas las actividad" Text="<span class='fas fa-file-signature me-2'></span>Ver Todas" CssClass="nav-link nav-link-card-details" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:LinkButton ID="lnk_mostrarPanelMantenimientoActividad" runat="server"></asp:LinkButton>
                                        <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMantenimientoActividad_ModalPopupExtender" runat="server"
                                            BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True"
                                            PopupControlID="pnlMantenimientoActividad" TargetControlID="lnk_mostrarPanelMantenimientoActividad">
                                        </ajaxToolkit:ModalPopupExtender>
                                        <asp:Panel ID="pnlMantenimientoActividad" runat="server" CssClass="container" DefaultButton="btnAceptarMantenimientoActividad">
                                            <div class="modal-dialog modal-lg">
                                                <div class="shadow rounded">
                                                    <div class="modal-dialog-scrollable">
                                                        <div class="modal-content">
                                                            <div class="modal-header bg-light">
                                                                <h4 class="modal-title" id="myModalLabelMantenimientoActividad">Mantenimiento de Actividad</h4>
                                                                <asp:Button ID="imgbtnCancelarMantenimientoActividadImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row pt-2 pb-2">
                                                                    <div class="col-lg-2 col-md-1 col-sm-5">
                                                                        <span class="far fa-address-card"></span>
                                                                        <label class="text-black-50 col-form-label fs--2 m-1">Id:</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtIdActividadMantenimientoActividad" runat="server" CssClass="form-control form-control-sm" TabIndex="100"
                                                                                Style="text-transform: uppercase" autocomplete="off"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-lg-7 col-md-12 col-sm-12">
                                                                        <span class="far fa-address-card"></span>
                                                                        <label class="text-black-50 col-form-label fs--2 m-1">Descripción:</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtDescripcionMantenimientoActividad" runat="server" CssClass="form-control form-control-sm" TabIndex="101"
                                                                                Style="text-transform: uppercase" placeholder="Descripción Actividad" autocomplete="off"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvDescripcionMantenimientoActividad" runat="server"
                                                                                ControlToValidate="txtDescripcionMantenimientoActividad" EnableClientScript="False"
                                                                                ErrorMessage="Ingrese la descripción" Font-Size="10px" ForeColor="Red"
                                                                                ValidationGroup="vgrpValidarMantenimientoActividad">(*)</asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-lg-3 col-md-8 col-sm-12">
                                                                        <span class="far fa-address-card"></span>
                                                                        <label class="text-black-50 col-form-label fs--2 m-1">Descripción Abreviada:</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtDescripcionAbreviadaMantenimientoActividad" runat="server" CssClass="form-control form-control-sm" TabIndex="102"
                                                                                Style="text-transform: uppercase" placeholder="Descripción Abreviada Actividad" autocomplete="off"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:ValidationSummary ID="ValidationSummary3" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarMantenimientoActividad" />
                                                                <div class="col-lg-6 col-md-7 text-end p-2">
                                                                    <asp:Button ID="btnAceptarMantenimientoActividad" runat="server" ValidationGroup="vgrpValidarMantenimientoActividad"
                                                                        ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                                    &nbsp;
                                                                <asp:Button ID="btnCancelarMantenimientoActividad" runat="server"
                                                                    ToolTip="Cancelar Registro" TabIndex="73" Text="Cancelar" CssClass="btn btn-outline-google-plus btn-sm" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <div id="divFiltrarActividadCatalogoComponente" runat="server" class="row justify-content-between">
                                        <div class="col-lg-12 col-md-12 col-sm-12">
                                            <asp:UpdatePanel ID="updpnlActividadCatalogoComponente" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                                <ContentTemplate>
                                                    <span class="fas fa-link"></span>
                                                    <label class="text-black-50 col-form-label fs--2 m-1">Filtrar Actividades:</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboActividadCatalogoComponente" runat="server" AppendDataBoundItems="True" TabIndex="29"
                                                            CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase">
                                                            <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btnAdicionarActividadCatalogoComponente" runat="server" Text="Agregar" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Caracteristica al equipo principal" TabIndex="30" />
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="row justify-content-between mt-3">
                                        <div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
                                            <asp:UpdatePanel ID="updpnlDetalleActividadCatalogoComponente" runat="server">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlDetalleActividadCatalogoComponente" runat="server" CssClass="bg-light">
                                                        <div id="divGridActividadCatalogoComponente" style="overflow: auto; height: 220px; align: left;" class="scrollbar">
                                                            <asp:GridView ID="grdDetalleActividadCatalogoComponente" runat="server" AutoGenerateColumns="False" TabIndex="4"
                                                                GridLines="None" CssClass="table table-striped table-hover small overflow-hidden" AllowPaging="True"
                                                                EmptyDataText="No hay registros a visualizar" PageSize="14">
                                                                <PagerStyle CssClass="mGrid" />
                                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                <Columns>
                                                                    <asp:CommandField ShowDeleteButton="True" HeaderStyle-Width="70px" />
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkRowDetalleActividadCatalogoComponente" runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Item" HeaderText="#" ControlStyle-Width="25px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                    <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-Width="40px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                    <asp:BoundField DataField="IdCatalogo" HeaderText="IdCatalogo" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                    <asp:BoundField DataField="IdJerarquia" HeaderText="IdJerarquia" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                    <asp:BoundField DataField="DescripcionComponente" HeaderText="Componente" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                    <asp:TemplateField HeaderText="Ubic." ItemStyle-Width="80px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                        <ItemTemplate>
                                                                            <div class="input-group">
                                                                                <asp:TextBox ID="txtUbicacionActividad" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("OrdenUbicacion") %>' OnTextChanged="txtUbicacionActividad_TextChanged" AutoPostBack="True"></asp:TextBox>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle CssClass="thead-dark" />
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <div class="position-fixed bottom-0 end-0 p-3 z-index-2">
                <asp:ValidationSummary ID="ValidationSummary4" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarComponente" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="updpnlDuplicarPlantillaChecklist" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDuplicar" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:LinkButton ID="lnk_mostrarPanelDuplicarPlantillaChecklist" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelDuplicarPlantillaChecklist_ModalPopupExtender"
                runat="server" BackgroundCssClass="FondoAplicacion"
                CancelControlID="btnCancelarDuplicarPlantillaChecklist"
                DynamicServicePath="" Enabled="True" PopupControlID="pnlIngresarDuplicarPlantillaChecklist"
                TargetControlID="lnk_mostrarPanelDuplicarPlantillaChecklist">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlIngresarDuplicarPlantillaChecklist" runat="server" CssClass="container">
                <div class="modal-dialog modal-lg">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title" id="myModalLabelDuplicarPlantillaChecklist">Datos de la Plantilla CheckList a Duplicar</h4>
                                    <asp:Button ID="imgbtnCancelarDuplicarPlantillaChecklistImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <div class="form-group">
                                        <div class="row mt-1">
                                            <div class="col-12 mb-0 bg-100 d-none d-md-block">
                                                <h6 class="mt-2 mb-1">DATOS PRINCIPALES</h6>
                                                <hr class="bg-300 m-0" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 col-sm-4 mt-2">
                                                <span class="far fa-address-card"></span>
                                                <label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Tipo de Mantenimiento:</label>
                                                <div class="input-group">
                                                    <asp:DropDownList ID="cboTipoMantenimientoDuplicarPlantillaChecklist" runat="server" AppendDataBoundItems="True"
                                                        AutoPostBack="False" CssClass="form-select form-select-sm js-choice"
                                                        Style="text-transform: uppercase" TabIndex="13">
                                                        <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-lg-4 col-md-4 col-sm-4 mt-2">
                                                <span class="fas fa-shipping-fast"></span>
                                                <label class="text-black-50 col-form-label fs--2 m-1">Descripción:</label>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtDescripcionDuplicarPlantillaChecklist" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="23"
                                                        Style="text-transform: uppercase" placeholder="Descripción Plantilla CheckList"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvDescripcionDuplicarPlantillaChecklist" runat="server"
                                                        ControlToValidate="txtDescripcionDuplicarPlantillaChecklist" EnableClientScript="False"
                                                        ErrorMessage="Ingrese la descripción del checklist" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidarDuplicarPlantillaChecklist">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-lg-4 col-md-8 col-sm-8 mt-2">
                                                <span class="fas fa-chalkboard-teacher"></span>
                                                <label class="text-black-50 col-form-label fs--2 m-1">Código de Documento:</label>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtFormatoArchivoDuplicarPlantillaChecklist" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="24"
                                                        Style="text-transform: uppercase" placeholder="Descripción Formato Archivo Plantilla CheckList"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvFormatoArchivoDuplicarPlantillaChecklist" runat="server"
                                                        ControlToValidate="txtFormatoArchivoDuplicarPlantillaChecklist" EnableClientScript="False"
                                                        ErrorMessage="Ingrese ingrese ek formato del achivo de la plantilla checklist" Font-Size="10px"
                                                        ForeColor="Red" ValidationGroup="vgrpValidarDuplicarPlantillaChecklist">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:ValidationSummary ID="ValidationSummary5" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarComponente" />
                                    <div class="col-md-4 text-end p-2">
                                        <asp:Button ID="btnAceptarDuplicarPlantillaChecklist" runat="server" ValidationGroup="vgrpValidarPlantillaChecklist" OnClick="btnAceptarDuplicarPlantillaChecklist_Click" OnClientClick="deshabilitar(this.id)"
                                            ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                        &nbsp;
                                        <asp:Button ID="btnCancelarDuplicarPlantillaChecklist" runat="server"
                                            ToolTip="Cancelar Registro" TabIndex="73" Text="Cancelar" CssClass="btn btn-outline-google-plus btn-sm" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <link href="cssbootstrap/bootstrap-switch.css" rel="stylesheet" />
    <script src="jsbootstrap/bootstrap-switch.js"></script>

    <script language="javascript" type="text/javascript">
        function popupEmitirCheckListPlantillaReporte(IdTipMan, IdNroDoc) {
            window.open("Informes/frmCmmsCheckListPlantillaReporte.aspx?IdTipMan=" + IdTipMan + "&IdNroDoc=" + IdNroDoc, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
        }

    </script>
</asp:Content>
