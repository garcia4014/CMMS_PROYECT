<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmLogiSolicitudServicio.aspx.vb" Inherits="pryCMMS.frmLogiSolicitudServicio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- ===============================================-->
    <!--    Stylesheets-->
    <!-- ===============================================-->
    <%--Inicio: Este es para la galeria--%>
    <%--Final: Este es para la galeria--%>
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
    <script type="text/javascript" language="javascript">
        function deshabilitar(boton) {
            document.getElementById(boton).style.visibility = 'hidden';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cuerpo" runat="server">
    <link href="Content/Calendario.css" rel="stylesheet" type="text/css" />
    <link href="Content/CuadroDialogo.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jsUpdateProgress.js" type="text/javascript"></script>
    <link href="Content/AutoCompletar.css" rel="stylesheet" type="text/css" />
    <asp:UpdatePanel ID="updpnlContent" runat="server">
        <ContentTemplate>
            <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
            </ajaxToolkit:ToolkitScriptManager>
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
            <asp:HiddenField ID="hfdIdArticuloSAPPrincipal" runat="server" />
            <asp:HiddenField ID="hfdFechaCreacionEquipo" runat="server" />
            <asp:HiddenField ID="hfdIdUsuarioCreacionEquipo" runat="server" />
            <asp:HiddenField ID="hfdIdCatalogoCabecera" runat="server" />
            <asp:HiddenField ID="hfdIdTipoActivoCabecera" runat="server" />
            <asp:HiddenField ID="hfdIdJerarquiaCatalogoCabecera" runat="server" />
            <asp:Panel ID="pnlCabecera" runat="server">
                <div class="card mb-3">
                    <div class="card-header">
                        <div class="row flex-between-end">
                            <div class="col-auto align-self-center">
                                <div class="nav nav-pills gap-2" role="tablist">
                                    <h5 class="mb-0" data-anchor="data-anchor">Listado de Ordenes de Servicio</h5>
                                    <a class="btn btn-falcon-default btn-sm ml-auto float-right" href="frmBienvenida.aspx" aria-expanded="true">
                                        <span class="fas fa-home"></span><span class="nav-link-text ps-1">Inicio</span>
                                    </a>
                                </div>
                            </div>
                            <div class="col-auto ms-auto">
                                <div class="nav gap-2" role="tablist">
                                    <asp:LinkButton ID="btnAsignarEquipos" runat="server" CssClass="btn btn-primary btn-sm ml-auto float-right" Text="<span class='fas fa-plus'></span> Asignar Equipos" ToolTip="Asignar Equipos" Visible="false" />
                                    <asp:LinkButton ID="btnNuevo" runat="server" OnClick="btnNuevo_Click" CssClass="btn btn-success btn-sm ml-auto float-right" Text="<span class='fas fa-plus'></span> Generar O.Trabajo" ToolTip="Generar Orden de Trabajo" />
                                    <div class="dropdown font-sans-serif btn-reveal-trigger">
                                        <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesEquipo" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                                        <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesEquipo">
                                            <asp:LinkButton ID="lnkbtnVerOrdenServicio" runat="server" ToolTip="Imprimir Orden de Servicio" Text="<span class='fas fa-file me-2'></span>Ver O.Servicio" CssClass="nav-link nav-link-card-details" />
                                            <asp:LinkButton ID="lnkbtnVerDetalleOrdenTrabajoServicio" runat="server" ToolTip="Ver Detalle OT" Text="<span class='fas fa-file me-2'></span>Ver Detalle OT. Servicio" CssClass="nav-link nav-link-card-details" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="row bg-100 p-2">
                            <div class="col-lg-6 col-sm-6">
                                <span class="fas fa-filter"></span>
                                <label class="text-black-50 col-form-label fs--2 m-1">Filtrar por:</label>
                                <div class="input-group">
                                    <asp:DropDownList ID="cboFiltroEquipo" runat="server" CssClass="form-select form-select-sm js-choice" TabIndex="1">
                                        <asp:ListItem>SELECCIONE EL FILTRO</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-6">
                                <span class="far fa-address-card"></span>
                                <label class="text-black-50 col-form-label fs--2 m-1">Buscar por:</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtBuscarEquipo" runat="server" CssClass="form-control form-control-sm" TabIndex="2"
                                        Style="text-transform: uppercase" placeholder="Ingrese Busqueda"></asp:TextBox>
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
                                        <asp:GridView ID="grdLista" runat="server" AutoGenerateColumns="False" TabIndex="4" DataKeyNames="IdNumeroCorrelativo"
                                            GridLines="None" CssClass="table table-responsive-sm table-hover small" AllowPaging="True"
                                            EmptyDataText="No hay registros a visualizar" PageSize="14" OnRowCommand="grdLista_RowCommand">
                                            <PagerStyle CssClass="pgr" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <AlternatingRowStyle CssClass="alt" />
                                            <Columns>
                                                <asp:BoundField DataField="IdTipoDocumento" HeaderText="Tip.Doc." HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="IdNumeroSerie" HeaderText="Nro.Serie" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="IdNumeroCorrelativo" HeaderText="Nro.Correlativo" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="FechaEmision" HeaderText="Fec.Emisión" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="IdCliente" HeaderText="Id.Cliente" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="IdClienteSAP" HeaderText="Id. Cliente SAP" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="RucCliente" HeaderText="RUC" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="RazonSocial" HeaderText="Razón Social" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:TemplateField HeaderText="Id.Equipo" ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small">
                                                    <ItemTemplate>
                                                        <div class="col-12">
                                                            <asp:LinkButton ID="lnkbtnVerEquipo" runat="server" CommandName="VerEquipo" ToolTip="Ver Equipo" CommandArgument='<%# Eval("IdEquipo") %>' Text='<%# Eval("IdEquipo") %>' CssClass="p-1 m-0" ValidationGroup="vgrpValidar" />
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="IdEquipoSAP" HeaderText="Id.Equipo SAP" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="IdUnidadTrabajo" HeaderText="Unid.Trabajo" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:TemplateField HeaderText="Nro.Ref.SAP" ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small">
                                                    <ItemTemplate>
                                                        <div class="col-12">
                                                            <asp:Label ID="lblOrdenVenta" runat="server" Visible='<%# If(Eval("OrdenVenta") = "", "false", "true") %>' Text='<%# "OV: " + Eval("OrdenVenta") %>'></asp:Label>
                                                        </div>
                                                        <div class="col-12">
                                                            <asp:Label ID="lblOrdenCompra" runat="server" Visible='<%# If(Eval("OrdenCompra") = "", "false", "true") %>' Text='<%# "OC: " + Eval("OrdenCompra") %>'></asp:Label>
                                                        </div>
                                                        <div class="col-12">
                                                            <asp:Label ID="lblOrdenFabricacion" runat="server" Visible='<%# If(Eval("OrdenFabricacion") = "", "false", "true") %>' Text='<%# "OF: " + Eval("OrdenFabricacion") %>'></asp:Label>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DescripcionEquipo" HeaderText="Descripción Equipo" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="NumeroSerieEquipo" HeaderText="Nro. Serie Equipo" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="IdArticuloSAPCabecera" HeaderText="Articulo Principal" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:TemplateField HeaderText="Estado" ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEstado" runat="server" Visible='<%# If(Eval("StatusOrdenFabricacion") = "", "false", "true") %>' CssClass='<%#If(Eval("StatusOrdenFabricacion") = "R", "texto_estado short_estado black borde_redondo1", If(Eval("StatusOrdenFabricacion") = "T", "texto_estado short_estado verde borde_redondo1", "texto_estado short_estado orange borde_redondo1")) %>' Text='<%# StrConv(IIf(Eval("StatusOrdenFabricacion") = "P", "PENDIENTE", IIf(Eval("StatusOrdenFabricacion") = "T", "TERMINADO", "REGISTRADO")), VbStrConv.ProperCase) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
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
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidar" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updpnlMantenimientoOrdenTrabajo" runat="server">
        <ContentTemplate>
            <asp:LinkButton ID="lnk_mostrarPanelMantenimientoOrdenTrabajo" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender" runat="server"
                BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True"
                CancelControlID="btnCancelarMantenimientoOrdenTrabajo"
                PopupControlID="pnlMantenimientoOrdenTrabajo" TargetControlID="lnk_mostrarPanelMantenimientoOrdenTrabajo">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlMantenimientoOrdenTrabajo" runat="server" CssClass="container" DefaultButton="btnAceptarMantenimientoOrdenTrabajo">
                <div class="modal-dialog modal-xl">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header bg-light">
                                    <h4 class="modal-title" id="myModalLabelOrdenTrabajo">Mantenimiento Orden de Trabajo</h4>
                                    <asp:Button ID="imgbtnCancelarOrdenTrabajoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="scrollbar" style="height: 380px;">
                                            <div class="row">
                                                <div class="col-lg-2 col-md-5 col-sm-6 mt-2">
                                                    <span class="far fa-address-card"></span>
                                                    <label class="text-black-50 col-form-label fs--2 m-1">Fecha Inicio</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtFechaInicioPlanificadaMantenimientoOrdenTrabajo" runat="server" CssClass="form-control form-control-sm"
                                                            Style="text-transform: uppercase" TabIndex="18" autocomplete="off" type="date"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                                            ControlToValidate="txtFechaInicioPlanificadaMantenimientoOrdenTrabajo" EnableClientScript="False"
                                                            ErrorMessage="Ingrese la fecha de inicio de planificación" Font-Size="10px" ForeColor="Red"
                                                            ValidationGroup="vgrpValidarCrearProducto">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-lg-2 col-md-5 col-sm-6 mt-2">
                                                    <span class="far fa-address-card"></span>
                                                    <label class="text-black-50 col-form-label fs--2 m-1">Fecha Termino</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo" runat="server" CssClass="form-control form-control-sm"
                                                            Style="text-transform: uppercase" TabIndex="18" autocomplete="off" type="date"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                            ControlToValidate="txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo" EnableClientScript="False"
                                                            ErrorMessage="Ingrese la fecha de termino de planificación" Font-Size="10px" ForeColor="Red"
                                                            ValidationGroup="vgrpValidarCrearProducto">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-lg-4 col-md-7 col-sm-6 mt-2">
                                                    <span class="far fa-address-card"></span>
                                                    <label class="text-black-50 col-form-label fs--2 m-1">Tipo de Mantenimiento</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboTipoMantenimientoMantenimientoOrdenTrabajo" runat="server" AutoPostBack="True"
                                                            class="form-select form-select-sm js-choice" Style="text-transform: uppercase"
                                                            TabIndex="11">
                                                            <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvTipoMantenimientoMantenimientoOrdenTrabajo" runat="server"
                                                            ControlToValidate="cboTipoMantenimientoMantenimientoOrdenTrabajo" EnableClientScript="False"
                                                            ErrorMessage="Ingrese el tipo de mantenimiento" Font-Size="10px" ForeColor="Red"
                                                            ValidationGroup="vgrpValidarMantenimientoOrdenTrabajo">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-lg-4 col-md-7 col-sm-6 mt-2">
                                                    <span class="far fa-address-card"></span>
                                                    <label class="text-black-50 col-form-label fs--2 m-1">Plantilla CheckList</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboListadoCheckListMantenimientoOrdenTrabajo" runat="server" AutoPostBack="True"
                                                            class="form-select form-select-sm js-choice" Style="text-transform: uppercase"
                                                            TabIndex="12">
                                                            <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvListadoCheckListMantenimientoOrdenTrabajo" runat="server"
                                                            ControlToValidate="cboListadoCheckListMantenimientoOrdenTrabajo" EnableClientScript="False"
                                                            ErrorMessage="Ingrese el tipo de plantilla checklist" Font-Size="10px" ForeColor="Red"
                                                            ValidationGroup="vgrpValidarMantenimientoOrdenTrabajo">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-lg-4 col-md-12 col-sm-6 mt-2 mb-2">
                                                    <span class="far fa-address-card"></span>
                                                    <label class="text-black-50 col-form-label fs--2 m-1">Responsable</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboPersonalResponsableMantenimientoOrdenTrabajo" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                                            class="form-select form-select-sm js-choice" Style="text-transform: uppercase"
                                                            TabIndex="13">
                                                            <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvPersonalResponsableMantenimientoOrdenTrabajo" runat="server"
                                                            ControlToValidate="cboPersonalResponsableMantenimientoOrdenTrabajo" EnableClientScript="False"
                                                            ErrorMessage="Ingrese el personal responsable" Font-Size="10px" ForeColor="Red"
                                                            ValidationGroup="vgrpValidarMantenimientoOrdenTrabajo">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-lg-4 col-md-12 col-sm-6 mt-2 mb-2">
                                                    <span class="far fa-address-card"></span>
                                                    <label class="text-black-50 col-form-label fs--2 m-1">Control de Tiempo</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboTipoControlTiempoOrdenTrabajoMantenimientoOrdenTrabajo" runat="server" AppendDataBoundItems="True"
                                                            class="form-select form-select-sm js-choice" Style="text-transform: uppercase"
                                                            TabIndex="14">
                                                            <asp:ListItem Value="D">POR ORDEN DE TRABAJO</asp:ListItem>
                                                            <asp:ListItem Value="C">POR COMPONENTES</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvTipoControlTiempoOrdenTrabajoMantenimientoOrdenTrabajo" runat="server"
                                                            ControlToValidate="cboTipoControlTiempoOrdenTrabajoMantenimientoOrdenTrabajo" EnableClientScript="False"
                                                            ErrorMessage="Ingrese el tipo control de tiempo de orden" Font-Size="10px" ForeColor="Red"
                                                            ValidationGroup="vgrpValidarMantenimientoOrdenTrabajo">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row bg-200">
                                                <div class="col-lg-12 col-md-12 col-sm-12 mt-2">
                                                    <span class="far fa-address-card"></span>
                                                    <label class="text-black-50 col-form-label fs--2 m-1">Personal Asignado</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboPersonalAsignadoMantenimientoOrdenTrabajo" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                                            class="form-select form-select-sm js-choice" Style="text-transform: uppercase"
                                                            TabIndex="14">
                                                            <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btnAdicionarPersonalAsignadoMantenimientoOrdenTrabajo" runat="server" Text="Agregar" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Personal" TabIndex="30" />
                                                        <asp:RequiredFieldValidator ID="rfvPersonalAsignadoMantenimientoOrdenTrabajo" runat="server"
                                                            ControlToValidate="cboPersonalAsignadoMantenimientoOrdenTrabajo" EnableClientScript="False"
                                                            ErrorMessage="Ingrese el personal asignado" Font-Size="10px" ForeColor="Red"
                                                            ValidationGroup="vgrpValidarMantenimientoOrdenTrabajo">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 mt-2">
                                                    <div id="divGridPersonalAsignado" style="overflow: auto; height: 220px; align: left;">
                                                        <asp:GridView ID="grdPersonalAsignadoMantenimientoOrdenTrabajo" runat="server" AutoGenerateColumns="False" TabIndex="17"
                                                            GridLines="None" CssClass="table table-responsive-sm table-striped table-hover small" AllowPaging="True"
                                                            EmptyDataText="No hay registros a visualizar" PageSize="1000">
                                                            <PagerStyle CssClass="pgr" />
                                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:CommandField ShowDeleteButton="True" HeaderStyle-Width="70px" />
                                                                <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900" />
                                                                <asp:BoundField DataField="Personal" HeaderText="Descripción" HeaderStyle-CssClass="bg-200 text-900" />
                                                                <asp:BoundField DataField="Responsable" HeaderText="Responsable" HeaderStyle-CssClass="bg-200 text-900" />
                                                            </Columns>
                                                            <HeaderStyle CssClass="thead-dark" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <div class="container">
                                        <div class="row">
                                            <div class="col-md-8 mt-2">
                                                <asp:ValidationSummary ID="ValidationSummary2" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarMantenimientoOrdenTrabajo" />
                                            </div>
                                            <div class="col-md-4 text-end p-2">
                                                <asp:Button ID="btnAceptarMantenimientoOrdenTrabajo" runat="server" ValidationGroup="vgrpValidarMantenimientoOrdenTrabajo" OnClientClick="deshabilitar(this.id)"
                                                    ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                &nbsp;
                                                <asp:Button ID="btnCancelarMantenimientoOrdenTrabajo" runat="server"
                                                    ToolTip="Cancelar Registro" TabIndex="73" Text="Cancelar" CssClass="btn btn-outline-google-plus btn-sm" />
                                            </div>
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

    <asp:UpdatePanel ID="updpnlMntOTxEquipos" runat="server">
        <ContentTemplate>
            <asp:LinkButton ID="lnk_mostrarPanelMntOTxEquipos" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMntOTxEquipos_ModalPopupExtender" runat="server"
                BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True"
                CancelControlID="btnCancelarMntOtxEquipos"
                PopupControlID="pnlMntOTxEquipos" TargetControlID="lnk_mostrarPanelMntOTxEquipos">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlMntOTxEquipos" runat="server" CssClass="container" DefaultButton="btnAceptarMntOTxEquipos">
                <div class="modal-dialog modal-xl">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header bg-light">
                                    <%--<h4 class="modal-title" id="myModalLabelOTxEquipos">Mantenimiento Orden de Trabajo</h4>--%>
                                    <%--<asp:Button ID="imgbtnCancelarOrdenTrabajoImagenx" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" /> --%>
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="scrollbar" style="height: 380px;">
                                            <div class="row mt-3">
                                                <div class="col-12 mb-0 bg-100">
                                                    <h6 class="mt-2 mb-1">Datos del Equipo</h6>
                                                    <hr class="bg-300 m-0" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-3 col-md-6 mt-2">
                                                    <span class="far fa-clone"></span>
                                                    <label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Tipo Activo:</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboTipoActivo" runat="server" AppendDataBoundItems="True"
                                                            AutoPostBack="True" CssClass="form-select form-select-sm js-choice"
                                                            Style="text-transform: uppercase" TabIndex="18">
                                                            <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-lg-3 col-md-6 mt-2">
                                                    <span class="far fa-clone"></span>
                                                    <label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Catalogo Principal:</label>
                                                    <div class="input-group">
                                                        <asp:HiddenField ID="hfdIdCatalogoAnterior" runat="server" />
                                                        <asp:DropDownList ID="cboCatalogo" runat="server" AppendDataBoundItems="True" onchange="guardarValor()"
                                                            AutoPostBack="True" CssClass="form-select form-select-sm js-choice"
                                                            Style="text-transform: uppercase" TabIndex="19">
                                                            <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                                        </asp:DropDownList>

                                                        <asp:RequiredFieldValidator ID="rfvCatalogo" runat="server"
                                                            ControlToValidate="cboCatalogo" Display="Static" EnableClientScript="False"
                                                            ErrorMessage="Ingrese el catalogo" Font-Size="10px" ForeColor="Red"
                                                            InitialValue="SELECCIONE DATO" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>

                                                    </div>
                                                </div>
                                                <div class="col-lg-3 col-md-6 mt-2">
                                                    <span class="far fa-clone"></span>
                                                    <label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Tipo Equipo:</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboTipoEquipo" runat="server" AppendDataBoundItems="True"
                                                            AutoPostBack="True" CssClass="form-select form-select-sm js-choice"
                                                            Style="text-transform: uppercase" TabIndex="16">
                                                            <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-lg-3 col-md-6 mt-2">
                                                    <span class="far fa-clone"></span>
                                                    <label class="text-black-50 col-form-label fs--2 m-1">Cantidad de Equipos</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtCantidadEquipos" runat="server" CssClass="form-control form-control-sm" TabIndex="2"
                                                            Style="text-transform: uppercase" placeholder="Ingrese Cantidad"></asp:TextBox>
                                                        &nbsp;
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-3 col-md-4 mt-2">
                                                    <span class="far fa-address-card"></span>
                                                    <label class="text-black-50 col-form-label fs--2 m-1">Código Cliente:</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtIdCliente" runat="server" CssClass="form-control form-control-sm" TabIndex="23"></asp:TextBox><%--</td>--%>
                                                        <asp:LinkButton ID="btnAdicionarCliente" runat="server" CssClass="btn btn-primary btn-sm" Text="<span class='fas fa-binoculars'></span>" ToolTip="Crear Cliente" />
                                                        <asp:RequiredFieldValidator ID="rfvIdCliente" runat="server"
                                                            ControlToValidate="txtIdCliente" EnableClientScript="False"
                                                            ErrorMessage="Ingrese el código del cliente" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                        <input type="hidden" runat="server" id="hfdIdTipoDocumentoCliente" />

                                                    </div>
                                                </div>
                                                <div class="col-lg-5 col-md-4 mt-2">
                                                    <span class="far fa-address-card"></span>
                                                    <label class="text-black-50 col-form-label fs--2 m-1">Razón Social:</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="form-control form-control-sm"
                                                            Enabled="False" TabIndex="24" Style="text-transform: uppercase" placeholder="Razón Social"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvRazonSocial" runat="server"
                                                            ControlToValidate="txtRazonSocial" EnableClientScript="False"
                                                            ErrorMessage="Ingrese la razón social" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="row mt-3">
                                                <div class="col-12 mb-0 bg-100">
                                                    <h6 class="mt-2 mb-1">Orden de Trabajo</h6>
                                                    <hr class="bg-300 m-0" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-2 col-md-5 col-sm-6 mt-2">
                                                    <span class="far fa-calendar"></span>
                                                    <label class="text-black-50 col-form-label fs--2 m-1">Fecha Inicio</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtFechaInicioPlanificadaMntOTxEquipos" runat="server" CssClass="form-control form-control-sm"
                                                            Style="text-transform: uppercase" TabIndex="18" autocomplete="off" type="date"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                            ControlToValidate="txtFechaInicioPlanificadaMntOTxEquipos" EnableClientScript="False"
                                                            ErrorMessage="Ingrese la fecha de inicio de planificación" Font-Size="10px" ForeColor="Red"
                                                            ValidationGroup="vgrpValidarCrearProducto">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-lg-2 col-md-5 col-sm-6 mt-2">
                                                    <span class="far fa-calendar"></span>
                                                    <label class="text-black-50 col-form-label fs--2 m-1">Fecha Termino</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtFechaTerminoPlanificadaMntOTxEquipos" runat="server" CssClass="form-control form-control-sm"
                                                            Style="text-transform: uppercase" TabIndex="18" autocomplete="off" type="date"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                            ControlToValidate="txtFechaTerminoPlanificadaMntOTxEquipos" EnableClientScript="False"
                                                            ErrorMessage="Ingrese la fecha de termino de planificación" Font-Size="10px" ForeColor="Red"
                                                            ValidationGroup="vgrpValidarCrearProducto">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-lg-4 col-md-7 col-sm-6 mt-2">
                                                    <span class="far fa-clone"></span>
                                                    <label class="text-black-50 col-form-label fs--2 m-1">Tipo de Mantenimiento</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboTipoMantenimientoMntOTxEquipos" runat="server" AutoPostBack="True"
                                                            class="form-select form-select-sm js-choice" Style="text-transform: uppercase"
                                                            TabIndex="11">
                                                            <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvTipoMantenimientoMntOTxEquipos" runat="server"
                                                            ControlToValidate="cboTipoMantenimientoMntOTxEquipos" EnableClientScript="False"
                                                            ErrorMessage="Ingrese el tipo de mantenimiento" Font-Size="10px" ForeColor="Red"
                                                            ValidationGroup="vgrpValidarMntOTxEquipos">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-lg-4 col-md-7 col-sm-6 mt-2">
                                                    <span class="far fa-clone"></span>
                                                    <label class="text-black-50 col-form-label fs--2 m-1">Plantilla CheckList</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboListadoCheckListMntOTxEquipos" runat="server" AutoPostBack="True"
                                                            class="form-select form-select-sm js-choice" Style="text-transform: uppercase"
                                                            TabIndex="12">
                                                            <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvListadoCheckListMntOTxEquipos" runat="server"
                                                            ControlToValidate="cboListadoCheckListMntOTxEquipos" EnableClientScript="False"
                                                            ErrorMessage="Ingrese el tipo de plantilla checklist" Font-Size="10px" ForeColor="Red"
                                                            ValidationGroup="vgrpValidarMntOTxEquipos">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-lg-4 col-md-12 col-sm-6 mt-2 mb-2">
                                                    <span class="far fa-address-card"></span>
                                                    <label class="text-black-50 col-form-label fs--2 m-1">Responsable</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboPersonalResponsableMntOTxEquipos" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                                            class="form-select form-select-sm js-choice" Style="text-transform: uppercase"
                                                            TabIndex="13">
                                                            <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvPersonalResponsableMntOTxEquipos" runat="server"
                                                            ControlToValidate="cboPersonalResponsableMntOTxEquipos" EnableClientScript="False"
                                                            ErrorMessage="Ingrese el personal responsable" Font-Size="10px" ForeColor="Red"
                                                            ValidationGroup="vgrpValidarMntOTxEquipos">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row bg-200">
                                                <div class="col-lg-12 col-md-12 col-sm-12 mt-2">
                                                    <span class="far fa-address-card"></span>
                                                    <label class="text-black-50 col-form-label fs--2 m-1">Personal Asignado</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboPersonalAsignadoMntOTxEquipos" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                                            class="form-select form-select-sm js-choice" Style="text-transform: uppercase"
                                                            TabIndex="14">
                                                            <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btnAdicionarPersonalAsignadoMntOTxEquipos" runat="server" Text="Agregar" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Personal" TabIndex="30" />
                                                        <asp:RequiredFieldValidator ID="rfvPersonalAsignadoMntOTxEquipos" runat="server"
                                                            ControlToValidate="cboPersonalAsignadoMntOTxEquipos" EnableClientScript="False"
                                                            ErrorMessage="Ingrese el personal asignado" Font-Size="10px" ForeColor="Red"
                                                            ValidationGroup="vgrpValidarMntOTxEquipos">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 mt-2">
                                                    <div id="divGridPersonalAsignadoMntOTxEquipos" style="overflow: auto; height: 220px; align: left;">
                                                        <asp:GridView ID="grdPersonalAsignadoMntOTxEquipos" runat="server" AutoGenerateColumns="False" TabIndex="17"
                                                            GridLines="None" CssClass="table table-responsive-sm table-striped table-hover small" AllowPaging="True"
                                                            EmptyDataText="No hay registros a visualizar" PageSize="1000">
                                                            <PagerStyle CssClass="pgr" />
                                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:CommandField ShowDeleteButton="True" HeaderStyle-Width="70px" />
                                                                <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900" />
                                                                <asp:BoundField DataField="Personal" HeaderText="Descripción" HeaderStyle-CssClass="bg-200 text-900" />
                                                                <asp:BoundField DataField="Responsable" HeaderText="Responsable" HeaderStyle-CssClass="bg-200 text-900" />
                                                            </Columns>
                                                            <HeaderStyle CssClass="thead-dark" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <div class="container">
                                        <div class="row">
                                            <div class="col-md-8 mt-2">
                                                <asp:ValidationSummary ID="ValidationSummary3" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarMntOTxEquipos" />
                                            </div>
                                            <div class="col-md-4 text-end p-2">
                                                <asp:Button ID="btnAceptarMntOTxEquipos" runat="server" ValidationGroup="vgrpValidarMntOTxEquipos" OnClientClick="deshabilitar(this.id)"
                                                    ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                &nbsp;
                                                <asp:Button ID="btnCancelarMntOtxEquipos" runat="server"
                                                    ToolTip="Cancelar Registro" TabIndex="73" Text="Cancelar" CssClass="btn btn-outline-google-plus btn-sm" />
                                            </div>
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


    <script language="javascript" type="text/javascript">
        function popupEmitirOrdenFabricacionReporte(TipDoc, NroSer, NroDoc) {
            window.open("Informes/frmCmmsOrdenFabricacionReporte.aspx?TipDoc=" + TipDoc + "&NroSerie=" + NroSer + "&NroDoc=" + NroDoc, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
        }
        function popupEmitirOrdenServicioReporte(TipDoc, NroSer, NroDoc) {
            window.open("Informes/frmCmmsOrdenServicioReporte.aspx?TipDoc=" + TipDoc + "&NroSerie=" + NroSer + "&NroDoc=" + NroDoc, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
        }
        function popupEmitirDetalleOrdenTrabajoServicioReporte(TipDoc, NroSer, NroDoc) {
            window.open("Informes/frmCmmsOrdenTrabajoServicioReporte.aspx?TipDoc=" + TipDoc + "&NroSerie=" + NroSer + "&NroDoc=" + NroDoc, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
        }
    </script>
    <link href="cssbootstrap/bootstrap-switch.css" rel="stylesheet" />
    <script src="jsbootstrap/bootstrap-switch.js"></script>

    <script src="vendors/anchorjs/anchor.min.js"></script>
    <script src="vendors/glightbox/glightbox.min.js"> </script>
    <script src="vendors/lodash/lodash.min.js"></script>
    <script src="assets/js/theme.js"></script>
</asp:Content>
