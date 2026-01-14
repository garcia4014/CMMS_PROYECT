<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmLogiGenerarEquipo3.aspx.vb" Inherits="pryCMMS.frmLogiGenerarEquipo3" %>
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
    <style type="text/css">
        #BarraHTML {
            /*background-color: #ffffff;*/
           /* background-color: #ffffff;*/
            /*padding: 10px;*/ /*1px;*/
            /*position: fixed;*/
            position: sticky;
            /*width: 100%;*/
            z-index: 100;
            top: 65px;
        }
        #BarraHTML ul{
           list-style-type: none;
        }
        #BarraHTML li{
           display: inline;
           text-align: center;
           margin: 0 0 0 0;
        }
        #BarraHTML li a {
           padding: 2px 7px 2px 7px;
           text-decoration: none;
        }
        #BarraHTML li a:hover{
           background-color: #333333;
           color: #ffffff;
        }
        #texto{
            padding: 60px 0 0 0;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function deshabilitar(boton) {
            document.getElementById(boton).style.visibility = 'hidden';
        }

        function HabilitaDeshabilita(check) {
            if (document.getElementById("cuerpo_fscIdentificadorComponente").checked == true) {
                document.getElementById("cuerpo_divSistemaFuncional").style.visibility = 'visible';
            }

            if (document.getElementById("cuerpo_fscIdentificadorComponente").checked == false) {
                document.getElementById("cuerpo_divSistemaFuncional").style.visibility = 'hidden';
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cuerpo" runat="server">
    <link href="Styles/CuadroDialogo.css" rel="stylesheet" type="text/css"/>
    <script src="Scripts/jsUpdateProgress.js" type="text/javascript"></script>
    <link href="Content/AutoCompletar.css" rel="stylesheet" type="text/css"/>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel ID ="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
            </ajaxToolkit:ToolkitScriptManager>--%>
            <asp:HiddenField ID="hfdOperacion" runat="server" />
            <asp:HiddenField ID="hfdFocusObjeto" runat="server" />
            <asp:HiddenField ID="hfdEstado" runat="server" />
            <asp:HiddenField ID="hfdOperacionDetalle" runat="server" />
            <asp:HiddenField ID="hfdOperacionDetalleAdicional" runat="server" />
            <%--<div id="BarraHTML" class="container-fluid">
				<div class="row bg-100 p-2">
					<div class="col-sm-3">
                        <span class="fas fa-filter"></span><label class="text-black-50 col-form-label fs--2 m-1">Filtrar por:</label>
                        <div class="input-group">
						    <asp:DropDownList ID="cboFiltroEquipo" runat="server" CssClass="form-select form-select-sm js-choice" TabIndex="1">
							    <asp:ListItem>SELECCIONE EL FILTRO</asp:ListItem>
						    </asp:DropDownList>
					    </div>
					</div>
					<div class="col-sm-3">
                        <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Buscar por:</label>
						<div class="input-group">
							<asp:TextBox ID="txtBuscarEquipo" runat="server" CssClass="form-control form-control-sm" TabIndex="2"
								style="text-transform :uppercase" placeholder="Ingrese Busqueda"></asp:TextBox>
							&nbsp;
							<asp:ImageButton ID="imgbtnBuscarEquipo" runat="server" Height="18px" TabIndex="3" CssClass="mt-1"
								ImageUrl="~/Imagenes/buscar.png" ToolTip="Buscar Registro" Width="21px" />
						</div>
					</div>
                    <div class="col-sm-6">
                        <div class="d-grid gap-2 d-md-flex justify-content-md-end">
							<asp:LinkButton ID="btnNuevo" runat="server" OnClick="btnNuevo_Click" CssClass="btn btn-success btn-sm ml-auto float-right" Text="<span class='fas fa-plus'></span> Nuevo" ToolTip="Buscar Equipo" />
							<asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-primary btn-sm ml-auto float-right" Text="<span class='fas fa-pencil-alt'></span> Editar" ToolTip="Buscar Equipo" />
							<asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-danger btn-sm ml-auto float-right"  Text="<span class='fas fa-trash-alt'></span> Eliminar" ToolTip="Buscar Equipo" />
                        </div>
                        <div class="dropdown font-sans-serif btn-reveal-trigger">
                          <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="kanbanColumn1" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                          <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="kanbanColumn1"><a class="dropdown-item" href="#!">Imprimir Tarjeta</a><a class="dropdown-item" href="#!">Imprimir Listado</a><a class="dropdown-item" href="#!">Imprimir por Cliente</a>
                            <div class="dropdown-divider"></div><a class="dropdown-item text-danger" href="#!">Eliminar</a>
                          </div>
                        </div>
                    </div>
                </div>
            </div>--%>
            <asp:Panel ID="pnlCabecera" runat="server">
                <div class="card mb-3">
                    <div class="card-header">
                      <div class="row flex-between-end">
                        <div class="col-auto align-self-center">
                            <div class="nav nav-pills gap-2" role="tablist">
                                <h5 class="mb-0" data-anchor="data-anchor">Listado de Equipos</h5>
                                <a class="btn btn-falcon-default btn-sm ml-auto float-right" href="../frmBienvenida.aspx" aria-expanded="true">
                                    <span class="fas fa-home"></span><span class="nav-link-text ps-1">Inicio</span>
                                </a>
                            </div>
                        </div>
                        <div class="col-auto ms-auto">
                          <div class="nav nav-pills gap-2" role="tablist">
							<asp:LinkButton ID="btnNuevo" runat="server" OnClick="btnNuevo_Click" CssClass="btn btn-success btn-sm ml-auto float-right" Text="<span class='fas fa-plus'></span> Nuevo" ToolTip="Buscar Equipo" />
							<asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-primary btn-sm ml-auto float-right" Text="<span class='fas fa-pencil-alt'></span> Editar" ToolTip="Buscar Equipo" />
							<asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-danger btn-sm ml-auto float-right"  Text="<span class='fas fa-trash-alt'></span> Eliminar" ToolTip="Buscar Equipo" />
                            <div class="dropdown font-sans-serif btn-reveal-trigger">
                              <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="kanbanColumn1" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                              <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="kanbanColumn1"><a class="dropdown-item" href="#!">Imprimir Tarjeta</a><a class="dropdown-item" href="#!">Imprimir Listado</a><a class="dropdown-item" href="#!">Imprimir por Cliente</a>
                                <div class="dropdown-divider"></div><a class="dropdown-item text-danger" href="#!">Eliminar</a>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                    <div class="card-body">
                        <div class="row bg-100 p-2">
					        <div class="col-lg-6 col-sm-6">
                                <span class="fas fa-filter"></span><label class="text-black-50 col-form-label fs--2 m-1">Filtrar por:</label>
                                <div class="input-group">
						            <asp:DropDownList ID="cboFiltroEquipo" runat="server" CssClass="form-select form-select-sm js-choice" TabIndex="1">
							            <asp:ListItem>SELECCIONE EL FILTRO</asp:ListItem>
						            </asp:DropDownList>
					            </div>
					        </div>
					        <div class="col-lg-6 col-sm-6">
                                <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Buscar por:</label>
						        <div class="input-group">
							        <asp:TextBox ID="txtBuscarEquipo" runat="server" CssClass="form-control form-control-sm" TabIndex="2"
								        style="text-transform :uppercase" placeholder="Ingrese Busqueda"></asp:TextBox>
							        &nbsp;
							        <asp:ImageButton ID="imgbtnBuscarEquipo" runat="server" Height="18px" TabIndex="3" CssClass="mt-1"
								        ImageUrl="~/Imagenes/buscar.png" ToolTip="Buscar Registro" Width="21px" />
						        </div>
					        </div>
                            <%--<div class="col-sm-4">
                                <div class="d-grid gap-2 d-md-flex justify-content-md-end">


                                </div>
                            </div>--%>
                        </div>
                        <div class="kanban-items-container scrollbar">
                            <div class="row">
                                <div class="col-md-12 mt-3">
                                    <%--<div class="table-responsive scrollbar">--%>
                                    <div class="table-responsive">
                                        <asp:GridView ID="grdLista" runat="server" AutoGenerateColumns="False" TabIndex="4"
                                            GridLines="None" CssClass="table table-responsive-sm table-hover small" AllowPaging="True" 
                                            EmptyDataText="No hay registros a visualizar" PageSize="14" >
                                            <PagerStyle CssClass="pgr" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <AlternatingRowStyle CssClass="alt" />
                                            <Columns>
                                                <asp:CommandField ShowSelectButton="True" HeaderStyle-CssClass="bg-200 text-900"/>
                                                <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900"/>
                                                <asp:BoundField DataField="IdTipoActivo" HeaderText="Código Tipo Activo" HeaderStyle-CssClass="bg-200 text-900"/>
                                                <asp:BoundField DataField="IdCatalogo" HeaderText="Código Catálogo" HeaderStyle-CssClass="bg-200 text-900"/>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-CssClass="bg-200 text-900"/>
                                                <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900"/>
                                                <asp:TemplateField HeaderStyle-CssClass="bg-200 text-900">
                                                    <ItemTemplate>
                                                        <div class="bootstrap-switch bootstrap-switch-on bootstrap bootstrap-switch-animate bootstrap-switch-mini ">
                                                            <div class="bootstrap-switch-container">
                                                                <asp:LinkButton ID="lnkEstadoOn" runat="server" CommandName="Activar" CommandArgument='<%# Eval("Codigo") %>' CssClass="bootstrap-switch-handle-on bootstrap-switch-primary" Visible='<%# If(Eval("Estado") = "True", "True", "False") %>'>Activado</asp:LinkButton>
                                                                <span class="bootstrap-switch-label">&nbsp;</span>
                                                                <asp:LinkButton ID="lnkEstadoOff" runat="server" CommandName="Desactivar" CommandArgument='<%# Eval("Codigo") %>' CssClass="bootstrap-switch-handle-off bootstrap-switch-danger" Visible='<%# If(Eval("Estado") = "True", "False", "True") %>'>Anulado</asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:BoundField DataField="Principal" HeaderText="Principal" HeaderStyle-CssClass="bg-200 text-900"/>
                                                <asp:TemplateField HeaderText="Principal" HeaderStyle-CssClass="bg-200 text-900">
                                                    <ItemTemplate>
                                                        <div class="bootstrap-switch bootstrap-switch-on bootstrap bootstrap-switch-animate bootstrap-switch-mini ">
                                                            <div class="bootstrap-switch-container">
                                                                <asp:LinkButton ID="lnkEstadoPrincipalOn" runat="server" CommandName="Si" CommandArgument='<%# Eval("Codigo") %>' CssClass="bootstrap-switch-handle-on bootstrap-switch-primary" Visible='<%# If(Eval("Principal") = "True", "True", "False") %>'>Si</asp:LinkButton>
                                                                <span class="bootstrap-switch-label">&nbsp;</span>
                                                                <asp:LinkButton ID="lnkEstadoPrincipalOff" runat="server" CommandName="No" CommandArgument='<%# Eval("Codigo") %>' CssClass="bootstrap-switch-handle-off bootstrap-switch-danger" Visible='<%# If(Eval("Principal") = "True", "False", "True") %>'>No</asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
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
<%--        </ContentTemplate>
    </asp:UpdatePanel>--%>


            <asp:Panel ID="pnlEquipo" runat="server">
              <div id="BarraHTML" class="container-fluid">
                <div class="row">
                    <div class="col-auto align-self-center mt-2 mb-2">
                        <div class="nav nav-pills gap-2" role="tablist">
                            <h5 class="mb-1" data-anchor="data-anchor">Detalle de Equipos</h5>
                            <asp:LinkButton ID="btnAtras" runat="server" CssClass="btn btn-falcon-default btn-sm ml-auto float-right" Text="<span class='fas fa-step-backward'></span> Atrás" ToolTip="Ir hacia atrás" />
                        </div>
                    </div>
                    <div class="col-auto ms-auto">
                        <div class="nav nav-pills gap-2" role="tablist">
						<asp:LinkButton ID="btnGuardar" runat="server" OnClick="btnNuevo_Click" CssClass="btn btn-success btn-sm ml-auto float-right" Text="<span class='fas fa-plus'></span> Nuevo" ToolTip="Buscar Equipo" />
						<%--<asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-primary btn-sm ml-auto float-right" Text="<span class='fas fa-pencil-alt'></span> Editar" ToolTip="Buscar Equipo" />
						<asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn btn-danger btn-sm ml-auto float-right"  Text="<span class='fas fa-trash-alt'></span> Eliminar" ToolTip="Buscar Equipo" />
                        <div class="dropdown font-sans-serif btn-reveal-trigger">
                            <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="kanbanColumn1" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                            <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="kanbanColumn1"><a class="dropdown-item" href="#!">Imprimir Tarjeta</a><a class="dropdown-item" href="#!">Imprimir Listado</a><a class="dropdown-item" href="#!">Imprimir por Cliente</a>
                            <div class="dropdown-divider"></div><a class="dropdown-item text-danger" href="#!">Eliminar</a>
                            </div>
                        </div>--%>
                        </div>
                    </div>
                </div>
              </div>
              <div class="card mb-3">
                <div class="card-body">
                  <%--<div class="row">
                    <div class="col-auto align-self-center">
                        <div class="nav nav-pills gap-2" role="tablist">
                            <h5 class="mb-1" data-anchor="data-anchor">Datos Principales</h5>
                            <asp:LinkButton ID="btnInicio" runat="server" CssClass="btn btn-falcon-default btn-sm ml-auto float-right" Text="<span class='fas fa-home'></span> Inicio" ToolTip="Ir a la página principal" />
                        </div>
                    </div>
                  </div>--%>
                  <div class="row mt-3">
                    <div class="col-12 mb-0 bg-100">
                      <h6 class="mt-2 mb-1">EQUIPO PRINCIPAL</h6>
                      <hr class="bg-300 m-0" />
                    </div>
                  </div>
                  <div class="row">
                    <div class="col-lg-3 col-md-4 mt-2">
                        <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Código Equipo:</label>
                        <div class="input-group">
                            <asp:TextBox ID="txtIdEquipo" runat="server" CssClass="form-control form-control-sm" TabIndex="1" placeholder="Id. del equipo"></asp:TextBox>
                            <%--<asp:LinkButton ID="btnAdicionarEquipo" runat="server" CssClass="btn btn-primary btn-sm" Text="<span class='fas fa-binoculars'></span>" ToolTip="Buscar Equipo" />
                            <span class="small">(F2)&nbsp;</span>
                            <asp:RequiredFieldValidator ID="rfvIdEquipo" runat="server" 
                                ControlToValidate="txtIdEquipo" EnableClientScript="False" 
                                ErrorMessage="Ingrese el código del equipo" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>--%>
                            <%--<input type=hidden runat=server id=hfdIdTipoDocumentoCliente/>
                            <asp:HiddenField ID="hfdIdAuxiliar" runat="server" />
                            <asp:HiddenField ID="hfdIdTipoPersonaCliente" runat="server" />
                            <asp:HiddenField ID="hfdIdTipoCliente" runat="server" />
                            <asp:HiddenField ID="hfdIdUbicacionGeograficaCliente" runat="server" />
                            <asp:HiddenField ID="hfdNroDocumentoCliente" runat="server" />
                            <asp:HiddenField ID="hfdDireccionFiscalCliente" runat="server" />
                            <asp:HiddenField ID="hfdTelefonoContactoCliente" runat="server" />--%>
                        </div>
                    </div>
                    <div class="col-lg-6 col-md-4 mt-2">
                        <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción:</label>
                        <div class="input-group">
                            <asp:TextBox ID="txtDescripcionEquipo" runat="server" CssClass="form-control form-control-sm" 
                                TabIndex="7" style="text-transform :uppercase" placeholder="Descripción del equipo"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDescripcionEquipo" runat="server" 
                                ControlToValidate="txtDescripcionEquipo" EnableClientScript="False" 
                                ErrorMessage="Ingrese la descripción del equipo" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-4 mt-2">
                        <div class="mt-1"><asp:Label ID="lblIdentificadorComponente" runat="server" CssClass="text-black-50 col-form-label col-form-label-sm fs--2 fw-semi-bold" Text="¿Es un componente?:"></asp:Label></div>
                        <div class="input-group">
                            <div class="form-check form-switch">
                                <input class="form-check-input" id="fscIdentificadorComponente" type="checkbox" runat="server" onclick="HabilitaDeshabilita(this)"/>
                            </div>
                        </div>
                    </div>
                  </div>
                  <div class="row">
                    <div class="col-lg-4 col-md-6 mt-2">
                        <span class="far fa-clone"></span><label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Tipo Activo:</label>
                        <div class="input-group">
                            <asp:DropDownList ID="cboTipoActivo" runat="server" AppendDataBoundItems="True" 
                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice" 
                                style="text-transform : uppercase" TabIndex="2">
                                <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                            </asp:DropDownList>
                            <%--<asp:LinkButton ID="btnAdicionarTipoActivo" runat="server" Text="<span class='fas fa-plus-circle'></span>" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Tipo de Activo" TabIndex="31" />--%>
                            <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                                <div class="dropdown font-sans-serif btn-reveal-trigger">
                                    <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesTipoActivo" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                                    <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesTipoActivo">
                                        <asp:LinkButton ID="lnkbtnNuevoTipoActivo" runat="server" ToolTip="Crear un tipo de activo" Text="<span class='fas fa-file me-2'></span>Nuevo" CssClass="nav-link nav-link-card-details" />
                                        <asp:LinkButton ID="lnkbtnEditarTipoActivo" runat="server" ToolTip="Editar un tipo de activo" Text="<span class='fas fa-file-signature me-2'></span>Editar" CssClass="nav-link nav-link-card-details" />
                                    </div>
                                </div>
                            </div>
                            <asp:LinkButton ID="lnk_mostrarPanelMantenimientoTipoActivo" runat="server"></asp:LinkButton>
                            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMantenimientoTipoActivo_ModalPopupExtender" runat="server" 
                                BackgroundCssClass="FondoAplicacion" DropShadow="False" DynamicServicePath="" Enabled="True" 
                                CancelControlID="btnCancelarMantenimientoTipoActivo" 
                                PopupControlID="updpnlMantenimientoTipoActivo" TargetControlID="lnk_mostrarPanelMantenimientoTipoActivo">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:UpdatePanel ID="updpnlMantenimientoTipoActivo" runat="server" EnableEventValidation="false">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlMantenimientoTipoActivo" runat="server" CssClass="container" DefaultButton="btnAceptarMantenimientoTipoActivo" >
                                        <div class="modal-dialog modal-lg">
                                            <div class="shadow rounded">
                                                <div class="modal-dialog-scrollable">
                                                    <div class="modal-content">
                                                        <div class="modal-header bg-light">
                                                            <h4 class="modal-title" id="myModalLabelProducto">Mantenimiento Tipo Activo</h4>
                                                            <asp:Button ID="imgbtnCancelarTipoActivoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                        </div>
                                                        <div class="modal-body">
                                                            <div class="row pt-2 pb-2">
										                        <div class="col-lg-3 col-md-4 col-sm-5">
											                        <%--<label class="text-black-50 col-form-label col-form-label-sm">IdTipoActivo:</label>--%>
                                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">IdTipoActivo:</label>
											                        <div class="input-group">
												                        <asp:TextBox ID="txtIdTipoActivoMantenimientoTipoActivo" runat="server" CssClass="form-control form-control-sm" TabIndex="10"
													                        style="text-transform :uppercase" autocomplete="off"></asp:TextBox>
											                        </div>
										                        </div>
										                        <div class="col-lg-9 col-md-12 col-sm-12">
											                       <%-- <label class="text-black-50 col-form-label col-form-label-sm">Descripción:</label>--%>
                                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción:</label>
											                        <div class="input-group">
												                        <asp:TextBox ID="txtDescripcionMantenimientoTipoActivo" runat="server" CssClass="form-control form-control-sm" TabIndex="11"
													                        style="text-transform :uppercase" placeholder="Descripción Tipo Activo" autocomplete="off"></asp:TextBox>
												                        <asp:RequiredFieldValidator ID="rfvDescripcionMantenimientoTipoActivo" runat="server" 
													                        ControlToValidate="txtDescripcionMantenimientoTipoActivo" EnableClientScript="False" 
													                        ErrorMessage="Ingrese la descripción" Font-Size="10px" ForeColor="Red" 
													                        ValidationGroup="vgrpValidarMantenimientoTipoActivo">(*)</asp:RequiredFieldValidator>
											                        </div>
										                        </div>
										                        <div class="col-lg-6 col-md-8 col-sm-12 pt-1">
											                        <%--<label class="text-black-50 col-form-label col-form-label-sm">Descripción Abreviada:</label>--%>
                                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción Abreviada:</label>
											                        <div class="input-group">
												                        <asp:TextBox ID="txtDescripcionAbreviadaMantenimientoTipoActivo" runat="server" CssClass="form-control form-control-sm" TabIndex="12"
													                        style="text-transform : uppercase" placeholder="Descripción Abreviada Tipo de Activo" autocomplete="off"></asp:TextBox>
											                        </div>
										                        </div>
									                        </div>
                                                        </div>
                                                        <div class="modal-footer">
                                                            <div class="container">
                                                                <div class="row">
                                                                    <asp:Label ID="lblMensajeMantenimientoTipoActivo" runat="server" CssClass="col-lg-6 col-md-5 col-form-label col-form-label-sm text-danger"></asp:Label>
                                                                    <div class="col-lg-6 col-md-7 text-end p-2">
                                                                        <asp:Button ID="btnAceptarMantenimientoTipoActivo" runat="server" ValidationGroup="vgrpValidarMantenimientoTipoActivo"
                                                                            ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                                        &nbsp;
                                                                        <asp:Button ID="btnCancelarMantenimientoTipoActivo" runat="server" 
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
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-6 mt-2">
                        <span class="far fa-clone"></span><label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Catalogo Principal:</label>
                        <div class="input-group">
                            <asp:DropDownList ID="cboCatalogo" runat="server" AppendDataBoundItems="True" 
                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice" 
                                style="text-transform : uppercase" TabIndex="3">
                                <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                            </asp:DropDownList>
                            <%--<asp:LinkButton ID="btnAdicionarCatalogo" runat="server" Text="<span class='fas fa-plus-circle'></span>" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Catalogo Principal" TabIndex="31" />--%>
                            <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                                <div class="dropdown font-sans-serif btn-reveal-trigger">
                                    <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesCatalogo" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                                    <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesCatalogo">
                                        <asp:LinkButton ID="lnkbtnNuevoCatalogo" runat="server" ToolTip="Crear un catalogo principal" Text="<span class='fas fa-file me-2'></span>Nuevo" CssClass="nav-link nav-link-card-details" />
                                        <asp:LinkButton ID="lnkbtnEditarCatalogo" runat="server" ToolTip="Editar un catalogo principal" Text="<span class='fas fa-file-signature me-2'></span>Editar" CssClass="nav-link nav-link-card-details" />
                                    </div>
                                </div>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvCatalogo" runat="server" 
                                ControlToValidate="cboCatalogo" Display="Static" EnableClientScript="False" 
                                ErrorMessage="Ingrese el catalogo" Font-Size="10px" ForeColor="Red" 
                                InitialValue="SELECCIONE DATO" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                            <asp:LinkButton ID="lnk_mostrarPanelMantenimientoCatalogo" runat="server"></asp:LinkButton>
                            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMantenimientoCatalogo_ModalPopupExtender" runat="server" 
                                BackgroundCssClass="FondoAplicacion" DropShadow="False" DynamicServicePath="" Enabled="True" 
                                CancelControlID="btnCancelarMantenimientoCatalogo" 
                                PopupControlID="pnlMantenimientoCatalogo" TargetControlID="lnk_mostrarPanelMantenimientoCatalogo">
                            </ajaxToolkit:ModalPopupExtender>
	                        <asp:UpdatePanel ID="updpnlMantenimientoCatalogo" runat="server" EnableEventValidation="false">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlMantenimientoCatalogo" runat="server" CssClass="container" DefaultButton="btnAceptarMantenimientoCatalogo" >
                                        <div class="modal-dialog modal-lg">
                                            <div class="shadow rounded">
                                                <div class="modal-dialog-scrollable">
                                                    <div class="modal-content">
                                                        <div class="modal-header bg-light">
                                                            <h4 class="modal-title" id="myModalLabelCatalogo">Mantenimiento Catalogo</h4>
                                                            <asp:Button ID="imgbtnCancelarCatalogoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                        </div>
                                                        <div class="modal-body">
									                        <div class="row pt-2 pb-2">
										                        <div class="col-lg-3 col-md-4 mt-2">
											                        <span class="fas fa-indent"></span><label class="text-black-50 col-form-label fs--2 m-1">Código Catalogo:</label>
											                        <div class="input-group">
												                        <asp:TextBox ID="txtIdCatalogoMantenimientoCatalogo" runat="server" CssClass="form-control form-control-sm" TabIndex="10"></asp:TextBox>
											                        </div>
										                        </div>
										                        <div class="col-lg-9 col-md-4 mt-2">
											                        <span class="fas fa-indent"></span><label class="text-black-50 col-form-label fs--2 m-1">Tipo Activo:</label>
											                        <div class="input-group">
												                        <asp:DropDownList ID="cboTipoActivoMantenimientoCatalogo" runat="server" AppendDataBoundItems="True" TabIndex="11"
													                        CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase">
													                        <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
												                        </asp:DropDownList>
												                        <ajaxToolkit:ListSearchExtender ID="cboTipoActivoMantenimientoCatalogo_ListSearchExtender" runat="server"
													                        Enabled="True" PromptText="Iniciar Búsqueda" TargetControlID="cboTipoActivoMantenimientoCatalogo">
												                        </ajaxToolkit:ListSearchExtender>
												                        <asp:RequiredFieldValidator ID="rfvTipoActivoMantenimientoCatalogo" runat="server" 
													                        ControlToValidate="cboTipoActivoMantenimientoCatalogo" EnableClientScript="False" InitialValue="SELECCIONE DATO"
													                        ErrorMessage="Ingrese el código de tipo de activo" Font-Size="10px" ForeColor="Red" 
													                        ValidationGroup="vgrpValidarMantenimientoCatalogo">(*)</asp:RequiredFieldValidator>
											                        </div>
										                        </div>
									                        </div>
									                        <div class="row">
										                        <div class="col-lg-6 col-md-4 mt-2">
											                        <span class="fas fa-object-group"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción:</label>
											                        <div class="input-group">
												                        <asp:TextBox ID="txtDescripcionMantenimientoCatalogo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="12"
													                        style="text-transform :uppercase" placeholder="Descripción Catalogo"></asp:TextBox>
												                        <asp:RequiredFieldValidator ID="rfvDescripcionMantenimientoCatalogo" runat="server" 
													                        ControlToValidate="txtDescripcionMantenimientoCatalogo" EnableClientScript="False" 
													                        ErrorMessage="Ingrese la descripción" Font-Size="10px" ForeColor="Red" 
													                        ValidationGroup="vgrpValidarMantenimientoCatalogo">(*)</asp:RequiredFieldValidator>
											                        </div>
										                        </div>
										                        <div class="col-lg-6 col-md-4 mt-2">
											                        <span class="fas fa-object-group"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción Abreviada:</label>
											                        <div class="input-group">
												                        <asp:TextBox ID="txtDescripcionAbreviadaMantenimientoCatalogo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="13"
													                        style="text-transform : uppercase" placeholder="Descripción Abreviada Catalogo"></asp:TextBox>
												                        <asp:RequiredFieldValidator ID="rfvDescripcionAbreviadaMantenimientoCatalogo" runat="server" 
													                        ControlToValidate="txtDescripcionAbreviadaMantenimientoCatalogo" EnableClientScript="False" 
													                        ErrorMessage="Ingrese la descripción abreviada" Font-Size="10px" 
													                        ForeColor="Red" ValidationGroup="vgrpValidarMantenimientoCatalogo">(*)</asp:RequiredFieldValidator>
											                        </div>
										                        </div>
									                        </div>
									                        <div class="row">
										                        <div class="col-lg-3 col-md-4 mt-2">
											                        <span class="fas fa-stopwatch"></span><label class="text-black-50 col-form-label fs--2 m-1">Vida Útil(Meses):</label>
											                        <div class="input-group">
												                        <asp:TextBox ID="txtVidaUtilMantenimientoCatalogo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="14"
													                        style="text-transform :uppercase" placeholder="Vida Util del Catalogo" ></asp:TextBox>
												                        <asp:RequiredFieldValidator ID="rfvVidaUtilMantenimientoCatalogo" runat="server" 
													                        ControlToValidate="txtVidaUtilMantenimientoCatalogo" EnableClientScript="False" 
													                        ErrorMessage="Ingrese la vida util en meses del catálogo" Font-Size="10px" ForeColor="Red"
													                        ValidationGroup="vgrpValidarMantenimientoCatalogo">(*)</asp:RequiredFieldValidator>
											                        </div>
										                        </div>
										                        <div class="col-lg-3 col-md-4 mt-2">
											                        <span class="fas fa-stream"></span><label class="text-black-50 col-form-label fs--2 m-1">Per.Garantía(Meses):</label>
											                        <div class="input-group">
												                        <asp:TextBox ID="txtPeriodoGarantiaMantenimientoCatalogo" runat="server" CssClass="form-control form-control-sm" TabIndex="16"
													                        style="text-transform :uppercase" placeholder="Periodo de Garantía" ></asp:TextBox>
											                        </div>
										                        </div>
										                        <div class="col-lg-3 col-md-4 mt-2">
											                        <span class="fas fa-stream"></span><label class="text-black-50 col-form-label fs--2 m-1">Per.Mín.Mnto.(Meses):</label>
											                        <div class="input-group">
												                        <asp:TextBox ID="txtPeriodoMinimoMantenimientoCatalogo" runat="server" CssClass="form-control form-control-sm" TabIndex="16"
													                        style="text-transform :uppercase" placeholder="Periodo Mínimo de Mantenimiento" ></asp:TextBox>
											                        </div>
										                        </div>
										                        <div class="col-lg-3 col-md-4 mt-2" id="divCuentaContableMantenimientoCatalogo" runat="server">
											                        <span class="fas fa-stream"></span><label class="text-black-50 col-form-label fs--2 m-1">Cuenta Contable:</label>
											                        <div class="input-group">
												                        <asp:TextBox ID="txtCuentaContableMantenimientoCatalogo" runat="server" CssClass="form-control form-control-sm" TabIndex="15"
													                        style="text-transform :uppercase" placeholder="Cuenta Contable" ></asp:TextBox>
											                        </div>
										                        </div>
										                        <div class="col-lg-3 col-md-4 mt-2" id="divCuentaContableLeasingMantenimientoCatalogo" runat="server">
											                        <span class="fas fa-stream"></span><label class="text-black-50 col-form-label fs--2 m-1">Cta. Ctble. Leasing:</label>
											                        <div class="input-group">
												                        <asp:TextBox ID="txtCuentaContableLeasingMantenimientoCatalogo" runat="server" CssClass="form-control form-control-sm" TabIndex="16"
													                        style="text-transform :uppercase" placeholder="Cuenta Contable Leasing" ></asp:TextBox>
											                        </div>
										                        </div>
									                        </div>
									                        <div class="row mt-3">
										                        <div class="col-12 mb-0 bg-100 d-none d-md-block">
										                          <h6 class="mt-2 mb-1">CARACTERISTICAS ASIGNADAS
											                        <asp:Button ID="btnAdicionarCaracteristicaMantenimientoCatalogo" runat="server" Text="[+] Características" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Características" ValidationGroup="vgrpValidarMantenimientoCatalogo"/>
										                          </h6>
										                          <hr class="bg-300 m-0" />
										                        </div>
										                        <div class="col-lg-3 col-md-4 mt-2">
										                        </div>
									                        </div>
									                        <div class="row justify-content-between mt-3"> 
										                        <div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
										                          <asp:UpdatePanel ID="updpnlDetalleCaracteristicaMantenimientoCatalogo" runat="server">
											                        <ContentTemplate>
												                        <asp:Panel ID="pnlDetalleCaracteristicaMantenimientoCatalogo" runat="server" CssClass="bg-light" >
													                        <div class="table-responsive scrollbar">
													                        <asp:GridView ID="grdDetalleCaracteristicaMantenimientoCatalogo" runat="server" AutoGenerateColumns="False" TabIndex="4" 
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
								                        </div>
                                                        <div class="modal-footer">
                                                            <div class="container">
                                                                <div class="row">
                                                                    <asp:Label ID="lblMensajeMantenimientoCatalogo" runat="server" CssClass="col-lg-6 col-md-5 col-form-label col-form-label-sm text-danger"></asp:Label>
                                                                    <div class="col-lg-6 col-md-7 text-end p-2">
                                                                        <asp:Button ID="btnAceptarMantenimientoCatalogo" runat="server" ValidationGroup="vgrpValidarMantenimientoCatalogo"
                                                                            ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                                        &nbsp;
                                                                        <asp:Button ID="btnCancelarMantenimientoCatalogo" runat="server" 
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
                            <asp:LinkButton ID="lnk_mostrarPanelCaracteristica" runat="server"></asp:LinkButton>
                            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelCaracteristica_ModalPopupExtender" runat="server" 
							    BackgroundCssClass="FondoAplicacion" DropShadow="False" DynamicServicePath="" Enabled="True" 
							    PopupControlID="pnlSeleccionarCaracteristica" TargetControlID="lnk_mostrarPanelCaracteristica">
							</ajaxToolkit:ModalPopupExtender>
                            <asp:UpdatePanel ID="updpnlSeleccionarCaracteristica" runat="server" EnableEventValidation="false">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlSeleccionarCaracteristica" runat="server" CssClass="container" DefaultButton="imgbtnBuscarCaracteristica" >
                                        <div class="modal-dialog modal-lg">
                                            <div class="shadow rounded">
                                                <div class="modal-dialog-scrollable">
                                                    <div class="modal-content">
                                                        <div class="modal-header bg-light">
                                                            <h4 class="modal-title" id="myModalLabelCaracteristica">
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
                                                                            <asp:TextBox ID="txtIdReferenciaSAPCaracteristica" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="17"
                                                                                style="text-transform :uppercase" placeholder="Código Referencia SAP" ></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-lg-4 col-md-4 mt-2">
                                                                        <span class="fas fa-phone-alt"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción Campo SAP:</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtDescripcionCampoSAPCaracteristica" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="17"
                                                                                style="text-transform :uppercase" placeholder="Campo SAP" ></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="container">
                                                                <div class="row">
                                                                    <asp:Label ID="lblMensajeCaracteristica" runat="server" CssClass="col-lg-6 col-md-5 col-form-label col-form-label-sm text-danger"></asp:Label>
                                                                    <div class="col-lg-6 col-md-7 text-end p-2">
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
                        </div>
                    </div>
                    <div id="divSistemaFuncional" runat="server" class="col-lg-4 col-md-12 mt-2">
                        <span class="far fa-clone"></span><label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Sistema Funcional:</label>
                        <div class="input-group">
                            <asp:DropDownList ID="cboSistemaFuncional" runat="server" AppendDataBoundItems="True" 
                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice" 
                                style="text-transform : uppercase" TabIndex="3">
                                <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                            </asp:DropDownList>
                            <asp:LinkButton ID="btnAdicionarSistemaFuncional" runat="server" Text="<span class='fas fa-plus-circle'></span>" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Sistema Funcional" TabIndex="31" />
                            <asp:RequiredFieldValidator ID="rfvSistemaFuncional" runat="server" 
                                ControlToValidate="cboSistemaFuncional" Display="Static" EnableClientScript="False" 
                                ErrorMessage="Ingrese el sistema funcional" Font-Size="10px" ForeColor="Red" 
                                InitialValue="SELECCIONE DATO" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                        </div>                    
                    </div>
                  </div>
                  <div class="row mt-3">
                    <div class="col-12 mb-0 bg-100">
                      <h6 class="mt-2 mb-1">CLIENTE</h6>
                      <hr class="bg-300 m-0" />
                    </div>
                  </div>
                  <div class="row">
                    <div class="col-lg-3 col-md-4 mt-2">
                        <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Código Cliente:</label>
                        <div class="input-group">
                            <asp:TextBox ID="txtIdCliente" runat="server" CssClass="form-control form-control-sm" TabIndex="6"></asp:TextBox><%--</td>--%>
                            <asp:LinkButton ID="btnAdicionarCliente" runat="server" CssClass="btn btn-primary btn-sm" Text="<span class='fas fa-binoculars'></span>" ToolTip="Crear Cliente" />
                            <span class="small">(F2)&nbsp;</span>
                            <asp:RequiredFieldValidator ID="rfvIdCliente" runat="server" 
                                ControlToValidate="txtIdCliente" EnableClientScript="False" 
                                ErrorMessage="Ingrese el código del cliente" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                            <input type=hidden runat=server id=hfdIdTipoDocumentoCliente/>
                            <asp:HiddenField ID="hfdIdAuxiliar" runat="server" />
                            <asp:HiddenField ID="hfdIdTipoPersonaCliente" runat="server" />
                            <asp:HiddenField ID="hfdIdTipoCliente" runat="server" />
                            <asp:HiddenField ID="hfdIdUbicacionGeograficaCliente" runat="server" />
                            <asp:HiddenField ID="hfdNroDocumentoCliente" runat="server" />
                            <asp:HiddenField ID="hfdDireccionFiscalCliente" runat="server" />
                            <asp:HiddenField ID="hfdTelefonoContactoCliente" runat="server" />
                        </div>
                    </div>
                    <div class="col-lg-6 col-md-4 mt-2">
                        <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Razón Social:</label>
                        <div class="input-group">
                            <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="form-control form-control-sm" 
                                Enabled="False" TabIndex="7" style="text-transform :uppercase" placeholder="Razón Social"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvRazonSocial" runat="server" 
                                ControlToValidate="txtRazonSocial" EnableClientScript="False" 
                                ErrorMessage="Ingrese la razón social" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-4 mt-2">
                        <span class="far fa-calendar-alt"></span><label class="text-black-50 col-form-label fs--2 m-1">Fecha Entrega:</label>
                        <div class="input-group">
                            <asp:TextBox ID="txtFechaEntrega" runat="server" CssClass="form-control form-control-sm" autocomplete="off" 
                                Enabled="True" style="text-transform : uppercase" TabIndex="8"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ControlToValidate="txtFechaEntrega" EnableClientScript="False" 
                                ErrorMessage="Ingrese la fecha de entrega" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                  </div>
                </div>
              </div>
            </asp:Panel>
            <asp:Panel ID="pnlComponentes" runat="server">
              <div class="card mb-3">
                <div class="card-body">
                  <div class="row justify-content-between">
                    <div class="col-lg-8 col-md-8 col-sm-7">
                      <h5 class="mb-1" data-anchor="data-anchor">Listado de Componentes</h5>
                    </div>
                    <asp:LinkButton ID="lnk_mostrarPanelDetalleCatalogo" runat="server"></asp:LinkButton>
		            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelDetalleCatalogo_ModalPopupExtender" 
			            runat="server" BackgroundCssClass="FondoAplicacion" 
			            CancelControlID="btnCancelarDetalleCatalogo"
			            DynamicServicePath="" Enabled="True" PopupControlID="pnlIngresarDetalleCatalogo" 
			            TargetControlID="lnk_mostrarPanelDetalleCatalogo">
		            </ajaxToolkit:ModalPopupExtender>
                    <asp:LinkButton ID="lnk_mostrarPanelMantenimientoSistemaFuncional" runat="server"></asp:LinkButton>
                    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMantenimientoSistemaFuncional_ModalPopupExtender" runat="server" 
                        BackgroundCssClass="FondoAplicacion" DropShadow="False" DynamicServicePath="" Enabled="True" 
                        CancelControlID="btnCancelarMantenimientoSistemaFuncional" 
                        PopupControlID="updpnlMantenimientoSistemaFuncional" TargetControlID="lnk_mostrarPanelMantenimientoSistemaFuncional">
                    </ajaxToolkit:ModalPopupExtender>
                    <asp:UpdatePanel ID="updpnlMantenimientoSistemaFuncional" runat="server" EnableEventValidation="false">
                        <ContentTemplate>
                            <asp:Panel ID="pnlMantenimientoSistemaFuncional" runat="server" CssClass="container" DefaultButton="btnAceptarMantenimientoSistemaFuncional" >
                                <div class="modal-dialog modal-lg">
                                    <div class="shadow rounded">
                                        <div class="modal-dialog-scrollable">
                                            <div class="modal-content">
                                                <div class="modal-header bg-light">
                                                    <h4 class="modal-title" id="myModalLabelSistemaFuncional">Mantenimiento Sistema Funcional</h4>
                                                    <asp:Button ID="imgbtnCancelarSistemaFuncionalImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                </div>
                                                <div class="modal-body">
                                                    <div class="row pt-2 pb-2">
										                <div class="col-lg-2 col-md-1 col-sm-5">
											                <%--<label class="text-black-50 col-form-label col-form-label-sm">IdSistemaFuncional:</label>--%>
                                                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Id:</label>
											                <div class="input-group">
												                <asp:TextBox ID="txtIdSistemaFuncionalMantenimientoSistemaFuncional" runat="server" CssClass="form-control form-control-sm" TabIndex="10"
													                style="text-transform :uppercase" autocomplete="off"></asp:TextBox>
											                </div>
										                </div>
										                <div class="col-lg-7 col-md-12 col-sm-12">
											                <%-- <label class="text-black-50 col-form-label col-form-label-sm">Descripción:</label>--%>
                                                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción:</label>
											                <div class="input-group">
												                <asp:TextBox ID="txtDescripcionMantenimientoSistemaFuncional" runat="server" CssClass="form-control form-control-sm" TabIndex="11"
													                style="text-transform :uppercase" placeholder="Descripción Sistema Funcional" autocomplete="off"></asp:TextBox>
												                <asp:RequiredFieldValidator ID="rfvDescripcionMantenimientoSistemaFuncional" runat="server" 
													                ControlToValidate="txtDescripcionMantenimientoSistemaFuncional" EnableClientScript="False" 
													                ErrorMessage="Ingrese la descripción" Font-Size="10px" ForeColor="Red" 
													                ValidationGroup="vgrpValidarMantenimientoSistemaFuncional">(*)</asp:RequiredFieldValidator>
											                </div>
										                </div>
										                <div class="col-lg-3 col-md-8 col-sm-12">
											                <%--<label class="text-black-50 col-form-label col-form-label-sm">Descripción Abreviada:</label>--%>
                                                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción Abreviada:</label>
											                <div class="input-group">
												                <asp:TextBox ID="txtDescripcionAbreviadaMantenimientoSistemaFuncional" runat="server" CssClass="form-control form-control-sm" TabIndex="12"
													                style="text-transform : uppercase" placeholder="Descripción Abreviada Sistema Funcional" autocomplete="off"></asp:TextBox>
											                </div>
										                </div>
									                </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <div class="container">
                                                        <div class="row">
                                                            <asp:Label ID="lblMensajeMantenimientoSistemaFuncional" runat="server" CssClass="col-lg-6 col-md-5 col-form-label col-form-label-sm text-danger"></asp:Label>
                                                            <div class="col-lg-6 col-md-7 text-end p-2">
                                                                <asp:Button ID="btnAceptarMantenimientoSistemaFuncional" runat="server" ValidationGroup="vgrpValidarMantenimientoSistemaFuncional"
                                                                    ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                                &nbsp;
                                                                <asp:Button ID="btnCancelarMantenimientoSistemaFuncional" runat="server" 
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
                    <asp:UpdatePanel ID="updpnlIngresarDetalleCatalogo" runat="server" EnableEventValidation="false">
                        <ContentTemplate>
                            <asp:Panel ID="pnlIngresarDetalleCatalogo" runat="server" CssClass="container">
                                <div class="modal-dialog modal-xl">
                                    <div class="shadow rounded">
                                        <div class="modal-dialog-scrollable">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h4 class="modal-title" id="myModalLabelDetalleCatalogo">
                                                        Mantenimiento del Componente</h4>
                                                    <asp:Button ID="Button1" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                </div>
                                                <div class="modal-body">
                                                    <div class="form-group">
                                                        <div class="row mt-1">
                                                            <div class="col-lg-7 col-sm-12">
                                                                <div class="row mt-1">
                                                                    <div class="col-12 mb-0 bg-100 d-none d-md-block">
                                                                        <h6 class="mt-2 mb-1">DATOS PRINCIPALES</h6>
                                                                        <hr class="bg-300 m-0" />
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-lg-3 col-md-4 mt-2">
                                                                        <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Id.Componente:</label>
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
                                                                            <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                                                                                <div class="dropdown font-sans-serif btn-reveal-trigger">
                                                                                    <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesTipoActivoDetalleCatalogo" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                                                                                    <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesTipoActivoDetalleCatalogo">
                                                                                        <asp:LinkButton ID="lnkbtnNuevoSistemaFuncionalDetalleCatalogo" runat="server" ToolTip="Crear un componente" Text="<span class='fas fa-file me-2'></span>Nuevo" CssClass="nav-link nav-link-card-details" />
                                                                                        <asp:LinkButton ID="lnkbtnEditarSistemaFuncionalDetalleCatalogo" runat="server" ToolTip="Editar un componente" Text="<span class='fas fa-file-signature me-2'></span>Editar" CssClass="nav-link nav-link-card-details" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
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
                                                                    <div class="col-lg-6 col-md-4 col-sm-4 mt-2">
                                                                        <span class="fas fa-shipping-fast"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción:</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtDescripcionDetalleCatalogo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="23"
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
                                                                            <asp:TextBox ID="txtDescripcionAbreviadaDetalleCatalogo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="24"
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
                                                                        <span class="fas fa-shipping-fast"></span><label class="text-black-50 col-form-label fs--2 m-1">Vida Útil(Mes):</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtVidaUtilDetalleCatalogo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="25"
                                                                                style="text-transform :uppercase" placeholder="Vita Útil Componente"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-lg-3 col-md-4 col-sm-4 mt-2">
                                                                        <span class="fas fa-chalkboard-teacher"></span><label class="text-black-50 col-form-label fs--2 m-1">Per.Garantía(Mes):</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtPeriodoGarantiaDetalleCatalogo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="26"
                                                                                style="text-transform : uppercase" placeholder="Periodo de Garantía del Componente"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                        <div class="col-lg-3 col-md-4 col-sm-4 mt-2">
                                                                        <span class="fas fa-chalkboard-teacher"></span><label class="text-black-50 col-form-label fs--2 m-1">Per.Min.Nant.(Mes):</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtPeriodoMinimoDetalleCatalogo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="27"
                                                                                style="text-transform : uppercase" placeholder="Periodo Mínimo de Mantenimiento del Componente"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-lg-3 col-md-4 col-sm-4 mt-2">
                                                                        <span class="fas fa-chalkboard-teacher"></span><label class="text-black-50 col-form-label fs--2 m-1">Cta.Contable:</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtCuentaContableDetalleCatalogo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="26"
                                                                                style="text-transform : uppercase" placeholder="Cta. Cble. del Componente"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                        <div class="col-lg-3 col-md-4 col-sm-4 mt-2">
                                                                        <span class="fas fa-chalkboard-teacher"></span><label class="text-black-50 col-form-label fs--2 m-1">Cta.Ctbl.Leasing:</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtCuentaContableLeasingDetalleCatalogo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="27"
                                                                                style="text-transform : uppercase" placeholder="Cta. Cble. Leasing Componente"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-lg-5 col-sm-12 bg-200">
                                                                <div class="row mt-1">
                                                                    <div class="col-12 mb-0 d-none d-md-block">
                                                                        <h6 class="mt-2 mb-1">CARACTERISTICAS ASIGNADAS</h6>
                                                                        <hr class="bg-300 m-0" />
                                                                    </div>
                                                                </div>
                                                                <script type="text/javascript">
                                                                    function CaracteristicaSeleccionada(source, eventArgs) {
                                                                        document.getElementById("cuerpo_hfdIdCaracteristicaDetalleCatalogo").value = eventArgs.get_value();
                                                                    }
                                                                    function PopupShown(sender, args) {
                                                                        sender._popupBehavior._element.style.zIndex = 99999999;
                                                                    }
                                                                </script>

                                                                <div id="divFiltrarCaracteristicaDetalleCatalogo" runat="server" class="row justify-content-between">
                                                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                                                        <asp:UpdatePanel ID="updpnlGeneral" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                                                            <ContentTemplate>
                                                                            <span class="fas fa-link"></span><label class="text-black-50 col-form-label fs--2 m-1">Filtrar Caracteristica:</label>
                                                                            <div class="input-group">
                                                                                <asp:TextBox ID="txtBuscarCaracteristicaDetalleCatalogo" runat="server" placeholder="Buscar por caracteristica"
                                                                                    class="form-control form-control-sm" TabIndex="1"></asp:TextBox>
                                                                                <ajaxToolkit:AutoCompleteExtender
                                                                                    runat="server" 
                                                                                    ID="txtBuscarCaracteristicaDetalleCatalogo_AutoCompleteExtender" 
                                                                                    TargetControlID="txtBuscarCaracteristicaDetalleCatalogo"
                                                                                    CompletionInterval="10" 
                                                                                    Enabled="True" MinimumPrefixLength="1" 
                                                                                    OnClientShown="PopupShown"
                                                                                    ServiceMethod="GetCompletionListCaracteristica"
                                                                                    ServicePath="~/wsPrueba.asmx"
                                                                                    CompletionSetCount="20"
                                                                                    DelimiterCharacters=";, :"
                                                                                    ShowOnlyCurrentWordInCompletionListItem="true"
                                                                                    UseContextKey="true" >
                                                                                </ajaxToolkit:AutoCompleteExtender>
                                                                                <asp:Button ID="btnAdicionarCaracteristicaDetalleCatalogo" runat="server" Text="Agregar" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Producto" TabIndex="31"/>
                                                                                <input ID="hfdIdCaracteristicaDetalleCatalogo" runat="server" type="hidden" />
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
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <asp:Label ID="lblMensajeDetalleCatalogo" runat="server" CssClass="col-form-label col-form-label-sm text-danger"></asp:Label>
                                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" role="alert" CssClass="alert alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarComponente" />
                                                    <div class="col-md-4 text-end p-2">
                                                        <asp:Button ID="btnAceptarDetalleCatalogo" runat="server" ValidationGroup="vgrpValidarComponente" OnClick="btnAceptarDetalleCatalogo_Click"
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
                  <div class="row justify-content-between mt-3"> 
                    <div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
						<asp:UpdatePanel ID="UpdPnlComponentes" runat="server">
							<ContentTemplate>
								<asp:Panel ID="pnlSeleccionarElemento" runat="server">
									<div class="form-group mt-2">
										<div class="row">
											<div class="col-md-6">
												<div class="card mb-3">
													<div class="card-header">
														<div class="row">
															<div class="col-8">
																<h6 class="modal-title">Listado de Componentes del Catálogo</h6>
															</div>
															<div class="col-4">
                                                                <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                                                                    <div class="dropdown font-sans-serif btn-reveal-trigger">
                                                                      <span class="fas fa-cog"></span><label class="text-black-50 col-form-label fs--2 m-1">Opciones:</label>
                                                                      <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesComponentesCatalogo" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                                                                      <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesComponentesCatalogo">
                                                                          <asp:LinkButton ID="lnkbtnNuevoComponente" runat="server" ToolTip="Crear un componente" Text="<span class='fas fa-file me-2'></span>Nuevo" CssClass="nav-link nav-link-card-details" />
                                                                          <asp:LinkButton ID="lnkbtnEditarComponente" runat="server" ToolTip="Editar un componente" Text="<span class='fas fa-file-signature me-2'></span>Editar" CssClass="nav-link nav-link-card-details" />
                                                                        <div class="dropdown-divider"></div>
                                                                          <asp:LinkButton ID="lnkbtnEliminarComponente" runat="server" ToolTip="Eliminar un componente" Text="<span class='fas fa-file-excel me-2'></span>Eliminar" CssClass="nav-link nav-link-card-details text-danger" />
                                                                      </div>
                                                                    </div>
                                                                </div>
															</div>
														</div>
													</div>
													<div class="card-body">
														<div id="divGridComponenteCatalogo" style="overflow:auto; height:220px; align:left;">
															<asp:GridView ID="grdComponenteCatalogo" runat="server" AutoGenerateColumns="False" TabIndex="20"
																GridLines="None" CssClass="table table-responsive-sm table-striped table-hover small" AllowPaging="True" 
																EmptyDataText="No hay registros a visualizar" PageSize="14" >
																<PagerStyle CssClass="pgr" />
																<SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
																<AlternatingRowStyle CssClass="alt" />
																<Columns>
																	<asp:CommandField ShowSelectButton="True" />
																	<asp:BoundField DataField="Codigo" HeaderText="Código" />
																	<asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
																	<asp:BoundField DataField="IdTipoActivo" HeaderText="Tipo Activo" />
																	<asp:BoundField DataField="IdSistemaFuncional" HeaderText="Sistema Funcional" />
																	<asp:BoundField DataField="DescripcionAbreviada" HeaderText="Descripción Abreviada" />
																</Columns>
																<HeaderStyle CssClass="thead-dark" />
															</asp:GridView>
														</div>
													</div>
												</div>
											</div>
											<div class="col-md-6">
												<div class="card mb-3">
													<div class="card-header">
														<div class="row">
															<div class="col-10">
                                                                <h6 class="modal-title">Listado de Componentes Asignados al Equipo Principal</h6>
															</div>
															<div class="col-1">
																<asp:ImageButton ID="imgbtnEliminarDetalleMaestroActivo" runat="server" Height="25px" 
																						ImageUrl="~/Imagenes/Remover.jpg" ToolTip="Nuevo Registro" Width="25px" />
															</div>
															<div class="col-1">
																<asp:ImageButton ID="imgbtnEditarDetalleMaestroActivo" runat="server" Height="25px" onfocus="fnSetFocus('ctl00$cuerpo$txtIdInternoDetalleMaestroActivo')"
																						ImageUrl="~/Imagenes/Editar.jpg" ToolTip="Editar Registro" Width="25px" />
															</div>
															<asp:HiddenField ID="hfdIdAlmacen" runat="server"></asp:HiddenField>
															<asp:HiddenField ID="hfdIdTipoAlmacen" runat="server"></asp:HiddenField>
														</div>
													</div>
													<div class="card-body">
														<div id="divGridComponenteMaestroActivo" style="overflow:auto; height:220px; align:left;">
															<asp:GridView ID="grdComponenteEquipo" runat="server" AutoGenerateColumns="False" TabIndex="21"
																GridLines="None" CssClass="table table-responsive-sm table-striped table-hover small" AllowPaging="True" 
																EmptyDataText="No hay registros a visualizar" PageSize="14" >
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
																	<asp:BoundField DataField="IdCuentaContable" HeaderText="Cuenta Contable Asignada" />
																	<asp:BoundField DataField="EstadoLeasing" HeaderText="Leasing" />
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
							</ContentTemplate>
						</asp:UpdatePanel>
                        <%--<asp:UpdatePanel ID="UpdPnlDetalleComponente" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlDetalle" runat="server" CssClass="bg-light">
                                <div class="table-responsive scrollbar">
                                    <asp:GridView ID="grdDetalle" runat="server" AutoGenerateColumns="False" TabIndex="4" 
                                        GridLines="None" CssClass="table table-striped table-hover small overflow-hidden" AllowPaging="True" 
                                        EmptyDataText="No hay registros a visualizar" PageSize="10" >
                                        <PagerStyle CssClass="pgr" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <AlternatingRowStyle CssClass="alt" />
                                        <Columns>
                                            <asp:CommandField ShowDeleteButton="True" HeaderStyle-Width="70px" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRowDetalle" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Item" HeaderText="#" ControlStyle-Width="25px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-Width="40px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:TemplateField HeaderText="Cantidad" HeaderStyle-Width="60px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCantidadDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("Cantidad") %>' CommandName="ButtonField" AutoPostBack="True" ></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        
                                            <asp:BoundField DataField="UnidMedidaAbrev" HeaderText="U. Med." HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:TemplateField HeaderText="Descripcion Producto" ControlStyle-Width="350px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescripcionDetalle" runat="server" CssClass="mgrid_small" Text='<%# Eval("Descripcion") %>'></asp:Label>
                                                    <br />
                                                    <asp:Label ID="lblTipoAfectacionDetalle" runat="server" CssClass="mgrid_tipo_afectacion" Text='<%# Eval("DescripcionTipoAfectacion") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="TMoneda" HeaderText="T. Mon." HeaderStyle-Width="40px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="PrecioOrigen" DataFormatString="{0:F}" HeaderText="Prec. Ori." HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="PrecioUnitario" DataFormatString="{0:F}" HeaderText="Prec. Unit." HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="PrecioUnitarioTotal" DataFormatString="{0:F10}" HeaderText="Prec. U.Tot." HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="ValorVenta" DataFormatString="{0:F}" HeaderText="Val. Vta." HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="ISC" DataFormatString="{0:F}" HeaderText="ISC" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="IGV" DataFormatString="{0:F}" HeaderText="IGV" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="DescuentoVenta" DataFormatString="{0:F}" HeaderText="Desc. Vta." HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="Percepcion" DataFormatString="{0:F}" HeaderText="Percepción" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="Total" DataFormatString="{0:F}" HeaderText="Total" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="TamanoPeso" HeaderText="Tamaño/Peso" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="Cantidad1" DataFormatString="{0:F}" HeaderText="Cantidad 1" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="IdUnidMedida1" HeaderText="U. Med. 1" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="Cantidad2" DataFormatString="{0:F}" HeaderText="Cantidad 2" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="IdUnidMedida2" HeaderText="U. Med. 2" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="CantidadTotalUnidMedida" DataFormatString="{0:F}" HeaderText="Cant.Tot." HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="IdUnidMedida" HeaderText="U. Med." HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="IdTipoAfectacion" HeaderText="Tip. Afec." HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="ValorVentaUnitarioTotal" HeaderText="Val. Vta. Unit." HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="PrecioUnitarioSugerido" HeaderText="Prec. Unit." HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="IdOnerosidad" HeaderText="Id. Onerosidad" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="IdSistemaISC" HeaderText="Sist. ISC." HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="PorcentajePercepcion" HeaderText="Porc. Percepción" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="PrecioOrigen" HeaderText="Prec. Ori." HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="PrecioUnitario" HeaderText="Prec. Unit." HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="PrecioUnitarioTotal" HeaderText="Prec. U.Tot." HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="ValorVenta" HeaderText="Val. Vta." HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="ISC" HeaderText="ISC" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="IGV" HeaderText="IGV" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="DescuentoVenta" HeaderText="Desc. Vta." HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="Percepcion" HeaderText="Percepción" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="Total" HeaderText="Total" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="PorcentajeISC" HeaderText="Porcentaje ISC" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="EstadoVariante" HeaderText="Estado Variante" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="CodigoVariante" HeaderText="Variante" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="DescripcionVariante" HeaderText="Descripción Variante" HeaderStyle-Width="140px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="IdUnidadMedidaOrigen" HeaderText="U. Med. Origen" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="Observacion" HeaderText="Observacion" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                            <asp:BoundField DataField="IdTipoExistencia" HeaderText="Tipo Existencia" HeaderStyle-Width="70px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                        </Columns>
                                        <HeaderStyle CssClass="thead-dark" />
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </div>
                  </div>
                </div>
              </div>
            </asp:Panel>
            <div class="position-fixed bottom-0 end-0 p-3">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidar" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
