<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmGnrlMntCliente.aspx.vb" Inherits="pryCMMS.frmGnrlMntCliente" %>
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
    <link href="Content/Calendario.css" rel="stylesheet" type="text/css"/>
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
                    <li class="nav-item"><a class="nav-link active" href="#tab1" aria-controls="tab1" role="tab" data-toggle="tab">Listado de Clientes</a></li>
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
                                <asp:HiddenField ID="hfdFocusObjeto" runat="server" />
                                <asp:Panel ID="pnlListado" runat="server" CssClass="container"
                                    DefaultButton="imgbtnBuscarCliente">
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
                                                <asp:ImageButton ID="imgbtnBuscarCliente" runat="server" Height="18px" TabIndex="3" CssClass="mt-1"
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
                                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Telefono" HeaderText="Telf." HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Celular" HeaderText="Celular" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Dni" HeaderText="DNI" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Ruc" HeaderText="RUC" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Tipo_Doc" HeaderText="Tipo Doc." HeaderStyle-CssClass="bg-200 text-900"/>
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
                                        <h3 class="h5">Mantenimiento de Clientes</h3>
                                    </div>
                                    <div class="mt-2">
                                        <div class="row">
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">IdCliente:</label>
                                            <div class="col-md-2 col-sm-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtIdCliente" runat="server" CssClass="form-control form-control-sm" TabIndex="10"
                                                        style="text-transform :uppercase" Enabled="False"></asp:TextBox>
                                                    <asp:HiddenField ID="lblCaptcha" runat="server" />
                                                </div>
                                            </div>
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Tipo Persona:</label>
                                            <div class="col-md-3 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:DropDownList ID="cboTipoPersona" runat="server" TabIndex="11"
                                                        AppendDataBoundItems="True" CssClass="form-select form-select-sm js-choice" 
                                                        style="text-transform :uppercase" AutoPostBack="True">
                                                        <asp:ListItem Value="SELECCIONE EL TIPO DE PERSONA">SELECCIONE EL TIPO DE PERSONA</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:CheckBox Text="Consultar WS" ID="chkValidarRucWS" runat="server" CssClass="form-check-inline form-check-label" TabIndex="12" AutoPostBack="True"/>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row"> <!-- pt-2">-->
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">RUC:</label>
                                            <div class="col-md-2 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtRuc" runat="server" CssClass="form-control form-control-sm" TabIndex="13" AutoComplete="off"
                                                        style="text-transform :uppercase" AutoPostBack="True"></asp:TextBox>
                                                    <asp:Label ID="lblEstado" runat="server" CssClass="text-danger" style="text-align:center;"></asp:Label>
                                                </div>
                                            </div>
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Razón Social:</label>
                                            <div class="col-md-6 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="form-control form-control-sm" TabIndex="14" AutoComplete="off"
                                                        style="text-transform :uppercase" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Repres. Legal:</label>
                                            <div class="col-md-6 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtRepresentanteLegal" runat="server" CssClass="form-control form-control-sm" TabIndex="15" AutoComplete="off"
                                                        style="text-transform :uppercase"></asp:TextBox>
                                                </div>
                                            </div>
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Tipo Cliente:</label>
                                            <div class="col-md-2 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:DropDownList ID="cboTipoCliente" runat="server" AppendDataBoundItems="True" CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase" TabIndex="16" >
                                                        <asp:ListItem Value="SELECCIONE EL TIPO DE CLIENTE">SELECCIONE EL TIPO DE CLIENTE</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Documento:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:DropDownList ID="cboTipoDocumento" runat="server" TabIndex="17"
                                                        AppendDataBoundItems="True" CssClass="form-select form-select-sm js-choice" 
                                                        style="text-transform :uppercase" AutoPostBack="True">
                                                        <asp:ListItem Value="SELECCIONE EL TIPO DE DOCUMENTO">SELECCIONE EL TIPO DE DOCUMENTO</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtDni" runat="server" CssClass="form-control form-control-sm" TabIndex="18"
                                                        style="text-transform :uppercase"></asp:TextBox>
                                                </div>
                                            </div>
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Nombres:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtNombres" runat="server" CssClass="form-control form-control-sm" TabIndex="19" AutoComplete="off"
                                                        style="text-transform :uppercase" Width="250px"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Apellido Paterno:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtApellidoPaterno" runat="server" CssClass="form-control form-control-sm" TabIndex="20" AutoComplete="off"
                                                        style="text-transform :uppercase"></asp:TextBox>
                                                </div>
                                            </div>
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Apellido Materno:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtApellidoMaterno" runat="server" CssClass="form-control form-control-sm" TabIndex="21" AutoComplete="off"
                                                        style="text-transform :uppercase" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <div class="row">
                                                <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">País:</label>
                                                <div class="col-md-4 pt-md-2 pt-sm-1">
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboPais" runat="server" AppendDataBoundItems="True" TabIndex="22" 
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
                                                            CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase" TabIndex="23"
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
                                            <div class="row">
                                                <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Provincia:</label>
                                                <div class="col-md-4 pt-md-2 pt-sm-1">
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboProvincia" runat="server" AppendDataBoundItems="True" TabIndex="24" 
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
                                                            CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase" TabIndex="25"
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
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Dirección:</label>
                                            <div class="col-md-10 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control form-control-sm" TabIndex="26" AutoComplete="off"
                                                        style="text-transform :uppercase" Width="698px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvDireccion" runat="server" 
                                                        ControlToValidate="txtDireccion" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese la dirección" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Teléfono:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control form-control-sm" TabIndex="27" AutoComplete="off"
                                                        style="text-transform :uppercase"></asp:TextBox>
                                                </div>
                                            </div>
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Celular:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtCelular" runat="server" CssClass="form-control form-control-sm" TabIndex="28" AutoComplete="off"
                                                        style="text-transform :uppercase"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Fax:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtFax" runat="server" CssClass="form-control form-control-sm" TabIndex="29" AutoComplete="off"
                                                        style="text-transform :uppercase"></asp:TextBox>
                                                </div>
                                            </div>
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">EMail:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-sm" TabIndex="30" AutoComplete="off"
                                                        style="text-transform :uppercase" Width="250px" TextMode="Email"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                                                        ControlToValidate="txtEmail" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese el correo electrónico" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Aceptante:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:CheckBox ID="chkAceptante" runat="server" TabIndex="31" CssClass="form-check-inline" />
                                                </div>
                                            </div>
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Genero:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:RadioButton ID="optM" runat="server" Checked="True" GroupName="Genero" TabIndex="32" CssClass="form-check-inline"
                                                        Text="Masculino" />
                                                    &nbsp;
                                                    <asp:RadioButton ID="optF" runat="server" GroupName="Genero" Text="Femenino" TabIndex="33" CssClass="form-check-inline" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Fecha de Nac.:</label>
                                            <div class="col-md-3 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="form-control form-control-sm" TabIndex="34"
                                                        style="text-transform :uppercase" type="date" AutoComplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1 pt-md-2 pt-sm-1"></div>
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Estado Civil:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:DropDownList ID="cboEstadoCivil" runat="server" TabIndex="35"
                                                        AppendDataBoundItems="True" CssClass="form-select form-select-sm js-choice"  
                                                        style="text-transform :uppercase" >
                                                        <asp:ListItem>SELECCIONE EL ESTADO CIVIL</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Estado Contribuyente:</label>
                                            <div class="col-md-3 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtEstadoContribuyente" runat="server" CssClass="form-control form-control-sm" TabIndex="36"
                                                        style="text-transform :uppercase" ></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1"></div>
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Condición Contribuyente:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtCondicionContribuyente" runat="server" CssClass="form-control form-control-sm" TabIndex="37"
                                                        style="text-transform :uppercase"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Tipo Empresa:</label>
                                            <div class="col-md-3 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtTipoEmpresa" runat="server" CssClass="form-control form-control-sm" TabIndex="38"
                                                        style="text-transform :uppercase"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1"></div>
                                            <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Fecha de Creación:</label>
                                            <div class="col-md-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <div class="form-group">
                                                        <asp:TextBox ID="txtFechaCreacion" runat="server" CssClass="form-control form-control-sm" TabIndex="39"
                                                            style="text-transform :uppercase" type="date" AutoComplete="off"></asp:TextBox>
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
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" role="alert" CssClass="alert alert-warning alert-dismissible fade show" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="d-flex flex-row justify-content-end pb-2">
                        <div class="col-4 text-end">
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

        setInterval("regresaFoco()", 1000);

        function regresaFoco() {
            if (document.getElementById("cuerpo_hfdFocusObjeto").value == "ctl00$cuerpo$cboTipoPersona") {
                document.getElementById("cuerpo_cboTipoPersona").focus();
                document.getElementById("cuerpo_hfdFocusObjeto").value = '';
            }
            if (document.getElementById("cuerpo_hfdFocusObjeto").value == "ctl00$cuerpo$cboTipoDocumento") {
                document.getElementById("cuerpo_cboTipoDocumento").focus();
                document.getElementById("cuerpo_hfdFocusObjeto").value = '';
            }
            if (document.getElementById("cuerpo_hfdFocusObjeto").value == "ctl00$cuerpo$cboPais") {
                document.getElementById("cuerpo_cboPais").focus();
                document.getElementById("cuerpo_hfdFocusObjeto").value = '';
            }
            if (document.getElementById("cuerpo_hfdFocusObjeto").value == "ctl00$cuerpo$cboDepartamento") {
                document.getElementById("cuerpo_cboDepartamento").focus();
                document.getElementById("cuerpo_hfdFocusObjeto").value = '';
            }
            if (document.getElementById("cuerpo_hfdFocusObjeto").value == "ctl00$cuerpo$cboProvincia") {
                document.getElementById("cuerpo_cboProvincia").focus();
                document.getElementById("cuerpo_hfdFocusObjeto").value = '';
            }
        }
        function fnSetFocus(txtClientId) {
            document.getElementById("cuerpo_hfdFocusObjeto").value = txtClientId;
        }

        var NroFrame = 0;
        function evalSearch() {
            var Ruc = document.getElementById("cuerpo_TabContCliente_TabPanel2_txtRuc").value;
            var Captcha = document.getElementById("cuerpo_TabContCliente_TabPanel2_txtCaptcha").value.toUpperCase();
            document.getElementById("cuerpo_TabContCliente_TabPanel2_lblRuta").value = "http://www.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias?accion=consPorRuc&nroRuc=" + Ruc + "&codigo=" + Captcha;
            NroFrame++;

            var ifr = document.createElement('iframe');
            ifr.width = "1300px";
            ifr.height = "250px";
            ifr.setAttribute("align", "center");
            ifr.src = document.getElementById("cuerpo_TabContCliente_TabPanel2_lblRuta").value;
            ifr.id = 'fraSunat';
            ifr.target = 'blank';

            if (NroFrame > 1) {
                var node = document.getElementById('fraSunat');
                if (node.parentNode) {
                    node.parentNode.removeChild(node);
                }
            }
            document.body.appendChild(ifr);
            var content = document.getElementById('fraSunat').contentWindow.document.body.innerHTML
            document.getElementById('fraSunat').contentWindow.document.body.innerHTML;
        }
    </script>
</asp:Content>