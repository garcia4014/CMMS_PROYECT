<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmGnrlMntEmpresa.aspx.vb" Inherits="pryCMMS.frmGnrlMntEmpresa" %>
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
                    <li class="nav-item"><a class="nav-link active" href="#tab1" aria-controls="tab1" role="tab" data-toggle="tab">Listado de Empresas</a></li>
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
                                <asp:HiddenField ID="hfdPrincipal" runat="server" />
                                <asp:HiddenField ID="hfdFocusObjeto" runat="server" />
                                <asp:Panel ID="pnlListado" runat="server" CssClass="container" 
                                    DefaultButton="imgbtnBuscarEmpresa"> 
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
                                                <asp:ImageButton ID="imgbtnBuscarEmpresa" runat="server" Height="18px" TabIndex="3" CssClass="mt-1"
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
                                                        <asp:BoundField DataField="Principal" HeaderText="Principal" HeaderStyle-CssClass="bg-200 text-900"/>
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
                                    <h3 class="h5">Mantenimiento de Empresas</h3>
                                </div>
                                <div class="mt-2">
                                    <div class="row">
                                        <div class="col-lg-9 col-md-8 col-sm-9 pt-md-2 pt-sm-1">
                                            <div class="row">
                                                <div class="col-lg-3 col-md-4 col-sm-5">
                                                    <label class="text-black-50 col-form-label col-form-label-sm">IdEmpresa:</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtIdEmpresa" runat="server" CssClass="form-control form-control-sm" TabIndex="10"
                                                            style="text-transform :uppercase"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-lg-9 col-md-12 col-sm-12 pt-1">
                                                    <label class="text-black-50 col-form-label col-form-label-sm">Razón Social:</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control form-control-sm" TabIndex="11"
                                                            style="text-transform :uppercase" placeholder="Razón Social"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" 
                                                            ControlToValidate="txtDescripcion" EnableClientScript="False" 
                                                            ErrorMessage="Ingrese la descripción" Font-Size="10px" ForeColor="Red" 
                                                            ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-lg-8 col-md-8 col-sm-12 pt-1">
                                                    <label class="text-black-50 col-form-label col-form-label-sm">Descripción Abreviada:</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtDescripcionCorta" runat="server" CssClass="form-control form-control-sm" TabIndex="12"
                                                            style="text-transform : uppercase" placeholder="Nombre Comercial"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-lg-4 col-md-4 col-sm-5 pt-1">
                                                    <label class="text-black-50 col-form-label col-form-label-sm">RUC:</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtRUC" runat="server" CssClass="form-control form-control-sm" TabIndex="13" 
                                                            style="text-transform : uppercase" placeholder="RUC"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvRUC" runat="server" 
                                                            ControlToValidate="txtRUC" EnableClientScript="False" 
                                                            ErrorMessage="Ingrese el R.U.C." Font-Size="10px" ForeColor="Red" 
                                                            ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-4 col-sm-3 mt-2 pt-md-2 pt-sm-1">
                                            <div class="input-group">
                                                <asp:Image ID="imgEmpresa" runat="server" CssClass="DiagonalRedonda" Height="100px" Width="100px" onerror="this.src='Imagenes/Prod/Sin_Imagen.jpg'"/>
                                                <asp:ImageButton ID="imgbtnCargarImagenEmpresa" runat="server" Height="25px" Width="25px" onerror="this.src='Imagenes/Prod/Sin_Imagen.jpg'" ImageUrl="Imagenes/25x25/Cargar_Imagen.png" alt="Mensajes" TabIndex="23"/>
                                                <ajaxToolkit:ModalPopupExtender ID="imgbtnCargarImagenEmpresa_ModalPopupExtender" 
                                                    runat="server" BackgroundCssClass="FondoAplicacion" 
                                                    CancelControlID="imgbtnCancelarImagenEmpresa" DropShadow="False" DynamicServicePath="" 
                                                    Enabled="True" PopupControlID="pnlSeleccionarImagenEmpresa" 
                                                    TargetControlID="imgbtnCargarImagenEmpresa">
                                                </ajaxToolkit:ModalPopupExtender>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Domicilio Fiscal:</label>
                                        <div class="col-md-10 pt-md-2 pt-sm-1">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtDomicilioFiscal" runat="server" CssClass="form-control form-control-sm" TabIndex="13" 
                                                    style="text-transform : uppercase" placeholder="Domicilio Fiscal"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvDomicilioFiscal" runat="server" 
                                                    ControlToValidate="txtRUC" EnableClientScript="False" 
                                                    ErrorMessage="Ingrese el domicilio fiscal" Font-Size="10px" ForeColor="Red" 
                                                    ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Nombre Comercial:</label>
                                        <div class="col-md-10 pt-md-2 pt-sm-1">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtNombreComercial" runat="server" CssClass="form-control form-control-sm" TabIndex="14" 
                                                    style="text-transform : uppercase" placeholder="Nombre Comercial"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvNombreComercial" runat="server" 
                                                    ControlToValidate="txtNombreComercial" EnableClientScript="False" 
                                                    ErrorMessage="Ingrese el nombre comercial" Font-Size="10px" ForeColor="Red" 
                                                    ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <div class="form-group row">
                                                    <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">País:</label>
                                                    <div class="col-md-4 pt-md-2 pt-sm-1">
                                                        <div class="input-group">
                                                            <asp:DropDownList ID="cboPais" runat="server" AppendDataBoundItems="True" TabIndex="15" 
                                                                CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase" 
                                                                AutoPostBack="True" onfocus="fnSetFocus('')">
                                                                <asp:ListItem>SELECCIONE EL PAIS</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Departamento:</label>
                                                    <div class="col-md-4 pt-md-2 pt-sm-1">
                                                        <div class="input-group">
                                                            <asp:DropDownList ID="cboDepartamento" runat="server" AppendDataBoundItems="True" 
                                                                CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase" TabIndex="16"
                                                                AutoPostBack="True">
                                                                <asp:ListItem>SELECCIONE EL DEPARTAMENTO</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Provincia:</label>
                                                    <div class="col-md-4 pt-md-2 pt-sm-1">
                                                        <div class="input-group">
                                                            <asp:DropDownList ID="cboProvincia" runat="server" AppendDataBoundItems="True" TabIndex="17" 
                                                                CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase" 
                                                                AutoPostBack="True">
                                                                <asp:ListItem>SELECCIONE LA PROVINCIA</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Distrito:</label>
                                                    <div class="col-md-4 pt-md-2 pt-sm-1">
                                                        <div class="input-group">
                                                            <asp:DropDownList ID="cboDistrito" runat="server" AppendDataBoundItems="True" 
                                                                CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase" TabIndex="18"
                                                                AutoPostBack="False">
                                                                <asp:ListItem>SELECCIONE EL DISTRITO</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Página Web:</label>
                                        <div class="col-md-4 pt-md-2 pt-sm-1">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtPaginaWeb" runat="server" CssClass="form-control form-control-sm" TabIndex="19" 
                                                    style="text-transform : uppercase" placeholder="Pagina Web"></asp:TextBox>
                                            </div>
                                        </div>
                                        <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Teléfono:</label>
                                        <div class="col-md-4 pt-md-2 pt-sm-1">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control form-control-sm" TabIndex="20" 
                                                    style="text-transform : uppercase" placeholder="Teléfono Comercial"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvTelefono" runat="server" 
                                                    ControlToValidate="txtTelefono" EnableClientScript="False" 
                                                    ErrorMessage="Ingrese el teléfono" Font-Size="10px" ForeColor="Red" 
                                                    ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Url Logo:</label>
                                        <div class="col-md-10 pt-md-2 pt-sm-1">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtURLLogo" runat="server" CssClass="form-control form-control-sm" Width="400px" TabIndex="21" 
                                                    style="text-transform : uppercase" placeholder="URL Imagen Logo"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvURLLogo" runat="server" 
                                                    ControlToValidate="txtURLLogo" EnableClientScript="False" 
                                                    ErrorMessage="Ingrese la URL del Logo para sus reportes" Font-Size="10px" ForeColor="Red" 
                                                    ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">T.Nomeclatura Doc.:</label>
                                        <div class="col-md-4 pt-md-2 pt-sm-1">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtDescripcionTipoDocumento" runat="server" CssClass="form-control form-control-sm" TabIndex="22" 
                                                    style="text-transform : uppercase" placeholder="Descripción del Tipo de Nomenclatura de Documento de la Empresa"></asp:TextBox>
                                            </div>
                                        </div>
                                        <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Tipo Moneda Base:</label>
                                        <div class="col-md-4 pt-md-2 pt-sm-1">
                                            <div class="input-group">
                                                <asp:DropDownList ID="cboTipoMonedaBase" runat="server" AppendDataBoundItems="True" 
                                                    CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase" TabIndex="18"
                                                    AutoPostBack="False">
                                                    <asp:ListItem>SELECCIONE EL DISTRITO</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Conf. Lista Precio:</label>
                                        <div class="col-md-10 pt-md-2 pt-sm-1">
                                            <div class="input-group">
                                                <asp:DropDownList ID="cboConfiguracionListaPrecio" runat="server" TabIndex="23"
                                                    AppendDataBoundItems="True" CssClass="form-select form-select-sm js-choice" 
                                                    style="text-transform :uppercase" AutoPostBack="True">
                                                    <asp:ListItem Value="SELECCIONE LA CONFIGURACION DE LISTA DE PRECIO">SELECCIONE LA CONFIGURACION DE LISTA DE PRECIO</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Conf. Id.Prod.:</label>
                                        <div class="col-md-10 pt-md-2 pt-sm-1">
                                            <div class="input-group">
                                                <asp:DropDownList ID="cboConfiguracionCodigoProducto" runat="server" TabIndex="24"
                                                    AppendDataBoundItems="True" CssClass="form-select form-select-sm js-choice" 
                                                    style="text-transform :uppercase" AutoPostBack="True">
                                                    <asp:ListItem Value="SELECCIONE LA CONFIGURACION DE CODIGO PRODUCTO">SELECCIONE LA CONFIGURACION DE CODIGO PRODUCTO</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Tipo Id.Prod a Usar:</label>
                                        <div class="col-md-4 pt-md-2 pt-sm-1">
                                            <div class="input-group">
                                                <asp:DropDownList ID="cboTipoProductoAUsar" runat="server" TabIndex="25"
                                                    AppendDataBoundItems="True" CssClass="form-select form-select-sm js-choice" 
                                                    style="text-transform :uppercase" AutoPostBack="True">
                                                    <asp:ListItem Value="SELECCIONE TIPO PRODUCTO A USAR">SELECCIONE TIPO PRODUCTO A USAR</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Tipo Empresa:</label>
                                        <div class="col-md-4 pt-md-2 pt-sm-1">
                                            <div class="input-group">
                                                <asp:DropDownList ID="cboTipoEmpresa" runat="server" TabIndex="25"
                                                    AppendDataBoundItems="True" CssClass="form-select form-select-sm js-choice" 
                                                    style="text-transform :uppercase" AutoPostBack="True">
                                                    <asp:ListItem Value="01">COMERCIAL</asp:ListItem>
                                                    <asp:ListItem Value="02">INMOBILIARIA</asp:ListItem>
                                                    <asp:ListItem Value="03">CASA DE CAMBIO</asp:ListItem>
                                                </asp:DropDownList>
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
    <asp:Panel ID="pnlSeleccionarImagenEmpresa" runat="server" CssClass="container">
        <div class="modal-dialog modal-sm modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">
                        Seleccionar Imagen</h4>
                    <asp:Button ID="imgbtnCancelarImagenEmpresa" runat="server" Text="&times;" CssClass="close bg-transparent border-0" data-dismiss="modal" aria-label="Close" />
                </div>
                <div class="modal-body">
                    <div class="form-group row">
                        <div class="col-md-4">
                            <label class="text-black-50 col-form-label col-form-label-sm">Archivo:</label>
                            <div class="input-group">
                                <asp:FileUpload ID="fupEmpresa" runat="server" EnableTheming="True" CssClass="form-control-sm" TabIndex="90" />
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-md-12">
                            <asp:Label ID="lblMensajeSeleccionarImagenEmpresa" runat="server" CssClass="col-form-label col-form-label-sm text-danger"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnAceptarImagenEmpresa" runat="server" Text="Cargar Imagen" CssClass="btn btn-primary btn-default" TabIndex ="91" />
                </div>
            </div>
        </div>
    </asp:Panel>

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
    </script>
</asp:Content>