<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmGnrlVideoAyuda.aspx.vb" Inherits="pryCMMS.frmGnrlVideoAyuda" %>
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
    
    <link href="vendors/plyr/plyr.css" rel="stylesheet" />
    <script src="vendors/plyr/plyr.polyfilled.min.js"></script>

    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="card mb-3">
        <!-- Inicio Panel -->
        <div class="panel panel-default">
            <div id="dvTab">
                <!-- Inicio Tabs de Navegación -->
                <ul class="nav nav-tabs" role="tablist">
                    <li class="nav-item"><a class="nav-link active" href="#tab1" aria-controls="tab1" role="tab" data-toggle="tab">Listado de Videos</a></li>
                    <li class="nav-item"><a class="nav-link" href="#tab2" aria-controls="tab2" role="tab" data-toggle="tab">Datos Generales</a></li>
                </ul>
                <!-- Final Tabs de Navegación -->
                <!-- Inicio Paneles Tab -->
                <div class="tab-content" style="padding-top: 10px">
                    <div role="tabpanel" class="tab-pane active" id="tab1">
                        <asp:Panel ID="pnlListado" runat="server" CssClass="container" 
                            DefaultButton="imgbtnBuscarVideos"> 
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
                                        <asp:ImageButton ID="imgbtnBuscarVideos" runat="server" Height="18px" TabIndex="3" CssClass="mt-1" OnClick="imgbtnBuscarVideos_Click"
                                            ImageUrl="~/Imagenes/buscar.png" ToolTip="Buscar Registro" Width="21px" />
                                    </div>
                                </div>
                            </div>
                            <div class="row mt-3">
                                <asp:UpdatePanel ID="updpnlTab1" runat="server">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="hfdOperacion" runat="server" />
                                        <asp:HiddenField ID="hfdEstado" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="container">
                                    <div class="row">
                                        <asp:Repeater ID="rptVideoTutorial" runat="server" OnItemCommand="rptVideoTutorial_ItemCommand">
                                            <ItemTemplate>
                                                <div class="col-md-4 mb-3">
                                                    <div class="card">
                                                        <div class="card-header p-0">
                                                            <div class="row flex-between-end">
                                                                <div class="col-auto ms-auto">
                                                                    <asp:ImageButton ID="imgbtnEditar" runat="server" CommandName="EditarVideo" CommandArgument='<%# Eval("Codigo") & "*" & Eval("Estado") %>' ToolTip="Editar datos del video"  AlternateText="Editar datos del video" ImageUrl="~/Imagenes/25x25/editar.png" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="card-body p-0">
                                                            <div class="col-12">
                                                                <!-- Contenido de cada tarjeta de video -->
                                                                <div class="player rounded-3" data-plyr-provider="youtube" data-plyr-embed-id='<%# Eval("IdYouTube") %>'></div>
                                                            </div>
                                                            <div class="row p-1">
                                                                <div class="col-10">
                                                                    <h1 class="h6 mt-2"><asp:Label ID="lblTitulo" runat="server" Text='<%# StrConv(Eval("Titulo"), VbStrConv.ProperCase) %>' CssClass="text-dark"></asp:Label></h1>
                                                                </div>
                                                                <div class="col-xl-12 col-md-12">
                                                                    <div class="row">
                                                                        <div class="col-xl-4 col-lg-4">
                                                                            <p class="fs--1 m-0"><strong class="small">Descripción:</strong></p>
                                                                        </div>
                                                                        <div class="col-xl-8 col-lg-8">
                                                                            <p class="fs--1 m-0"><asp:Label ID="lblDescripcion" runat="server" Text='<%# StrConv(Eval("Descripcion"), VbStrConv.Uppercase) %>' CssClass="text-500 small"></asp:Label></p>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <div role="tabpanel" class="tab-pane" id="tab2">
                        <asp:UpdatePanel ID="updpnlTab2" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlGeneral" runat="server" CssClass="container">
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <h5 class="mb-1" data-anchor="data-anchor">Mantenimiento de Videos</h5>
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
                                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Código Videos:</label>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtIdVideo" runat="server" CssClass="form-control form-control-sm" TabIndex="10"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-6 col-md-8 mt-2">
                                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Titulo:</label>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="12"
                                                    style="text-transform :uppercase" placeholder="Titulo del Video"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvTitulo" runat="server" 
                                                    ControlToValidate="txtTitulo" EnableClientScript="False" 
                                                    ErrorMessage="Ingrese el titulo" Font-Size="10px" ForeColor="Red" 
                                                    ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 col-md-8 mt-2">
                                            <span class="fas fa-shipping-fast"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción:</label>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="12"
                                                    style="text-transform :uppercase" placeholder="Descripción del Video"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" 
                                                    ControlToValidate="txtDescripcion" EnableClientScript="False" 
                                                    ErrorMessage="Ingrese la descripción" Font-Size="10px" ForeColor="Red" 
                                                    ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-6 col-md-4 mt-2">
                                            <span class="fas fa-phone-alt"></span><label class="text-black-50 col-form-label fs--2 m-1">URL Youtube:</label>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtLinkYoutube" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="14"
                                                    style="text-transform :uppercase" placeholder="Vida Util del Videos" ></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvLinkYouTube" runat="server" 
                                                    ControlToValidate="txtLinkYoutube" EnableClientScript="False" 
                                                    ErrorMessage="Ingrese la vida util en meses del catálogo" Font-Size="10px" ForeColor="Red"
                                                    ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="col-lg-2 col-md-4 mt-2">
										    <span class="fas fa-stream"></span><label class="text-black-50 col-form-label fs--2 m-1">Tiempo Video:</label>
										    <div class="input-group">
											    <asp:TextBox ID="txtTiempoMinutos" runat="server" CssClass="form-control form-control-sm" TabIndex="16"
												    style="text-transform :uppercase" placeholder="Periodo de Garantía" ></asp:TextBox>
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
                    <div class="d-flex flex-row justify-content-end pb-2">
                        <div class="col-lg-4 col-md-5 text-end">
                            <asp:Button ID="btnNuevo" Text="Nuevo" runat="server" OnClick="btnNuevo_Click" CssClass="btn btn-primary" TabIndex="100" ToolTip="Nuevo Registro" />
                            &nbsp;
                            <asp:Button ID="btnGuardar" Text="Guardar" runat="server" OnClick="btnGuardar_Click" CssClass="btn btn-success" TabIndex="101" ToolTip="Guardar Registro" ValidationGroup="vgrpValidar"/>
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
