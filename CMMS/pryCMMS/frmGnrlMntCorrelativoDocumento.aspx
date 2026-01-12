<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmGnrlMntCorrelativoDocumento.aspx.vb" Inherits="pryCMMS.frmGnrlMntCorrelativoDocumento" %>
<%--<%@ Register Assembly="DevExpress.Web.Bootstrap.v17.1, Version=17.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>--%>
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
                    <li class="nav-item"><a class="nav-link active" href="#tab1" aria-controls="tab1" role="tab" data-toggle="tab">Listado de Correlativos / Documentos</a></li>
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
                                    DefaultButton="imgbtnBuscarCorrelativoDocumento">
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
                                                <asp:ImageButton ID="imgbtnBuscarCorrelativoDocumento" runat="server" Height="18px" TabIndex="3" CssClass="mt-1"
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
                                                        <asp:BoundField DataField="TipDoc" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="NroDesde" HeaderText="Correlativo Desde" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="NroHasta" HeaderText="Correlativo Hasta" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Serie" HeaderText="Serie" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="NroActual" HeaderText="Correlativo Actual" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="FacturacionElectronica" HeaderText="Facturación Electrónica" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="TipDocRef" HeaderText="Tipo Doc. Ref." HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900"/>
                                                        <asp:TemplateField HeaderStyle-CssClass="bg-200 text-900">
                                                            <ItemTemplate>
                                                                <div class="bootstrap-switch bootstrap-switch-on bootstrap bootstrap-switch-animate bootstrap-switch-mini ">
                                                                    <div class="bootstrap-switch-container">
                                                                        <asp:LinkButton ID="lnkEstadoOn" runat="server" CommandName="Activar" CommandArgument='<%# Eval("TipDoc") & "," & Eval("Serie") %>' CssClass="bootstrap-switch-handle-on bootstrap-switch-primary" Visible='<%# If(Eval("Estado") = "True", "True", "False") %>'>Activado</asp:LinkButton>
                                                                        <span class="bootstrap-switch-label">&nbsp;</span>
                                                                        <asp:LinkButton ID="lnkEstadoOff" runat="server" CommandName="Desactivar" CommandArgument='<%# Eval("TipDoc") & "," & Eval("Serie") %>' CssClass="bootstrap-switch-handle-off bootstrap-switch-danger" Visible='<%# If(Eval("Estado") = "True", "False", "True") %>'>Anulado</asp:LinkButton>
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
                                        <h3 class="h5">Mantenimiento de Correlativo por Documentos</h3>
                                    </div>
                                    <div class="mt-2">
                                        <div class="row">
                                            <label class="col-lg-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Tipo Documento:</label>
                                            <div class="col-lg-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:DropDownList ID="cboTipoDoc" runat="server" AppendDataBoundItems="True" TabIndex="11"
                                                        CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase">
                                                        <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvTipoDocumento" runat="server" 
                                                        ControlToValidate="cboTipoDoc" EnableClientScript="False" InitialValue="SELECCIONE DATO"
                                                        ErrorMessage="Ingrese el punto de venta" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-lg-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Nro. Serie:</label>
                                            <div class="col-lg-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtNroSerie" runat="server" CssClass="form-control form-control-sm" TabIndex="12"
                                                        style="text-transform :uppercase"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvNroSerie" runat="server" 
                                                        ControlToValidate="txtNroSerie" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese el número de serie" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <label class="col-lg-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Nro. Corr. Actual:</label>
                                            <div class="col-lg-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtNroCorrelativoActual" runat="server" CssClass="form-control form-control-sm" TabIndex="13"
                                                        style="text-transform :uppercase"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvNroCorrelativoActual" runat="server" 
                                                        ControlToValidate="txtNroCorrelativoActual" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese el número de correlativo actual" Font-Size="10px" ForeColor="Red" 
                                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-lg-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Nro. Corr. Desde:</label>
                                            <div class="col-lg-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtNroCorrelativoDesde" runat="server" CssClass="form-control form-control-sm" TabIndex="14"
                                                        style="text-transform :uppercase"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="frvNroCorrelativoDesde" runat="server" 
                                                        ControlToValidate="txtNroCorrelativoDesde" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese el número de correlativo inicial" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <label class="col-lg-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Nro. Corr. Hasta:</label>
                                            <div class="col-lg-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtNroCorrelativoHasta" runat="server" CssClass="form-control form-control-sm" TabIndex="15"
                                                        style="text-transform :uppercase"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="frvNroCorrelativoHasta" runat="server" 
                                                        ControlToValidate="txtNroCorrelativoHasta" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese el número de correlativo inicial" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-lg-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Fact. Electrónica:</label>
                                            <div class="col-lg-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:CheckBox ID="chkFacturacionElectronica" runat="server" AutoPostBack="True" CssClass="form-check" TabIndex="16" />
                                                </div>
                                            </div>
                                            <label class="col-lg-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Fecha Vigencia:</label>
                                            <div class="col-lg-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtFechaVigencia" runat="server" CssClass="form-control form-control-sm" TabIndex="17"
                                                        style="text-transform :uppercase" ></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtFechaVigencia_CalendarExtender" runat="server" 
                                                        Enabled="True" TargetControlID="txtFechaVigencia">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvFechaVigencia" runat="server" 
                                                        ControlToValidate="txtFechaVigencia" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese la fecha de vigencia" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-lg-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Nro. Digito Serie:</label>
                                            <div class="col-lg-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtNroDigitoSerie" runat="server" CssClass="form-control form-control-sm" TabIndex="18"
                                                        style="text-transform :uppercase"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvNroDigitoSerie" runat="server" 
                                                        ControlToValidate="txtNroDigitoSerie" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese el número de digitos de la serie" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <label class="col-lg-2 text-black-50 col-form-label col-form-label-sm pt-md-2 pt-sm-1">Nro. Digito Doc.:</label>
                                            <div class="col-lg-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtNroDigitoDoc" runat="server" CssClass="form-control form-control-sm" TabIndex="19"
                                                        style="text-transform :uppercase"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvNroDigitoDoc" runat="server" 
                                                        ControlToValidate="txtNroDigitoDoc" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese el número de dígitos del documento" Font-Size="10px" ForeColor="Red" 
                                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <asp:Label ID="lblTipoDocumentoReferencial" runat="server" CssClass="col-lg-2 text-danger col-form-label col-form-label-sm pt-md-2 pt-sm-1" Text="Tipo Doc. Refer."></asp:Label>
                                            <div class="col-lg-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:DropDownList ID="cboTipoDocumentoReferencial" runat="server" 
                                                        AppendDataBoundItems="True" CssClass="form-select form-select-sm js-choice text-danger" TabIndex="19"
                                                        style="text-transform :uppercase" >
                                                        <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <asp:Label ID="lblSerieAlfanumerico" runat="server" CssClass="col-lg-2 text-danger col-form-label col-form-label-sm pt-md-2 pt-sm-1" Text="Ser.Alfanumér."></asp:Label>
                                            <div class="col-lg-4 pt-md-2 pt-sm-1">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtSerieAlfanumerico" runat="server" CssClass="form-control form-control-sm text-danger" TabIndex="20"
                                                        style="text-transform :uppercase"></asp:TextBox>
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