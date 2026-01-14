<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmLogiMntCatalogo.aspx.vb" Inherits="pryCMMS.frmLogiMntCatalogo" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cuerpo" runat="server">
    <link href="Content/CuadroDialogo.css" rel="stylesheet" type="text/css"/>
    <script src="Scripts/jsUpdateProgress.js" type="text/javascript"></script>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="card mb-3">
        <!-- Inicio Panel -->
        <div class="panel panel-default">
            <div id="dvTab">
                <!-- Inicio Tabs de Navegación -->
                <ul class="nav nav-tabs" role="tablist">
                    <li class="nav-item"><a class="nav-link active" href="#tab1" aria-controls="tab1" role="tab" data-toggle="tab">Listado de Catalogo</a></li>
                    <li class="nav-item"><a class="nav-link" href="#tab2" aria-controls="tab2" role="tab" data-toggle="tab">Datos Generales</a></li>
                </ul>
                <!-- Final Tabs de Navegación -->
                <!-- Inicio Paneles Tab -->
                <div class="tab-content" style="padding-top: 10px">
                    <div role="tabpanel" class="tab-pane active" id="tab1">
                        <asp:UpdatePanel ID="updpnlTab1" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="hfdOperacion" runat="server" />
                                <asp:HiddenField ID="hfdOperacionDetalle" runat="server" />
                                <asp:HiddenField ID="hfdEstado" runat="server" />
                                <asp:Panel ID="pnlListado" runat="server" CssClass="container" 
                                    DefaultButton="imgbtnBuscarCatalogo"> 
                                    <div class="row pt-2">
                                        <span class="col-sm-2 col-form-label-sm">Buscar por</span>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="cboFiltro" runat="server" CssClass="form-select form-select-sm js-choice" TabIndex="1">
                                                <asp:ListItem>SELECCIONE EL FILTRO</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control form-control-sm" TabIndex="2"
                                                    style="text-transform :uppercase" placeholder="Ingrese Busqueda"></asp:TextBox>
                                                &nbsp;
                                                <asp:ImageButton ID="imgbtnBuscarCatalogo" runat="server" Height="18px" TabIndex="3" CssClass="mt-1"
                                                    ImageUrl="~/Imagenes/buscar.png" ToolTip="Buscar Registro" Width="21px" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12 mt-3">
                                            <div class="table-responsive scrollbar">
                                                <asp:GridView ID="grdLista" runat="server" AutoGenerateColumns="False" TabIndex="4"
                                                    GridLines="None" CssClass="table table-responsive-sm table-hover small" AllowPaging="True" 
                                                    EmptyDataText="No hay registros a visualizar" PageSize="14" OnRowCommand ="grdLista_RowCommand" >
                                                    <PagerStyle CssClass="pgr" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:CommandField ShowSelectButton="True"  />
                                                        <asp:BoundField DataField="Codigo" HeaderText="Código" />  
                                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                                                        <asp:BoundField DataField="IdTipoActivo" HeaderText="Id. Tip. Act." />
                                                        <asp:BoundField DataField="DescripcionTipoActivo" HeaderText="Tipo de Activo" />
                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <div class="bootstrap-switch bootstrap-switch-on bootstrap bootstrap-switch-animate bootstrap-switch-mini ">
                                                                    <div class="bootstrap-switch-container">
                                                                        <asp:LinkButton ID="lnkEstadoOn" runat="server" CommandName="Activar" CommandArgument='<%# Eval("Codigo") & "," & Eval("IdTipoActivo") %>' CssClass="bootstrap-switch-handle-on bootstrap-switch-primary" Visible='<%# If(Eval("Estado") = "True", "True", "False") %>'>Activado</asp:LinkButton>
                                                                        <span class="bootstrap-switch-label">&nbsp;</span>
                                                                        <asp:LinkButton ID="lnkEstadoOff" runat="server" CommandName="Desactivar" CommandArgument='<%# Eval("Codigo") & "," & Eval("IdTipoActivo") %>' CssClass="bootstrap-switch-handle-off bootstrap-switch-danger" Visible='<%# If(Eval("Estado") = "True", "False", "True") %>'>Anulado</asp:LinkButton>
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
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div role="tabpanel" class="tab-pane" id="tab2">
                        <asp:UpdatePanel ID="updpnlTab2" runat="server">
                            <ContentTemplate>
                            <asp:Panel ID="pnlGeneral" runat="server" CssClass="container">
                                <div class="row">
                                    <div class="col-lg-12">
                                      <h5 class="mb-1" data-anchor="data-anchor">Mantenimiento de Catalogo</h5>
                                    </div>
                                </div>
                                <div class="row mt-3">
                                    <div class="col-12 mb-0 bg-100 d-none d-md-block">
                                      <h6 class="mt-2 mb-1">DATOS PRINCIPALES</h6>
                                      <hr class="bg-300 m-0" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-3 col-md-4 mt-2">
                                        <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Código Catalogo:</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtIdCatalogo" runat="server" CssClass="form-control form-control-sm" TabIndex="10"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-9 col-md-4 mt-2">
                                        <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Tipo Activo:</label>
                                        <div class="input-group">
                                            <asp:DropDownList ID="cboTipoActivo" runat="server" AppendDataBoundItems="True" TabIndex="11"
												AutoPostBack="True" CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase">
												<asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
											</asp:DropDownList>
											<ajaxToolkit:ListSearchExtender ID="cboTipoActivo_ListSearchExtender" runat="server"
												Enabled="True" PromptText="Iniciar Búsqueda" TargetControlID="cboTipoActivo">
											</ajaxToolkit:ListSearchExtender>
											<asp:RequiredFieldValidator ID="rfvTipoActivo" runat="server" 
												ControlToValidate="cboTipoActivo" EnableClientScript="False" InitialValue="SELECCIONE DATO"
												ErrorMessage="Ingrese el código de tipo de activo" Font-Size="10px" ForeColor="Red" 
												ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6 col-md-8 mt-2">
                                        <span class="fas fa-shipping-fast"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción:</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="12"
                                                style="text-transform :uppercase" placeholder="Descripción Catalogo"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" 
                                                ControlToValidate="txtDescripcion" EnableClientScript="False" 
                                                ErrorMessage="Ingrese la descripción" Font-Size="10px" ForeColor="Red" 
                                                ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-md-4 mt-2">
                                        <span class="fas fa-chalkboard-teacher"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción Abreviada:</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtDescripcionAbreviada" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="13"
                                                style="text-transform : uppercase" placeholder="Descripción Abreviada Catalogo"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDescripcionAbreviada" runat="server" 
                                                ControlToValidate="txtDescripcionAbreviada" EnableClientScript="False" 
                                                ErrorMessage="Ingrese la descripción abreviada" Font-Size="10px" 
                                                ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-2 col-md-4 mt-2">
                                        <span class="fas fa-phone-alt"></span><label class="text-black-50 col-form-label fs--2 m-1">Vida Útil (Meses):</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtVidaUtil" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="14"
                                                style="text-transform :uppercase" placeholder="Vida Util del Catalogo" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-md-4 mt-2">
										<span class="fas fa-stream"></span><label class="text-black-50 col-form-label fs--2 m-1">Per.Garantía(Meses):</label>
										<div class="input-group">
											<asp:TextBox ID="txtPeriodoGarantia" runat="server" CssClass="form-control form-control-sm" TabIndex="16"
												style="text-transform :uppercase" placeholder="Periodo de Garantía" ></asp:TextBox>
										</div>
									</div>
									<div class="col-lg-2 col-md-4 mt-2">
										<span class="fas fa-stream"></span><label class="text-black-50 col-form-label fs--2 m-1">Per.Mín.Mnto.(Meses):</label>
										<div class="input-group">
											<asp:TextBox ID="txtPeriodoMinimo" runat="server" CssClass="form-control form-control-sm" TabIndex="16"
												style="text-transform :uppercase" placeholder="Periodo Mínimo de Mantenimiento" ></asp:TextBox>
										</div>
									</div>
									<div class="col-lg-2 col-md-4 mt-2 invisible" id="divCuentaContableMantenimientoCatalogo" runat="server">
										<span class="fas fa-stream"></span><label class="text-black-50 col-form-label fs--2 m-1">Cuenta Contable:</label>
										<div class="input-group">
											<asp:TextBox ID="txtCuentaContable" runat="server" CssClass="form-control form-control-sm" TabIndex="15"
												style="text-transform :uppercase" placeholder="Cuenta Contable" ></asp:TextBox>
										</div>
									</div>
									<div class="col-lg-2 col-md-4 mt-2 invisible" id="divCuentaContableLeasingMantenimientoCatalogo" runat="server">
										<span class="fas fa-stream"></span><label class="text-black-50 col-form-label fs--2 m-1">Cta. Ctble. Leasing:</label>
										<div class="input-group">
											<asp:TextBox ID="txtCuentaContableLeasing" runat="server" CssClass="form-control form-control-sm" TabIndex="16"
												style="text-transform :uppercase" placeholder="Cuenta Contable Leasing" ></asp:TextBox>
										</div>
									</div>
                                    <asp:LinkButton ID="lnk_mostrarPanelCaracteristica" runat="server"></asp:LinkButton>
                                    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelCaracteristica_ModalPopupExtender" runat="server" 
                                        BackgroundCssClass="FondoAplicacion" DropShadow="False" DynamicServicePath="" Enabled="True" 
                                        CancelControlID="btnCancelarCaracteristica" 
                                        PopupControlID="pnlSeleccionarCaracteristica" TargetControlID="lnk_mostrarPanelCaracteristica">
                                    </ajaxToolkit:ModalPopupExtender>
                                </div>
                                <div class="row mt-3">
                                    <div class="col-12 mb-0 bg-100 d-none d-md-block">
                                      <h6 class="mt-2 mb-1">CARACTERISTICAS ASIGNADAS
                                        <asp:Button ID="btnAdicionarCaracteristica" runat="server" Text="[+] Características" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Características" ValidationGroup="vgrpValidar"/>
                                      </h6>
                                      <hr class="bg-300 m-0" />
                                    </div>
                                    <div class="col-lg-3 col-md-4 mt-2">
                                    </div>
                                </div>
                                <div class="row justify-content-between mt-3"> 
                                    <div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
                                      <asp:UpdatePanel ID="updpnlDetalleCaracteristica" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlDetalleCaracteristica" runat="server" CssClass="bg-light" >
                                                <div class="table-responsive scrollbar">
                                                <asp:GridView ID="grdDetalleCaracteristica" runat="server" AutoGenerateColumns="False" TabIndex="4" 
                                                    GridLines="None" CssClass="table table-striped table-hover small overflow-hidden" AllowPaging="True" 
                                                    EmptyDataText="No hay registros a visualizar" PageSize="50" >
                                                    <PagerStyle CssClass="mGrid" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <Columns>
                                                        <asp:CommandField ShowDeleteButton="True" HeaderStyle-Width="70px" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkRowDetalleCaracteristica" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Item" HeaderText="#" ControlStyle-Width="25px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                        <asp:BoundField DataField="IdCaracteristica" HeaderText="Código" HeaderStyle-Width="40px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                        <asp:TemplateField HeaderText="Valor" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
															<ItemTemplate>
																<div class="input-group">
																	<asp:TextBox ID="txtValorDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("Valor") %>' CommandName="ButtonField" AutoPostBack="False" style="text-transform :uppercase" ></asp:TextBox>
																</div>
															</ItemTemplate>
														</asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Id. Ref. SAP" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                            <ItemTemplate>
                                                                <div class="input-group">
                                                                    <asp:TextBox ID="txtIdReferenciaSAPDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("IdReferenciaSAP") %>' CommandName="ButtonField" AutoPostBack="False" style="text-transform :uppercase" ></asp:TextBox>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Campo SAP" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                            <ItemTemplate>
                                                                <div class="input-group">
                                                                    <asp:TextBox ID="txtDescripcionCampoSAPDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("DescripcionCampoSAP") %>' CommandName="ButtonField" AutoPostBack="False" style="text-transform :uppercase" ></asp:TextBox>
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
                            </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <!-- Final Paneles Tab -->
            </div>
            <br />
            <div class="container">
                <div class="row justify-content-center">
                    <div class="my-2 col-12 col-form-label col-form-label-sm">
                        <asp:UpdatePanel ID="updpnlValidacion" runat="server">
                            <ContentTemplate>
                                <div class="position-fixed bottom-0 end-0 p-3">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="d-flex flex-row justify-content-end pb-2">
                        <div class="col-lg-4 col-md-5 text-end">
                            <asp:Button ID="btnNuevo" Text="Nuevo" runat="server" OnClick="btnNuevo_Click"  CssClass="btn btn-primary" TabIndex="100" ToolTip="Nuevo Registro" />
                            &nbsp;
                            <asp:Button ID="btnGuardar" Text="Guardar" runat="server" OnClick="btnGuardar_Click" CssClass="btn btn-success" TabIndex="101" ToolTip="Guardar Registro" ValidationGroup="vgrpValidar"/>
                            &nbsp;
                            <asp:Button ID="btnEditar" Text="Editar" runat="server" OnClick="btnEditar_Click" CssClass="btn btn-primary" TabIndex="102" ToolTip="Editar Registro" ValidationGroup="vgrpValidarBusqueda"/>
                            &nbsp;
                            <asp:Button ID="btnDeshacer" Text="Deshacer" runat="server" OnClick="btnDeshacer_Click" CssClass="btn btn-secondary" TabIndex="103" ToolTip="Deshacer Registro" ValidationGroup="vgrpValidar" CausesValidation="False"/>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hfTab" runat="server" />
        </div>
        <!-- Final Panel -->
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var selectedTab = $("#<%=hfTab.ClientID%>");
        var tabId = selectedTab.val() != "" ? selectedTab.val() : "tab1";
        $('#dvTab a[href="#' + tabId + '"]').tab('show');
        $("#dvTab a").click(function () {
            selectedTab.val($(this).attr("href").substring(1));
        });
    });
    </script>
    <br />
    <asp:UpdatePanel ID="updpnlEspera" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMensajeEspera" runat="server" CssClass="CajaDialogoProgreso border border-info">
                <asp:UpdateProgress ID="uprProceso" runat="server" DisplayAfter="0">
                    <ProgressTemplate>
                        <table align="center" style="text-align:center; width:100%;">
                            <tr align="center">
                                <td>
                                    <asp:Image ID="Image1" runat="server" alt="Procesando" 
                                        ImageUrl="~/Imagenes/Loading.gif" Width="40px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="text-900 small">Cargando...</span>
                                </td>
                            </tr>
                        </table>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="ModalProgress" runat="server" 
                BackgroundCssClass="FondoAplicacion" DropShadow="False" DynamicServicePath="" 
                Enabled="True" PopupControlID="pnlMensajeEspera" 
                TargetControlID="pnlMensajeEspera">
            </ajaxToolkit:ModalPopupExtender>
            <br/>
        </ContentTemplate>
    </asp:UpdatePanel>

    <link href="cssbootstrap/bootstrap-switch.css" rel="stylesheet" />
    <script src="jsbootstrap/bootstrap-switch.js"></script>

    <asp:UpdatePanel ID="updpnlSeleccionarCaracteristica" runat="server" EnableEventValidation="false">
        <ContentTemplate>
            <asp:Panel ID="pnlSeleccionarCaracteristica" runat="server" CssClass="container" DefaultButton="imgbtnBuscarCaracteristica" >
                <div class="modal-dialog modal-lg">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header bg-light">
                                    <h4 class="modal-title" id="myModalLabelProducto">
                                        Seleccionar Caracteristica</h4>
                                    <asp:Button ID="imgbtnCancelarCaracteristicaImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <div class="row pt-2 pb-2">
                                        <span class="col-sm-2 col-form-label-sm">Buscar por</span>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="cboFiltroCaracteristica" runat="server" CssClass="form-select form-select-sm js-choice"
                                                TabIndex ="60">
                                                <asp:ListItem Value="vDescripcionCaracteristica">DESCRIPCION</asp:ListItem>
                                                <asp:ListItem Value="cIdCaracteristica">CODIGO</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtBuscarCaracteristica" runat="server" CssClass="form-control form-control-sm" TabIndex="61"
                                                    style="text-transform :uppercase" placeholder="Ingrese Busqueda" onfocus="fnSetFocus('ctl00$cuerpo$txtBuscarCaracteristica')" ></asp:TextBox>
                                                &nbsp;
                                                <asp:ImageButton ID="imgbtnBuscarCaracteristica" runat="server" Height="18px" TabIndex="62" CssClass="mt-1"
                                                    ImageUrl="~/Imagenes/buscar.png" ToolTip="Buscar Registro" Width="21px" onfocus="fnSetFocus('ctl00$cuerpo$txtBuscarCaracteristica')" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                      <div class="viewport table-responsive scrollbar">
                                        <div class="col-md-12 mt-3">
                                            <asp:GridView ID="grdListaCaracteristica" runat="server" AutoGenerateColumns="False" TabIndex="63" DataKeyNames="Codigo"
                                                GridLines="None" CssClass="table table-responsive-sm table-hover small" AllowPaging="True" 
                                                EmptyDataText="No hay registros a visualizar" PageSize="6" >
                                                <PagerStyle CssClass="pgr" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <AlternatingRowStyle CssClass="alt" />
                                                <Columns>
                                                    <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="mx-0 bg-200 text-900 fs--1 fw-semi-bold" />
                                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-Width="440px" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="mx-0 bg-200 text-900 fs--1 fw-semi-bold" />
                                                </Columns>
                                                <HeaderStyle CssClass="thead-dark" />
                                            </asp:GridView>
                                        </div>
                                      </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <div class="container overflow-scroll scrollbar">
                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 mt-2">
                                                <span class="fas fa-phone-alt"></span><label class="text-black-50 col-form-label fs--2 m-1">Valor Caract.:</label>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtValorCaracteristica" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="20"
                                                        style="text-transform :uppercase" placeholder="Valor de la Caracteristica" ></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-lg-4 col-md-4 mt-2">
                                                <span class="fas fa-phone-alt"></span><label class="text-black-50 col-form-label fs--2 m-1">Id. Referencia SAP:</label>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtIdReferenciaSAPCaracteristica" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="21"
                                                        style="text-transform :uppercase" placeholder="Código Referencia SAP" ></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-lg-4 col-md-4 mt-2">
                                                <span class="fas fa-phone-alt"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción Campo SAP:</label>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtDescripcionCampoSAPCaracteristica" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="22"
                                                        style="text-transform :uppercase" placeholder="Campo SAP" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="container">
                                        <div class="row">
                                            <asp:Label ID="lblMensajeCaracteristica" runat="server" CssClass="col-lg-5 col-md-5 col-form-label col-form-label-sm text-danger"></asp:Label>
                                            <div class="col-lg-6 text-end p-2">
                                                <asp:Button ID="btnAceptarCaracteristica" runat="server" ValidationGroup="vgrpValidarBusqueda"
                                                    ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                &nbsp;
                                                <asp:Button ID="btnCancelarCaracteristica" runat="server" 
                                                    ToolTip="Cancelar Registro" TabIndex ="73" Text="Cancelar" cssclass="btn btn-outline-google-plus btn-sm"/>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <link href="Styles/cssbootstrap/bootstrap-switch.css" rel="stylesheet" />
    <script src="js/jsbootstrap/bootstrap-switch.js"></script>

    <script type="text/javascript" language="javascript">
        var ModalProgress = '<%= ModalProgress.ClientID %>';

        //Función para deshabilitar el ENTER y ejecutar el botón por defecto.
        function stopRKey(evt) {
            var evt = (evt) ? evt : ((event) ? event : null);
            var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
            if ((evt.keyCode == 13) && ((node.type == "text") || (node.type == "radio") || (node.type == "checkbox"))) { return false; }
        }
        document.onkeypress = stopRKey;

        //Función para utilizar para al presionar el ENTER se cambie de foco.
        function CambiaFoco(cajadestino) {
            var key = window.event.keyCode;
            if (key == 13)
                document.getElementById(cajadestino).focus();
        }
    </script>
</asp:Content>
