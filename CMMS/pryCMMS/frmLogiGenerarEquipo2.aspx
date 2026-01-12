<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmLogiGenerarEquipo2.aspx.vb" Inherits="pryCMMS.frmLogiGenerarEquipo2" %>
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cuerpo" runat="server">
    <link href="Styles/CuadroDialogo.css" rel="stylesheet" type="text/css"/>
    <script src="Scripts/jsUpdateProgress.js" type="text/javascript"></script>
    <link href="Content/AutoCompletar.css" rel="stylesheet" type="text/css"/>
    <asp:UpdatePanel ID ="UpdatePanel1" runat="server">
        <ContentTemplate>
            <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
            </ajaxToolkit:ToolkitScriptManager>
            <asp:HiddenField ID="hfdOperacion" runat="server" />
            <asp:HiddenField ID="hfdFocusObjeto" runat="server" />
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
                    <div class="card-body">

                        				<div class="row bg-100 p-2">
					<div class="col-lg-2 col-sm-4">
                        <span class="fas fa-filter"></span><label class="text-black-50 col-form-label fs--2 m-1">Filtrar por:</label>
                        <div class="input-group">
						    <asp:DropDownList ID="cboFiltroEquipo" runat="server" CssClass="form-select form-select-sm js-choice" TabIndex="1">
							    <asp:ListItem>SELECCIONE EL FILTRO</asp:ListItem>
						    </asp:DropDownList>
					    </div>
					</div>
					<div class="col-lg-2 col-sm-4">
                        <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Buscar por:</label>
						<div class="input-group">
							<asp:TextBox ID="txtBuscarEquipo" runat="server" CssClass="form-control form-control-sm" TabIndex="2"
								style="text-transform :uppercase" placeholder="Ingrese Busqueda"></asp:TextBox>
							&nbsp;
							<asp:ImageButton ID="imgbtnBuscarEquipo" runat="server" Height="18px" TabIndex="3" CssClass="mt-1"
								ImageUrl="~/Imagenes/buscar.png" ToolTip="Buscar Registro" Width="21px" />
						</div>
					</div>
                    <div class="col-sm-4">
                        <div class="d-grid gap-2 d-md-flex justify-content-md-end">
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
              <div class="card mb-3">
                <div class="card-body">
                  <div class="row">
                    <div class="col-lg-12">
                      <h5 class="mb-1" data-anchor="data-anchor">Datos Principales</h5>
                    </div>
                  </div>
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
                            <asp:TextBox ID="txtIdEquipo" runat="server" CssClass="form-control form-control-sm" TabIndex="1"></asp:TextBox>
                            <asp:LinkButton ID="btnAdicionarEquipo" runat="server" CssClass="btn btn-primary btn-sm" Text="<span class='fas fa-binoculars'></span>" ToolTip="Buscar Equipo" />
                            <span class="small">(F2)&nbsp;</span>
                            <asp:RequiredFieldValidator ID="rfvIdEquipo" runat="server" 
                                ControlToValidate="txtIdEquipo" EnableClientScript="False" 
                                ErrorMessage="Ingrese el código del equipo" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
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
                                Enabled="False" TabIndex="7" style="text-transform :uppercase" placeholder="Descripción del equipo"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDescripcionEquipo" runat="server" 
                                ControlToValidate="txtDescripcionEquipo" EnableClientScript="False" 
                                ErrorMessage="Ingrese la descripción del equipo" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-4 mt-2">
                        <div class="mt-1"><asp:Label ID="lblIdentificadorComponente" runat="server" CssClass="text-black-50 col-form-label col-form-label-sm fs--2 fw-semi-bold" Text="¿Es un componente?:"></asp:Label></div>
                        <div class="input-group">
                            <div class="form-check form-switch">
                                <input class="form-check-input" id="fscIdentificadorComponente" type="checkbox" runat="server"/>
                            </div>
                        </div>
                        <%--<div class="input-group">
                            <asp:TextBox ID="txtFechaEntrega" runat="server" CssClass="form-control form-control-sm" autocomplete="off" 
                                Enabled="True" style="text-transform : uppercase" TabIndex="8"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFechaEntrega" runat="server" 
                                ControlToValidate="txtFechaEntrega" EnableClientScript="False" 
                                ErrorMessage="Ingrese la fecha de entrega" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                        </div>--%>
                    </div>
                  </div>


                  <div class="row">
                    <div class="col-lg-3 col-md-3 mt-2">
                        <span class="far fa-clone"></span><label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Tipo Activo:</label>
                        <div class="input-group">
                            <asp:DropDownList ID="cboTipoActivo" runat="server" AppendDataBoundItems="True" 
                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice" 
                                style="text-transform : uppercase" Width="170px" TabIndex="2">
                                <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                            </asp:DropDownList>
                            <asp:LinkButton ID="btnAdicionarTipoActivo" runat="server" Text="<span class='fas fa-plus-circle'></span>" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Tipo de Activo" TabIndex="31" />
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-6 mt-2">
                        <span class="far fa-clone"></span><label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Catalogo Principal:</label>
                        <div class="input-group">
                            <asp:DropDownList ID="cboCatalogo" runat="server" AppendDataBoundItems="True" 
                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice" 
                                style="text-transform : uppercase" Width="50px" TabIndex="3">
                                <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                            </asp:DropDownList>
                            <asp:LinkButton ID="btnAdicionarCatalogo" runat="server" Text="<span class='fas fa-plus-circle'></span>" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Catalogo Principal" TabIndex="31" />
                            <asp:RequiredFieldValidator ID="rfvCatalogo" runat="server" 
                                ControlToValidate="cboCatalogo" Display="Static" EnableClientScript="False" 
                                ErrorMessage="Ingrese el catalogo" Font-Size="10px" ForeColor="Red" 
                                InitialValue="SELECCIONE DATO" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-3 mt-2">
                        <span class="far fa-clone"></span><label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Sistema Funcional:</label>
                        <div class="input-group">
                            <asp:DropDownList ID="cboSistemaFuncional" runat="server" AppendDataBoundItems="True" 
                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice" 
                                style="text-transform : uppercase" Width="50px" TabIndex="3">
                                <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                            </asp:DropDownList>
                            <asp:LinkButton ID="btnAdicionarSistemaFuncional" runat="server" Text="<span class='fas fa-plus-circle'></span>" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Sistema Funcional" TabIndex="31" />
                            <asp:RequiredFieldValidator ID="rfvSistemaFuncional" runat="server" 
                                ControlToValidate="cboSistemaFuncional" Display="Static" EnableClientScript="False" 
                                ErrorMessage="Ingrese el sistema funcional" Font-Size="10px" ForeColor="Red" 
                                InitialValue="SELECCIONE DATO" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                        </div>                    
                    </div>
                    <div class="col-lg-3 col-md-6 mt-2">
                        <span class="far fa-clone"></span><label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Catalogo/Componente:</label>
                        <div class="input-group">
                            <asp:DropDownList ID="cboCatalogoComponente" runat="server" AppendDataBoundItems="True" 
                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice" 
                                style="text-transform : uppercase" Width="50px" TabIndex="3">
                                <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                            </asp:DropDownList>
                            <asp:LinkButton ID="btnAdicionarCatalogoComponente" runat="server" Text="<span class='fas fa-plus-circle'></span>" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Catalogo Principal" TabIndex="31" />
                            <asp:RequiredFieldValidator ID="rfvCatalogoComponente" runat="server" 
                                ControlToValidate="cboCatalogoComponente" Display="Static" EnableClientScript="False" 
                                ErrorMessage="Ingrese el catalogo/componente" Font-Size="10px" ForeColor="Red" 
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
                            <%--<asp:HiddenField ID="hfdIdUbicacionGeograficaEntregaCliente" runat="server" />--%>
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
                    <div class="col-lg-3 col-md-4 col-sm-5">
                        <asp:LinkButton ID="btnAdicionarComponente" runat="server" Text="<span class='fas fa-plus-circle'></span>Adicionar Componente" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Componente" TabIndex="31" />
                    </div>
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
													<%--<div class="card-header">Listado de Componentes del Catálogo</div>--%>
													<div class="card-header">
														<div class="row">
															<div class="col-11">
																Listado de Componentes del Catálogo
															</div>
															<div class="col-1">
																<asp:ImageButton ID="imgbtnEditarComponenteCatalogo" runat="server" Height="25px" onfocus="fnSetFocus('ctl00$cuerpo$txtIdInternoDetalleMaestroActivo')"
																	ImageUrl="~/Imagenes/Editar.jpg" ToolTip="Editar Registro" Width="25px" />
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
																Listado de Componentes Asignados al Activo Mayor
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
															<asp:GridView ID="grdComponenteMaestroActivo" runat="server" AutoGenerateColumns="False" TabIndex="21"
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
					
					
					
					
					
					
					
					
					
					
					
					
					
                      <asp:UpdatePanel ID="UpdatePanel6" runat="server">
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
                      </asp:UpdatePanel>
                    </div>
                  </div>
                </div>
              </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
