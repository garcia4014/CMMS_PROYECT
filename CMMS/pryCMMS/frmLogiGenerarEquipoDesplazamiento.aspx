<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmLogiGenerarEquipoDesplazamiento.aspx.vb" Inherits="pryCMMS.frmLogiGenerarEquipoDesplazamiento" %>
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
    <link href="Content/AutoCompletar.css" rel="stylesheet" type="text/css"/>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="card mb-3">
        <!-- Inicio Panel -->
        <div class="panel panel-default">
            <div id="dvTab">
                <!-- Inicio Tabs de Navegación -->
                <ul class="nav nav-tabs" role="tablist">
                    <li class="nav-item"><a class="nav-link active" href="#tab1" aria-controls="tab1" role="tab" data-toggle="tab">Listado de Equipos</a></li>
                    <li class="nav-item"><a class="nav-link" href="#tab2" aria-controls="tab2" role="tab" data-toggle="tab">Datos Generales</a></li>
                </ul>
                <!-- Final Tabs de Navegación -->
                <!-- Inicio Paneles Tab -->
                <div class="tab-content" style="padding-top: 10px">
                    <div role="tabpanel" class="tab-pane active" id="tab1">
                        <asp:UpdatePanel ID ="updpnlContent" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="hfdOperacion" runat="server" />
                                <asp:HiddenField ID="hfdFocusObjeto" runat="server" />
                                <asp:HiddenField ID="hfdEstado" runat="server" />
                                <asp:HiddenField ID="hfdOperacionDetalle" runat="server" />
                                <asp:HiddenField ID="hfdOperacionDetalleAdicional" runat="server" />

                                <asp:HiddenField ID="hfdCorreoElectronicoCliente" runat="server" />
                                <asp:HiddenField ID="hfdDNICliente" runat="server" />
                                <asp:HiddenField ID="hfdRUCCliente" runat="server" />

                                <asp:HiddenField ID="hfdIdClienteSAPEquipo" runat="server" />
                                <asp:HiddenField ID="hfdIdEquipoSAPEquipo" runat="server" />
                                <asp:HiddenField ID="hfdFechaRegistroTarjetaSAPEquipo" runat="server" />
                                <asp:HiddenField ID="hfdFechaManufacturaTarjetaSAPEquipo" runat="server" />
                                <asp:HiddenField ID="hfdFechaCreacionEquipo" runat="server" />
                                <asp:HiddenField ID="hfdIdUsuarioCreacionEquipo" runat="server" />
                                <asp:Panel ID="pnlListado" runat="server" CssClass="container"
                                    DefaultButton="imgbtnBuscarEquipo">
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
                                    </div>
                                    <div class="kanban-items-container scrollbar">
                                        <div class="row">
                                            <div class="col-md-12 mt-3">
                                                <div class="table-responsive">
                                                    <asp:GridView ID="grdLista" runat="server" AutoGenerateColumns="False" TabIndex="4" DataKeyNames="Codigo"
                                                        GridLines="None" CssClass="table table-responsive-sm table-hover small" AllowPaging="True" 
                                                        EmptyDataText="No hay registros a visualizar" PageSize="14" OnRowCommand="grdLista_RowCommand">
                                                        <PagerStyle CssClass="pgr" />
                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <Columns>
                                                            <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                            <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>
                                                        </Columns>
                                                        <HeaderStyle CssClass="thead-dark" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div role="tabpanel" class="tab-pane" id="tab2">
                        <asp:UpdatePanel ID ="updpnlGeneral" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlGeneral" runat="server" CssClass="container">
                                    <div class="card mb-3">
                                        <div class="card-body">
                                            <div class="row mt-3">
                                                <div class="col-12 mb-0 bg-100">
                                                    <h6 class="mt-2 mb-1">EQUIPO PRINCIPAL</h6>
                                                    <hr class="bg-300 m-0" />
                                                </div>
                                            </div>
                                            <div class="row mt-1">
                                                <div class="col-lg-6 col-sm-12">
                                                    <div class="row">
                                                        <div class="col-lg-4 col-md-6 mt-2">
                                                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Código Equipo:</label>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtIdEquipo" runat="server" CssClass="form-control form-control-sm" TabIndex="10" placeholder="Id. del equipo"></asp:TextBox>
                                                                <asp:HiddenField ID="hfdFechaEquipo" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-8 col-md-6 mt-2">
                                                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción Equipo:</label>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtDescripcionEquipo" runat="server" CssClass="form-control form-control-sm" 
                                                                    TabIndex="11" style="text-transform :uppercase" placeholder="Descripción del equipo" autocomplete="off"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvDescripcionEquipo" runat="server" 
                                                                    ControlToValidate="txtDescripcionEquipo" EnableClientScript="False" 
                                                                    ErrorMessage="Ingrese la descripción del equipo" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-lg-4 col-md-6 mt-2">
                                                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">N.Ser.:</label>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtNroSerieEquipo" runat="server" CssClass="form-control form-control-sm" 
                                                                    TabIndex="12" style="text-transform :uppercase" placeholder="Número de serie del equipo" autocomplete="off" AutoPostBack="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-4 col-md-6 mt-2">
                                                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">#Part:</label>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtNroParteEquipo" runat="server" CssClass="form-control form-control-sm" 
                                                                    TabIndex="13" style="text-transform :uppercase" placeholder="Número de parte del equipo" autocomplete="off" AutoPostBack="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-4 col-md-6 mt-2">
                                                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Tag:</label>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtTagEquipo" runat="server" CssClass="form-control form-control-sm" 
                                                                    TabIndex="14" style="text-transform :uppercase" placeholder="Tag del equipo" autocomplete="off" AutoPostBack="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-4 col-md-6 mt-2">
                                                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Capacidad:</label>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtCapacidadEquipo" runat="server" CssClass="form-control form-control-sm" 
                                                                    TabIndex="15" style="text-transform :uppercase" placeholder="Tag del equipo" autocomplete="off" AutoPostBack="True"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-4 col-md-6 mt-2">
                                                            <span class="far fa-clone"></span><label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Tipo Equipo:</label>
                                                            <div class="input-group">
                                                                <asp:DropDownList ID="cboTipoEquipo" runat="server" AppendDataBoundItems="True" 
                                                                    AutoPostBack="True" CssClass="form-select form-select-sm js-choice" 
                                                                    style="text-transform : uppercase" TabIndex="16">
                                                                    <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                                                </asp:DropDownList>
							                                </div>
                                                        </div>	
                                                        <div class="col-lg-4 col-md-6 mt-2">
                                                            <span class="far fa-clone"></span><label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Tipo Activo:</label>
                                                            <div class="input-group">
                                                                <asp:DropDownList ID="cboTipoActivo" runat="server" AppendDataBoundItems="True" 
                                                                    AutoPostBack="True" CssClass="form-select form-select-sm js-choice" 
                                                                    style="text-transform : uppercase" TabIndex="17">
                                                                    <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                                                                    <div class="dropdown font-sans-serif btn-reveal-trigger">
                                                                        <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesTipoActivo" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                                                                        <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesTipoActivo">
                                                                            <asp:LinkButton ID="lnkbtnNuevoTipoActivo" runat="server" ToolTip="Crear un tipo de activo" Text="<span class='fas fa-file me-2'></span>Nuevo" CssClass="nav-link nav-link-card-details" />
                                                                            <asp:LinkButton ID="lnkbtnEditarTipoActivo" runat="server" ToolTip="Editar un tipo de activo" Text="<span class='fas fa-file-signature me-2'></span>Editar" CssClass="nav-link nav-link-card-details" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                
                                                            </div>
                                                        </div>
						                                <div class="col-lg-8 col-md-6 mt-2">
                                                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción Equipo SAP:</label>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtDescripcionEquipoSAP" runat="server" CssClass="form-control form-control-sm" 
                                                                    TabIndex="18" style="text-transform :uppercase" placeholder="Descripción del equipo" autocomplete="off"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvDescripcionEquipoSAP" runat="server" 
                                                                    ControlToValidate="txtDescripcionEquipoSAP" EnableClientScript="False" 
                                                                    ErrorMessage="Ingrese la descripción del equipo SAP" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-4 col-md-6 mt-2">
                                                            <span class="far fa-clone"></span><label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Catalogo Principal:</label>
                                                            <div class="input-group">
                                                                <asp:DropDownList ID="cboCatalogo" runat="server" AppendDataBoundItems="True" 
                                                                    AutoPostBack="True" CssClass="form-select form-select-sm js-choice" 
                                                                    style="text-transform : uppercase" TabIndex="19">
                                                                    <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                                                </asp:DropDownList>
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