<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmGnrlMntTipoCambio.aspx.vb" Inherits="pryCMMS.frmGnrlMntTipoCambio" %>
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
                    <li class="nav-item"><a class="nav-link active" href="#tab1" aria-controls="tab1" role="tab" data-toggle="tab">Listado de Tipo de Cambio</a></li>
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
                                    DefaultButton="imgbtnBuscarTipoCambio">
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
                                                <asp:ImageButton ID="imgbtnBuscarTipoCambio" runat="server" Height="18px" TabIndex="3" CssClass="mt-1"
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
                                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" HeaderStyle-CssClass="bg-200 text-900"/>  
                                                        <asp:BoundField DataField="Tip_Mon" HeaderText="Tipo Moneda" HeaderStyle-CssClass="bg-200 text-900"/>  
                                                        <asp:BoundField DataField="TC_Venta" HeaderText="T.C. Venta" HeaderStyle-CssClass="bg-200 text-900"/>  
                                                        <asp:BoundField DataField="TC_Compra" HeaderText="T.C. Compra" HeaderStyle-CssClass="bg-200 text-900"/>  
                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900"/>  
                                                        <asp:TemplateField HeaderStyle-CssClass="bg-200 text-900">
                                                            <ItemTemplate>
                                                                <div class="bootstrap-switch bootstrap-switch-on bootstrap bootstrap-switch-animate bootstrap-switch-mini ">
                                                                    <div class="bootstrap-switch-container">
                                                                        <asp:LinkButton ID="lnkEstadoOn" runat="server" CommandName="Activar" CommandArgument='<%# Eval("Fecha") %>' CssClass="bootstrap-switch-handle-on bootstrap-switch-primary" Visible='<%# If(Eval("Estado") = "True", "True", "False") %>'>Activado</asp:LinkButton>
                                                                        <span class="bootstrap-switch-label">&nbsp;</span>
                                                                        <asp:LinkButton ID="lnkEstadoOff" runat="server" CommandName="Desactivar" CommandArgument='<%# Eval("Fecha") %>' CssClass="bootstrap-switch-handle-off bootstrap-switch-danger" Visible='<%# If(Eval("Estado") = "True", "False", "True") %>'>Anulado</asp:LinkButton>
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
                                    <h3 class="h5">Mantenimiento de Tipo de Cambio</h3>
                                </div>
                                <div class="mt-2">
                                    <div class="row">
                                        <label class="col-md-3 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Fecha:</label>
                                        <div class="col-md-5 col-sm-4 pt-md-2 pt-sm-1">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control form-control-sm" TabIndex="10"
                                                    ToolTip="Fecha del tipo de cambio." style="text-transform :uppercase" AutoPostBack="True"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="txtFecha_CalendarExtender" runat="server" 
                                                    CssClass="calendario" TargetControlID="txtFecha" Enabled="True">
                                                </ajaxToolkit:CalendarExtender>
                                                &nbsp;<asp:ImageButton ID="imgbtnCargarTipoCambio" runat="server" TabIndex="11"
                                                    ImageUrl="~/Imagenes/25x25/Refrescar.png" 
                                                    ToolTip="Cargar el tipo de cambio de la Sunat" Width="25px" />
                                                <asp:RequiredFieldValidator ID="rfvFecha" runat="server" 
                                                    ControlToValidate="txtFecha" EnableClientScript="False" 
                                                    ErrorMessage="Ingrese la fecha" Font-Size="10px" ForeColor="Red"
                                                    ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-md-3 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">T. Moneda:</label>
                                        <div class="col-md-9 pt-md-2 pt-sm-1">
                                            <div class="input-group">
                                                    <asp:DropDownList ID="cboTipoMoneda" runat="server" AppendDataBoundItems="True" TabIndex="11"
                                                    AutoPostBack="True" CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase">
                                                    <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvTipoMoneda" runat="server" 
                                                    ControlToValidate="cboTipoMoneda" EnableClientScript="False" InitialValue="SELECCIONE DATO"
                                                    ErrorMessage="Ingrese la moneda" Font-Size="10px" ForeColor="Red"
                                                    ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-md-3 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">T.C. Compra:</label>
                                        <div class="col-md-9 pt-md-2 pt-sm-1">
                                            <div class="input-group">
                                                    <asp:TextBox ID="txtTCCompra" runat="server" CssClass="form-control form-control-sm" TabIndex="12"
                                                            style="text-transform :uppercase" ></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvTCCompra" runat="server" 
                                                    ControlToValidate="txtTCCompra" EnableClientScript="False"
                                                    ErrorMessage="Ingrese el tipo de cambio" Font-Size="10px" ForeColor="Red"
                                                    ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-md-3 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">T.C. Venta:</label>
                                        <div class="col-md-9 pt-md-2 pt-sm-1">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtTCVenta" runat="server" CssClass="form-control form-control-sm" TabIndex="13"
                                                    style="text-transform :uppercase" ></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvTCVenta" runat="server" 
                                                    ControlToValidate="txtTCCompra" EnableClientScript="False"
                                                    ErrorMessage="Ingrese el tipo de cambio" Font-Size="10px" ForeColor="Red"
                                                    ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
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