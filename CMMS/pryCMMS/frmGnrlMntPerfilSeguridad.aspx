<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmGnrlMntPerfilSeguridad.aspx.vb" Inherits="pryCMMS.frmGnrlMntPerfilSeguridad" %>
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
                    <li class="nav-item"><a class="nav-link active" href="#tab1" aria-controls="tab1" role="tab" data-toggle="tab">Listado de Perfiles de Seguridad</a></li>
                    <li class="nav-item"><a class="nav-link" href="#tab2" aria-controls="tab2" role="tab" data-toggle="tab">Datos Generales</a></li>
                </ul>
                <!-- Final Tabs de Navegación -->
                <!-- Inicio Paneles Tab -->
                <div class="tab-content" style="padding-top: 10px">
                    <div role="tabpanel" class="tab-pane active" id="tab1">
                        <asp:UpdatePanel ID="updpnlTab1" runat="server">
                            <ContentTemplate>
								<%--Inicio:Se utiliza para que al seleccionar la grilla no se posicione al inicio--%>
								<script type="text/javascript">
                                    var xPos, yPos;
                                    var xPos1, yPos1;
                                    var xPos2, yPos2;
                                    var prm = Sys.WebForms.PageRequestManager.getInstance();
                                    prm.add_beginRequest(BeginRequestHandler);
                                    prm.add_endRequest(EndRequestHandler);
                                    function BeginRequestHandler(sender, args) {
                                        xPos = $get("divGridModulos").scrollLeft;
                                        yPos = $get("divGridModulos").scrollTop;
                                        xPos1 = $get("divGridOpcion").scrollLeft;
                                        yPos1 = $get("divGridOpcion").scrollTop;
                                        xPos2 = $get("divGridPermisoXArea").scrollLeft;
                                        yPos2 = $get("divGridPermisoXArea").scrollTop;
                                    }
                                    function EndRequestHandler(sender, args) {
                                        $get("divGridModulos").scrollLeft = xPos;
                                        $get("divGridModulos").scrollTop = yPos;
                                        $get("divGridOpcion").scrollLeft = xPos1;
                                        $get("divGridOpcion").scrollTop = yPos1;
                                        $get("divGridPermisoXArea").scrollLeft = xPos2;
                                        $get("divGridPermisoXArea").scrollTop = yPos2;
                                    }
                                </script>
								<%--Final:Se utiliza para que al seleccionar la grilla no se posicione al inicio--%>
								<asp:HiddenField ID="hfdOperacion" runat="server" />
                                <asp:HiddenField ID="hfdEstado" runat="server" />
                                <asp:Panel ID="pnlListado" runat="server" CssClass="container"
                                    DefaultButton="imgbtnBuscarPerfilSeguridad">
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
                                                <asp:ImageButton ID="imgbtnBuscarPerfilSeguridad" runat="server" Height="18px" TabIndex="3" CssClass="mt-1"
                                                    ImageUrl="~/Imagenes/buscar.png" ToolTip="Buscar Registro" Width="21px" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12 mt-3">
                                            <div class="table-responsive scrollbar">
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
                                <div class="col-12 bg-dark p-2 mt-2">
                                    <h3 class="h5 text-white">Mantenimiento de Perfiles de Seguridad</h3>
                                </div>
                                <div class="row mt-2">
                                    <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Cód. Perfil:</label>
                                    <div class="col-md-2 col-sm-4 pt-md-2 pt-sm-1">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtIdPerfil" runat="server" CssClass="form-control form-control-sm" TabIndex="10"
                                                style="text-transform : uppercase" Enabled="False" ></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Descrip. Perfil:</label>
                                    <div class="col-md-10 col-sm-4 pt-md-2 pt-sm-1">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtDescripcion" runat="server"  CssClass="form-control form-control-sm" TabIndex="11"
                                                style="text-transform :uppercase" Enabled="False" placeholder="Ingrese el Perfil"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Cód. Usuario:</label>
                                    <div class="col-md-2 col-sm-4 pt-md-2 pt-sm-1">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtIdUsuario" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="12"
                                                style="text-transform : uppercase" placeholder="Ingrese Código"></asp:TextBox>
                                            <ajaxToolkit:ModalPopupExtender ID="txtIdUsuario_ModalPopupExtender" runat="server" 
                                                DynamicServicePath="" Enabled="True" TargetControlID="txtIdUsuario" 
                                                BackgroundCssClass="FondoAplicacion" CancelControlID="imgbtnCancelarUsuImagen"
                                                DropShadow="False" PopupControlID="pnlSeleccionarDatos">
                                            </ajaxToolkit:ModalPopupExtender>
                                            <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" 
                                                ControlToValidate="cboArea" EnableClientScript="False" 
                                                ErrorMessage="Ingrese el Usuario" Font-Size="10px" ForeColor="Red" 
                                                InitialValue="SELECCIONE DATO">(*)</asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                     <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Descrip. Usuario:</label>
                                    <div class="col-md-6 col-sm-4 pt-md-2 pt-sm-1">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control form-control-sm" TabIndex="13"
                                                Enabled="False"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Áreas:</label>
                                    <div class="col-md-2 col-sm-4 pt-md-2 pt-sm-1">
                                        <div class="input-group">
                                            <asp:DropDownList ID="cboArea" runat="server" AppendDataBoundItems="True" TabIndex="14"
                                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice" style="text-transform : uppercase">
                                                <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvArea" runat="server" 
                                                ControlToValidate="cboArea" EnableClientScript="False" InitialValue="SELECCIONE DATO"
                                                ErrorMessage="Ingrese el área" Font-Size="10px" ForeColor="Red">(*)</asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                     <label class="col-md-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Sistemas:</label>
                                    <div class="col-md-6 col-sm-4 pt-md-2 pt-sm-1">
                                        <div class="input-group">
                                            <asp:DropDownList ID="cboSistema" runat="server" AppendDataBoundItems="True" TabIndex="15"
                                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice" style="text-transform : uppercase">
                                                <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvSistema" runat="server" 
                                                ControlToValidate="cboSistema" EnableClientScript="False" InitialValue="SELECCIONE DATO"
                                                ErrorMessage="Ingrese el sistema" Font-Size="10px" ForeColor="Red">(*)</asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlSeleccionarElemento" runat="server">
                                            <div class="form-group row">
                                                <div class="col-md-6">
                                                    <div class="card mb-3">
                                                        <div class="card-header">Listado de Módulos</div>
                                                        <div class="card-body">
                                                            <div id="divGridModulos" style="overflow:auto; height:220px; align:left;">
                                                                <asp:GridView ID="grdModulo" runat="server" AutoGenerateColumns="False" TabIndex="16"
                                                                    GridLines="None" CssClass="table table-responsive-sm table-striped table-hover small" AllowPaging="True"
                                                                    EmptyDataText="No hay registros a visualizar" PageSize="1000">
                                                                    <PagerStyle CssClass="pgr" />
                                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                    <Columns>
                                                                        <asp:CommandField ShowSelectButton="True" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                        <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900"/>  
                                                                        <asp:BoundField DataField="Sistema" HeaderText="Sistema" HeaderStyle-CssClass="bg-200 text-900"/>  
                                                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-CssClass="bg-200 text-900"/>  
                                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900"/>  
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="thead-dark" />
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="card mb-3">
                                                        <div class="card-header">Listado de Opciones del Módulo</div>
                                                        <div class="card-body">
                                                            <div id="divGridOpcion" style="overflow:auto; height:220px; align:left;">
                                                                <asp:GridView ID="grdOpcion" runat="server" AutoGenerateColumns="False" TabIndex="17"
                                                                    GridLines="None" CssClass="table table-responsive-sm table-striped table-hover small" AllowPaging="True" 
                                                                    EmptyDataText="No hay registros a visualizar" PageSize="1000">
                                                                    <PagerStyle CssClass="pgr" />
                                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                    <Columns>
                                                                        <asp:CommandField ShowSelectButton="True" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                        <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900"/>  
                                                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-CssClass="bg-200 text-900"/>  
                                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900"/>  
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="thead-dark" />
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="card mb-3">
                                                        <div class="card-header">Permisos del Perfil por Area</div>
                                                        <div class="card-body">
                                                            <div id="divGridPermisoXArea"  style="overflow:auto; height:220px; align:left;">
                                                                <asp:GridView ID="grdPermisoXArea" runat="server" AutoGenerateColumns="False" TabIndex="18"
                                                                    GridLines="None" CssClass="table table-responsive-sm table-striped table-hover small" AllowPaging="True" 
                                                                    EmptyDataText="No hay registros a visualizar" PageSize="10000" AutoGenerateDeleteButton="True" >
                                                                    <PagerStyle CssClass="pgr" />
                                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="IdSistema" HeaderText="Cód.Sist." HeaderStyle-CssClass="bg-200 text-900"/>
                                                                        <asp:BoundField DataField="IdArea" HeaderText="Cód.Área" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                        <asp:BoundField DataField="Area" HeaderText="Área" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                        <asp:BoundField DataField="IdElemento" HeaderText="Cód.Elem." HeaderStyle-CssClass="bg-200 text-900"/>
                                                                        <asp:BoundField DataField="Elemento" HeaderText="Elemento" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                        <asp:BoundField DataField="IdModulo" HeaderText="Cód.Mód." HeaderStyle-CssClass="bg-200 text-900"/>
                                                                        <asp:BoundField DataField="Modulo" HeaderText="Módulo" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                        <asp:BoundField DataField="IdPerfil" HeaderText="Cód.Perf." HeaderStyle-CssClass="bg-200 text-900"/>
                                                                        <asp:BoundField DataField="IdUsuario" HeaderText="Cód.Usu." HeaderStyle-CssClass="bg-200 text-900"/>
                                                                        <asp:BoundField DataField="Usuario" HeaderText="Usuario" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="thead-dark" />
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
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
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" role="alert" CssClass="alert alert-warning alert-dismissible fade show" />
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
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlSeleccionarDatos" runat="server" CssClass="container">
               <div class="modal-dialog modal-xl">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title" id="myModalLabel">
                                        Seleccionar Usuario</h4>
                                    <asp:Button ID="imgbtnCancelarUsuImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <div class="form-group row pt-2">
                                        <span class="col-sm-2 col-form-label-sm">Buscar por</span>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="cboFiltroUsu" runat="server" CssClass="form-select form-select-sm js-choice" TabIndex="60">
                                                <asp:ListItem>SELECCIONE EL FILTRO</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtBuscarUsu" runat="server" CssClass="form-control form-control-sm" TabIndex="61"
                                                    style="text-transform :uppercase" placeholder="Ingrese Busqueda"></asp:TextBox>
                                                &nbsp;
                                                <asp:ImageButton ID="imgbtnBuscarUsu" runat="server" Height="18px" TabIndex="3" CssClass="mt-1"
                                                    ImageUrl="~/Imagenes/buscar.png" ToolTip="Buscar Registro" Width="21px" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12 mt-3">
                                            <div class="table-responsive scrollbar">
                                                <asp:GridView ID="grdListaUsu" runat="server" AutoGenerateColumns="False" TabIndex="63"
                                                    GridLines="None" CssClass="table table-responsive-sm table-striped table-hover small" AllowPaging="True" 
                                                    EmptyDataText="No hay registros a visualizar" PageSize="6" >
                                                    <PagerStyle CssClass="pgr" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:CommandField ShowSelectButton="True" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Login" HeaderText="Login" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Apellido_Paterno" HeaderText="Ape. Paterno" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Apellido_Materno" HeaderText="Ape. Materno" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Nombres" HeaderText="Nombres" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900"/>
                                                    </Columns>
                                                    <HeaderStyle CssClass="thead-dark" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Label ID="lblMensajeUsu" runat="server" CssClass="col-form-label col-form-label-sm text-danger"></asp:Label>
                                    <div class="col-md-4 text-end p-2">
                                        <asp:Button ID="btnAceptarUsu" runat="server" ValidationGroup="vgrpValidarBusqueda"
                                            ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-primary btn-block" />
                                        &nbsp;&nbsp;<asp:Button ID="btnCancelarUsu" runat="server" 
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
    <script type="text/javascript" language="javascript">
        var ModalProgress = '<%= ModalProgress.ClientID %>';

        //Función para deshabilitar el ENTER y ejecutar el botón por defecto.
        function stopRKey(evt) {
            var evt = (evt) ? evt : ((event) ? event : null);
            var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
            if ((evt.keyCode == 13) && ((node.type == "text") || (node.type == "radio") || (node.type == "checkbox"))) { return false; }
        }
        document.onkeypress = stopRKey;

        function CambiaFoco(cajadestino) {
            var key = window.event.keyCode;
            if (key == 13)
                document.getElementById(cajadestino).focus();
        }
    </script>
</asp:Content>