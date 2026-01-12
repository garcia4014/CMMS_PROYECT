<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmLogiMntCatalogoComponente.aspx.vb" Inherits="pryCMMS.frmLogiMntCatalogoComponente" %>
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
    <script type="text/javascript" language="javascript">
        function deshabilitar(boton) {
            document.getElementById(boton).style.visibility = 'hidden';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cuerpo" runat="server">
    <link href="Content/CuadroDialogo.css" rel="stylesheet" type="text/css"/>
    <script src="Scripts/jsUpdateProgress.js" type="text/javascript"></script>
    <link href="Content/AutoCompletar.css" rel="stylesheet" type="text/css"/>
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
								<asp:HiddenField ID="hfdSistemaFuncional" runat="server" />
								<asp:HiddenField ID="hfdOperacionDetalle" runat="server" />
								<asp:HiddenField ID="hfdFocusObjeto" runat="server" />
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
                                                    EmptyDataText="No hay registros a visualizar" PageSize="50" OnRowCommand="grdLista_RowCommand" >
                                                    <PagerStyle CssClass="pgr" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:CommandField ShowSelectButton="True" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900"/>
														<asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-CssClass="bg-200 text-900"/>
														<asp:BoundField DataField="DescripcionTipoActivo" HeaderText="Tipo Activo" HeaderStyle-CssClass="bg-200 text-900"/>
														<asp:BoundField DataField="IdCuentaContable" HeaderText="Cta. Contable" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:TemplateField HeaderStyle-CssClass="bg-200 text-900">
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
                                                        <asp:BoundField DataField="IdTipoActivo" HeaderText="IdTipoActivo" />
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
                                <div class="col-12 mt-2">
                                    <h3 class="h5">Mantenimiento de Catálogo / Componente</h3>
                                </div>
                                <div class="row mt-3">
                                    <div class="col-12 mb-0 bg-100 d-none d-md-block">
                                      <h6 class="mt-2 mb-1">DATOS PRINCIPALES DEL CATALOGO PRINCIPAL</h6>
                                      <hr class="bg-300 m-0" />
                                    </div>
                                </div>
                                <div class="row mt-1">
                                    <div class="col-lg-6 col-sm-12">
                                        <div class="row">
                                            <div class="col-lg-3 col-md-4 mt-2">
                                                <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Id.Catalogo:</label>
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
                                            <div class="col-lg-8 col-md-4 mt-2">
                                                <span class="fas fa-shipping-fast"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción:</label>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="12"
                                                        style="text-transform :uppercase" placeholder="Descripción del Catalogo Principal"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" 
                                                        ControlToValidate="txtDescripcion" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese la descripción" Font-Size="10px" ForeColor="Red" 
                                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-lg-4 col-md-4 mt-2">
                                                <span class="fas fa-chalkboard-teacher"></span><label class="text-black-50 col-form-label fs--2 m-1">Descr.Abreviada:</label>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtDescripcionAbreviada" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="13"
                                                        style="text-transform : uppercase" placeholder="Descripción Abreviada del Catalogo Principal"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvDescripcionAbreviada" runat="server" 
                                                        ControlToValidate="txtDescripcionAbreviada" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese la descripción abreviada" Font-Size="10px" 
                                                        ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-3 col-md-4 mt-2">
                                                <span class="fas fa-phone-alt"></span><label class="text-black-50 col-form-label fs--2 m-1">Vida Útil:</label>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtVidaUtil" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="14"
                                                        style="text-transform :uppercase" placeholder="Vida Util del Catalogo" ></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvVidaUtil" runat="server" 
                                                        ControlToValidate="txtVidaUtil" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese la vida util en meses del catálogo" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-lg-3 col-md-4 mt-2">
										        <span class="fas fa-stream"></span><label class="text-black-50 col-form-label fs--2 m-1">P.Garantía:</label>
										        <div class="input-group">
											        <asp:TextBox ID="txtPeriodoGarantia" runat="server" CssClass="form-control form-control-sm" TabIndex="16"
												        style="text-transform :uppercase" placeholder="Periodo de Garantía" ></asp:TextBox>
										        </div>
									        </div>
									        <div class="col-lg-3 col-md-4 mt-2">
										        <span class="fas fa-stream"></span><label class="text-black-50 col-form-label fs--2 m-1">P.Mín.M.:</label>
										        <div class="input-group">
											        <asp:TextBox ID="txtPeriodoMinimo" runat="server" CssClass="form-control form-control-sm" TabIndex="17"
												        style="text-transform :uppercase" placeholder="Periodo Mínimo de Mantenimiento" ></asp:TextBox>
										        </div>
									        </div>
									        <div class="col-lg-1 col-md-4 mt-2 invisible" id="div1" runat="server">
										        <span class="fas fa-stream"></span><label class="text-black-50 col-form-label fs--2 m-1">Cuenta Contable:</label>
										        <div class="input-group">
											        <asp:TextBox ID="txtCuentaContable" runat="server" CssClass="form-control form-control-sm" TabIndex="18"
												        style="text-transform :uppercase" placeholder="Cuenta Contable" ></asp:TextBox>
										        </div>
									        </div>
									        <div class="col-lg-1 col-md-4 mt-2 invisible" id="div2" runat="server">
										        <span class="fas fa-stream"></span><label class="text-black-50 col-form-label fs--2 m-1">Cta. Ctble. Leasing:</label>
										        <div class="input-group">
											        <asp:TextBox ID="txtCuentaContableLeasing" runat="server" CssClass="form-control form-control-sm" TabIndex="19"
												        style="text-transform :uppercase" placeholder="Cuenta Contable Leasing" ></asp:TextBox>
										        </div>
									        </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-sm-12 bg-200">
                                        <div class="row mt-1">
                                            <div class="col-12 mb-0 d-none d-md-block">
                                                <h6 class="mt-2 mb-1">CARACTERISTICAS ASIGNADAS AL CATALOGO PRINCIPAL</h6>
                                                <hr class="bg-300 m-0" />
                                            </div>
                                        </div>
                                        <div id="divFiltrarCaracteristicaCatalogo" runat="server" class="row justify-content-between">
                                            <div class="col-lg-12 col-md-12 col-sm-12">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                                    <ContentTemplate>
                                                    <span class="fas fa-link"></span><label class="text-black-50 col-form-label fs--2 m-1">Filtrar Caracteristica:</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboCaracteristicaCatalogo" runat="server" AppendDataBoundItems="True" TabIndex="29"
												            CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase">
												            <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
											            </asp:DropDownList>
                                                        <asp:Button ID="btnAdicionarCaracteristicaCatalogo" runat="server" Text="Agregar" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Caracteristica" TabIndex="30"/>
                                                    </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="row justify-content-between mt-3"> 
                                            <div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
                                                <asp:UpdatePanel ID="updpnlDetalleCaracteristicaCatalogo" runat="server">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlDetalleCaracteristicaCatalogo" runat="server" CssClass="bg-light" >
                                                        <div style="overflow:auto; height:220px; align:left;" class="scrollbar">
                                                            <asp:GridView ID="grdDetalleCaracteristicaCatalogo" runat="server" AutoGenerateColumns="False" TabIndex="4" 
                                                                GridLines="None" CssClass="table table-striped table-hover small overflow-hidden" AllowPaging="True" 
                                                                EmptyDataText="No hay registros a visualizar" PageSize="50" >
                                                                <PagerStyle CssClass="mGrid" />
                                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                <HeaderStyle CssClass="grid-header" />
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
																		        <asp:TextBox ID="txtValorDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("Valor") %>' CommandName="ButtonField" AutoPostBack="True" style="text-transform :uppercase" OnTextChanged="txtValorDetalleCatalogo_TextChanged" ></asp:TextBox>
																	        </div>
																        </ItemTemplate>
															        </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Id. Ref. SAP" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                        <ItemTemplate>
                                                                            <div class="input-group">
                                                                                <asp:TextBox ID="txtIdReferenciaSAPDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("IdReferenciaSAP") %>' CommandName="ButtonField" AutoPostBack="True" OnTextChanged="txtIdReferenciaSAPDetalleCatalogo_TextChanged"></asp:TextBox>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Campo SAP" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                        <ItemTemplate>
                                                                            <div class="input-group">
                                                                                <asp:TextBox ID="txtDescripcionCampoSAPDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("DescripcionCampoSAP") %>' CommandName="ButtonField" AutoPostBack="True" style="text-transform :uppercase" OnTextChanged="txtDescripcionCampoSAPDetalleCatalogo_TextChanged"></asp:TextBox>
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
                                <div class="row mt-3">
                                    <div class="col-lg-9 col-md-8 mb-0 bg-100 d-none d-md-block">
                                      <h6 class="mt-2 mb-1">LISTADO DE COMPONENTES ASIGNADOS AL CATALOGO PRINCIPAL</h6>
                                      <hr class="bg-300 m-0" />
                                    </div>
                                    <div class="col-lg-3 col-md-4 mt-2">
                                        <div class="dropdown font-sans-serif position-static">
                                            <span class="fab fa-slack-hash"></span><label class="text-black-50 col-form-label fs--2 m-1">Opciones:</label>
                                            <button class="btn btn-link text-600 btn-sm dropdown-toggle btn-reveal" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h fs--1"></span></button>
                                            <div class="dropdown-menu dropdown-menu-end border py-0">
                                                <div class="bg-white py-2">
                                                    <div class="d-grid gap-0">
                                                        <div class="col-12">
                                                            <ul class="nav flex-lg-column fs--1">
                                                                <li class="nav-item me-2 me-lg-0">
                                                                    <asp:LinkButton ID="lnkbtnAgregarComponente" runat="server"  CommandName="Agregar" ToolTip="Agregar Componente" CommandArgument='<%# Eval("IdCliente") & "*" & Eval("StatusSunat") & "*" & Eval("FechaEmision") & "*" & Eval("NombreArchivo") & "*" & Eval("IdTipoDocumento") & "*" & Eval("NumeroSerieDocumento") & "*" & Eval("NumeroCorrelativo") & "*" & Eval("IdEmpresa") %>' Text="<span class='fas fa-mail-bulk me-2'></span>Agregar" CssClass="nav-link nav-link-card-details" Visible='<%# If(Eval("IdTipoDocumento") = "CO" And Eval("StatusSunat") <> "ANULADO", "true", "false") %>' ValidationGroup="vgrpValidar" />
                                                                </li>
                                                                <li class="nav-item me-2 me-lg-0">
                                                                    <asp:LinkButton ID="lnkbtnEditarComponente" runat="server"  CommandName="Editar" ToolTip="Editar Componente" CommandArgument='<%# Eval("IdCliente") & "*" & Eval("StatusSunat") & "*" & Eval("FechaEmision") & "*" & Eval("NombreArchivo") & "*" & Eval("IdTipoDocumento") & "*" & Eval("NumeroSerieDocumento") & "*" & Eval("NumeroCorrelativo") & "*" & Eval("IdEmpresa") %>' Text="<span class='fas fa-mail-bulk me-2'></span>Editar" CssClass="nav-link nav-link-card-details" Visible='<%# If(Eval("IdTipoDocumento") = "CO" And Eval("StatusSunat") <> "ANULADO", "true", "false") %>' ValidationGroup="vgrpValidar" />
                                                                </li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row pt-1">
                                    <div class="col-md-12">
                                        <%--Inicio:Se utiliza para que al seleccionar la grilla no se posicione al inicio--%>
								        <script type="text/javascript">
                                            var xPos, yPos;
                                            var xPos1, yPos1;
                                            var xPos2, yPos2;
                                            var prm = Sys.WebForms.PageRequestManager.getInstance();
                                            prm.add_beginRequest(BeginRequestHandler);
                                            prm.add_endRequest(EndRequestHandler);
                                            function BeginRequestHandler(sender, args) {
                                                xPos = $get("divGridListaDetalle").scrollLeft;
                                                yPos = $get("divGridListaDetalle").scrollTop;
                                            }
                                            function EndRequestHandler(sender, args) {
                                                $get("divGridListaDetalle").scrollLeft = xPos;
                                                $get("divGridListaDetalle").scrollTop = yPos;
                                            }
                                        </script>
								        <%--Final:Se utiliza para que al seleccionar la grilla no se posicione al inicio--%>
                                        <div id="divGridListaDetalle" style="overflow:auto; height:220px; align:left;" class="scrollbar">
                                            <asp:GridView ID="grdListaDetalle" runat="server" AutoGenerateColumns="False" TabIndex="4"
                                                GridLines="None" CssClass="table table-responsive-sm table-striped table-hover small" AllowPaging="True" 
                                                EmptyDataText="No hay registros a visualizar" PageSize="50" OnRowCommand ="grdListaDetalle_RowCommand" >
                                                <PagerStyle CssClass="pgr" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <AlternatingRowStyle CssClass="alt" />
                                                <Columns>
                                                    <asp:CommandField ShowSelectButton="True" />
                                                    <asp:BoundField DataField="IdEnlace" HeaderText="Catalogo" />
                                                    <asp:BoundField DataField="IdCatalogo" HeaderText="Componente" />
                                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                                                    <asp:BoundField DataField="DescripcionAbreviada" HeaderText="Descripción Abreviada" />
                                                    <asp:BoundField DataField="IdTipoActivo" HeaderText="Id Tipo de Activo" />
                                                    <asp:BoundField DataField="DescAbrevTipoActivo" HeaderText="Tipo de Activo" />
                                                    <asp:BoundField DataField="IdSistemaFuncional" HeaderText="Id Sistema Funcional" />
                                                    <asp:BoundField DataField="DescAbrevSistemaFuncional" HeaderText="Sistema Funcional" />
                                                    <asp:BoundField DataField="IdCuentaContable" HeaderText="Cta. Contable" />
                                                    <asp:BoundField DataField="IdCuentaContableLeasing" HeaderText="Cta. Ctbl. Leasing" />
                                                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <div class="bootstrap-switch bootstrap-switch-on bootstrap bootstrap-switch-animate bootstrap-switch-mini ">
                                                                <div class="bootstrap-switch-container">
                                                                    <asp:LinkButton ID="lnkEstadoDetalleOn" runat="server" CommandName="Activar" CommandArgument='<%# Container.DataItemIndex & "," & Eval("IdEnlace") & "," & Eval("IdCatalogo") & "," & Eval("IdTipoActivo") %>' CssClass="bootstrap-switch-handle-on bootstrap-switch-primary" Visible='<%# If(Eval("Estado") = "True", "True", "False") %>'>Activado</asp:LinkButton>
                                                                    <span class="bootstrap-switch-label">&nbsp;</span>
                                                                    <asp:LinkButton ID="lnkEstadoDetalleOff" runat="server" CommandName="Desactivar" CommandArgument='<%# Container.DataItemIndex & "," & Eval("IdEnlace") & "," & Eval("IdCatalogo") & "," & Eval("IdTipoActivo") %>' CssClass="bootstrap-switch-handle-off bootstrap-switch-danger" Visible='<%# If(Eval("Estado") = "True", "False", "True") %>'>Anulado</asp:LinkButton>
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
								<div class="col-lg-12 col-md-12">
									<div class="float-right">
									</div>
									<div class="float-right">
									</div>
								</div>
                            </asp:Panel>
                            <br/>
                            <asp:LinkButton ID="lnk_mostrarPanelDetalleCatalogo" runat="server"></asp:LinkButton>
		                    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender" 
			                    runat="server" BackgroundCssClass="FondoAplicacion" 
			                    CancelControlID="btnCancelarDetalleCatalogo"
			                    DynamicServicePath="" Enabled="True" PopupControlID="pnlIngresarDetalleCatalogo" 
			                    TargetControlID="lnk_mostrarPanelDetalleCatalogo">
		                    </ajaxToolkit:ModalPopupExtender>
                            <asp:Panel ID="pnlIngresarDetalleCatalogo" runat="server" CssClass="container">
                                <div class="modal-dialog modal-xl">
                                    <div class="shadow rounded">
                                        <div class="modal-dialog-scrollable">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h4 class="modal-title" id="myModalLabelCatalogo">
                                                        Datos del Componente</h4>
                                                    <asp:Button ID="imgbtnCancelarCatalogoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                </div>
                                                <div class="modal-body">
                                                    <div class="form-group">
                                                        <div class="row mt-1">
                                                            <div class="col-lg-6 col-sm-12">
                                                                <div class="row mt-1">
                                                                    <div class="col-12 mb-0 bg-100 d-none d-md-block">
                                                                      <h6 class="mt-2 mb-1">DATOS PRINCIPALES</h6>
                                                                      <hr class="bg-300 m-0" />
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-lg-3 col-md-4 mt-2">
                                                                        <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Id.Compon.:</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtIdDetalleCatalogo" runat="server" CssClass="form-control form-control-sm" TabIndex="20"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-lg-4 col-md-4 mt-2">
                                                                        <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Tipo Activo:</label>
                                                                        <div class="input-group">
                                                                            <asp:DropDownList ID="cboTipoActivoDetalleCatalogo" runat="server" AppendDataBoundItems="True" TabIndex="21"
												                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase">
												                                <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
											                                </asp:DropDownList>
											                                <ajaxToolkit:ListSearchExtender ID="cboTipoActivoDetalleCatalogo_ListSearchExtender" runat="server"
												                                Enabled="True" PromptText="Iniciar Búsqueda" TargetControlID="cboTipoActivoDetalleCatalogo">
											                                </ajaxToolkit:ListSearchExtender>
											                                <asp:RequiredFieldValidator ID="rfvTipoActivoDetalleCatalogo" runat="server" 
												                                ControlToValidate="cboTipoActivoDetalleCatalogo" EnableClientScript="False" InitialValue="SELECCIONE DATO"
												                                ErrorMessage="Ingrese el código de tipo de activo" Font-Size="10px" ForeColor="Red" 
												                                ValidationGroup="vgrpValidarComponente">(*)</asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-lg-5 col-md-4 mt-2">
                                                                        <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Sist.Funcional:</label>
                                                                        <div class="input-group">
                                                                            <asp:DropDownList ID="cboSistemaFuncionalDetalleCatalogo" runat="server" AppendDataBoundItems="True" TabIndex="22"
                                                                                AutoPostBack="False" CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase">
                                                                                <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <ajaxToolkit:ListSearchExtender ID="cboSistemaFuncionalDetalleCatalogo_ListSearchExtender" runat="server"
                                                                                Enabled="True" PromptText="Iniciar Búsqueda" TargetControlID="cboSistemaFuncionalDetalleCatalogo">
                                                                            </ajaxToolkit:ListSearchExtender>
                                                                            <asp:RequiredFieldValidator ID="rfvSistemaFuncionalDetalleCatalogo" runat="server" 
                                                                                ControlToValidate="cboSistemaFuncionalDetalleCatalogo" EnableClientScript="False" InitialValue="SELECCIONE DATO"
                                                                                ErrorMessage="Ingrese el sistema funcional" Font-Size="10px" ForeColor="Red" 
                                                                                ValidationGroup="vgrpValidarComponente">(*)</asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-lg-12 col-md-4 col-sm-4 mt-2">
                                                                        <span class="fas fa-shipping-fast"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción:</label>
                                                                        <div class="input-group">
                                                                            <asp:DropDownList ID="cboDescripcionDetalleCatalogo" runat="server" AppendDataBoundItems="True" TabIndex="23" 
                                                                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice" style="text-transform: uppercase">
                                                                                <asp:ListItem Value="">SELECCIONE DATO</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:TextBox ID="txtDescripcionDetalleCatalogo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="24"
                                                                                style="text-transform :uppercase" placeholder="Descripción Componente"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvDescripcionDetalleCatalogo" runat="server" 
                                                                                ControlToValidate="txtDescripcionDetalleCatalogo" EnableClientScript="False" 
                                                                                ErrorMessage="Ingrese la descripción del componente / catálogo" Font-Size="10px" ForeColor="Red" 
                                                                                ValidationGroup="vgrpValidarComponente">(*)</asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-lg-6 col-md-8 col-sm-8 mt-2">
                                                                        <span class="fas fa-chalkboard-teacher"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción Abreviada:</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtDescripcionAbreviadaDetalleCatalogo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="25"
                                                                                style="text-transform : uppercase" placeholder="Descripción Abreviada Componente"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvDescripcionAbreviadaDetalleCatalogo" runat="server" 
                                                                                ControlToValidate="txtDescripcionAbreviadaDetalleCatalogo" EnableClientScript="False" 
                                                                                ErrorMessage="Ingrese la descripción abreviada del componente / catálogo" Font-Size="10px" 
                                                                                ForeColor="Red" ValidationGroup="vgrpValidarComponente">(*)</asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-lg-3 col-md-4 col-sm-4 mt-2">
                                                                        <span class="fas fa-shipping-fast"></span><label class="text-black-50 col-form-label fs--2 m-1">Vida Útil:</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtVidaUtilDetalleCatalogo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="26"
                                                                                style="text-transform :uppercase" placeholder="Vita Útil Componente"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-lg-3 col-md-4 mt-2">
											                            <span class="fas fa-stream"></span><label class="text-black-50 col-form-label fs--2 m-1">Per.Garantía:</label>
											                            <div class="input-group">
												                            <asp:TextBox ID="txtPeriodoGarantiaDetalleCatalogo" runat="server" CssClass="form-control form-control-sm" TabIndex="27"
													                            style="text-transform :uppercase" placeholder="Periodo de Garantía" ></asp:TextBox>
											                            </div>
										                            </div>
										                            <div class="col-lg-3 col-md-4 mt-2">
											                            <span class="fas fa-stream"></span><label class="text-black-50 col-form-label fs--2 m-1">Per.Mín.Mnto.:</label>
											                            <div class="input-group">
												                            <asp:TextBox ID="txtPeriodoMinimoDetalleCatalogo" runat="server" CssClass="form-control form-control-sm" TabIndex="28"
													                            style="text-transform :uppercase" placeholder="Periodo Mínimo de Mantenimiento" ></asp:TextBox>
											                            </div>
										                            </div>
										                            <div class="col-lg-3 col-md-4 mt-2 invisible" id="divCuentaContableMantenimientoCatalogo" runat="server">
											                            <span class="fas fa-stream"></span><label class="text-black-50 col-form-label fs--2 m-1">Cuenta Contable:</label>
											                            <div class="input-group">
												                            <asp:TextBox ID="txtCuentaContableDetalleCatalogo" runat="server" CssClass="form-control form-control-sm" TabIndex="29"
													                            style="text-transform :uppercase" placeholder="Cuenta Contable" ></asp:TextBox>
											                            </div>
										                            </div>
										                            <div class="col-lg-3 col-md-4 mt-2 invisible" id="divCuentaContableLeasingMantenimientoCatalogo" runat="server">
											                            <span class="fas fa-stream"></span><label class="text-black-50 col-form-label fs--2 m-1">Cta. Ctble. Leasing:</label>
											                            <div class="input-group">
												                            <asp:TextBox ID="txtCuentaContableLeasingDetalleCatalogo" runat="server" CssClass="form-control form-control-sm" TabIndex="30"
													                            style="text-transform :uppercase" placeholder="Cuenta Contable Leasing" ></asp:TextBox>
											                            </div>
										                            </div>
                                                               </div>
                                                            </div>
                                                            <div class="col-lg-6 col-sm-12 bg-200">
                                                                <div class="row mt-1">
                                                                    <div class="col-12 mb-0 d-none d-md-block">
                                                                      <h6 class="mt-2 mb-1">CARACTERISTICAS ASIGNADAS</h6>
                                                                      <hr class="bg-300 m-0" />
                                                                    </div>
                                                                </div>
                                                                <div id="divFiltrarCaracteristicaDetalleCatalogo" runat="server" class="row justify-content-between">
                                                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                                                        <asp:UpdatePanel ID="updpnlGeneral" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                                                          <ContentTemplate>
                                                                            <span class="fas fa-link"></span><label class="text-black-50 col-form-label fs--2 m-1">Filtrar Caracteristica:</label>
                                                                            <div class="input-group">
                                                                                <asp:DropDownList ID="cboCaracteristicaDetalleCatalogo" runat="server" AppendDataBoundItems="True" TabIndex="29"
												                                    CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase">
												                                    <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
											                                    </asp:DropDownList>
                                                                                <asp:Button ID="btnAdicionarCaracteristicaDetalleCatalogo" runat="server" Text="Agregar" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Caracteristica" TabIndex="31"/>
                                                                            </div>
                                                                          </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </div>
                                                                <div class="row justify-content-between mt-3"> 
                                                                    <div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
                                                                        <asp:UpdatePanel ID="updpnlDetalleCaracteristica" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:Panel ID="pnlDetalleCaracteristica" runat="server" CssClass="bg-light" >
                                                                                <div style="overflow:auto; height:220px; align:left;" class="scrollbar">
                                                                                    <asp:GridView ID="grdDetalleCaracteristicaDetalleCatalogo" runat="server" AutoGenerateColumns="False" TabIndex="4" 
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
																	                                    <asp:TextBox ID="txtValorDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("Valor") %>' CommandName="ButtonField" AutoPostBack="False" style="text-transform :uppercase" OnTextChanged="txtValorDetalleCaracteristica_TextChanged"></asp:TextBox>
																                                    </div>
															                                    </ItemTemplate>
														                                    </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Id. Ref. SAP" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                                                <ItemTemplate>
                                                                                                    <div class="input-group">
                                                                                                        <asp:TextBox ID="txtIdReferenciaSAPDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("IdReferenciaSAP") %>' CommandName="ButtonField" AutoPostBack="False" OnTextChanged="txtIdReferenciaSAPDetalleCaracteristica_TextChanged"></asp:TextBox>
                                                                                                    </div>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Campo SAP" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                                                <ItemTemplate>
                                                                                                    <div class="input-group">
                                                                                                        <asp:TextBox ID="txtDescripcionCampoSAPDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("DescripcionCampoSAP") %>' CommandName="ButtonField" AutoPostBack="False" style="text-transform :uppercase" OnTextChanged="txtDescripcionCampoSAPDetalleCaracteristica_TextChanged"></asp:TextBox>
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
                                                <div class="modal-footer">
                                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarComponente" />
                                                    <div class="col-md-4 text-end p-2">
                                                        <asp:Button ID="btnAceptarDetalleCatalogo" runat="server" ValidationGroup="vgrpValidarComponente" OnClick="btnAceptarDetalleCatalogo_Click" OnClientClick="deshabilitar(this.id)"
                                                            ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                        &nbsp;
                                                        <asp:Button ID="btnCancelarDetalleCatalogo" runat="server" 
                                                            ToolTip="Cancelar Registro" TabIndex ="73" Text="Cancelar" cssclass="btn btn-outline-google-plus btn-sm"/>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
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
                                    <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" role="alert" CssClass="alert alert-warning alert-dismissible fade show" />--%>
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
                            &nbsp;
                            <asp:Button ID="btnDuplicar" Text="Duplicar" runat="server" OnClick="btnDuplicar_Click" CssClass="btn btn-secondary" TabIndex="103" ToolTip="Duplicar Registro" ValidationGroup="vgrpValidarDuplicado" />
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
        function PopupShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 99999999;
        }
    </script>
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
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="updpnlDuplicarCatalogoComponente" runat="server" updatemode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger controlid="btnDuplicar" eventname="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:LinkButton ID="lnk_mostrarPanelDuplicarCatalogoComponente" runat="server"></asp:LinkButton>
		    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelDuplicarCatalogoComponente_ModalPopupExtender" 
			    runat="server" BackgroundCssClass="FondoAplicacion" 
			    CancelControlID="btnCancelarDuplicarCatalogoComponente"
			    DynamicServicePath="" Enabled="True" PopupControlID="pnlIngresarDuplicarCatalogoComponente" 
			    TargetControlID="lnk_mostrarPanelDuplicarCatalogoComponente">
		    </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlIngresarDuplicarCatalogoComponente" runat="server" CssClass="container">
                <div class="modal-dialog modal-lg">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title" id="myModalLabelDuplicarCatalogoComponente">
                                        Datos del Catalogo a Duplicar</h4>
                                    <asp:Button ID="imgbtnCancelarDuplicarCatalogoComponenteImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
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
                                                    <div class="col-lg-7 col-md-4 col-sm-4 mt-2">
                                                        <span class="fas fa-shipping-fast"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción:</label>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtDescripcionDuplicarCatalogoComponente" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="23"
                                                                style="text-transform :uppercase" placeholder="Descripción Catalogo"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvDescripcionDuplicarCatalogoComponente" runat="server" 
                                                                ControlToValidate="txtDescripcionDuplicarCatalogoComponente" EnableClientScript="False" 
                                                                ErrorMessage="Ingrese la descripción del componente / catálogo" Font-Size="10px" ForeColor="Red" 
                                                                ValidationGroup="vgrpValidarDuplicarCatalogoComponente">(*)</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-5 col-md-8 col-sm-8 mt-2">
                                                        <span class="fas fa-chalkboard-teacher"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción Abreviada:</label>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtDescripcionAbreviadaDuplicarCatalogoComponente" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="24"
                                                                style="text-transform : uppercase" placeholder="Descripción Abreviada Catalogo Principal"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvDescripcionAbreviadaDuplicarCatalogoComponente" runat="server" 
                                                                ControlToValidate="txtDescripcionAbreviadaDuplicarCatalogoComponente" EnableClientScript="False" 
                                                                ErrorMessage="Ingrese la descripción abreviada del componente / catálogo" Font-Size="10px" 
                                                                ForeColor="Red" ValidationGroup="vgrpValidarDuplicarCatalogoComponente">(*)</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarComponente" />
                                    <div class="col-md-4 text-end p-2">
                                        <asp:Button ID="btnAceptarDuplicarCatalogoComponente" runat="server" ValidationGroup="vgrpValidarComponente" OnClick="btnAceptarDuplicarCatalogoComponente_Click" OnClientClick="deshabilitar(this.id)"
                                            ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                        &nbsp;
                                        <asp:Button ID="btnCancelarDuplicarCatalogoComponente" runat="server" 
                                            ToolTip="Cancelar Registro" TabIndex ="73" Text="Cancelar" cssclass="btn btn-outline-google-plus btn-sm"/>
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

        //Esto es para dar el enfoque en una ventana modal
        function fnSetFocus(txtClientId) {
            document.getElementById("cuerpo_lblFocusObjeto").value = txtClientId;
        }
        setInterval("regresaFoco()", 1000);

        function regresaFoco() {
            if (document.getElementById("cuerpo_lblFocusObjeto").value == "ctl00$cuerpo$txtDescripcionDetalleCatalogo") {
                document.getElementById("cuerpo_txtDescripcionDetalleCatalogo").focus();
                document.getElementById("cuerpo_lblFocusObjeto").value = '';
            }
        }
    </script>
</asp:Content>
