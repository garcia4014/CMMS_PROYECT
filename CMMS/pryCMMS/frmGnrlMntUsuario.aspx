<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmGnrlMntUsuario.aspx.vb" Inherits="pryCMMS.frmGnrlMntUsuario" %>
<%@ Register assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

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
                    <li class="nav-item"><a class="nav-link active" href="#tab1" aria-controls="tab1" role="tab" data-toggle="tab">Listado de Usuario</a></li>
                    <li class="nav-item"><a class="nav-link" href="#tab2" aria-controls="tab2" role="tab" data-toggle="tab">Datos Generales</a></li>
                </ul>
                <!-- Final Tabs de Navegación -->
                <!-- Inicio Paneles Tab -->
                <div class="tab-content" style="padding-top: 10px">
                    <div role="tabpanel" class="tab-pane active" id="tab1">
                        <asp:UpdatePanel ID="updpnlTab1" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="hfdOperacion" runat="server" />
                                <asp:HiddenField ID="hfdEstado" runat="server" />
                                <asp:Panel ID="pnlListado" runat="server" CssClass="container"
                                    DefaultButton="imgbtnBuscarUsuario">
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
                                                <asp:ImageButton ID="imgbtnBuscarUsuario" runat="server" Height="18px" TabIndex="3" CssClass="mt-1"
                                                    ImageUrl="~/Imagenes/buscar.png" ToolTip="Buscar Registro" Width="21px" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12 mt-3">
                                            <div class="table-responsive scrollbar">
                                                <asp:GridView ID="grdLista" runat="server" AutoGenerateColumns="False" TabIndex="4"
                                                    GridLines="None" CssClass="table table-responsive-sm table-hover small" AllowPaging="True" 
                                                    EmptyDataText="No hay registros a visualizar" PageSize="14" OnRowCommand="grdLista_RowCommand" >
                                                    <PagerStyle CssClass="pgr" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:CommandField ShowSelectButton="True" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Login" HeaderText="Login" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Apellido_Paterno" HeaderText="Apellido Paterno" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Apellido_Materno" HeaderText="Apellido Materno" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Nombres" HeaderText="Nombres" HeaderStyle-CssClass="bg-200 text-900"/>
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
                                        <h3 class="h5">Mantenimiento de Usuarios</h3>
                                    </div>
                                    <div class="mt-2">
                                        <div class="row">
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">IdUsuario:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtIdUsuario" runat="server" CssClass="form-control form-control-sm" TabIndex="10"
                                                        style="text-transform :uppercase" Enabled="False"></asp:TextBox>
                                                </div>
                                            </div>
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Login:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtLogin" runat="server" CssClass="form-control form-control-sm" TabIndex="11"
                                                        style="text-transform :uppercase" ></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvLogin" runat="server" 
                                                        ControlToValidate="txtLogin" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese el login" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Apellido Paterno:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtApellidoPaterno" runat="server" CssClass="form-control form-control-sm" TabIndex="12"
                                                        style="text-transform :uppercase" ></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvApellidoPaterno" runat="server" 
                                                        ControlToValidate="txtApellidoPaterno" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese el apellido paterno" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Apellido Materno:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtApellidoMaterno" runat="server" CssClass="form-control form-control-sm" TabIndex="13"
                                                        style="text-transform :uppercase"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvApellidoMaterno" runat="server" 
                                                        ControlToValidate="txtApellidoMaterno" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese el apellido materno" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Nombres:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtNombres" runat="server" CssClass="form-control form-control-sm" TabIndex="14"
                                                        style="text-transform :uppercase"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvNombres" runat="server" 
                                                        ControlToValidate="txtNombres" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese el nombre" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">DNI:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtDNI" runat="server" CssClass="form-control form-control-sm" TabIndex="15"
                                                        style="text-transform :uppercase"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row pt-1">
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Password:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control form-control-sm" TabIndex="16"
                                                        style="text-transform :uppercase" TextMode="Password"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                                                        ControlToValidate="txtPassword" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese el password" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="rcvPassword" runat="server" 
                                                        ControlToCompare="txtPassword" ControlToValidate="txtValidarContrasena" 
                                                        ErrorMessage="No coincide la contraseña " Font-Size="10px" ForeColor="Red" 
                                                        ValidationGroup="vgrpValidar"></asp:CompareValidator>
                                                </div>
                                            </div>
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Repetir Password:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtValidarContrasena" runat="server" CssClass="form-control form-control-sm" TabIndex="17"
                                                        style="text-transform :uppercase" TextMode="Password"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:UpdatePanel ID="UpdPnlUbicacionGeografica" runat="server">
                                        <ContentTemplate>
                                            <div class="row pt-1">
                                                <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">País:</label>
                                                <div class="col-md-4 pt-md-2 pt-sm-1">
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboPais" runat="server" AppendDataBoundItems="True" TabIndex="18" 
                                                            CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase" 
                                                            AutoPostBack="True">
                                                            <asp:ListItem>SELECCIONE EL PAIS</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvPais" runat="server" 
                                                            ControlToValidate="cboPais" EnableClientScript="False" 
                                                            ErrorMessage="Ingrese el País" Font-Size="10px" ForeColor="Red"
                                                            ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Departamento:</label>
                                                <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:DropDownList ID="cboDepartamento" runat="server" AppendDataBoundItems="True" 
                                                        CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase" TabIndex="19"
                                                        AutoPostBack="True">
                                                        <asp:ListItem>SELECCIONE EL DEPARTAMENTO</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvDepartamento" runat="server" 
                                                        ControlToValidate="cboDepartamento" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese el Departamento" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            </div>
                                            <div class="row"">
                                                <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Provincia:</label>
                                                <div class="col-md-4 pt-md-2 pt-sm-1">
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboProvincia" runat="server" AppendDataBoundItems="True" TabIndex="20" 
                                                            CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase" 
                                                            AutoPostBack="True">
                                                            <asp:ListItem>SELECCIONE LA PROVINCIA</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvProvincia" runat="server" 
                                                            ControlToValidate="cboProvincia" EnableClientScript="False" 
                                                            ErrorMessage="Ingrese la Provincia" Font-Size="10px" ForeColor="Red"
                                                            ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Distrito:</label>
                                                <div class="col-md-4 pt-md-2 pt-sm-1">
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboDistrito" runat="server" AppendDataBoundItems="True" 
                                                            CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase" TabIndex="21"
                                                            AutoPostBack="False">
                                                            <asp:ListItem>SELECCIONE EL DISTRITO</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvDistrito" runat="server" 
                                                            ControlToValidate="cboDistrito" EnableClientScript="False" 
                                                            ErrorMessage="Ingrese el Distrito" Font-Size="10px" ForeColor="Red"
                                                            ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="row">
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Cargo:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtCargo" runat="server" CssClass="form-control form-control-sm" TabIndex="22"
                                                        style="text-transform :uppercase"></asp:TextBox>
                                                </div>
                                            </div>
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Permisos:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="form-check-inline col-12">
                                                    <div class="row">
                                                        <div class="col-auto"><asp:CheckBox ID="chkLectura" runat="server" CssClass="form-check" Text="Lectura" TabIndex="23" /></div>
                                                        <div class="col-auto"><asp:CheckBox ID="chkEscritura" runat="server" CssClass="form-check" Text="Escritura" TabIndex="24" /></div>
                                                        <div class="col-auto"><asp:CheckBox ID="chkEjecucion" runat="server" CssClass="form-check" Text="Ejecución" TabIndex="25" /></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Unidad de Trabajo:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtUnidadTrabajo" runat="server" CssClass="form-control form-control-sm" TabIndex="23"
                                                        style="text-transform :uppercase" ToolTip="Asigne la Unidad de Trabajo para poder Filtrarlo en la Orden de Servicio o Fabricación"></asp:TextBox>
                                                </div>
                                            </div>
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Contrato asignado:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:DropDownList ID="cboContrato" runat="server" AppendDataBoundItems="True" 
                                                        CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase" TabIndex="19"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvContrato" runat="server" 
                                                        ControlToValidate="cboContrato" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese el Contrato" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:UpdatePanel ID="UpdPnlAccesoGeneral" runat="server">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlSeleccionarAccesoGeneral" runat="server">
                                                    <div class="form-group mt-2">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="card mb-3">
                                                                    <div class="card-header bg-dark text-white">
                                                                        <div class="row justify-content-between">
                                                                            <div class="col-lg-8 col-md-8 col-sm-7">
                                                                                <h3 class="h5 text-white input-group">Acceso General</h3>
                                                                            </div>
                                                                            <div class="col-lg-3 col-md-4 col-sm-5">
                                                                                <asp:Button ID="btnAdicionarUsuarioAcceso" runat="server" Text="[+] Agregar Accesos" CssClass="btn btn-primary" ToolTip="Adicionar Accesos al Usuario" TabIndex="31" ValidationGroup="vgrpValidar" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="card-body">
                                                                        <div class="col-xl-12">
                                                                            <asp:GridView ID="grdListaUsuarioAcceso" runat="server" AutoGenerateColumns="False" TabIndex="4"
                                                                                GridLines="None" CssClass="table table-responsive-sm table-striped table-hover small" AllowPaging="True" 
                                                                                EmptyDataText="No hay registros a visualizar" PageSize="14" OnRowCommand ="grdListaUsuarioAcceso_RowCommand" >
                                                                                <PagerStyle CssClass="pgr" />
                                                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                                <AlternatingRowStyle CssClass="alt" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="IdEmpresa" HeaderText="IdEmpresa" Visible="false" />
                                                                                    <asp:BoundField DataField="Empresa" HeaderText="Empresa" />
                                                                                    <asp:BoundField DataField="IdPaisOrigenUsuarioAcceso" HeaderText="IdPais" Visible="false" />
                                                                                    <asp:BoundField DataField="PaisOrigenUsuarioAcceso" HeaderText="Pais" />
                                                                                    <asp:BoundField DataField="IdUsuario" HeaderText="IdUsuario" />
                                                                                    <asp:BoundField DataField="IdTipoUsuario" HeaderText="IdTipoUsuario" Visible="false" />
                                                                                    <asp:BoundField DataField="TipoUsuario" HeaderText="Tipo Usuario" />
                                                                                    <asp:BoundField DataField="IdProyectoPredeterminado" HeaderText="IdLocal" Visible="false" />
                                                                                    <asp:BoundField DataField="ProyectoPredeterminado" HeaderText="Local" />
                                                                                    <asp:BoundField DataField="IdPerfil" HeaderText="IdPerfil" Visible="false" />
                                                                                    <asp:BoundField DataField="Perfil" HeaderText="Perfil" />
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <div class="bootstrap-switch bootstrap-switch-on bootstrap bootstrap-switch-animate bootstrap-switch-mini ">
                                                                                                <div class="bootstrap-switch-container">
                                                                                                    <asp:LinkButton ID="lnkEstadoOn" runat="server" CommandName="Activar" CommandArgument='<%# Container.DataItemIndex & "," & Eval("IdEmpresa") & "," & Eval("IdPaisOrigenUsuarioAcceso") & "," & Eval("IdProyectoPredeterminado") & "," & Eval("IdUsuario") & "," & Eval("IdPerfil") & "," & Eval("IdTipoUsuario") & "," & Eval("IdPerfil") %>' CssClass="bootstrap-switch-handle-on bootstrap-switch-primary" Visible='<%# If(Eval("Estado") = "True", "True", "False") %>'>Activado</asp:LinkButton>
                                                                                                    <span class="bootstrap-switch-label">&nbsp;</span>
                                                                                                    <asp:LinkButton ID="lnkEstadoOff" runat="server" CommandName="Desactivar" CommandArgument='<%# Container.DataItemIndex & "," & Eval("IdEmpresa") & "," & Eval("IdPaisOrigenUsuarioAcceso") & "," & Eval("IdProyectoPredeterminado") & "," & Eval("IdUsuario") & "," & Eval("IdPerfil") & "," & Eval("IdTipoUsuario") & "," & Eval("IdPerfil") %>' CssClass="bootstrap-switch-handle-off bootstrap-switch-danger" Visible='<%# If(Eval("Estado") = "True", "False", "True") %>'>Anulado</asp:LinkButton>
                                                                                                </div>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>  
                                                                                    <asp:TemplateField HeaderText="Mant." ShowHeader="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="btnEdit" runat="server" CommandName="EditarUsuarioAcceso" ToolTip="Editar Accesos del Usuario" CommandArgument='<%# Container.DataItemIndex & "," & Eval("IdEmpresa") & "," & Eval("IdPaisOrigenUsuarioAcceso") & "," & Eval("IdProyectoPredeterminado") & "," & Eval("IdUsuario") & "," & Eval("IdPerfil") & "," & Eval("IdTipoUsuario") %>' Text="Editar" ImageUrl="~/Imagenes/editar.png"  Width="25px" Height="25px"/>
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
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <asp:LinkButton ID="lnk_mostrarPanelDetalleAccesoGeneral" runat="server"></asp:LinkButton>
                                    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelDetalleAccesoGeneral_ModalPopupExtender" 
                                        runat="server" BackgroundCssClass="FondoAplicacion" 
                                        CancelControlID="btnCancelarDetalleAccesoGeneral"
                                        DynamicServicePath="" Enabled="True" PopupControlID="pnlIngresarDetalleAccesoGeneral" 
                                        TargetControlID="lnk_mostrarPanelDetalleAccesoGeneral">
                                    </ajaxToolkit:ModalPopupExtender>
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
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" role="alert" CssClass="alert alert-warning alert-dismissible fade show" />
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

    <asp:UpdatePanel ID="updpnlIngresarDetalleAccesoGeneral" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlIngresarDetalleAccesoGeneral" runat="server" CssClass="container">
                        <div class="modal-dialog modal-xl">
                            <div class="shadow rounded">
                                <div class="modal-dialog-scrollable">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h4 class="modal-title" id="myModalLabelDetalleAccesoGeneral">
                                                Ingresar Datos de Acceso</h4>
                                            <asp:Button ID="imgbtnCancelarDetalleAccesoGeneralImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                        </div>
                                        <div class="modal-body">
                                            <div class="form-group pt-2">
                                                <div class="row">
                                                    <label class="col-md-2 text-black-50 col-form-label col-form-label-sm">País:</label>
                                                    <div class="col-md-4 col-sm-4">
                                                        <div class="input-group">
                                                            <asp:DropDownList ID="cboPaisDetalleAccesoGeneral" runat="server" AppendDataBoundItems="True" TabIndex="11"
                                                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase">
                                                                <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <ajaxToolkit:ListSearchExtender ID="cboPaisDetalleAccesoGeneral_ListSearchExtender" runat="server"
                                                                Enabled="True" PromptText="Iniciar Búsqueda" TargetControlID="cboPaisDetalleAccesoGeneral">
                                                            </ajaxToolkit:ListSearchExtender>
                                                            <asp:RequiredFieldValidator ID="rfvPaisDetalleAccesoGeneral" runat="server" 
                                                                ControlToValidate="cboPaisDetalleAccesoGeneral" EnableClientScript="False" InitialValue="SELECCIONE DATO"
                                                                ErrorMessage="Ingrese el País" Font-Size="10px" ForeColor="Red" 
                                                                ValidationGroup="vgrpValidarDetalleAccesoGeneral">(*)</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row pt-1">
                                                    <label class="col-md-2 text-black-50 col-form-label col-form-label-sm">Empresa:</label>
                                                    <div class="col-md-4 col-sm-4">
                                                        <div class="input-group">
                                                            <asp:DropDownList ID="cboEmpresaDetalleAccesoGeneral" runat="server" AppendDataBoundItems="True" TabIndex="11"
                                                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase">
                                                                <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <ajaxToolkit:ListSearchExtender ID="cboEmpresaDetalleAccesoGeneral_ListSearchExtender" runat="server"
                                                                Enabled="True" PromptText="Iniciar Búsqueda" TargetControlID="cboEmpresaDetalleAccesoGeneral">
                                                            </ajaxToolkit:ListSearchExtender>
                                                            <asp:RequiredFieldValidator ID="rfvEmpresaDetalleAccesoGeneral" runat="server" 
                                                                ControlToValidate="cboEmpresaDetalleAccesoGeneral" EnableClientScript="False" InitialValue="SELECCIONE DATO"
                                                                ErrorMessage="Ingrese la Empresa" Font-Size="10px" ForeColor="Red" 
                                                                ValidationGroup="vgrpValidarDetalleAccesoGeneral">(*)</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <label class="col-md-2 text-black-50 col-form-label col-form-label-sm">Tipo Usuario:</label>
                                                    <div class="col-md-4 col-sm-4">
                                                        <div class="input-group">
                                                            <asp:DropDownList ID="cboTipoUsuarioDetalleAccesoGeneral" runat="server" AppendDataBoundItems="True" TabIndex="30"
                                                                AutoPostBack="False" CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase">
                                                                <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                                <asp:ListItem Value="A">ADMINISTRADOR</asp:ListItem>
                                                                <asp:ListItem Value="U">USUARIO</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <ajaxToolkit:ListSearchExtender ID="cboTipoUsuarioDetalleAccesoGeneral_ListSearchExtender" runat="server"
                                                                Enabled="True" PromptText="Iniciar Búsqueda" TargetControlID="cboTipoUsuarioDetalleAccesoGeneral">
                                                            </ajaxToolkit:ListSearchExtender>
                                                            <asp:RequiredFieldValidator ID="rfvTipoUsuarioDetalleAccesoGeneral" runat="server" 
                                                                ControlToValidate="cboTipoUsuarioDetalleAccesoGeneral" EnableClientScript="False" InitialValue="SELECCIONE DATO"
                                                                ErrorMessage="Ingrese el Tipo de Usuario" Font-Size="10px" ForeColor="Red" 
                                                                ValidationGroup="vgrpValidarDetalleAccesoGeneral">(*)</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row pt-1">
                                                    <label class="col-md-2 text-black-50 col-form-label col-form-label-sm">Local por Defecto:</label>
                                                    <div class="col-md-10">
                                                        <div class="input-group">
                                                            <asp:DropDownList ID="cboProyectoDefectoDetalleAccesoGeneral" runat="server" AppendDataBoundItems="True" TabIndex="11"
                                                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase">
                                                                <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <ajaxToolkit:ListSearchExtender ID="cboProyectoDefectoDetalleAccesoGeneral_ListSearchExtender" runat="server"
                                                                Enabled="True" PromptText="Iniciar Búsqueda" TargetControlID="cboProyectoDefectoDetalleAccesoGeneral">
                                                            </ajaxToolkit:ListSearchExtender>
                                                            <asp:RequiredFieldValidator ID="rfvProyectoDefectoDetalleAccesoGeneral" runat="server" 
                                                                ControlToValidate="cboProyectoDefectoDetalleAccesoGeneral" EnableClientScript="False" InitialValue="SELECCIONE DATO"
                                                                ErrorMessage="Ingrese el Local por Defecto" Font-Size="10px" ForeColor="Red" 
                                                                ValidationGroup="vgrpValidarDetalleAccesoGeneral">(*)</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row pt-1">
                                                    <label class="col-md-2 text-black-50 col-form-label col-form-label-sm">Perfil:</label>
                                                    <div class="col-md-10">
                                                        <div class="input-group">
                                                            <asp:DropDownList ID="cboPerfilDetalleAccesoGeneral" runat="server" AppendDataBoundItems="True" TabIndex="11"
                                                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase">
                                                                <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <ajaxToolkit:ListSearchExtender ID="cboPerfilDetalleAccesoGeneralListSearchExtender" runat="server"
                                                                Enabled="True" PromptText="Iniciar Búsqueda" TargetControlID="cboPerfilDetalleAccesoGeneral">
                                                            </ajaxToolkit:ListSearchExtender>
                                                            <asp:RequiredFieldValidator ID="rfvPerfilDetalleAccesoGeneral" runat="server" 
                                                                ControlToValidate="cboPerfilDetalleAccesoGeneral" EnableClientScript="False" InitialValue="SELECCIONE DATO"
                                                                ErrorMessage="Ingrese el Perfil" Font-Size="10px" ForeColor="Red" 
                                                                ValidationGroup="vgrpValidarDetalleAccesoGeneral">(*)</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" role="alert" CssClass="alert alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarDetalleAccesoGeneral" />
                                            <div class="col-md-4 text-end p-2">
                                                <asp:Button ID="btnAceptarDetalleAccesoGeneral" runat="server" ValidationGroup="vgrpValidarDetalleAccesoGeneral"
                                                    ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-primary btn-block" />
                                                &nbsp;&nbsp;<asp:Button ID="btnCancelarDetalleAccesoGeneral" runat="server" 
                                                    ToolTip="Cancelar Registro" TabIndex ="73" Text="Cancelar" cssclass="btn btn-danger btn-block"/>
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
    </script>
</asp:Content>