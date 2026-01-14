<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmGnrlMntTablaSistema.aspx.vb" Inherits="pryCMMS.frmGnrlMntTablaSistema" %>
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
                    <li class="nav-item"><a class="nav-link active" href="#tab1" aria-controls="tab1" role="tab" data-toggle="tab">Listado de Tablas del Sistema</a></li>
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
                                    var prm = Sys.WebForms.PageRequestManager.getInstance();
                                    prm.add_beginRequest(BeginRequestHandler);
                                    prm.add_endRequest(EndRequestHandler);
                                    function BeginRequestHandler(sender, args) {
                                        xPos = $get("divGridParametros").scrollLeft;
                                        yPos = $get("divGridParametros").scrollTop;
                                    }
                                    function EndRequestHandler(sender, args) {
                                        $get("divGridParametros").scrollLeft = xPos;
                                        $get("divGridParametros").scrollTop = yPos;
                                    }
                                </script>
                                <%--Final:Se utiliza para que al seleccionar la grilla no se posicione al inicio--%>
                                <asp:HiddenField ID="hfdOperacion" runat="server" />
                                <asp:HiddenField ID="hfdEstado" runat="server" />
                                <asp:Panel ID="pnlListado" runat="server" CssClass="container" 
                                    DefaultButton="imgbtnBuscarTablaSistema">
                                    <div class="row pt-2">
                                        <span class="col-sm-2 col-form-label-sm pt-md-2">Periodo</span>
                                        <div class="col-sm-4">
                                            <div class="input-group">
                                                <asp:DropDownList ID="cboPeriodo" runat="server" AutoPostBack="True" 
                                                    class="form-select form-select-sm js-choice" style="text-transform : uppercase" 
                                                    TabIndex="1">
                                                    <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvIdPeriodo" runat="server" 
                                                    ControlToValidate="cboPeriodo" EnableClientScript="False" 
                                                    ErrorMessage="Ingrese el periodo" Font-Size="10px" ForeColor="Red">(*)</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <span class="col-sm-2 col-form-label-sm pt-md-2 pt-sm-1">Buscar por</span>
                                        <div class="col-sm-4 pt-md-2 pt-sm-1">
                                            <asp:DropDownList ID="cboFiltro" runat="server" CssClass="form-select form-select-sm js-choice" TabIndex="2">
                                                <asp:ListItem>SELECCIONE EL FILTRO</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-6 pt-md-2 pt-sm-1">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control form-control-sm" TabIndex="3"
                                                    style="text-transform :uppercase" placeholder="Ingrese Busqueda"></asp:TextBox>
                                                &nbsp;
                                                <asp:ImageButton ID="imgbtnBuscarTablaSistema" runat="server" Height="18px" TabIndex="3" CssClass="mt-1"
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
                                                        <asp:BoundField DataField="IdTablaSistema" HeaderText="Cod. Tabla Sistema" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Periodo" HeaderText="Periodo" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Valor" HeaderText="Valor" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:TemplateField HeaderStyle-CssClass="bg-200 text-900">
                                                                <ItemTemplate>
                                                                    <div class="bootstrap-switch bootstrap-switch-on bootstrap bootstrap-switch-animate bootstrap-switch-mini ">
                                                                        <div class="bootstrap-switch-container">
                                                                            <asp:LinkButton ID="lnkEstadoOn" runat="server" CommandName="Activar" CommandArgument='<%# Eval("IdTablaSistema") & "," & Eval("Codigo") & "," & Eval("Periodo") & "," & Eval("IdSistema") %>' CssClass="bootstrap-switch-handle-on bootstrap-switch-primary" Visible='<%# If(Eval("Estado") = "True", "True", "False") %>'>Activado</asp:LinkButton>
                                                                            <span class="bootstrap-switch-label">&nbsp;</span>
                                                                            <asp:LinkButton ID="lnkEstadoOff" runat="server" CommandName="Desactivar" CommandArgument='<%# Eval("IdTablaSistema") & "," & Eval("Codigo") & "," & Eval("Periodo") & "," & Eval("IdSistema") %>' CssClass="bootstrap-switch-handle-off bootstrap-switch-danger" Visible='<%# If(Eval("Estado") = "True", "False", "True") %>'>Anulado</asp:LinkButton>
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
                                        <h3 class="h5">Mantenimiento de Tablas del Sistema</h3>
                                    </div>
                                    <div class="mt-2">
                                        <div class="row">
                                            <div class="col-12 bg-dark p-2 mt-2">
                                                <h3 class="h5 text-white">Listado de Parámetros</h3>
                                            </div>
                                            <asp:UpdatePanel ID="updpnlSeleccionarElemento" runat="server">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlSeleccionarElemento" runat="server" CssClass="scrollable" ><%-- onscroll="fScrollMove('divGridParametros');" ScrollBars="Vertical" Height="220px">--%>
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div id="divGridParametros" style="overflow:auto; height:220px; align:left;" >
                                                                    <asp:GridView ID="grdParametros" runat="server" AutoGenerateColumns="False" TabIndex="10"
                                                                        GridLines="None" CssClass="table table-responsive-sm table-striped table-hover small" AllowPaging="True"
                                                                        EmptyDataText="No hay registros a visualizar" PageSize="1000" >
                                                                        <PagerStyle CssClass="pgr" />
                                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                        <AlternatingRowStyle CssClass="alt" />
                                                                        <Columns>
                                                                            <asp:CommandField ShowSelectButton="True" />
                                                                            <asp:BoundField DataField="IdTablaSistema" HeaderText="Cod. Tabla Sistema" />
                                                                            <asp:BoundField DataField="Codigo" HeaderText="Código" />
                                                                            <asp:BoundField DataField="Periodo" HeaderText="Periodo" />
                                                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                                                                            <asp:BoundField DataField="Valor" HeaderText="Valor" />
                                                                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                                        </Columns>
                                                                        <HeaderStyle CssClass="thead-dark" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="col-12 bg-dark p-2 mt-2">
                                                                    <h3 class="h5 text-white">Detalle</h3>
                                                                </div>
                                                                <div class="form-group row mt-2">
                                                                    <label class="col-md-3 text-black-50 col-form-label col-form-label-sm">Código:</label>
                                                                    <div class="col-md-3 col-sm-5">
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control form-control-sm" TabIndex="11"
                                                                                style="text-transform : uppercase"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvCodigo" runat="server" 
                                                                                ControlToValidate="txtCodigo" EnableClientScript="False" 
                                                                                ErrorMessage="Ingrese el código" Font-Size="10px" ForeColor="Red" 
                                                                                InitialValue="SELECCIONE DATO">(*)</asp:RequiredFieldValidator>
                                                                            <asp:HiddenField ID="hfdbEstadoEncryptado" runat="server" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <label class="col-md-3 text-black-50 col-form-label col-form-label-sm">Descripción:</label>
                                                                    <div class="col-md-9">
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control form-control-sm" TabIndex="12"
                                                                                style="text-transform : uppercase"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" 
                                                                                ControlToValidate="txtDescripcion" EnableClientScript="False" 
                                                                                ErrorMessage="Ingrese la descripción" Font-Size="10px" ForeColor="Red" 
                                                                                InitialValue="SELECCIONE DATO">(*)</asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <label class="col-md-3 text-black-50 col-form-label col-form-label-sm">Desc. Abreviada:</label>
                                                                    <div class="col-md-3">
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtDescripcionAbreviada" runat="server" CssClass="form-control form-control-sm" TabIndex="13"
                                                                                style="text-transform : uppercase"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvDescripcionAbreviada" runat="server" 
                                                                                ControlToValidate="txtDescripcionAbreviada" 
                                                                                EnableClientScript="False" ErrorMessage="Ingrese la descripción abreviada" 
                                                                                Font-Size="10px" ForeColor="Red" InitialValue="SELECCIONE DATO">(*)</asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>
                                                                    <label class="col-md-3 text-black-50 col-form-label col-form-label-sm">Valores:</label>
                                                                    <div class="col-md-3">
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtValor" runat="server" CssClass="form-control form-control-sm" TabIndex="14"
                                                                                style="text-transform : uppercase" ></asp:TextBox>
                                                                            <asp:ImageButton ID="imgbtnCargarCertificado" runat="server" Height="25px" Width="25px" onerror="this.src='Imagenes/Usr/Sin_Imagen.jpg'" ImageUrl="Imagenes/25x25/Cargar_Certificado.png" alt="Certificado Digital"/>
                                                                            <asp:RequiredFieldValidator ID="rfvValor" runat="server" 
                                                                                ControlToValidate="txtValor" EnableClientScript="False" 
                                                                                ErrorMessage="Ingrese el valor" Font-Size="10px" ForeColor="Red" 
                                                                                InitialValue="SELECCIONE DATO">(*)</asp:RequiredFieldValidator>
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
                                </asp:Panel>
                                <asp:LinkButton ID="lnk_mostrarPanelCertificado" runat="server"></asp:LinkButton>
                                <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelCertificado_ModalPopupExtender" runat="server" 
                                    BackgroundCssClass="FondoAplicacion" CancelControlID="imgbtnCancelarCertificado" DynamicServicePath="" Enabled="True" 
                                    PopupControlID="pnlSeleccionarCertificado" TargetControlID="lnk_mostrarPanelCertificado">
                                </ajaxToolkit:ModalPopupExtender>
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
    <asp:Panel ID="pnlSeleccionarCertificado" runat="server" CssClass="container">
        <div class="modal-dialog modal-xl">
            <div class="shadow rounded">
                <div class="modal-dialog-scrollable">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title" id="myModalLabel">
                                Seleccionar Certificado Digital</h4>
                            <asp:Button ID="imgbtnCancelarCertificado" runat="server" Text="&times;" CssClass="close bg-transparent border-0" data-dismiss="modal" aria-label="Close" />
                        </div>
                        <div class="modal-body">
                            <div class="form-group row pt-2">
                                <span class="col-sm-2 col-form-label-sm">Buscar por</span>
                                <div class="col-sm-4">
                                    <asp:FileUpload ID="fupCertificado" runat="server" EnableTheming="True" CssClass="form-control-sm" TabIndex="90" />
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div class="form-group row">
                                <div class="col-md-12">
                                    <asp:Label ID="lblMensajeSeleccionarCertificado" runat="server" CssClass="col-form-label col-form-label-sm text-danger"></asp:Label>
                                </div>
                            </div>
                            <asp:Button ID="btnCargarCertificado" runat="server" Text="Cargar Certificado" CssClass="btn btn-primary btn-default" TabIndex ="91" />
                        </div>
                    </div>
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

        function CambiaFoco(cajadestino) {
            var key = window.event.keyCode;
            if (key == 13)
                document.getElementById(cajadestino).focus();
        }
    </script>
</asp:Content>