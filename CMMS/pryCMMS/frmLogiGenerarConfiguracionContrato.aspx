<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/PM_Cmms.Master"
    CodeBehind="frmLogiGenerarConfiguracionContrato.aspx.vb" Inherits="pryCMMS.frmLogiGenerarConfiguracionContrato" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

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
            position: sticky;
            z-index: 100;
            top: 65px;
        }

            #BarraHTML ul {
                list-style-type: none;
            }

            #BarraHTML li {
                display: inline;
                text-align: center;
                margin: 0 0 0 0;
            }

                #BarraHTML li a {
                    padding: 2px 7px 2px 7px;
                    text-decoration: none;
                }

                    #BarraHTML li a:hover {
                        background-color: #333333;
                        color: #ffffff;
                    }

        #texto {
            padding: 60px 0 0 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cuerpo" runat="server">
    <link href="Content/Calendario.css" rel="stylesheet" type="text/css" />
    <link href="Content/CuadroDialogo.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jsUpdateProgress.js" type="text/javascript"></script>
    <link href="Content/AutoCompletar.css" rel="stylesheet" type="text/css" />
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"
        EnableScriptGlobalization="True">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel ID="updpnlContent" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfdOperacion" runat="server" />
            <asp:HiddenField ID="hfdFocusObjeto" runat="server" />
            <asp:HiddenField ID="hfdEstado" runat="server" />
            <asp:HiddenField ID="hfdOperacionDetalle" runat="server" />
            <asp:HiddenField ID="hfdOperacionDetalleAdicional" runat="server" />
            <asp:HiddenField ID="hfdCorreoElectronicoCliente" runat="server" />
            <asp:HiddenField ID="hfdDNICliente" runat="server" />
            <asp:HiddenField ID="hfdRUCCliente" runat="server" />
            <asp:HiddenField ID="hfdFechaCreacionContrato" runat="server" />
            <asp:HiddenField ID="hfdIdUsuarioCreacionContrato" runat="server" />

            <asp:HiddenField ID="hfdIdEquipo" runat="server" />
            <asp:HiddenField ID="hfdIdEquipoSAPEquipo" runat="server" />
            <asp:HiddenField ID="hfdIdCatalogoEquipo" runat="server" />
            <asp:HiddenField ID="hfdIdTipoActivoEquipo" runat="server" />
            <asp:HiddenField ID="hfdJerarquiaEquipo" runat="server" />
            <asp:HiddenField ID="hfdIdOrdenTrabajo" runat="server" />
            <asp:HiddenField ID="hfdIdOrdenTrabajoCliente" runat="server" />
            <asp:HiddenField ID="hfdFechaPlanificacion" runat="server" />
            <asp:Panel ID="pnlCabecera" runat="server">
                <div class="card mb-3">
                    <div class="card-header">
                        <div class="row flex-between-end">
                            <div class="col-auto align-self-center">
                                <div class="nav nav-pills gap-2" role="tablist">
                                    <h5 class="mb-0" data-anchor="data-anchor">Configuración</h5>
                                    <a class="btn btn-falcon-default btn-sm ml-auto float-right"
                                        href="frmBienvenida.aspx" aria-expanded="true">
                                        <span class="fas fa-home"></span><span
                                            class="nav-link-text ps-1">Inicio</span>
                                    </a>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="card-body">
                        <div class="row bg-100 p-2">
                            <div class="col-lg-6 col-sm-6">
                                <span class="fas fa-filter"></span>
                                <label class="text-black-50 col-form-label fs--2 m-1">Contrato</label>
                                <div class="input-group">
                                    <asp:DropDownList ID="cboFiltroContrato" runat="server"
                                        CssClass="form-select form-select-sm js-choice" TabIndex="1">
                                        <asp:ListItem>SELECCIONE EL FILTRO</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                        </div>
                        <div class="row bg-100 p-2">
                            <div class="col-lg-4 col-md-7 col-sm-6 mt-2">
                                <span class="far fa-address-card"></span>
                                <label
                                    class="text-black-50 col-form-label fs--2 m-1">
                                    Tipo de Mantenimiento</label>
                                <div class="input-group">
                                    <asp:DropDownList
                                        ID="cboTipoMantenimientoMantenimientoOrdenTrabajo"
                                        runat="server" AutoPostBack="True"
                                        class="form-select form-select-sm js-choice"
                                        Style="text-transform: uppercase" TabIndex="11">
                                        <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator
                                        ID="rfvTipoMantenimientoMantenimientoOrdenTrabajo"
                                        runat="server"
                                        ControlToValidate="cboTipoMantenimientoMantenimientoOrdenTrabajo"
                                        EnableClientScript="False"
                                        ErrorMessage="Ingrese el tipo de mantenimiento"
                                        Font-Size="10px" ForeColor="Red"
                                        ValidationGroup="vgrpValidarMantenimientoOrdenTrabajo">
                                                                        (*)</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 mt-2">
                                <span class="far fa-clone"></span>
                                <label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Tipo Activo:</label>
                                <div class="input-group">
                                    <asp:DropDownList ID="cboTipoActivo" runat="server" AppendDataBoundItems="True"
                                        AutoPostBack="True" CssClass="form-select form-select-sm js-choice"
                                        Style="text-transform: uppercase" TabIndex="13">
                                        <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 mt-2">
                                <span class="far fa-clone"></span>
                                <label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Catalogo Principal:</label>
                                <div class="input-group">
                                    <asp:DropDownList ID="cboCatalogo" runat="server" AppendDataBoundItems="True"
                                        AutoPostBack="True" CssClass="form-select form-select-sm js-choice"
                                        Style="text-transform: uppercase" TabIndex="14">
                                        <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCatalogo" runat="server"
                                        ControlToValidate="cboCatalogo" Display="Static" EnableClientScript="False"
                                        ErrorMessage="Ingrese el catalogo" Font-Size="10px" ForeColor="Red"
                                        InitialValue="SELECCIONE DATO" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row bg-100 p-2">
                            <div class="col-lg-4 col-md-7 col-sm-6 mt-2">
                                <span class="far fa-address-card"></span>
                                <label
                                    class="text-black-50 col-form-label fs--2 m-1">
                                    Plantilla CheckList</label>
                                <div class="input-group">
                                    <asp:DropDownList
                                        ID="cboListadoCheckListMantenimientoOrdenTrabajo"
                                        runat="server" AutoPostBack="True"
                                        class="form-select form-select-sm js-choice"
                                        Style="text-transform: uppercase" TabIndex="12">
                                        <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator
                                        ID="rfvListadoCheckListMantenimientoOrdenTrabajo"
                                        runat="server"
                                        ControlToValidate="cboListadoCheckListMantenimientoOrdenTrabajo"
                                        EnableClientScript="False"
                                        ErrorMessage="Ingrese el tipo de plantilla checklist"
                                        Font-Size="10px" ForeColor="Red"
                                        ValidationGroup="vgrpValidarMantenimientoOrdenTrabajo">
                                                                        (*)</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-7 col-sm-6 mt-2">
                                <span class="far fa-address-card"></span>
                                <label
                                    class="text-black-50 col-form-label fs--2 m-1">
                                    Tipo de Frecuencia</label>
                                <div class="input-group">
                                    <asp:DropDownList
                                        ID="cboTipoFrecuencia"
                                        runat="server" AutoPostBack="True"
                                        class="form-select form-select-sm js-choice"
                                        Style="text-transform: uppercase" TabIndex="12">
                                        <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator1"
                                        runat="server"
                                        ControlToValidate="cboListadoCheckListMantenimientoOrdenTrabajo"
                                        EnableClientScript="False"
                                        ErrorMessage="Ingrese el tipo el tipo de frecuencia"
                                        Font-Size="10px" ForeColor="Red"
                                        ValidationGroup="vgrpValidarMantenimientoOrdenTrabajo">
                                                                        (*)</asp:RequiredFieldValidator>
                                </div>
                            </div>
                             <div class="col-lg-4 col-sm-4 mt-2">
                                <span class="far fa-address-card"></span>
                                <label class="text-black-50 col-form-label fs--2 m-1">Frecuencia</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtBuscarContrato" runat="server"
                                        CssClass="form-control form-control-sm" TabIndex="2"
                                        Style="text-transform: uppercase" placeholder="Ingrese Frecuencia">
                                    </asp:TextBox>
                                    &nbsp;
                                </div>
                            </div>
                        </div>

                        <div class="kanban-items-container scrollbar">
                            <div class="row">
                                <div class="col-md-12 mt-3">
                                    <div class="table-responsive">
                                        <asp:GridView ID="grdLista" runat="server" AutoGenerateColumns="False"
                                            TabIndex="4" DataKeyNames="IdNumeroCorrelativo" GridLines="None"
                                            CssClass="table table-responsive-sm table-hover small"
                                            AllowPaging="True" EmptyDataText="No hay registros a visualizar"
                                            PageSize="14">
                                            <PagerStyle CssClass="pgr" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True"
                                                ForeColor="#333333" />
                                            <AlternatingRowStyle CssClass="alt" />
                                            <Columns>
                                                <asp:BoundField DataField="IdTipoDocumento"
                                                    HeaderText="Tip.Doc."
                                                    HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" />
                                                <asp:BoundField DataField="IdNumeroSerie" HeaderText="Nro.Serie"
                                                    HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" />
                                                <asp:BoundField DataField="IdNumeroCorrelativo"
                                                    HeaderText="Nro.Correlativo"
                                                    HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" />
                                                <asp:BoundField DataField="FechaEmision"
                                                    HeaderText="Fec.Emisión" DataFormatString="{0:dd/MM/yyyy}"
                                                    HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" />
                                                <asp:BoundField DataField="IdCliente" HeaderText="Id.Cliente"
                                                    HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" />
                                                <asp:BoundField DataField="RucCliente" HeaderText="RUC"
                                                    HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" />
                                                <asp:BoundField DataField="RazonSocialCliente"
                                                    HeaderText="Razón Social"
                                                    HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" />
                                                <asp:TemplateField HeaderText="Estado" ShowHeader="false"
                                                    HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"
                                                    ItemStyle-CssClass="small">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEstado" runat="server"
                                                            Visible='<%# If(Eval("StatusContrato") = "", "false", "true") %>'
                                                            CssClass='<%#If(Eval("StatusContrato") = "R", "texto_estado short_estado black borde_redondo1", If(Eval("StatusContrato") = "T", "texto_estado short_estado verde borde_redondo1", "texto_estado short_estado orange borde_redondo1")) %>'
                                                            Text='<%# StrConv(IIf(Eval("StatusContrato") = "P", "PENDIENTE", IIf(Eval("StatusContrato") = "T", "TERMINADO", "REGISTRADO")), VbStrConv.ProperCase) %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Estado" HeaderText="Estado"
                                                    HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" />
                                                <asp:BoundField DataField="DescripcionCabeceraContrato"
                                                    HeaderText="Descripción Contrato"
                                                    HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" />

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
            <div class="position-fixed bottom-0 end-0 p-3">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" role="alert"
                    CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidar" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <link href="cssbootstrap/bootstrap-switch.css" rel="stylesheet" />
    <script src="jsbootstrap/bootstrap-switch.js"></script>
</asp:Content>
