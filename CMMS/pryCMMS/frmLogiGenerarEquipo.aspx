<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmLogiGenerarEquipo.aspx.vb" Inherits="pryCMMS.frmLogiGenerarEquipo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- ===============================================-->
    <!--    Stylesheets-->
    <!-- ===============================================-->
    <%--Inicio: Este es para la galeria--%>
    <%--    <link href="vendors/flatpickr/flatpickr.min.css" rel="stylesheet">
    <link href="vendors/glightbox/glightbox.min.css" rel="stylesheet">--%>
    <%--Final: Este es para la galeria--%>
    <link href="assets/css/theme-rtl.min.css" rel="stylesheet" id="style-rtl">
    <link href="assets/css/theme.min.css" rel="stylesheet" id="style-default">
    <%--este guarda la grilla--%>
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
            /*background-color: #ffffff;*/
            /* background-color: #ffffff;*/
            /*padding: 10px;*/ /*1px;*/
            /*position: fixed;*/
            position: sticky;
            /*width: 100%;*/
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
        /*function deshabilitar(boton) {
            document.getElementById(boton).style.visibility = 'hidden';
        }

        function HabilitaDeshabilita(check) {
            if (document.getElementById("cuerpo_fscIdentificadorComponente").checked == true) {
                document.getElementById("cuerpo_divSistemaFuncional").style.visibility = 'visible';
            }

            if (document.getElementById("cuerpo_fscIdentificadorComponente").checked == false) {
                document.getElementById("cuerpo_divSistemaFuncional").style.visibility = 'hidden';
            }
        }*/
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cuerpo" runat="server">
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
            <asp:HiddenField ID="hfdIdArticuloSAPEquipo" runat="server" />
            <asp:HiddenField ID="hfdFechaRegistroTarjetaSAPEquipo" runat="server" />
            <asp:HiddenField ID="hfdFechaManufacturaTarjetaSAPEquipo" runat="server" />
            <asp:HiddenField ID="hfdFechaCreacionEquipo" runat="server" />
            <asp:HiddenField ID="hfdIdUsuarioCreacionEquipo" runat="server" />

            <asp:Panel ID="pnlCabecera" runat="server">
                <div class="card mb-3">
                    <div class="card-header">
                        <div class="row flex-between-end">
                            <div class="col-auto align-self-center">
                                <div class="nav nav-pills gap-2" role="tablist">
                                    <h5 class="mb-0" data-anchor="data-anchor">Listado de Equipos</h5>
                                    <a class="btn btn-falcon-default btn-sm ml-auto float-right" href="frmBienvenida.aspx" aria-expanded="true">
                                        <span class="fas fa-home"></span><span class="nav-link-text ps-1">Inicio</span>
                                    </a>
                                </div>
                            </div>
                            <div class="col-auto ms-auto">
                                <%--<div class="nav nav-pills gap-2" role="tablist">--%>
                                <div class="nav gap-2" role="tablist">
                                    <asp:LinkButton ID="btnNuevo" runat="server" OnClick="btnNuevo_Click" CssClass="btn btn-success btn-sm ml-auto float-right" Text="<span class='fas fa-plus'></span> Nuevo" ToolTip="Nuevo Equipo" />
                                    <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-primary btn-sm ml-auto float-right" Text="<span class='fas fa-pencil-alt'></span> Editar" ToolTip="Editar Equipo" />
                                    <div class="dropdown font-sans-serif btn-reveal-trigger">
                                        <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesEquipo" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                                        <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesEquipo">
                                            <asp:LinkButton ID="lnkbtnImprimirTarjetaEquipo" runat="server" ToolTip="Imprimir la tarjeta del equipo" Text="<span class='fas fa-file me-2'></span>Ver Tarjeta" CssClass="nav-link nav-link-card-details" />
                                            <asp:LinkButton ID="lnkbtnVerOrdenFabricacion" runat="server" ToolTip="Imprimir la orden de fabricación" Text="<span class='fas fa-file me-2'></span>Ver O.Fabric." CssClass="nav-link nav-link-card-details" />
                                            <%--<asp:LinkButton ID="lnkbtnVerOrdenTrabajo" runat="server" ToolTip="Imprimir las ordenes de trabajo" Text="<span class='fas fa-file me-2'></span>Ver O.Trab." CssClass="nav-link nav-link-card-details" />--%>
                                            <asp:LinkButton ID="lnkbtnVerOrdenTrabajoModal" runat="server" ToolTip="Imprimir las ordenes de trabajo" Text="<span class='fas fa-file me-2'></span>Ver O.Trab." CssClass="nav-link nav-link-card-details" />
                                            <asp:LinkButton ID="lnk_mostrarPanelOrdenTrabajoModal" runat="server">
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lnkbtnVerGaleriaFotosEquipo" runat="server" ToolTip="Ver la galeria de fotos del equipo" Text="<span class='fas fa-file-signature me-2'></span>Ver Galeria" CssClass="nav-link nav-link-card-details" />
                                            <asp:LinkButton ID="lnkbtnCargarFotoEquipo" runat="server" ToolTip="Agregar una foto del equipo" Text="<span class='fas fa-file-signature me-2'></span>Agregar Foto" CssClass="nav-link nav-link-card-details" />
                                            <asp:LinkButton ID="lnkbtnSubirDocumentacionEquipo" runat="server" ToolTip="Ver documentación del equipo" Text="<span class='fas fa-file-signature me-2'></span>Documentos" CssClass="nav-link nav-link-card-details" />
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
                            <%--<div class="col-sm-4">
                                <div class="d-grid gap-2 d-md-flex justify-content-md-end">


                                </div>
                            </div>--%>
                        </div>
                        <div class="row">
                            <div class="col-md-12 mt-3">
                                <%--<div class="table-responsive scrollbar">--%>
                                <div class="table-responsive">
                                    <%--<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" TabIndex="4" DataKeyNames="Codigo"
                                            GridLines="None" CssClass="table table-responsive-sm table-hover small" AllowPaging="True" 
                                            EmptyDataText="No hay registros a visualizar" PageSize="14" OnRowCommand="grdLista_RowCommand">--%>
                                    <asp:GridView ID="grdLista" runat="server" AutoGenerateColumns="False" TabIndex="4" DataKeyNames="Codigo"
                                        GridLines="None" CssClass="table table-responsive-sm table-hover small" AllowPaging="True"
                                        EmptyDataText="No hay registros a visualizar" PageSize="30" OnRowCommand="grdLista_RowCommand">
                                        <PagerStyle CssClass="pgr" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <AlternatingRowStyle CssClass="alt" />
                                        <Columns>
                                            <%--<asp:CommandField ShowSelectButton="True" HeaderStyle-CssClass="bg-200 text-900"/>--%>
                                            <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                            <%--<asp:BoundField DataField="RucCliente" HeaderText="RUC" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>
                                                <asp:BoundField DataField="RazonSocialCliente" HeaderText="Razón Social" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>--%>
                                            <asp:TemplateField HeaderText="Cliente" ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small">
                                                <ItemTemplate>
                                                    <div class="col-12">
                                                        <asp:Label ID="lblRucCliente" runat="server" Visible='<%# If(Eval("RucCliente") = "", "false", "true") %>' Text='<%# Eval("RucCliente") %>'></asp:Label>
                                                    </div>
                                                    <div class="col-12">
                                                        <asp:Label ID="lblRazonSocialCliente" runat="server" Visible='<%# If(Eval("RazonSocialCliente") = "", "false", "true") %>' Text='<%# Eval("RazonSocialCliente") %>'></asp:Label>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="FechaRegistroTarjetaSAP" HeaderText="Fec.Reg.SAP" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                            <asp:BoundField DataField="FechaUltimaModificacion" HeaderText="Fec.Modif." DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                            <asp:BoundField DataField="IdTipoActivo" HeaderText="Id.Tipo Activo" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                            <asp:BoundField DataField="IdCatalogo" HeaderText="Id.Catálogo" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                            <asp:BoundField DataField="NumeroSerieEquipo" HeaderText="Nro.Serie" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                            <asp:BoundField DataField="Descripcion" HeaderText="Equipo" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                            <asp:BoundField DataField="DescripcionEquipoSAP" HeaderText="Equipo SAP" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                            <asp:TemplateField HeaderText="Estado" ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEstado" runat="server" Visible='<%# If(Eval("StatusEquipo") = "", "false", "true") %>' CssClass='<%#If(Eval("StatusEquipo") = "R", "texto_estado short_estado black borde_redondo1", If(Eval("StatusEquipo") = "T", "texto_estado short_estado verde borde_redondo1", "texto_estado short_estado orange borde_redondo1")) %>' Text='<%# StrConv(IIf(Eval("StatusEquipo") = "B", "BLOQUEADO", IIf(Eval("StatusEquipo") = "T", "TERMINADO", "REGISTRADO")), VbStrConv.ProperCase) %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" />
                                            <%--<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900">
                                                    <ItemTemplate>
                                                        <div class="bootstrap-switch bootstrap-switch-on bootstrap bootstrap-switch-animate bootstrap-switch-mini ">
                                                            <div class="bootstrap-switch-container">
                                                                <asp:LinkButton ID="lnkEstadoOn" runat="server" CommandName="Activar" CommandArgument='<%# Eval("Codigo") %>' CssClass="bootstrap-switch-handle-on bootstrap-switch-primary" Visible='<%# If(Eval("Estado") = "True", "True", "False") %>'>Activado</asp:LinkButton>
                                                                <span class="bootstrap-switch-label">&nbsp;</span>
                                                                <asp:LinkButton ID="lnkEstadoOff" runat="server" CommandName="Desactivar" CommandArgument='<%# Eval("Codigo") %>' CssClass="bootstrap-switch-handle-off bootstrap-switch-danger" Visible='<%# If(Eval("Estado") = "True", "False", "True") %>'>Anulado</asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                        </Columns>
                                        <HeaderStyle CssClass="thead-dark" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <ajaxToolkit:ModalPopupExtender
                    ID="lnk_mostrarPanelOrdenTrabajoModal_ModalPopupExtender" runat="server"
                    BackgroundCssClass="FondoAplicacion" DropShadow="False"
                    DynamicServicePath="" Enabled="True"
                    CancelControlID="btnCancelarSeleccionarEquipo"
                    PopupControlID="pnlOrdenTrabajoModal"
                    TargetControlID="lnk_mostrarPanelOrdenTrabajoModal">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlOrdenTrabajoModal" runat="server" CssClass="container">
                    <div class="modal-dialog modal-xl">
                        <div class="shadow rounded">
                            <div class="modal-dialog-scrollable">
                                <div class="modal-content">
                                    <div class="modal-header bg-light">
                                        <%--<h4 class="modal-title"
                                                                        id="myModalLabelSeleccionarEquipo">Seleccionar Equipo</h4>--%>

                                        <%--<asp:LinkButton ID="lnkbtnVerOrdenTrabajo" runat="server" ToolTip="Imprimir las ordenes de trabajo" Text="<span class='fas fa-file me-2'></span>Ver O.Trab." CssClass="nav-link nav-link-card-details" />--%>
                                        <asp:LinkButton
                                            ID="lnkbtnImprimirOrdenTrabajo"
                                            runat="server"
                                            Text="<span class='fas fa-print me-2'></span>"
                                            CssClass=""
                                            OnClick="lnkbtnImprimirOrdenTrabajo_Click"
                                            data-dismiss="modal" aria-label="Close" />
                                    </div>
                                    <div class="modal-body">

                                        <div class="row">
                                            <div class="col-md-12 mt-3">
                                                <div style="overflow: auto">
                                                    <asp:GridView
                                                        ID="gvData"
                                                        runat="server"
                                                        AutoGenerateColumns="False"
                                                        TabIndex="63" GridLines="None"
                                                        CssClass="table table-responsive-sm table-striped table-hover small"
                                                        AllowPaging="True"
                                                        OnRowCommand="grdEquiposOrdenTrabajo_RowCommand"
                                                        EmptyDataText="No hay registros a visualizar"
                                                        PageSize="10">
                                                        <PagerStyle CssClass="pgr" />
                                                        <SelectedRowStyle
                                                            BackColor="#E2DED6"
                                                            Font-Bold="True"
                                                            ForeColor="#333333" />
                                                        <AlternatingRowStyle
                                                            CssClass="alt" />
                                                        <Columns>
                                                            <asp:BoundField
                                                                Visible="false"
                                                                DataField="cIdTipoDocumentoCabeceraOrdenTrabajo"
                                                                HeaderText="cIdTipoDocumentoCabeceraOrdenTrabajo"
                                                                HeaderStyle-CssClass="bg-200 text-900" />
                                                            <asp:BoundField
                                                                Visible="false"
                                                                DataField="vIdNumeroSerieCabeceraOrdenTrabajo"
                                                                HeaderText="vIdNumeroSerieCabeceraOrdenTrabajo"
                                                                HeaderStyle-CssClass="bg-200 text-900" />
                                                            <asp:BoundField
                                                                Visible="false"
                                                                DataField="vIdNumeroCorrelativoCabeceraOrdenTrabajo"
                                                                HeaderText="vIdNumeroCorrelativoCabeceraOrdenTrabajo"
                                                                HeaderStyle-CssClass="bg-200 text-900" />
                                                            <asp:BoundField
                                                                DataField="Codigo"
                                                                HeaderText="#ORDEN TRABAJO"
                                                                HeaderStyle-CssClass="bg-200 text-900" />

                                                            <asp:BoundField
                                                                DataField="dFechaEmisionCabeceraOrdenTrabajo"
                                                                HeaderText="FEC.EMI."
                                                                HeaderStyle-CssClass="bg-200 text-900" />

                                                            <asp:BoundField
                                                                DataField="dFechaInicioPlanificadaCabeceraOrdenTrabajo"
                                                                HeaderText="FEC.INI."
                                                                HeaderStyle-CssClass="bg-200 text-900" />

                                                            <asp:BoundField
                                                                DataField="vDescripcionTipoMantenimiento"
                                                                HeaderText="TIPO MANTENIMIENTO"
                                                                HeaderStyle-CssClass="bg-200 text-900" />
                                                            <asp:BoundField
                                                                DataField="vOrdenFabricacionReferenciaCabeceraOrdenTrabajo"
                                                                HeaderText="ORD.FAB."
                                                                HeaderStyle-CssClass="bg-200 text-900" />
                                                            <asp:BoundField
                                                                DataField="vContratoReferenciaCabeceraOrdenTrabajo"
                                                                HeaderText="CONTRATO"
                                                                HeaderStyle-CssClass="bg-200 text-900" />
                                                            <asp:BoundField
                                                                DataField="vEstadoCabeceraOrdenTrabajo"
                                                                HeaderText="ESTADO"
                                                                HeaderStyle-CssClass="bg-200 text-900" />
                                                            <asp:BoundField
                                                                Visible="false"
                                                                DataField="cIdTipoMantenimientoCabeceraOrdenTrabajo"
                                                                HeaderText="cIdTipoMantenimientoCabeceraOrdenTrabajo"
                                                                HeaderStyle-CssClass="bg-200 text-900" />
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                   
                                                                        <asp:Button 
                                                                            ID="btnAction" 
                                                                            runat="server"
                                                                            Text="Ver"
                                                                            CommandName="SelectOrdenTrabajo"
                                                                            CommandArgument="<%# Container.DataItemIndex %>"
                                                                            >
                                                                        </asp:Button>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle
                                                            CssClass="thead-dark" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Label ID="lblMensajeSeleccionarEquipo"
                                            runat="server"
                                            CssClass="col-form-label col-form-label-sm text-danger">
                                        </asp:Label>
                                        <%--<asp:Button ID="btnAceptarSeleccionarEquipo"
                                            runat="server"
                                            ValidationGroup="vgrpValidarBusqueda"
                                            ToolTip="Aceptar Registro" TabIndex="72"
                                            Text="Aceptar"
                                            CssClass="btn btn-outline-facebook btn-sm d-block" />--%>
                                        &nbsp;
                                                                        <asp:Button ID="btnCancelarSeleccionarEquipo"
                                                                            runat="server" ToolTip="Cancelar Registro"
                                                                            TabIndex="73" Text="Cerrar"
                                                                            CssClass="btn btn-outline-google-plus btn-sm d-block" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

            </asp:Panel>

            <asp:Panel ID="pnlEquipo" runat="server">
                <div id="BarraHTML" class="container-fluid">
                    <div class="row">
                        <div class="col-auto align-self-center mt-2 mb-2">
                            <div class="nav nav-pills gap-2" role="tablist">
                                <asp:LinkButton ID="btnAtras" runat="server" CssClass="btn btn-falcon-default btn-sm ml-auto float-right" Text="<span class='fas fa-step-backward'></span> Atrás" ToolTip="Ir hacia atrás" />
                            </div>
                        </div>
                        <div class="col-auto ms-auto mt-2 mb-2">
                            <div class="nav nav-pills gap-2" role="tablist">
                                <asp:LinkButton ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" CssClass="btn btn-success btn-sm ml-auto float-right" Text="<span class='fas fa-plus'></span> Guardar" ToolTip="Guardar Registro" />
                            </div>
                        </div>
                    </div>
                </div>
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
                                        <span class="far fa-address-card"></span>
                                        <label class="text-black-50 col-form-label fs--2 m-1">Código Equipo:</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtIdEquipo" runat="server" CssClass="form-control form-control-sm" TabIndex="10" placeholder="Id. del equipo"></asp:TextBox>
                                            <asp:HiddenField ID="hfdFechaEquipo" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-lg-8 col-md-6 mt-2">
                                        <span class="far fa-address-card"></span>
                                        <label class="text-black-50 col-form-label fs--2 m-1">Descripción Equipo:</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtDescripcionEquipo" runat="server" CssClass="form-control form-control-sm"
                                                TabIndex="11" Style="text-transform: uppercase" placeholder="Descripción del equipo" autocomplete="off"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDescripcionEquipo" runat="server"
                                                ControlToValidate="txtDescripcionEquipo" EnableClientScript="False"
                                                ErrorMessage="Ingrese la descripción del equipo" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4 col-md-6 mt-2">
                                        <span class="far fa-address-card"></span>
                                        <label class="text-black-50 col-form-label fs--2 m-1">N.Serie SAP:</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtNroSerieEquipo" runat="server" CssClass="form-control form-control-sm"
                                                TabIndex="12" Style="text-transform: uppercase" placeholder="Número de serie del equipo" autocomplete="off" AutoPostBack="True"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-8 col-md-6 mt-2">
                                        <span class="far fa-address-card"></span>
                                        <label class="text-black-50 col-form-label fs--2 m-1">Cod. Articulo SAP:</label>
                                        <div class="input-group">
                                            <asp:DropDownList ID="cboArticuloSAPEquipo" runat="server" AppendDataBoundItems="True"
                                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice"
                                                Style="text-transform: uppercase" TabIndex="17">
                                                <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-md-6 mt-2">
                                        <span class="far fa-address-card"></span>
                                        <label class="text-black-50 col-form-label fs--2 m-1">Descripción Equipo SAP:</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtDescripcionEquipoSAP" runat="server" CssClass="form-control form-control-sm"
                                                TabIndex="18" Style="text-transform: uppercase" placeholder="Descripción del equipo" autocomplete="off"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDescripcionEquipoSAP" runat="server"
                                                ControlToValidate="txtDescripcionEquipoSAP" EnableClientScript="False"
                                                ErrorMessage="Ingrese la descripción del equipo SAP" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-6 mt-2">
                                        <span class="far fa-clone"></span>
                                        <label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Tipo Activo:</label>
                                        <div class="input-group">
                                            <asp:DropDownList ID="cboTipoActivo" runat="server" AppendDataBoundItems="True"
                                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice"
                                                Style="text-transform: uppercase" TabIndex="18">
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
                                            <asp:LinkButton ID="lnk_mostrarPanelMantenimientoTipoActivo" runat="server"></asp:LinkButton>
                                            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMantenimientoTipoActivo_ModalPopupExtender" runat="server"
                                                BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True"
                                                CancelControlID="btnCancelarMantenimientoTipoActivo"
                                                PopupControlID="pnlMantenimientoTipoActivo" TargetControlID="lnk_mostrarPanelMantenimientoTipoActivo">
                                            </ajaxToolkit:ModalPopupExtender>
                                            <%--<asp:UpdatePanel ID="updpnlMantenimientoTipoActivo" runat="server">
                                    <ContentTemplate>--%>
                                            <asp:Panel ID="pnlMantenimientoTipoActivo" runat="server" CssClass="container" DefaultButton="btnAceptarMantenimientoTipoActivo">
                                                <div class="modal-dialog modal-lg">
                                                    <div class="shadow rounded">
                                                        <div class="modal-dialog-scrollable">
                                                            <div class="modal-content">
                                                                <div class="modal-header bg-light">
                                                                    <h4 class="modal-title" id="myModalLabelProducto">Mantenimiento Tipo Activo</h4>
                                                                    <asp:Button ID="imgbtnCancelarTipoActivoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                                </div>
                                                                <div class="modal-body">
                                                                    <div class="row pt-2 pb-2">
                                                                        <div class="col-lg-3 col-md-4 col-sm-5">
                                                                            <span class="far fa-address-card"></span>
                                                                            <label class="text-black-50 col-form-label fs--2 m-1">IdTipoActivo:</label>
                                                                            <div class="input-group">
                                                                                <asp:TextBox ID="txtIdTipoActivoMantenimientoTipoActivo" runat="server" CssClass="form-control form-control-sm" TabIndex="10"
                                                                                    Style="text-transform: uppercase" autocomplete="off"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-lg-9 col-md-12 col-sm-12">
                                                                            <span class="far fa-address-card"></span>
                                                                            <label class="text-black-50 col-form-label fs--2 m-1">Descripción:</label>
                                                                            <div class="input-group">
                                                                                <asp:TextBox ID="txtDescripcionMantenimientoTipoActivo" runat="server" CssClass="form-control form-control-sm" TabIndex="11"
                                                                                    Style="text-transform: uppercase" placeholder="Descripción Tipo Activo" autocomplete="off"></asp:TextBox>
                                                                                <asp:RequiredFieldValidator ID="rfvDescripcionMantenimientoTipoActivo" runat="server"
                                                                                    ControlToValidate="txtDescripcionMantenimientoTipoActivo" EnableClientScript="False"
                                                                                    ErrorMessage="Ingrese la descripción" Font-Size="10px" ForeColor="Red"
                                                                                    ValidationGroup="vgrpValidarMantenimientoTipoActivo">(*)</asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-lg-6 col-md-8 col-sm-12 pt-1">
                                                                            <span class="far fa-address-card"></span>
                                                                            <label class="text-black-50 col-form-label fs--2 m-1">Descripción Abreviada:</label>
                                                                            <div class="input-group">
                                                                                <asp:TextBox ID="txtDescripcionAbreviadaMantenimientoTipoActivo" runat="server" CssClass="form-control form-control-sm" TabIndex="12"
                                                                                    Style="text-transform: uppercase" placeholder="Descripción Abreviada Tipo de Activo" autocomplete="off"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="modal-footer">
                                                                    <div class="container">
                                                                        <div class="row">
                                                                            <asp:Label ID="lblMensajeMantenimientoTipoActivo" runat="server" CssClass="col-lg-6 col-md-5 col-form-label col-form-label-sm text-danger"></asp:Label>
                                                                            <div class="col-lg-6 col-md-7 text-end p-2">
                                                                                <asp:Button ID="btnAceptarMantenimientoTipoActivo" runat="server" ValidationGroup="vgrpValidarMantenimientoTipoActivo"
                                                                                    ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                                                &nbsp;
                                                                            <asp:Button ID="btnCancelarMantenimientoTipoActivo" runat="server"
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
                                            <%--</ContentTemplate>
                                </asp:UpdatePanel>--%>
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-6 mt-2">
                                        <span class="far fa-clone"></span>
                                        <label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Catalogo Principal:</label>
                                        <div class="input-group">
                                            <asp:HiddenField ID="hfdIdCatalogoAnterior" runat="server" />
                                            <asp:DropDownList ID="cboCatalogo" runat="server" AppendDataBoundItems="True" onchange="guardarValor()"
                                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice"
                                                Style="text-transform: uppercase" TabIndex="19">
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
                                            <asp:LinkButton ID="lnk_mostrarPanelMantenimientoCatalogo" runat="server"></asp:LinkButton>
                                            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMantenimientoCatalogo_ModalPopupExtender" runat="server"
                                                BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True"
                                                CancelControlID="btnCancelarMantenimientoCatalogo"
                                                PopupControlID="pnlMantenimientoCatalogo" TargetControlID="lnk_mostrarPanelMantenimientoCatalogo">
                                            </ajaxToolkit:ModalPopupExtender>
                                            <asp:UpdatePanel ID="updpnlMantenimientoCatalogo" runat="server">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlMantenimientoCatalogo" runat="server" CssClass="container" DefaultButton="btnAceptarMantenimientoCatalogo">
                                                        <div class="modal-dialog modal-lg">
                                                            <div class="shadow rounded">
                                                                <div class="modal-dialog-scrollable">
                                                                    <div class="modal-content">
                                                                        <div class="modal-header bg-light">
                                                                            <h4 class="modal-title" id="myModalLabelCatalogo">Mantenimiento Catalogo</h4>
                                                                            <asp:Button ID="imgbtnCancelarCatalogoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                                        </div>
                                                                        <div class="modal-body">
                                                                            <div class="row pt-2 pb-2">
                                                                                <div class="col-lg-3 col-md-4 mt-2">
                                                                                    <span class="fas fa-indent"></span>
                                                                                    <label class="text-black-50 col-form-label fs--2 m-1">Código Catalogo:</label>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtIdCatalogoMantenimientoCatalogo" runat="server" CssClass="form-control form-control-sm" TabIndex="10"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-lg-9 col-md-4 mt-2">
                                                                                    <span class="fas fa-indent"></span>
                                                                                    <label class="text-black-50 col-form-label fs--2 m-1">Tipo Activo:</label>
                                                                                    <div class="input-group">
                                                                                        <asp:DropDownList ID="cboTipoActivoMantenimientoCatalogo" runat="server" AppendDataBoundItems="True" TabIndex="11"
                                                                                            CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase">
                                                                                            <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                        <ajaxToolkit:ListSearchExtender ID="cboTipoActivoMantenimientoCatalogo_ListSearchExtender" runat="server"
                                                                                            Enabled="True" PromptText="Iniciar Búsqueda" TargetControlID="cboTipoActivoMantenimientoCatalogo">
                                                                                        </ajaxToolkit:ListSearchExtender>
                                                                                        <asp:RequiredFieldValidator ID="rfvTipoActivoMantenimientoCatalogo" runat="server"
                                                                                            ControlToValidate="cboTipoActivoMantenimientoCatalogo" EnableClientScript="False" InitialValue="SELECCIONE DATO"
                                                                                            ErrorMessage="Ingrese el código de tipo de activo" Font-Size="10px" ForeColor="Red"
                                                                                            ValidationGroup="vgrpValidarMantenimientoCatalogo">(*)</asp:RequiredFieldValidator>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-lg-6 col-md-4 mt-2">
                                                                                    <span class="fas fa-object-group"></span>
                                                                                    <label class="text-black-50 col-form-label fs--2 m-1">Descripción:</label>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtDescripcionMantenimientoCatalogo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="12"
                                                                                            Style="text-transform: uppercase" placeholder="Descripción Catalogo"></asp:TextBox>
                                                                                        <asp:RequiredFieldValidator ID="rfvDescripcionMantenimientoCatalogo" runat="server"
                                                                                            ControlToValidate="txtDescripcionMantenimientoCatalogo" EnableClientScript="False"
                                                                                            ErrorMessage="Ingrese la descripción" Font-Size="10px" ForeColor="Red"
                                                                                            ValidationGroup="vgrpValidarMantenimientoCatalogo">(*)</asp:RequiredFieldValidator>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-lg-6 col-md-4 mt-2">
                                                                                    <span class="fas fa-object-group"></span>
                                                                                    <label class="text-black-50 col-form-label fs--2 m-1">Descripción Abreviada:</label>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtDescripcionAbreviadaMantenimientoCatalogo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="13"
                                                                                            Style="text-transform: uppercase" placeholder="Descripción Abreviada Catalogo"></asp:TextBox>
                                                                                        <asp:RequiredFieldValidator ID="rfvDescripcionAbreviadaMantenimientoCatalogo" runat="server"
                                                                                            ControlToValidate="txtDescripcionAbreviadaMantenimientoCatalogo" EnableClientScript="False"
                                                                                            ErrorMessage="Ingrese la descripción abreviada" Font-Size="10px"
                                                                                            ForeColor="Red" ValidationGroup="vgrpValidarMantenimientoCatalogo">(*)</asp:RequiredFieldValidator>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-lg-3 col-md-4 mt-2">
                                                                                    <span class="fas fa-stopwatch"></span>
                                                                                    <label class="text-black-50 col-form-label fs--2 m-1">Vida Útil:</label>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtVidaUtilMantenimientoCatalogo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="14"
                                                                                            Style="text-transform: uppercase" placeholder="Vida Util del Catalogo"></asp:TextBox>
                                                                                        <asp:RequiredFieldValidator ID="rfvVidaUtilMantenimientoCatalogo" runat="server"
                                                                                            ControlToValidate="txtVidaUtilMantenimientoCatalogo" EnableClientScript="False"
                                                                                            ErrorMessage="Ingrese la vida util en meses del catálogo" Font-Size="10px" ForeColor="Red"
                                                                                            ValidationGroup="vgrpValidarMantenimientoCatalogo">(*)</asp:RequiredFieldValidator>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-lg-3 col-md-4 mt-2">
                                                                                    <span class="fas fa-stream"></span>
                                                                                    <label class="text-black-50 col-form-label fs--2 m-1">Per.Garantía(Meses):</label>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtPeriodoGarantiaMantenimientoCatalogo" runat="server" CssClass="form-control form-control-sm" TabIndex="16"
                                                                                            Style="text-transform: uppercase" placeholder="Periodo de Garantía"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-lg-3 col-md-4 mt-2">
                                                                                    <span class="fas fa-stream"></span>
                                                                                    <label class="text-black-50 col-form-label fs--2 m-1">Per.Mín.Mnto.(Meses):</label>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtPeriodoMinimoMantenimientoCatalogo" runat="server" CssClass="form-control form-control-sm" TabIndex="16"
                                                                                            Style="text-transform: uppercase" placeholder="Periodo Mínimo de Mantenimiento"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-lg-3 col-md-4 mt-2" id="divCuentaContableMantenimientoCatalogo" runat="server">
                                                                                    <span class="fas fa-stream"></span>
                                                                                    <label class="text-black-50 col-form-label fs--2 m-1">Cuenta Contable:</label>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtCuentaContableMantenimientoCatalogo" runat="server" CssClass="form-control form-control-sm" TabIndex="15"
                                                                                            Style="text-transform: uppercase" placeholder="Cuenta Contable"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-lg-3 col-md-4 mt-2" id="divCuentaContableLeasingMantenimientoCatalogo" runat="server">
                                                                                    <span class="fas fa-stream"></span>
                                                                                    <label class="text-black-50 col-form-label fs--2 m-1">Cta. Ctble. Leasing:</label>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtCuentaContableLeasingMantenimientoCatalogo" runat="server" CssClass="form-control form-control-sm" TabIndex="16"
                                                                                            Style="text-transform: uppercase" placeholder="Cuenta Contable Leasing"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row mt-3">
                                                                                <div class="col-12 mb-0 bg-100 d-none d-md-block">
                                                                                    <h6 class="mt-2 mb-1">CARACTERISTICAS ASIGNADAS
											                            <asp:Button ID="btnAdicionarCaracteristicaMantenimientoCatalogo" runat="server" Text="[+] Características" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Características" ValidationGroup="vgrpValidarMantenimientoCatalogo" />
                                                                                    </h6>
                                                                                    <hr class="bg-300 m-0" />
                                                                                </div>
                                                                                <div class="col-lg-3 col-md-4 mt-2">
                                                                                </div>
                                                                            </div>
                                                                            <div class="row justify-content-between mt-3">
                                                                                <div class="viewport table-responsive scrollbar">
                                                                                    <div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
                                                                                        <asp:UpdatePanel ID="updpnlDetalleCaracteristicaMantenimientoCatalogo" runat="server">
                                                                                            <ContentTemplate>
                                                                                                <asp:Panel ID="pnlDetalleCaracteristicaMantenimientoCatalogo" runat="server" CssClass="bg-light">
                                                                                                    <div class="table-responsive scrollbar">
                                                                                                        <asp:GridView ID="grdDetalleCaracteristicaMantenimientoCatalogo" runat="server" AutoGenerateColumns="False" TabIndex="4"
                                                                                                            GridLines="None" CssClass="table table-striped table-hover small overflow-hidden" AllowPaging="True"
                                                                                                            EmptyDataText="No hay registros a visualizar" PageSize="50">
                                                                                                            <PagerStyle CssClass="mGrid" />
                                                                                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                                                            <Columns>
                                                                                                                <asp:CommandField ShowDeleteButton="True" HeaderStyle-Width="70px" />
                                                                                                                <asp:TemplateField>
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:CheckBox ID="chkRowDetalleCaracteristica" runat="server" />
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:BoundField DataField="Item" HeaderText="#" ControlStyle-Width="25px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                                                                <asp:BoundField DataField="IdCaracteristica" HeaderText="Código" HeaderStyle-Width="40px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                                                                <asp:TemplateField HeaderText="Valor" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                                                                    <ItemTemplate>
                                                                                                                        <div class="input-group">
                                                                                                                            <asp:TextBox ID="txtValorDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("Valor") %>' AutoPostBack="False" Style="text-transform: uppercase" OnTextChanged="txtValordDetalleCaracteristicaMantenimientoCatalogo_TextChanged"></asp:TextBox>
                                                                                                                        </div>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Id. Ref. SAP" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                                                                    <ItemTemplate>
                                                                                                                        <div class="input-group">
                                                                                                                            <asp:TextBox ID="txtIdReferenciaSAPDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("IdReferenciaSAP") %>' CommandName="ButtonField" AutoPostBack="False" Style="text-transform: uppercase"></asp:TextBox>
                                                                                                                        </div>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Campo SAP" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                                                                    <ItemTemplate>
                                                                                                                        <div class="input-group">
                                                                                                                            <asp:TextBox ID="txtDescripcionCampoSAPDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("DescripcionCampoSAP") %>' CommandName="ButtonField" AutoPostBack="False" Style="text-transform: uppercase"></asp:TextBox>
                                                                                                                        </div>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                            <HeaderStyle CssClass="thead-dark" />
                                                                                                        </asp:GridView>
                                                                                                    </div>
                                                                                                </asp:Panel>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="modal-footer">
                                                                            <div class="container">
                                                                                <div class="row">
                                                                                    <asp:Label ID="lblMensajeMantenimientoCatalogo" runat="server" CssClass="col-lg-6 col-md-5 col-form-label col-form-label-sm text-danger"></asp:Label>
                                                                                    <div class="col-lg-6 col-md-7 text-end p-2">
                                                                                        <asp:Button ID="btnAceptarMantenimientoCatalogo" runat="server" ValidationGroup="vgrpValidarMantenimientoCatalogo"
                                                                                            ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                                                        &nbsp;
                                                                            <asp:Button ID="btnCancelarMantenimientoCatalogo" runat="server"
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
                                            <asp:LinkButton ID="lnk_mostrarPanelCaracteristica" runat="server"></asp:LinkButton>
                                            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelCaracteristica_ModalPopupExtender" runat="server"
                                                BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True"
                                                PopupControlID="pnlSeleccionarCaracteristica" TargetControlID="lnk_mostrarPanelCaracteristica">
                                            </ajaxToolkit:ModalPopupExtender>
                                            <asp:UpdatePanel ID="updpnlSeleccionarCaracteristica" runat="server">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlSeleccionarCaracteristica" runat="server" CssClass="container" DefaultButton="imgbtnBuscarCaracteristica">
                                                        <div class="modal-dialog modal-lg">
                                                            <div class="shadow rounded">
                                                                <div class="modal-dialog-scrollable">
                                                                    <div class="modal-content">
                                                                        <div class="modal-header bg-light">
                                                                            <h4 class="modal-title" id="myModalLabelCaracteristica">Seleccionar Caracteristica</h4>
                                                                            <asp:Button ID="imgbtnCancelarCaracteristicaImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                                        </div>
                                                                        <div class="modal-body">
                                                                            <div class="row pt-2 pb-2">
                                                                                <span class="col-sm-2 col-form-label-sm">Buscar por</span>
                                                                                <div class="col-sm-4">
                                                                                    <asp:DropDownList ID="cboFiltroCaracteristica" runat="server" CssClass="form-select form-select-sm js-choice"
                                                                                        TabIndex="60">
                                                                                        <asp:ListItem Value="vDescripcionCaracteristica">DESCRIPCION</asp:ListItem>
                                                                                        <asp:ListItem Value="cIdCaracteristica">CODIGO</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                                <div class="col-sm-6">
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtBuscarCaracteristica" runat="server" CssClass="form-control form-control-sm" TabIndex="61"
                                                                                            Style="text-transform: uppercase" placeholder="Ingrese Busqueda" onfocus="fnSetFocus('ctl00$cuerpo$txtBuscarCaracteristica')"></asp:TextBox>
                                                                                        &nbsp;
                                                                            <asp:ImageButton ID="imgbtnBuscarCaracteristica" runat="server" Height="18px" TabIndex="62" CssClass="mt-1"
                                                                                ImageUrl="~/Imagenes/buscar.png" ToolTip="Buscar Registro" Width="21px" onfocus="fnSetFocus('ctl00$cuerpo$txtBuscarCaracteristica')" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="viewport table-responsive scrollbar">
                                                                                    <div class="col-md-12 mt-3">
                                                                                        <asp:GridView ID="grdListaCaracteristica" runat="server" AutoGenerateColumns="False" TabIndex="63" DataKeyNames="Codigo"
                                                                                            GridLines="None" CssClass="table table-responsive-sm table-hover small" AllowPaging="True"
                                                                                            EmptyDataText="No hay registros a visualizar" PageSize="50">
                                                                                            <PagerStyle CssClass="pgr" />
                                                                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                                            <AlternatingRowStyle CssClass="alt" />
                                                                                            <Columns>
                                                                                                <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="mx-0 bg-200 text-900 fs--1 fw-semi-bold" />
                                                                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-Width="440px" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="mx-0 bg-200 text-900 fs--1 fw-semi-bold" />
                                                                                            </Columns>
                                                                                            <HeaderStyle CssClass="thead-dark" />
                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="modal-footer">
                                                                            <div class="container overflow-scroll scrollbar">
                                                                                <div class="row">
                                                                                    <div class="col-lg-4 col-md-4 mt-2">
                                                                                        <span class="fas fa-phone-alt"></span>
                                                                                        <label class="text-black-50 col-form-label fs--2 m-1">Valor Caract.:</label>
                                                                                        <div class="input-group">
                                                                                            <asp:TextBox ID="txtValorCaracteristica" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="20"
                                                                                                Style="text-transform: uppercase" placeholder="Valor de la Caracteristica"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-lg-4 col-md-4 mt-2">
                                                                                        <div id="divIdReferenciaSAPCaracteristica" runat="server">
                                                                                            <span class="fas fa-phone-alt"></span>
                                                                                            <label class="text-black-50 col-form-label fs--2 m-1">Id. Referencia SAP:</label>
                                                                                        </div>
                                                                                        <div class="input-group">
                                                                                            <asp:TextBox ID="txtIdReferenciaSAPCaracteristica" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="17"
                                                                                                Style="text-transform: uppercase" placeholder="Código Referencia SAP"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-lg-4 col-md-4 mt-2">
                                                                                        <div id="divDescripcionCampoSAPCaracteristica" runat="server">
                                                                                            <span class="fas fa-phone-alt"></span>
                                                                                            <label class="text-black-50 col-form-label fs--2 m-1">Descripción Campo SAP:</label>
                                                                                        </div>
                                                                                        <div class="input-group">
                                                                                            <asp:TextBox ID="txtDescripcionCampoSAPCaracteristica" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="17"
                                                                                                Style="text-transform: uppercase" placeholder="Campo SAP"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="container">
                                                                                <div class="row">
                                                                                    <asp:Label ID="lblMensajeCaracteristica" runat="server" CssClass="col-lg-6 col-md-5 col-form-label col-form-label-sm text-danger"></asp:Label>
                                                                                    <div class="col-lg-6 col-md-7 text-end p-2">
                                                                                        <asp:Button ID="btnAceptarCaracteristica" runat="server" ValidationGroup="vgrpValidarBusqueda"
                                                                                            ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                                                        &nbsp;
                                                                            <asp:Button ID="btnCancelarCaracteristica" runat="server"
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
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-6 mt-2">
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
                                    <div class="col-lg-4 col-md-6 mt-2">
                                        <span class="far fa-address-card"></span>
                                        <label class="text-black-50 col-form-label fs--2 m-1">#Part:</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtNroParteEquipo" runat="server" CssClass="form-control form-control-sm"
                                                TabIndex="13" Style="text-transform: uppercase" placeholder="Número de parte del equipo" autocomplete="off" AutoPostBack="True"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-6 mt-2">
                                        <span class="far fa-address-card"></span>
                                        <label class="text-black-50 col-form-label fs--2 m-1">Área:</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtAreaEquipo" runat="server" CssClass="form-control form-control-sm"
                                                TabIndex="13" Style="text-transform: uppercase" placeholder="Área de ubicación" autocomplete="off" AutoPostBack="True"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-6 mt-2">
                                        <span class="far fa-address-card"></span>
                                        <label class="text-black-50 col-form-label fs--2 m-1">Tag:</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtTagEquipo" runat="server" CssClass="form-control form-control-sm"
                                                TabIndex="14" Style="text-transform: uppercase" placeholder="Tag del equipo" autocomplete="off" AutoPostBack="True"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-6 mt-2">
                                        <span class="far fa-address-card"></span>
                                        <label class="text-black-50 col-form-label fs--2 m-1">Capacidad:</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtCapacidadEquipo" runat="server" CssClass="form-control form-control-sm"
                                                TabIndex="15" Style="text-transform: uppercase" placeholder="Capacidad del equipo" autocomplete="off" AutoPostBack="True"></asp:TextBox>
                                        </div>
                                    </div>
                                    <%--</div>
                      <div class="row">--%>
                                    <div class="col-lg-4 col-md-12 mt-2">
                                        <div class="mt-1">
                                            <asp:Label ID="lblIdentificadorComponente" runat="server" CssClass="text-black-50 col-form-label col-form-label-sm fs--2 fw-semi-bold" Text="¿Es un componente?:"></asp:Label>
                                        </div>
                                        <div class="input-group">
                                            <div class="bootstrap-switch bootstrap-switch-on bootstrap bootstrap-switch-animate bootstrap-switch-mini ">
                                                <div class="bootstrap-switch-container">
                                                    <asp:LinkButton ID="lnkEsComponenteOn" runat="server" CommandName="SiComponente" CssClass="bootstrap-switch-handle-on bootstrap-switch-primary" Visible="True">Si</asp:LinkButton>
                                                    <span class="bootstrap-switch-label">&nbsp;</span>
                                                    <asp:LinkButton ID="lnkEsComponenteOff" runat="server" CommandName="NoComponente" CssClass="bootstrap-switch-handle-off bootstrap-switch-danger" Visible="False">No</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="divSistemaFuncional" runat="server" class="col-lg-4 col-md-12 mt-2">
                                        <span class="far fa-clone"></span>
                                        <label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Sistema Funcional:</label>
                                        <div class="input-group">
                                            <asp:DropDownList ID="cboSistemaFuncional" runat="server" AppendDataBoundItems="True"
                                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice"
                                                Style="text-transform: uppercase" TabIndex="20">
                                                <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:LinkButton ID="btnAdicionarSistemaFuncional" runat="server" Text="<span class='fas fa-plus-circle'></span>" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Sistema Funcional" TabIndex="16" />
                                            <asp:RequiredFieldValidator ID="rfvSistemaFuncional" runat="server"
                                                ControlToValidate="cboSistemaFuncional" Display="Static" EnableClientScript="False"
                                                ErrorMessage="Ingrese el sistema funcional" Font-Size="10px" ForeColor="Red"
                                                InitialValue="SELECCIONE DATO" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-12 mt-2">
                                        <div class="mt-1">
                                            <asp:Label ID="lblTieneContrato" runat="server" CssClass="text-black-50 col-form-label col-form-label-sm fs--2 fw-semi-bold" Text="¿Tiene Contrato?:"></asp:Label>
                                        </div>
                                        <div class="input-group">
                                            <div class="bootstrap-switch bootstrap-switch-on bootstrap bootstrap-switch-animate bootstrap-switch-mini ">
                                                <div class="bootstrap-switch-container">
                                                    <asp:LinkButton ID="lnkEsConContratoOn" runat="server" CommandName="SiContrato" CssClass="bootstrap-switch-handle-on bootstrap-switch-primary" Visible="True">Si</asp:LinkButton>
                                                    <span class="bootstrap-switch-label">&nbsp;</span>
                                                    <asp:LinkButton ID="lnkEsConContratoOff" runat="server" CommandName="NoContrato" CssClass="bootstrap-switch-handle-off bootstrap-switch-danger" Visible="False">No</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="divContratoReferencia" runat="server" class="col-lg-8 col-md-6 mt-2">
                                        <span class="far fa-clone"></span>
                                        <label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Contrato de Referencia:</label>
                                        <div class="input-group">
                                            <asp:DropDownList ID="cboContratoReferencia" runat="server" AppendDataBoundItems="True"
                                                AutoPostBack="True" CssClass="form-select form-select-sm js-choice"
                                                Style="text-transform: uppercase" TabIndex="21">
                                                <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-12 bg-200">
                                <div class="row mt-1">
                                    <div class="col-12 mb-0 d-none d-md-block">
                                        <h6 class="mt-2 mb-1">CARACTERISTICAS ASIGNADAS AL EQUIPO PRINCIPAL</h6>
                                        <hr class="bg-300 m-0" />
                                    </div>
                                </div>
                                <div id="divFiltrarCaracteristicaEquipo" runat="server" class="row justify-content-between">
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <asp:UpdatePanel ID="updpnlCaracteristicaEquipo" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                            <ContentTemplate>
                                                <span class="fas fa-link"></span>
                                                <label class="text-black-50 col-form-label fs--2 m-1">Filtrar Caracteristica:</label>
                                                <div class="input-group">
                                                    <asp:DropDownList ID="cboCaracteristicaEquipo" runat="server" AppendDataBoundItems="True" TabIndex="21"
                                                        CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase">
                                                        <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btnAdicionarCaracteristicaEquipo" runat="server" Text="Agregar" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Caracteristica al equipo principal" TabIndex="30" />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="row justify-content-between mt-3">
                                    <div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
                                        <div id="divGridDetalleCaracteristicaEquipo" style="overflow: auto; height: 260px; align: left;" class="scrollbar">
                                            <asp:UpdatePanel ID="updpnlDetalleCaracteristicaEquipo" runat="server">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlDetalleCaracteristicaEquipo" runat="server" CssClass="bg-light">
                                                        <div class="table-responsive scrollbar">
                                                            <asp:GridView ID="grdDetalleCaracteristicaEquipo" runat="server" AutoGenerateColumns="False" TabIndex="22"
                                                                GridLines="None" CssClass="table table-striped table-hover small overflow-hidden" AllowPaging="True"
                                                                EmptyDataText="No hay registros a visualizar" PageSize="50">
                                                                <PagerStyle CssClass="mGrid" />
                                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                <Columns>
                                                                    <asp:CommandField ShowDeleteButton="True" HeaderStyle-Width="70px" />
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkRowDetalleCaracteristica" runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Item" HeaderText="#" ControlStyle-Width="25px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                    <asp:BoundField DataField="IdCaracteristica" HeaderText="Código" HeaderStyle-Width="40px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                    <asp:TemplateField HeaderText="Valor" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                        <ItemTemplate>
                                                                            <div class="input-group">
                                                                                <asp:TextBox ID="txtValorDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("Valor") %>' AutoPostBack="False" Style="text-transform: uppercase" OnTextChanged="txtValorDetalle_TextChanged"></asp:TextBox>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Id. Ref. SAP" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                        <ItemTemplate>
                                                                            <div class="input-group">
                                                                                <asp:TextBox ID="txtIdReferenciaSAPDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("IdReferenciaSAP") %>' CommandName="ButtonField" AutoPostBack="True"></asp:TextBox>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Campo SAP" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                        <ItemTemplate>
                                                                            <div class="input-group">
                                                                                <asp:TextBox ID="txtDescripcionCampoSAPDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("DescripcionCampoSAP") %>' CommandName="ButtonField" AutoPostBack="True" Style="text-transform: uppercase"></asp:TextBox>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle CssClass="thead-dark" />
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-12 mb-0 bg-100">
                                <h6 class="mt-2 mb-1">CLIENTE</h6>
                                <hr class="bg-300 m-0" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-3 col-md-4 mt-2">
                                <span class="far fa-address-card"></span>
                                <label class="text-black-50 col-form-label fs--2 m-1">Código Cliente:</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtIdCliente" runat="server" CssClass="form-control form-control-sm" TabIndex="23"></asp:TextBox><%--</td>--%>
                                    <asp:LinkButton ID="btnAdicionarCliente" runat="server" CssClass="btn btn-primary btn-sm" Text="<span class='fas fa-binoculars'></span>" ToolTip="Crear Cliente" />
                                    <%--<span class="small">(F2)&nbsp;</span>--%>
                                    <asp:RequiredFieldValidator ID="rfvIdCliente" runat="server"
                                        ControlToValidate="txtIdCliente" EnableClientScript="False"
                                        ErrorMessage="Ingrese el código del cliente" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                    <input type="hidden" runat="server" id="hfdIdTipoDocumentoCliente" />
                                    <asp:HiddenField ID="hfdIdAuxiliar" runat="server" />
                                    <asp:HiddenField ID="hfdIdTipoPersonaCliente" runat="server" />
                                    <asp:HiddenField ID="hfdIdTipoCliente" runat="server" />
                                    <asp:HiddenField ID="hfdIdUbicacionGeograficaCliente" runat="server" />
                                    <asp:HiddenField ID="hfdIdUbicacionGeograficaClienteUbicacion" runat="server" />
                                    <asp:HiddenField ID="hfdNroDocumentoCliente" runat="server" />
                                    <asp:HiddenField ID="hfdDireccionFiscalCliente" runat="server" />
                                    <asp:HiddenField ID="hfdTelefonoContactoCliente" runat="server" />

                                    <asp:LinkButton ID="lnk_mostrarPanelMensajeCliente" runat="server"></asp:LinkButton>
                                    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMensajeCliente_ModalPopupExtender"
                                        runat="server" BackgroundCssClass="FondoAplicacion"
                                        CancelControlID="imgbtnCancelarMensajeClienteImagen" DropShadow="False"
                                        DynamicServicePath="" Enabled="True" PopupControlID="pnlMensajeCliente"
                                        TargetControlID="lnk_mostrarPanelMensajeCliente">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="pnlMensajeCliente" runat="server" CssClass="container">
                                        <div class="modal-dialog modal-sm">
                                            <div class="shadow rounded">
                                                <div class="modal-dialog-scrollable">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <h4 class="modal-title" id="myModalLabelMensajeCliente">Mensaje Informativo</h4>
                                                            <asp:Button ID="imgbtnCancelarMensajeClienteImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                        </div>
                                                        <div class="modal-body">
                                                            <div class="row pt-2 pb-2">
                                                                <asp:Label ID="lblTituloMensajeCliente" runat="server" CssClass="col-sm-12 col-form-label-sm text-center">Se creó el siguiente número de cliente</asp:Label>
                                                                <div class="col-sm-12 text-center">
                                                                    <h1>
                                                                        <asp:Label ID="lblNroClienteMensajeCliente" runat="server" Text="00000" CssClass="col-form-label-sm h4"></asp:Label></h1>
                                                                </div>
                                                                <div class="col-sm-12 text-center">
                                                                    <asp:Label ID="lblDatoInformativoMensajeCliente" runat="server" Text="" CssClass="texto_estado short_estado light-orange borde_redondo1"></asp:Label>
                                                                </div>
                                                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="row pt-1">
                                                                            <label class="col-md-4 text-black-50 col-form-label col-form-label-sm">País:</label>
                                                                            <div class="col-md-8">
                                                                                <div class="input-group">
                                                                                    <asp:DropDownList ID="cboPaisMensajeCliente" runat="server" AppendDataBoundItems="True" TabIndex="100"
                                                                                        CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase"
                                                                                        AutoPostBack="True">
                                                                                        <asp:ListItem>SELECCIONE EL PAIS</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvPaisMensajeCliente" runat="server"
                                                                                        ControlToValidate="cboPaisMensajeCliente" EnableClientScript="False"
                                                                                        ErrorMessage="Ingrese el País" Font-Size="10px" ForeColor="Red"
                                                                                        ValidationGroup="vgrpValidarCliente">(*)</asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row pt-1">
                                                                            <label class="col-md-4 text-black-50 col-form-label col-form-label-sm">Departamento:</label>
                                                                            <div class="col-md-8">
                                                                                <div class="input-group">
                                                                                    <asp:DropDownList ID="cboDepartamentoMensajeCliente" runat="server" AppendDataBoundItems="True"
                                                                                        CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase" TabIndex="101"
                                                                                        AutoPostBack="True">
                                                                                        <asp:ListItem>SELECCIONE EL DEPARTAMENTO</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvDepartamentoMensajeCliente" runat="server"
                                                                                        ControlToValidate="cboDepartamentoMensajeCliente" EnableClientScript="False"
                                                                                        ErrorMessage="Ingrese el Departamento" Font-Size="10px" ForeColor="Red"
                                                                                        ValidationGroup="vgrpValidarCliente">(*)</asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row pt-1">
                                                                            <label class="col-md-4 text-black-50 col-form-label col-form-label-sm">Provincia:</label>
                                                                            <div class="col-md-8">
                                                                                <div class="input-group">
                                                                                    <asp:DropDownList ID="cboProvinciaMensajeCliente" runat="server" AppendDataBoundItems="True" TabIndex="102"
                                                                                        CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase"
                                                                                        AutoPostBack="True">
                                                                                        <asp:ListItem>SELECCIONE LA PROVINCIA</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvProvinciaMensajeCliente" runat="server"
                                                                                        ControlToValidate="cboProvinciaMensajeCliente" EnableClientScript="False"
                                                                                        ErrorMessage="Ingrese la Provincia" Font-Size="10px" ForeColor="Red"
                                                                                        ValidationGroup="vgrpValidarCliente">(*)</asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row pt-1">
                                                                            <label class="col-md-4 text-black-50 col-form-label col-form-label-sm">Distrito:</label>
                                                                            <div class="col-md-8">
                                                                                <div class="input-group">
                                                                                    <asp:DropDownList ID="cboDistritoMensajeCliente" runat="server" AppendDataBoundItems="True"
                                                                                        CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase" TabIndex="103"
                                                                                        AutoPostBack="False">
                                                                                        <asp:ListItem>SELECCIONE EL DISTRITO</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvDistritoMensajeCliente" runat="server"
                                                                                        ControlToValidate="cboDistritoMensajeCliente" EnableClientScript="False"
                                                                                        ErrorMessage="Ingrese el Distrito" Font-Size="10px" ForeColor="Red"
                                                                                        ValidationGroup="vgrpValidarCliente">(*)</asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>

                                                                <div class="row pt-1">
                                                                    <div class="col-sm-12">
                                                                        <asp:Label ID="lblDireccionAdicionalMensajeCliente" runat="server" Text="Dirección:" CssClass="text-black-50 col-form-label col-form-label-sm"></asp:Label>
                                                                        <asp:TextBox ID="txtDireccionAdicionalMensajeCliente" runat="server" CssClass="form-control form-control-sm" Style="text-transform: uppercase;" TabIndex="104"></asp:TextBox>
                                                                    </div>
                                                                    <div class="col-sm-12">
                                                                        <asp:Label ID="lblCorreoAdicionalMensajeCliente" runat="server" Text="Correo:" CssClass="text-black-50 col-form-label col-form-label-sm"></asp:Label>
                                                                        <asp:TextBox ID="txtCorreoAdicionalMensajeCliente" runat="server" CssClass="form-control form-control-sm" Style="text-transform: uppercase;" TabIndex="105" TextMode="Email"></asp:TextBox>
                                                                    </div>
                                                                    <div class="col-sm-12">
                                                                        <asp:Label ID="lblTelefonoMensajeCliente" runat="server" Text="Teléfono:" CssClass="text-black-50 col-form-label col-form-label-sm"></asp:Label>
                                                                        <asp:TextBox ID="txtTelefonoMensajeCliente" runat="server" CssClass="form-control form-control-sm" Style="text-transform: uppercase;" TabIndex="106"></asp:TextBox>
                                                                    </div>
                                                                    <div class="col-md-12">
                                                                        <asp:Label ID="lblMensajeMensajeCliente" runat="server" Style="text-align: center;" CssClass="col-form-label col-form-label-sm text-danger"></asp:Label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="modal-footer">
                                                            <asp:Button ID="btnAceptarMensajeClienteImagen" runat="server" Text="Aceptar" CssClass="btn btn-primary btn-sm me-0 mb-0" ToolTip="Aceptar Información" ValidationGroup="vgrpValidarCliente" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <asp:LinkButton ID="lnk_mostrarPanelCliente" runat="server"></asp:LinkButton>
                                    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelCliente_ModalPopupExtender" runat="server"
                                        BackgroundCssClass="FondoAplicacion" DropShadow="False" DynamicServicePath="" Enabled="True"
                                        CancelControlID="btnCancelarCliente"
                                        PopupControlID="pnlSeleccionarCliente" TargetControlID="lnk_mostrarPanelCliente">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:UpdatePanel ID="updpnlSeleccionarCliente" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlSeleccionarCliente" runat="server" CssClass="container" DefaultButton="imgbtnBuscarCliente">
                                                <div class="modal-dialog modal-xl">
                                                    <div class="shadow rounded">
                                                        <div class="modal-dialog-scrollable">
                                                            <div class="modal-content">
                                                                <div class="modal-header bg-light">
                                                                    <h4 class="modal-title" id="myModalLabelCliente">Seleccionar Cliente</h4>
                                                                    <%--<asp:Button ID="imgbtnCancelarClienteImagen" runat="server" Text="&times;" CssClass="close bg-transparent border-0" data-dismiss="modal" aria-label="Close" />--%>
                                                                    <asp:Button ID="imgbtnCancelarClienteImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                                </div>
                                                                <div class="modal-body">
                                                                    <div class="row pt-2 pb-2">
                                                                        <span class="col-sm-2 col-form-label-sm">Buscar por</span>
                                                                        <div class="col-sm-4">
                                                                            <asp:DropDownList ID="cboFiltroCliente" runat="server" CssClass="form-select form-select-sm js-choice"
                                                                                TabIndex="74">
                                                                                <asp:ListItem Value="vRazonSocialCliente">RAZ.SOC.</asp:ListItem>
                                                                                <asp:ListItem Value="cIdCliente">CODIGO</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                        <div class="col-sm-6">
                                                                            <div class="input-group">
                                                                                <asp:TextBox ID="txtBuscarCliente" runat="server" CssClass="form-control form-control-sm" TabIndex="61"
                                                                                    Style="text-transform: uppercase" placeholder="Ingrese Busqueda" onfocus="fnSetFocus('ctl00$cuerpo$txtBuscarCliente')"></asp:TextBox>
                                                                                &nbsp;
                                                                        <asp:ImageButton ID="imgbtnBuscarCliente" runat="server" Height="18px" TabIndex="3" CssClass="mt-1"
                                                                            ImageUrl="~/Imagenes/buscar.png" ToolTip="Buscar Registro" Width="21px" onfocus="fnSetFocus('ctl00$cuerpo$txtBuscarCliente')" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-12 mt-3">
                                                                            <div style="overflow: auto">
                                                                                <asp:GridView ID="grdListaCliente" runat="server" AutoGenerateColumns="False" TabIndex="63"
                                                                                    GridLines="None" CssClass="table table-responsive-sm table-striped table-hover small" AllowPaging="True"
                                                                                    EmptyDataText="No hay registros a visualizar" PageSize="6">
                                                                                    <PagerStyle CssClass="pgr" />
                                                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                                    <Columns>
                                                                                        <asp:CommandField ShowSelectButton="True" HeaderStyle-CssClass="bg-200 text-900" />
                                                                                        <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900" />
                                                                                        <asp:BoundField DataField="DNI" HeaderText="DNI" HeaderStyle-CssClass="bg-200 text-900" />
                                                                                        <asp:BoundField DataField="RUC" HeaderText="RUC" HeaderStyle-CssClass="bg-200 text-900" />
                                                                                        <asp:BoundField DataField="Descripcion" HeaderText="Razón Social" HeaderStyle-CssClass="bg-200 text-900" />
                                                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900" />
                                                                                        <asp:BoundField DataField="Direccion" HeaderText="Dirección" HeaderStyle-CssClass="bg-200 text-900" />
                                                                                    </Columns>
                                                                                    <HeaderStyle CssClass="thead-dark" />
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="modal-footer">
                                                                    <asp:Label ID="lblMensajeCliente" runat="server" CssClass="col-form-label col-form-label-sm text-danger"></asp:Label>
                                                                    <%--<div class="col-md-4 text-end p-2">--%>
                                                                    <asp:Button ID="btnAceptarCliente" runat="server" ValidationGroup="vgrpValidarBusqueda"
                                                                        ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm d-block" />
                                                                    &nbsp;
                                                            <asp:Button ID="btnCancelarCliente" runat="server"
                                                                ToolTip="Cancelar Registro" TabIndex="73" Text="Cancelar" CssClass="btn btn-outline-google-plus btn-sm d-block" />
                                                                    <%--</div>--%>
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
                            <%--<div class="col-lg-3 col-md-4 mt-2">
                        <span class="fas fa-phone-alt"></span><label class="text-black-50 col-form-label fs--2 m-1">Teléfono Contacto:</label>
                        <div class="input-group">
                            <asp:TextBox ID="txtTelefonoContactoCliente" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="19"></asp:TextBox>
                        </div>
                    </div>--%>
                            <div class="col-lg-4 col-md-4 mt-2">
                                <span class="fas fa-shipping-fast"></span>
                                <label class="text-black-50 col-form-label fs--2 m-1">Ubicación:</label>
                                <div class="input-group">
                                    <asp:DropDownList ID="cboClienteUbicacion" runat="server" AppendDataBoundItems="True"
                                        AutoPostBack="False" CssClass="form-select form-select-sm js-choice"
                                        Style="text-transform: uppercase" TabIndex="25">
                                        <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Button ID="btnAdicionarClienteUbicacion" runat="server" CssClass="btn btn-primary btn-sm me-0 mb-0" Text="[+]" ToolTip="Cargar Cotización" />
                                    <asp:RequiredFieldValidator ID="rfvClienteUbicacion" runat="server"
                                        ControlToValidate="cboClienteUbicacion" Display="Static" EnableClientScript="False"
                                        ErrorMessage="Ingrese la ubicación del cliente" Font-Size="10px" ForeColor="Red"
                                        InitialValue="SELECCIONE DATO" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlComponentes" runat="server">
                <div class="card mb-3">
                    <div class="card-body">
                        <div class="row justify-content-between">
                            <div class="col-lg-8 col-md-8 col-sm-7">
                                <h5 class="mb-1" data-anchor="data-anchor">Listado de Componentes</h5>
                            </div>
                            <asp:LinkButton ID="lnk_mostrarPanelCatalogoComponente" runat="server"></asp:LinkButton>
                            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelCatalogoComponente_ModalPopupExtender"
                                runat="server" BackgroundCssClass="FondoAplicacion"
                                CancelControlID="btnCancelarCatalogoComponente"
                                Enabled="True" PopupControlID="pnlIngresarCatalogoComponente"
                                TargetControlID="lnk_mostrarPanelCatalogoComponente">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:LinkButton ID="lnk_mostrarPanelEquipoComponente" runat="server"></asp:LinkButton>
                            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelEquipoComponente_ModalPopupExtender"
                                runat="server" BackgroundCssClass="FondoAplicacion"
                                CancelControlID="btnCancelarEquipoComponente"
                                Enabled="True" PopupControlID="pnlIngresarEquipoComponente"
                                TargetControlID="lnk_mostrarPanelEquipoComponente">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:LinkButton ID="lnk_mostrarPanelMantenimientoSistemaFuncional" runat="server"></asp:LinkButton>
                            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMantenimientoSistemaFuncional_ModalPopupExtender" runat="server"
                                BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True"
                                CancelControlID="btnCancelarMantenimientoSistemaFuncional"
                                PopupControlID="pnlMantenimientoSistemaFuncional" TargetControlID="lnk_mostrarPanelMantenimientoSistemaFuncional">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:UpdatePanel ID="updpnlIngresarCatalogoComponente" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlIngresarCatalogoComponente" runat="server" CssClass="container">
                                        <div class="modal-dialog modal-xl">
                                            <div class="shadow rounded">
                                                <div class="modal-dialog-scrollable">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <h4 class="modal-title" id="myModalLabelCatalogoComponente">Mantenimiento del Componente / Catalogo</h4>
                                                            <asp:Button ID="imgbtnCancelarCatalogoComponenteImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                        </div>
                                                        <div class="modal-body">
                                                            <div class="form-group">
                                                                <div class="row mt-1">
                                                                    <div class="col-lg-6 col-sm-12">
                                                                        <div class="row mt-1">
                                                                            <div class="col-12 mb-0 bg-100 d-none d-md-block">
                                                                                <h6 class="mt-2 mb-1">DATOS PRINCIPALES</h6>
                                                                                <hr class="bg-300 m-0" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-lg-3 col-md-4 mt-2">
                                                                                <span class="far fa-address-card"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Id.Compte.:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtIdCatalogoComponente" runat="server" CssClass="form-control form-control-sm" TabIndex="20"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-4 col-md-4 mt-2">
                                                                                <span class="far fa-address-card"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Tipo Activo:</label>
                                                                                <div class="input-group">
                                                                                    <asp:DropDownList ID="cboTipoActivoCatalogoComponente" runat="server" AppendDataBoundItems="True" TabIndex="21"
                                                                                        AutoPostBack="True" CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase">
                                                                                        <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <ajaxToolkit:ListSearchExtender ID="cboTipoActivoCatalogoComponente_ListSearchExtender" runat="server"
                                                                                        Enabled="True" PromptText="Iniciar Búsqueda" TargetControlID="cboTipoActivoCatalogoComponente">
                                                                                    </ajaxToolkit:ListSearchExtender>
                                                                                    <asp:RequiredFieldValidator ID="rfvTipoActivoCatalogoComponente" runat="server"
                                                                                        ControlToValidate="cboTipoActivoCatalogoComponente" EnableClientScript="False" InitialValue="SELECCIONE DATO"
                                                                                        ErrorMessage="Ingrese el código de tipo de activo" Font-Size="10px" ForeColor="Red"
                                                                                        ValidationGroup="vgrpValidarCatalogoComponente">(*)</asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-5 col-md-4 mt-2">
                                                                                <span class="far fa-address-card"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Sist.Funcional:</label>
                                                                                <div class="input-group">
                                                                                    <asp:DropDownList ID="cboSistemaFuncionalCatalogoComponente" runat="server" AppendDataBoundItems="True" TabIndex="22"
                                                                                        AutoPostBack="False" CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase">
                                                                                        <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                                                                                        <div class="dropdown font-sans-serif btn-reveal-trigger">
                                                                                            <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesTipoActivoCatalogoComponente" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                                                                                            <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesTipoActivoCatalogoComponente">
                                                                                                <asp:LinkButton ID="lnkbtnNuevoSistemaFuncionalCatalogoComponente" runat="server" ToolTip="Crear un componente" Text="<span class='fas fa-file me-2'></span>Nuevo" CssClass="nav-link nav-link-card-details" />
                                                                                                <asp:LinkButton ID="lnkbtnEditarSistemaFuncionalCatalogoComponente" runat="server" ToolTip="Editar un componente" Text="<span class='fas fa-file-signature me-2'></span>Editar" CssClass="nav-link nav-link-card-details" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <ajaxToolkit:ListSearchExtender ID="cboSistemaFuncionalCatalogoComponente_ListSearchExtender" runat="server"
                                                                                        Enabled="True" PromptText="Iniciar Búsqueda" TargetControlID="cboSistemaFuncionalCatalogoComponente">
                                                                                    </ajaxToolkit:ListSearchExtender>
                                                                                    <asp:RequiredFieldValidator ID="rfvSistemaFuncionalCatalogoComponente" runat="server"
                                                                                        ControlToValidate="cboSistemaFuncionalCatalogoComponente" EnableClientScript="False" InitialValue="SELECCIONE DATO"
                                                                                        ErrorMessage="Ingrese el sistema funcional" Font-Size="10px" ForeColor="Red"
                                                                                        ValidationGroup="vgrpValidarCatalogoComponente">(*)</asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-lg-12 col-md-4 col-sm-4 mt-2">
                                                                                <span class="fas fa-shipping-fast"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Descripción:</label>
                                                                                <div class="input-group">
                                                                                    <asp:DropDownList ID="cboDescripcionCatalogoComponente" runat="server" AppendDataBoundItems="True" TabIndex="23"
                                                                                        AutoPostBack="True" CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase">
                                                                                        <asp:ListItem Value="">SELECCIONE DATO</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <asp:TextBox ID="txtDescripcionCatalogoComponente" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="24"
                                                                                        Style="text-transform: uppercase" placeholder="Descripción Componente"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="rfvDescripcionCatalogoComponente" runat="server"
                                                                                        ControlToValidate="txtDescripcionCatalogoComponente" EnableClientScript="False"
                                                                                        ErrorMessage="Ingrese la descripción del componente / catálogo" Font-Size="10px" ForeColor="Red"
                                                                                        ValidationGroup="vgrpValidarCatalogoComponente">(*)</asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-12 col-md-8 col-sm-8 mt-2">
                                                                                <span class="fas fa-chalkboard-teacher"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Desc.Abreviada:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtDescripcionAbreviadaCatalogoComponente" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="25"
                                                                                        Style="text-transform: uppercase" placeholder="Descripción Abreviada Componente"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="rfvDescripcionAbreviadaCatalogoComponente" runat="server"
                                                                                        ControlToValidate="txtDescripcionAbreviadaCatalogoComponente" EnableClientScript="False"
                                                                                        ErrorMessage="Ingrese la descripción abreviada del componente / catálogo" Font-Size="10px"
                                                                                        ForeColor="Red" ValidationGroup="vgrpValidarCatalogoComponente">(*)</asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-lg-3 col-md-4 col-sm-4 mt-2">
                                                                                <span class="fas fa-shipping-fast"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Vida Útil:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtVidaUtilCatalogoComponente" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="26"
                                                                                        Style="text-transform: uppercase" placeholder="Vita Útil Componente" ToolTip="Vida útil del componente en meses"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-3 col-md-4 col-sm-4 mt-2">
                                                                                <span class="fas fa-chalkboard-teacher"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">P.Garantía:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtPeriodoGarantiaCatalogoComponente" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="27"
                                                                                        Style="text-transform: uppercase" placeholder="Periodo de Garantía en meses del Componente" ToolTip="Periodo de garantía en meses"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-3 col-md-4 col-sm-4 mt-2">
                                                                                <span class="fas fa-chalkboard-teacher"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">P.Mant.:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtPeriodoMinimoCatalogoComponente" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="28"
                                                                                        Style="text-transform: uppercase" placeholder="Periodo Mínimo de Mantenimiento en meses del Componente" ToolTip="Periodo mínimo en meses"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-3 col-md-4 col-sm-4 mt-2 invisible">
                                                                                <span class="fas fa-chalkboard-teacher"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Cta.Contable:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtCuentaContableCatalogoComponente" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="29"
                                                                                        Style="text-transform: uppercase" placeholder="Cta. Cble. del Componente"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-3 col-md-4 col-sm-4 mt-2 invisible">
                                                                                <span class="fas fa-chalkboard-teacher"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Cta.Ctbl.Leasing:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtCuentaContableLeasingCatalogoComponente" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="30"
                                                                                        Style="text-transform: uppercase" placeholder="Cta. Cble. Leasing Componente"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-lg-6 col-sm-12 bg-200">
                                                                        <div class="row mt-1">
                                                                            <div class="col-12 mb-0 d-none d-md-block">
                                                                                <h6 class="mt-2 mb-1">CARACTERISTICAS ASIGNADAS</h6>
                                                                                <hr class="bg-300 m-0" />
                                                                            </div>
                                                                        </div>
                                                                        <%--<script type="text/javascript">
                                                                    function CaracteristicaSeleccionada(source, eventArgs) {
                                                                        document.getElementById("cuerpo_hfdIdCaracteristicaCatalogoComponente").value = eventArgs.get_value();
                                                                    }
                                                                    function PopupShown(sender, args) {
                                                                        sender._popupBehavior._element.style.zIndex = 99999999;
                                                                    }
                                                                </script>--%>

                                                                        <div id="divFiltrarCaracteristicaCatalogoComponente" runat="server" class="row justify-content-between">
                                                                            <div class="col-lg-12 col-md-12 col-sm-12">
                                                                                <asp:UpdatePanel ID="updpnlGeneral" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                                                                    <ContentTemplate>
                                                                                        <span class="fas fa-link"></span>
                                                                                        <label class="text-black-50 col-form-label fs--2 m-1">Filtrar Caracteristica:</label>
                                                                                        <div class="input-group">
                                                                                            <asp:DropDownList ID="cboCaracteristicaCatalogoComponente" runat="server" AppendDataBoundItems="True" TabIndex="31"
                                                                                                CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase">
                                                                                                <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                            <asp:Button ID="btnAdicionarCaracteristicaCatalogoComponente" runat="server" Text="Agregar" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Caracteristica" TabIndex="32" />
                                                                                            <%--<input ID="hfdIdCaracteristicaCatalogoComponente" runat="server" type="hidden" />--%>
                                                                                        </div>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row justify-content-between mt-3">
                                                                            <div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
                                                                                <asp:UpdatePanel ID="updpnlDetalleCaracteristicaCatalogoComponente" runat="server">
                                                                                    <ContentTemplate>
                                                                                        <asp:Panel ID="pnlDetalleCaracteristicaCatalogoComponente" runat="server" CssClass="bg-light">
                                                                                            <div class="table-responsive scrollbar">
                                                                                                <asp:GridView ID="grdDetalleCaracteristicaCatalogoComponente" runat="server" AutoGenerateColumns="False" TabIndex="33"
                                                                                                    GridLines="None" CssClass="table table-striped table-hover small overflow-hidden" AllowPaging="True"
                                                                                                    EmptyDataText="No hay registros a visualizar" PageSize="50">
                                                                                                    <PagerStyle CssClass="mGrid" />
                                                                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                                                    <Columns>
                                                                                                        <asp:CommandField ShowDeleteButton="True" HeaderStyle-Width="70px" />
                                                                                                        <asp:TemplateField>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:CheckBox ID="chkRowDetalleCaracteristica" runat="server" />
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:BoundField DataField="Item" HeaderText="#" ControlStyle-Width="25px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                                                        <asp:BoundField DataField="IdCaracteristica" HeaderText="Código" HeaderStyle-Width="40px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />

                                                                                                        <asp:TemplateField HeaderText="Valor" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                                                            <ItemTemplate>
                                                                                                                <div class="input-group">
                                                                                                                    <asp:TextBox ID="txtValorDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("Valor") %>' AutoPostBack="False" Style="text-transform: uppercase" OnTextChanged="txtValorDetalleCaracteristicaCatalogoComponente_TextChanged"></asp:TextBox>
                                                                                                                </div>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Id. Ref. SAP" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                                                            <ItemTemplate>
                                                                                                                <div class="input-group">
                                                                                                                    <asp:TextBox ID="txtIdReferenciaSAPDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("IdReferenciaSAP") %>' CommandName="ButtonField" AutoPostBack="False" Style="text-transform: uppercase"></asp:TextBox>
                                                                                                                </div>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Campo SAP" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                                                            <ItemTemplate>
                                                                                                                <div class="input-group">
                                                                                                                    <asp:TextBox ID="txtDescripcionCampoSAPDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("DescripcionCampoSAP") %>' CommandName="ButtonField" AutoPostBack="False" Style="text-transform: uppercase"></asp:TextBox>
                                                                                                                </div>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                    </Columns>
                                                                                                    <HeaderStyle CssClass="thead-dark" />
                                                                                                </asp:GridView>
                                                                                            </div>
                                                                                        </asp:Panel>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="modal-footer">
                                                            <%--<asp:Label ID="lblMensajeCatalogoComponente" runat="server" CssClass="col-form-label col-form-label-sm text-danger"></asp:Label>--%>
                                                            <%--<asp:ValidationSummary ID="ValidationSummary2" runat="server" role="alert" CssClass="alert alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarComponente" />--%>
                                                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarCatalogoComponente" />
                                                            <div class="col-md-4 text-end p-2">
                                                                <asp:Button ID="btnAceptarCatalogoComponente" runat="server" ValidationGroup="vgrpValidarCatalogoComponente" OnClick="btnAceptarCatalogoComponente_Click"
                                                                    ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                                &nbsp;
                                                        <asp:Button ID="btnCancelarCatalogoComponente" runat="server"
                                                            ToolTip="Cancelar Registro" TabIndex="73" Text="Cancelar" CssClass="btn btn-outline-google-plus btn-sm" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:UpdatePanel ID="updpnlIngresarEquipoComponente" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlIngresarEquipoComponente" runat="server" CssClass="container">
                                        <div class="modal-dialog modal-xl">
                                            <div class="shadow rounded">
                                                <div class="modal-dialog-scrollable">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <h4 class="modal-title" id="myModalLabelEquipoComponente">Mantenimiento del Componente / Equipo</h4>
                                                            <asp:Button ID="imgbtnCancelarEquipoComponenteImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                        </div>
                                                        <div class="modal-body">
                                                            <div class="form-group">
                                                                <div class="row mt-1">
                                                                    <div class="col-lg-6 col-sm-12">
                                                                        <div class="row mt-1">
                                                                            <div class="col-12 mb-0 bg-100 d-none d-md-block">
                                                                                <h6 class="mt-2 mb-1">DATOS PRINCIPALES</h6>
                                                                                <hr class="bg-300 m-0" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-lg-3 col-md-4 mt-2">
                                                                                <span class="far fa-address-card"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Id.Compte.:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtIdEquipoComponente" runat="server" CssClass="form-control form-control-sm" TabIndex="20"></asp:TextBox>
                                                                                    <asp:HiddenField ID="hfdFechaTransaccionEquipoComponente" runat="server" />
                                                                                    <asp:HiddenField ID="hfdFechaRegistroTarjetaSAPEquipoComponente" runat="server" />
                                                                                    <asp:HiddenField ID="hfdFechaManufacturaTarjetaSAPEquipoComponente" runat="server" />
                                                                                    <asp:HiddenField ID="hfdFechaCreacionEquipoComponente" runat="server" />
                                                                                    <asp:HiddenField ID="hfdIdUsuarioCreacionEquipoComponente" runat="server" />
                                                                                    <asp:HiddenField ID="hfdIdCatalogoEquipoComponente" runat="server" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-4 col-md-4 mt-2">
                                                                                <span class="far fa-address-card"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Tipo Activo:</label>
                                                                                <div class="input-group">
                                                                                    <asp:DropDownList ID="cboTipoActivoEquipoComponente" runat="server" AppendDataBoundItems="True" TabIndex="21"
                                                                                        AutoPostBack="True" CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase">
                                                                                        <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <ajaxToolkit:ListSearchExtender ID="cboTipoActivoEquipoComponente_ListSearchExtender" runat="server"
                                                                                        Enabled="True" PromptText="Iniciar Búsqueda" TargetControlID="cboTipoActivoEquipoComponente">
                                                                                    </ajaxToolkit:ListSearchExtender>
                                                                                    <asp:RequiredFieldValidator ID="rfvTipoActivoEquipoComponente" runat="server"
                                                                                        ControlToValidate="cboTipoActivoEquipoComponente" EnableClientScript="False" InitialValue="SELECCIONE DATO"
                                                                                        ErrorMessage="Ingrese el código de tipo de activo" Font-Size="10px" ForeColor="Red"
                                                                                        ValidationGroup="vgrpValidarEquipoComponente">(*)</asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-5 col-md-4 mt-2">
                                                                                <span class="far fa-address-card"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Sist.Funcional:</label>
                                                                                <div class="input-group">
                                                                                    <asp:DropDownList ID="cboSistemaFuncionalEquipoComponente" runat="server" AppendDataBoundItems="True" TabIndex="22"
                                                                                        AutoPostBack="False" CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase">
                                                                                        <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                                                                                        <div class="dropdown font-sans-serif btn-reveal-trigger">
                                                                                            <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesTipoActivoEquipoComponente" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                                                                                            <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesTipoActivoEquipoComponente">
                                                                                                <asp:LinkButton ID="lnkbtnNuevoSistemaFuncionalEquipoComponente" runat="server" ToolTip="Crear un componente" Text="<span class='fas fa-file me-2'></span>Nuevo" CssClass="nav-link nav-link-card-details" />
                                                                                                <asp:LinkButton ID="lnkbtnEditarSistemaFuncionalEquipoComponente" runat="server" ToolTip="Editar un componente" Text="<span class='fas fa-file-signature me-2'></span>Editar" CssClass="nav-link nav-link-card-details" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <ajaxToolkit:ListSearchExtender ID="cboSistemaFuncionalEquipoComponente_ListSearchExtender" runat="server"
                                                                                        Enabled="True" PromptText="Iniciar Búsqueda" TargetControlID="cboSistemaFuncionalEquipoComponente">
                                                                                    </ajaxToolkit:ListSearchExtender>
                                                                                    <asp:RequiredFieldValidator ID="rfvSistemaFuncionalEquipoComponente" runat="server"
                                                                                        ControlToValidate="cboSistemaFuncionalEquipoComponente" EnableClientScript="False" InitialValue="SELECCIONE DATO"
                                                                                        ErrorMessage="Ingrese el sistema funcional" Font-Size="10px" ForeColor="Red"
                                                                                        ValidationGroup="vgrpValidarEquipoComponente">(*)</asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-lg-7 col-md-4 col-sm-4 mt-2">
                                                                                <span class="fas fa-shipping-fast"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Descripción:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtDescripcionEquipoComponente" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="23"
                                                                                        Style="text-transform: uppercase" placeholder="Descripción Componente"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="rfvDescripcionEquipoComponente" runat="server"
                                                                                        ControlToValidate="txtDescripcionEquipoComponente" EnableClientScript="False"
                                                                                        ErrorMessage="Ingrese la descripción del componente / catálogo" Font-Size="10px" ForeColor="Red"
                                                                                        ValidationGroup="vgrpValidarEquipoComponente">(*)</asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-5 col-md-8 col-sm-8 mt-2">
                                                                                <span class="fas fa-chalkboard-teacher"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Desc.Abreviada:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtDescripcionAbreviadaEquipoComponente" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="24"
                                                                                        Style="text-transform: uppercase" placeholder="Descripción Abreviada Componente"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="rfvDescripcionAbreviadaEquipoComponente" runat="server"
                                                                                        ControlToValidate="txtDescripcionAbreviadaEquipoComponente" EnableClientScript="False"
                                                                                        ErrorMessage="Ingrese la descripción abreviada del componente / catálogo" Font-Size="10px"
                                                                                        ForeColor="Red" ValidationGroup="vgrpValidarEquipoComponente">(*)</asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-lg-3 col-md-4 col-sm-4 mt-2">
                                                                                <span class="fas fa-shipping-fast"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Vida Útil:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtVidaUtilEquipoComponente" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="25"
                                                                                        Style="text-transform: uppercase" placeholder="Vita Útil Componente" ToolTip="Vida útil del componente en meses"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-3 col-md-4 col-sm-4 mt-2">
                                                                                <span class="fas fa-chalkboard-teacher"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">P.Garantía:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtPeriodoGarantiaEquipoComponente" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="26"
                                                                                        Style="text-transform: uppercase" placeholder="Periodo de Garantía en meses del Componente" ToolTip="Periodo de garantía en meses"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-3 col-md-4 col-sm-4 mt-2">
                                                                                <span class="fas fa-chalkboard-teacher"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">P.Mant.:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtPeriodoMinimoEquipoComponente" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="27"
                                                                                        Style="text-transform: uppercase" placeholder="Periodo Mínimo de Mantenimiento en meses del Componente" ToolTip="Periodo mínimo en meses"></asp:TextBox>
                                                                                </div>
                                                                            </div>

                                                                            <div class="col-lg-3 col-md-4 mt-2">
                                                                                <span class="far fa-address-card"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">N.Ser.:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtNroSerieEquipoComponente" runat="server" CssClass="form-control form-control-sm"
                                                                                        TabIndex="28" Style="text-transform: uppercase" placeholder="Número de serie del componente"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-3 col-md-4 mt-2">
                                                                                <span class="far fa-address-card"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">#Part:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtNroParteEquipoComponente" runat="server" CssClass="form-control form-control-sm"
                                                                                        TabIndex="29" Style="text-transform: uppercase" placeholder="Número de parte del componente"></asp:TextBox>
                                                                                </div>
                                                                            </div>

                                                                            <div class="col-lg-3 col-md-4 col-sm-4 mt-2 invisible">
                                                                                <span class="fas fa-chalkboard-teacher"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Cta.Contable:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtCuentaContableEquipoComponente" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="30"
                                                                                        Style="text-transform: uppercase" placeholder="Cta. Cble. del Componente"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-lg-3 col-md-4 col-sm-4 mt-2 invisible">
                                                                                <span class="fas fa-chalkboard-teacher"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Cta.Ctbl.Leasing:</label>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtCuentaContableLeasingEquipoComponente" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="31"
                                                                                        Style="text-transform: uppercase" placeholder="Cta. Cble. Leasing Componente"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-lg-6 col-sm-12 bg-200">
                                                                        <div class="row mt-1">
                                                                            <div class="col-12 mb-0 d-none d-md-block">
                                                                                <h6 class="mt-2 mb-1">CARACTERISTICAS ASIGNADAS</h6>
                                                                                <hr class="bg-300 m-0" />
                                                                            </div>
                                                                        </div>
                                                                        <%--<div id="divFiltrarCaracteristicaEquipoComponente" runat="server" class="row justify-content-between">--%>
                                                                        <div id="divFiltrarCaracteristicaEquipoComponente" runat="server" class="row justify-content-between">
                                                                            <div class="col-lg-12 col-md-12 col-sm-12">
                                                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                                                                    <ContentTemplate>
                                                                                        <span class="fas fa-link"></span>
                                                                                        <label class="text-black-50 col-form-label fs--2 m-1">Filtrar Caracteristica:</label>
                                                                                        <div class="input-group">
                                                                                            <asp:DropDownList ID="cboCaracteristicaEquipoComponente" runat="server" AppendDataBoundItems="True" TabIndex="29"
                                                                                                CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase">
                                                                                                <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                            <asp:Button ID="btnAdicionarCaracteristicaEquipoComponente" runat="server" Text="Agregar" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Caracteristica" TabIndex="30" />
                                                                                            <%--<input ID="hfdIdCaracteristicaEquipoComponente" runat="server" type="hidden" />--%>
                                                                                        </div>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row justify-content-between mt-3">
                                                                            <div class="viewport table-responsive scrollbar">
                                                                                <div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
                                                                                    <asp:UpdatePanel ID="updpnlDetalleCaracteristicaEquipoComponente" runat="server">
                                                                                        <ContentTemplate>
                                                                                            <asp:Panel ID="pnlDetalleCaracteristicaEquipoComponente" runat="server" CssClass="bg-light">
                                                                                                <%--<div class="table-responsive scrollbar">--%>
                                                                                                <asp:GridView ID="grdDetalleCaracteristicaEquipoComponente" runat="server" AutoGenerateColumns="False" TabIndex="4"
                                                                                                    GridLines="None" CssClass="table table-striped table-hover small overflow-hidden" AllowPaging="True"
                                                                                                    EmptyDataText="No hay registros a visualizar" PageSize="50">
                                                                                                    <PagerStyle CssClass="mGrid" />
                                                                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                                                    <Columns>
                                                                                                        <asp:CommandField ShowDeleteButton="True" HeaderStyle-Width="70px" />
                                                                                                        <asp:TemplateField>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:CheckBox ID="chkRowDetalleCaracteristica" runat="server" />
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:BoundField DataField="Item" HeaderText="#" ControlStyle-Width="25px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                                                        <asp:BoundField DataField="IdCaracteristica" HeaderText="Código" HeaderStyle-Width="40px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                                                        <asp:TemplateField HeaderText="Valor" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                                                            <ItemTemplate>
                                                                                                                <div class="input-group">
                                                                                                                    <asp:TextBox ID="txtValorDetalleEquipoComponente" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("Valor") %>' AutoPostBack="False" Style="text-transform: uppercase" OnTextChanged="txtValorDetalleEquipoComponente_TextChanged"></asp:TextBox>
                                                                                                                </div>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Id. Ref. SAP" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                                                            <ItemTemplate>
                                                                                                                <div class="input-group">
                                                                                                                    <asp:TextBox ID="txtIdReferenciaSAPDetalleEquipoComponente" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("IdReferenciaSAP") %>' CommandName="ButtonField" AutoPostBack="False" Style="text-transform: uppercase" OnTextChanged="txtIdReferenciaSAPDetalleEquipoComponente_TextChanged"></asp:TextBox>
                                                                                                                </div>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Campo SAP" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                                                            <ItemTemplate>
                                                                                                                <div class="input-group">
                                                                                                                    <asp:TextBox ID="txtDescripcionCampoSAPDetalleEquipoComponente" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("DescripcionCampoSAP") %>' CommandName="ButtonField" AutoPostBack="False" Style="text-transform: uppercase" OnTextChanged="txtDescripcionCampoSAPDetalleEquipoComponente_TextChanged"></asp:TextBox>
                                                                                                                </div>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                    </Columns>
                                                                                                    <HeaderStyle CssClass="thead-dark" />
                                                                                                </asp:GridView>
                                                                                                <%--</div>--%>
                                                                                            </asp:Panel>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                                <%--</div>--%>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:ValidationSummary ID="ValidationSummary4" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarEquipoComponente" />
                                                                <div class="col-md-4 text-end p-2">
                                                                    <asp:Button ID="btnAceptarEquipoComponente" runat="server" ValidationGroup="vgrpValidarEquipoComponente" OnClick="btnAceptarEquipoComponente_Click"
                                                                        ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                                    &nbsp;
                                                        <asp:Button ID="btnCancelarEquipoComponente" runat="server"
                                                            ToolTip="Cancelar Registro" TabIndex="73" Text="Cancelar" CssClass="btn btn-outline-google-plus btn-sm" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:UpdatePanel ID="updpnlMantenimientoSistemaFuncional" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlMantenimientoSistemaFuncional" runat="server" CssClass="container" DefaultButton="btnAceptarMantenimientoSistemaFuncional">
                                        <div class="modal-dialog modal-lg">
                                            <div class="shadow rounded">
                                                <div class="modal-dialog-scrollable">
                                                    <div class="modal-content">
                                                        <div class="modal-header bg-light">
                                                            <h4 class="modal-title" id="myModalLabelMantenimientoSistemaFuncional">Mantenimiento Sistema Funcional</h4>
                                                            <asp:Button ID="imgbtnCancelarSistemaFuncionalImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                        </div>
                                                        <div class="modal-body">
                                                            <div class="row pt-2 pb-2">
                                                                <div class="col-lg-2 col-md-1 col-sm-5">
                                                                    <span class="far fa-address-card"></span>
                                                                    <label class="text-black-50 col-form-label fs--2 m-1">Id:</label>
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtIdSistemaFuncionalMantenimientoSistemaFuncional" runat="server" CssClass="form-control form-control-sm" TabIndex="100"
                                                                            Style="text-transform: uppercase" autocomplete="off"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-lg-7 col-md-12 col-sm-12">
                                                                    <span class="far fa-address-card"></span>
                                                                    <label class="text-black-50 col-form-label fs--2 m-1">Descripción:</label>
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtDescripcionMantenimientoSistemaFuncional" runat="server" CssClass="form-control form-control-sm" TabIndex="101"
                                                                            Style="text-transform: uppercase" placeholder="Descripción Sistema Funcional" autocomplete="off"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvDescripcionMantenimientoSistemaFuncional" runat="server"
                                                                            ControlToValidate="txtDescripcionMantenimientoSistemaFuncional" EnableClientScript="False"
                                                                            ErrorMessage="Ingrese la descripción" Font-Size="10px" ForeColor="Red"
                                                                            ValidationGroup="vgrpValidarMantenimientoSistemaFuncional">(*)</asp:RequiredFieldValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="col-lg-3 col-md-8 col-sm-12">
                                                                    <span class="far fa-address-card"></span>
                                                                    <label class="text-black-50 col-form-label fs--2 m-1">Descripción Abreviada:</label>
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtDescripcionAbreviadaMantenimientoSistemaFuncional" runat="server" CssClass="form-control form-control-sm" TabIndex="102"
                                                                            Style="text-transform: uppercase" placeholder="Descripción Abreviada Sistema Funcional" autocomplete="off"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="modal-footer">
                                                            <%--<div class="container">--%>
                                                            <asp:ValidationSummary ID="ValidationSummary3" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarMantenimientoSistemaFuncional" />
                                                            <div class="col-lg-6 col-md-7 text-end p-2">
                                                                <asp:Button ID="btnAceptarMantenimientoSistemaFuncional" runat="server" ValidationGroup="vgrpValidarMantenimientoSistemaFuncional"
                                                                    ToolTip="Aceptar Registro" TabIndex="103" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                                &nbsp;
                                                        <asp:Button ID="btnCancelarMantenimientoSistemaFuncional" runat="server"
                                                            ToolTip="Cancelar Registro" TabIndex="104" Text="Cancelar" CssClass="btn btn-outline-google-plus btn-sm" />
                                                            </div>
                                                            <%--</div>--%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="row justify-content-between mt-3">
                            <div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
                                <asp:UpdatePanel ID="UpdPnlComponentes" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="grdCatalogoComponente" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="lnkbtnAgregarCatalogoComponente" EventName="Click" />
                                    </Triggers>
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
                                                xPos = $get("divGridCatalogoComponente").scrollLeft;
                                                yPos = $get("divGridCatalogoComponente").scrollTop;
                                            }
                                            function EndRequestHandler(sender, args) {
                                                $get("divGridCatalogoComponente").scrollLeft = xPos;
                                                $get("divGridCatalogoComponente").scrollTop = yPos;
                                            }
                                        </script>
                                        <%--Final:Se utiliza para que al seleccionar la grilla no se posicione al inicio--%>

                                        <asp:Panel ID="pnlSeleccionarElemento" runat="server">
                                            <div class="form-group mt-2">
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="card mb-3">
                                                            <div class="card-header">
                                                                <div class="row">
                                                                    <div class="col-8">
                                                                        <h6 class="modal-title">Componentes del Catálogo</h6>
                                                                    </div>
                                                                    <div class="col-4">
                                                                        <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                                                                            <div class="dropdown font-sans-serif btn-reveal-trigger">
                                                                                <span class="fas fa-cog"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Opciones:</label>
                                                                                <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesComponentesCatalogo" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                                                                                <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesComponentesCatalogo">
                                                                                    <asp:LinkButton ID="lnkbtnNuevoCatalogoComponente" runat="server" ToolTip="Crear un componente" Text="<span class='fas fa-file me-2'></span>Nuevo" CssClass="nav-link nav-link-card-details" />
                                                                                    <asp:LinkButton ID="lnkbtnEditarCatalogoComponente" runat="server" ToolTip="Editar un componente" Text="<span class='fas fa-file-signature me-2'></span>Editar" CssClass="nav-link nav-link-card-details" />
                                                                                    <asp:LinkButton ID="lnkbtnAgregarCatalogoComponente" runat="server" ToolTip="Agregar componente al equipo" Text="<span class='fas fa-file-signature me-2'></span>Agregar Componente" CssClass="nav-link nav-link-card-details" />
                                                                                    <asp:LinkButton ID="lnkbtnAgregarTodosCatalogoComponente" runat="server" ToolTip="Agregar componente al equipo" Text="<span class='fas fa-file-signature me-2'></span>Agregar Todos" CssClass="nav-link nav-link-card-details" />
                                                                                    <%--<div class="dropdown-divider"></div>
                                                                          <asp:LinkButton ID="lnkbtnEliminarCatalogoComponente" runat="server" ToolTip="Eliminar un componente" Text="<span class='fas fa-file-excel me-2'></span>Eliminar" CssClass="nav-link nav-link-card-details text-danger" />--%>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="card-body">
                                                                <div id="divGridCatalogoComponente" style="overflow: auto; height: 220px; align: left;" class="scrollbar">
                                                                    <asp:GridView ID="grdCatalogoComponente" runat="server" AutoGenerateColumns="False" TabIndex="20"
                                                                        GridLines="None" CssClass="table table-responsive-sm table-striped table-hover small" AllowPaging="True"
                                                                        EmptyDataText="No hay registros a visualizar" PageSize="50">
                                                                        <PagerStyle CssClass="pgr" />
                                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                        <AlternatingRowStyle CssClass="alt" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="IdCatalogo" HeaderText="Código" />
                                                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                                                                            <asp:BoundField DataField="IdTipoActivo" HeaderText="Tipo Activo" />
                                                                            <asp:BoundField DataField="IdSistemaFuncional" HeaderText="Sistema Funcional" />
                                                                            <asp:BoundField DataField="DescripcionAbreviada" HeaderText="Descripción Abreviada" />
                                                                        </Columns>
                                                                        <HeaderStyle CssClass="thead-dark" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="card mb-3">
                                                            <div class="card-header">
                                                                <div class="row">
                                                                    <div class="col-8">
                                                                        <h6 class="modal-title">Componentes del Equipo</h6>
                                                                    </div>
                                                                    <div class="col-4">
                                                                        <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                                                                            <div class="dropdown font-sans-serif btn-reveal-trigger">
                                                                                <span class="fas fa-cog"></span>
                                                                                <label class="text-black-50 col-form-label fs--2 m-1">Opciones:</label>
                                                                                <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesComponentesEquipo" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                                                                                <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesComponentesEquipo">
                                                                                    <asp:LinkButton ID="lnkbtnEditarEquipoComponente" runat="server" ToolTip="Crear un componente" Text="<span class='fas fa-file-signature me-2'></span>Editar" CssClass="nav-link nav-link-card-details" />
                                                                                    <div class="dropdown-divider"></div>
                                                                                    <asp:LinkButton ID="lnkbtnEliminarEquipoComponente" runat="server" ToolTip="Editar un componente" Text="<span class='fas fa-file-excel me-2'></span>Eliminar" CssClass="nav-link nav-link-card-details" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <asp:LinkButton ID="lnk_mostrarPanelMensajeDocumentoValidacion" runat="server"></asp:LinkButton>
                                                                    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMensajeDocumentoValidacion_ModalPopupExtender"
                                                                        runat="server" BackgroundCssClass="FondoAplicacion"
                                                                        CancelControlID="imgbtnCancelarDocumentoValidacionImagen" DropShadow="False"
                                                                        DynamicServicePath="" Enabled="True" PopupControlID="pnlMensajeDocumentoValidacion"
                                                                        TargetControlID="lnk_mostrarPanelMensajeDocumentoValidacion">
                                                                    </ajaxToolkit:ModalPopupExtender>
                                                                    <asp:Panel ID="pnlMensajeDocumentoValidacion" runat="server" CssClass="container">
                                                                        <div class="modal-dialog modal-sm">
                                                                            <div class="shadow rounded">
                                                                                <div class="modal-dialog-scrollable">
                                                                                    <div class="modal-content">
                                                                                        <div class="modal-header">
                                                                                            <h4 class="modal-title" id="myModalLabelMensajeDocumentoValidacion">Mensaje Informativo</h4>
                                                                                            <asp:Button ID="imgbtnCancelarDocumentoValidacionImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                                                        </div>
                                                                                        <div class="modal-body">
                                                                                            <div class="form-group row pt-2">
                                                                                                <asp:HiddenField ID="hfdTipoOperacionMensajeDocumentoValidacion" runat="server" />
                                                                                                <span class="col-sm-12 col-form-label-sm text-center">
                                                                                                    <asp:Label ID="lblDescripcionMensajeDocumentoValidacion" runat="server" class="text-black-50 m-1">¿Seguro desea eliminar este componente asignado?</asp:Label>
                                                                                                </span>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="modal-footer">
                                                                                            <asp:Button ID="btnSiMensajeDocumentoValidacion" runat="server" Text="Si" CssClass="btn btn-outline-facebook btn-sm d-block" ToolTip="Si desea generar Plantilla" />
                                                                                            &nbsp;&nbsp;<asp:Button ID="btnNoMensajeDocumentoValidacion" runat="server" Text="No" CssClass="btn btn-outline-google-plus btn-sm d-block" ToolTip="No desea generar la Plantilla" />
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </asp:Panel>
                                                                    <br />
                                                                </div>
                                                            </div>
                                                            <div class="card-body">
                                                                <div id="divGridComponenteEquipo" style="overflow: auto; height: 220px; align: left;" class="scrollbar">
                                                                    <asp:GridView ID="grdEquipoComponente" runat="server" AutoGenerateColumns="False" TabIndex="21"
                                                                        GridLines="None" CssClass="table table-responsive-sm table-striped table-hover small" AllowPaging="True"
                                                                        EmptyDataText="No hay registros a visualizar" PageSize="50">
                                                                        <PagerStyle CssClass="pgr" />
                                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                        <AlternatingRowStyle CssClass="alt" />
                                                                        <Columns>
                                                                            <asp:CommandField ShowSelectButton="True" />
                                                                            <asp:BoundField DataField="IdCatalogo" HeaderText="Código" />
                                                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                                                                            <asp:BoundField DataField="IdTipoActivo" HeaderText="Tipo Activo" />
                                                                            <asp:BoundField DataField="IdSistemaFuncional" HeaderText="Sistema Funcional" />
                                                                            <asp:BoundField DataField="DescripcionAbreviada" HeaderText="Descripción Abreviada" />
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
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <br />
                                <asp:LinkButton ID="lnk_mostrarPanelMensajeClienteUbicacion" runat="server"></asp:LinkButton>
                                <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMensajeClienteUbicacion_ModalPopupExtender"
                                    runat="server" BackgroundCssClass="FondoAplicacion"
                                    DropShadow="False"
                                    DynamicServicePath="" Enabled="True" PopupControlID="pnlMensajeClienteUbicacion"
                                    TargetControlID="lnk_mostrarPanelMensajeClienteUbicacion">
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:Panel ID="pnlMensajeClienteUbicacion" runat="server" CssClass="container">
                                    <div class="modal-dialog modal-md">
                                        <div class="shadow rounded">
                                            <div class="modal-dialog-scrollable">
                                                <div class="modal-content">
                                                    <div class="modal-header">
                                                        <h4 class="modal-title" id="myModalLabelMensajeClienteUbicacion">Mensaje Informativo</h4>
                                                    </div>
                                                    <div class="modal-body">
                                                        <div class="form-group pt-2">
                                                            <div class="row">
                                                                <asp:Label ID="lblTituloMensajeClienteUbicacion" runat="server" CssClass="col-sm-12 col-form-label-sm text-center">Se procederá adicionar una nueva ubicación para el cliente</asp:Label>
                                                                <div class="col-sm-12 text-center">
                                                                    <h1>
                                                                        <asp:Label ID="lblRazonSocialClienteMensajeClienteUbicacion" runat="server" Text="00000" CssClass="col-form-label-sm h4"></asp:Label></h1>
                                                                </div>
                                                                <div class="col-sm-12 text-center">
                                                                    <asp:Label ID="lblDatoInformativoMensajeClienteUbicacion" runat="server" Text="" CssClass="texto_estado short_estado light-orange borde_redondo1"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                                <ContentTemplate>
                                                                    <div class="row pt-1">
                                                                        <div class="col-md-12">
                                                                            <label class="text-black-50 col-form-label fs--2 m-1">País:</label>
                                                                            <div class="input-group">
                                                                                <asp:DropDownList ID="cboPaisMensajeClienteUbicacion" runat="server" AppendDataBoundItems="True" TabIndex="100"
                                                                                    CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase"
                                                                                    AutoPostBack="True">
                                                                                    <asp:ListItem>SELECCIONE EL PAIS</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvPaisMensajeClienteUbicacion" runat="server"
                                                                                    ControlToValidate="cboPaisMensajeClienteUbicacion" EnableClientScript="False"
                                                                                    ErrorMessage="Ingrese el País" Font-Size="10px" ForeColor="Red"
                                                                                    ValidationGroup="vgrpValidarClienteUbicacion">(*)</asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row pt-1">
                                                                        <div class="col-md-12">
                                                                            <label class="text-black-50 col-form-label fs--2 m-1">Departamento:</label>
                                                                            <div class="input-group">
                                                                                <asp:DropDownList ID="cboDepartamentoMensajeClienteUbicacion" runat="server" AppendDataBoundItems="True"
                                                                                    CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase" TabIndex="101"
                                                                                    AutoPostBack="True">
                                                                                    <asp:ListItem>SELECCIONE EL DEPARTAMENTO</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvDepartamentoMensajeClienteUbicacion" runat="server"
                                                                                    ControlToValidate="cboDepartamentoMensajeClienteUbicacion" EnableClientScript="False"
                                                                                    ErrorMessage="Ingrese el Departamento" Font-Size="10px" ForeColor="Red"
                                                                                    ValidationGroup="vgrpValidarClienteUbicacion">(*)</asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row pt-1">
                                                                        <div class="col-md-12">
                                                                            <label class="text-black-50 col-form-label fs--2 m-1">Provincia:</label>
                                                                            <div class="input-group">
                                                                                <asp:DropDownList ID="cboProvinciaMensajeClienteUbicacion" runat="server" AppendDataBoundItems="True" TabIndex="102"
                                                                                    CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase"
                                                                                    AutoPostBack="True">
                                                                                    <asp:ListItem>SELECCIONE LA PROVINCIA</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvProvinciaMensajeClienteUbicacion" runat="server"
                                                                                    ControlToValidate="cboProvinciaMensajeClienteUbicacion" EnableClientScript="False"
                                                                                    ErrorMessage="Ingrese la Provincia" Font-Size="10px" ForeColor="Red"
                                                                                    ValidationGroup="vgrpValidarClienteUbicacion">(*)</asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row pt-1">
                                                                        <div class="col-md-12">
                                                                            <label class="text-black-50 col-form-label fs--2 m-1">Distrito:</label>
                                                                            <div class="input-group">
                                                                                <asp:DropDownList ID="cboDistritoMensajeClienteUbicacion" runat="server" AppendDataBoundItems="True"
                                                                                    CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase" TabIndex="103"
                                                                                    AutoPostBack="False">
                                                                                    <asp:ListItem>SELECCIONE EL DISTRITO</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvDistritoMensajeClienteUbicacion" runat="server"
                                                                                    ControlToValidate="cboDistritoMensajeClienteUbicacion" EnableClientScript="False"
                                                                                    ErrorMessage="Ingrese el Distrito" Font-Size="10px" ForeColor="Red"
                                                                                    ValidationGroup="vgrpValidarClienteUbicacion">(*)</asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>

                                                            <div class="row pt-1">
                                                                <div class="col-sm-12">
                                                                    <asp:Label ID="lblDireccionAdicionalMensajeClienteUbicacion" runat="server" Text="Ubicación:" CssClass="text-black-50 col-form-label fs--2 m-1"></asp:Label>
                                                                    <asp:TextBox ID="txtDireccionAdicionalMensajeClienteUbicacion" runat="server" CssClass="form-control form-control-sm" autocomplete="off" Style="text-transform: uppercase;" TabIndex="104"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <asp:Label ID="lblMensajeClienteUbicacion" runat="server" Style="text-align: center;" CssClass="col-form-label col-form-label-sm text-danger"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="modal-footer">
                                                        <div class="container">
                                                            <div class="row">
                                                                <div class="col-md-8 mt-2">
                                                                    <asp:ValidationSummary ID="ValidationSummary8" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarClienteUbicacion" />
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-8 text-end p-2">
                                                                        <asp:Button ID="btnAceptarMensajeClienteUbicacion" runat="server" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" ToolTip="Aceptar Información" ValidationGroup="vgrpValidarClienteUbicacion" />
                                                                        &nbsp;
                                                                <asp:Button ID="btnCancelarMensajeClienteUbicacion" runat="server" Text="Cancelar" CssClass="btn btn-outline-google-plus btn-sm" ToolTip="Cancelar Registro" ValidationGroup="vgrpValidarClienteUbicacion" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <br />
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

    <asp:LinkButton ID="lnk_mostrarPanelSubirImagenEquipo" runat="server"></asp:LinkButton>
    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelSubirImagenEquipo_ModalPopupExtender" runat="server"
        BackgroundCssClass="FondoAplicacion" DropShadow="False" DynamicServicePath="" Enabled="True"
        CancelControlID="btnCancelarSubirImagenEquipo"
        PopupControlID="pnlSeleccionarSubirImagenEquipo" TargetControlID="lnk_mostrarPanelSubirImagenEquipo">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="pnlSeleccionarSubirImagenEquipo" runat="server" CssClass="container">
        <div class="modal-dialog modal-lg">
            <div class="shadow rounded">
                <div class="modal-dialog-scrollable">
                    <div class="modal-content">
                        <div class="modal-header bg-light">
                            <h4 class="modal-title" id="myModalLabelSubirImagenEquipo">Agregar Imagen al Equipo</h4>
                            <asp:Button ID="imgbtnCancelarSubirImagenEquipoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <div class="row mt-0">
                                        <div class="col-6">
                                            <div class="col-12 mt-2">
                                                <label class="text-black-50 col-form-label col-form-label-sm">Título:</label>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtTituloSubirImagenEquipo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="12"
                                                        Style="text-transform: uppercase" placeholder="Titulo de la imagen"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvTituloSubirImagenEquipo" runat="server"
                                                        ControlToValidate="txtTituloSubirImagenEquipo" EnableClientScript="False"
                                                        ErrorMessage="Ingrese el título de la imagen" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidarSubirImagenEquipo">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-12 mt-2">
                                                <label class="text-black-50 col-form-label col-form-label-sm">Descripción:</label>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtDescripcionSubirImagenEquipo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="12"
                                                        Style="text-transform: uppercase" placeholder="Descripción de la imagen"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvDescripcionSubirImagenEquipo" runat="server"
                                                        ControlToValidate="txtDescripcionSubirImagenEquipo" EnableClientScript="False"
                                                        ErrorMessage="Ingrese la descripción de la imagen" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidarSubirImagenEquipo">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-12 mt-2">
                                                <label class="text-black-50 col-form-label col-form-label-sm">Observación:</label>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtObservacionSubirImagenEquipo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="12"
                                                        Style="text-transform: uppercase" placeholder="Observación de la imagen"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvObservacionSubirImagenEquipo" runat="server"
                                                        ControlToValidate="txtObservacionSubirImagenEquipo" EnableClientScript="False"
                                                        ErrorMessage="Ingrese la observación de la imagen" Font-Size="10px" ForeColor="Red"
                                                        ValidationGroup="vgrpValidarSubirImagenEquipo">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <asp:Image ID="imgEquipo" runat="server" CssClass="img-fluid rounded-top" />
                                            <script type="text/javascript">
                                                function MostrarImagen() {
                                                    var input = document.getElementById('<%= fupSubirImagenEquipo.ClientID %>');
                                                    var img = document.getElementById('<%= imgEquipo.ClientID %>');

                                                    if (input.files && input.files[0]) {
                                                        var reader = new FileReader();

                                                        reader.onload = function (e) {
                                                            img.src = e.target.result;
                                                        };

                                                        reader.readAsDataURL(input.files[0]);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="row">
                                <label class="text-black-50 col-form-label col-form-label-sm">Archivo:</label>
                                <div class="input-group">
                                    <asp:FileUpload ID="fupSubirImagenEquipo" runat="server" EnableTheming="True" CssClass="form-control-sm" TabIndex="90" Accept="image/jpeg, image/png" onchange="MostrarImagen()" />
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:ValidationSummary ID="ValidationSummary5" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarSubirImagenEquipo" />
                            <div class="col-md-4 text-end p-2">
                                <asp:Button ID="btnAceptarSubirImagenEquipo" runat="server" ValidationGroup="vgrpValidarSubirImagenEquipo"
                                    ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                &nbsp;
                                <asp:Button ID="btnCancelarSubirImagenEquipo" runat="server"
                                    ToolTip="Cancelar Registro" TabIndex="73" Text="Cancelar" CssClass="btn btn-outline-google-plus btn-sm" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:LinkButton ID="lnk_mostrarPanelSubirDocumentacionEquipo" runat="server"></asp:LinkButton>
    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelSubirDocumentacionEquipo_ModalPopupExtender" runat="server"
        BackgroundCssClass="FondoAplicacion" DropShadow="False" DynamicServicePath="" Enabled="True"
        CancelControlID="btnCancelarSubirDocumentacionEquipo"
        PopupControlID="pnlSeleccionarSubirDocumentacionEquipo" TargetControlID="lnk_mostrarPanelSubirDocumentacionEquipo">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlSeleccionarSubirDocumentacionEquipo" runat="server" CssClass="container">
        <div class="modal-dialog modal-lg">
            <div class="shadow rounded">
                <div class="modal-dialog-scrollable">
                    <div class="modal-content">
                        <div class="modal-header bg-light">
                            <h4 class="modal-title" id="myModalLabelSubirDocumentacionEquipo">Ver Documentación del Equipo</h4>
                            <asp:Button ID="imgbtnCancelarSubirDocumentacionEquipoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-5 mt-2">
                                    <asp:UpdatePanel ID="updpnlSubirDoocumentacionEquipoImagen" runat="server">
                                        <ContentTemplate>
                                            <div id="divGridVerDocumentosEquipo" style="overflow: auto; height: 220px; align: left;" class="scrollbar">
                                                <asp:GridView ID="grdListaVerDocumentosEquipo" runat="server" AutoGenerateColumns="False" TabIndex="4" DataKeyNames="NombreArchivo"
                                                    GridLines="None" CssClass="table table-responsive-sm table-hover small" AllowPaging="True"
                                                    EmptyDataText="No hay registros a visualizar" PageSize="14" OnRowCommand="grdLista_RowCommand">
                                                    <PagerStyle CssClass="pgr" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Detalle" ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0" ItemStyle-CssClass="small">
                                                            <ItemTemplate>
                                                                <div class="col-12">
                                                                    <asp:Label ID="lblTituloVerDocumento" runat="server" Text='<%# Eval("Titulo") %>'></asp:Label>
                                                                </div>
                                                                <div class="col-12">
                                                                    <asp:Label ID="lblDescripcionVerDocumento" runat="server" Text='<%# Eval("Descripcion") %>'></asp:Label>
                                                                </div>
                                                                <div class="col-12">
                                                                    <asp:Label ID="lbFechaCarga" runat="server" Text='<%# Eval("Carga") %>'></asp:Label>
                                                                </div>
                                                                <div class="col-12">
                                                                    <asp:HyperLink ID="hlnkVerDocumento" runat="server" Target="_blank"
                                                                        NavigateUrl='<%# IIf(Trim(Eval("NombreArchivo")) = "", "", "~/Downloads/Equipo/" & Eval("NombreArchivo")) %>'
                                                                        Text='<%# Eval("NombreArchivo") %>' CssClass="button short green borde_redondo2"></asp:HyperLink>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mant." ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEliminarDocumento" runat="server" CommandName="Eliminar" Text="Eliminar" CommandArgument='<%# Eval("Item") & "," & Eval("IdEquipo") %>' ImageUrl="imagenes/eliminar.png" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="thead-dark" />
                                                </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:Button ID="btnAgregarVerDocumentosEquipo" runat="server" ValidationGroup="vgrpValidarSubirDocumentacionEquipo"
                                        ToolTip="Nuevo Documento" TabIndex="72" Text="Nuevo Documento" CssClass="btn btn-outline-facebook btn-sm" />
                                </div>
                                <div class="col-7">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <div id="divDetalleVerDocumentosEquipo" runat="server" class="scrollbar">
                                                <div class="row">
                                                    <div class="col-12 mt-2">
                                                        <label class="text-black-50 col-form-label col-form-label-sm">Título:</label>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtTituloSubirDocumentacionEquipo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="12"
                                                                Style="text-transform: uppercase" placeholder="Titulo del archivo"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvTituloSubirDocumentacionEquipo" runat="server"
                                                                ControlToValidate="txtTituloSubirDocumentacionEquipo" EnableClientScript="False"
                                                                ErrorMessage="Ingrese el título del documento" Font-Size="10px" ForeColor="Red"
                                                                ValidationGroup="vgrpValidarSubirDocumentacionEquipo">(*)</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-12 mt-2">
                                                        <label class="text-black-50 col-form-label col-form-label-sm">Descripción:</label>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtDescripcionSubirDocumentacionEquipo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="12"
                                                                Style="text-transform: uppercase" placeholder="Descripción del archivo"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvDescripcionSubirDocumentacionEquipo" runat="server"
                                                                ControlToValidate="txtDescripcionSubirDocumentacionEquipo" EnableClientScript="False"
                                                                ErrorMessage="Ingrese la descripción del documento" Font-Size="10px" ForeColor="Red"
                                                                ValidationGroup="vgrpValidarSubirDocumentacionEquipo">(*)</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-12 mt-2">
                                                        <label class="text-black-50 col-form-label col-form-label-sm">Archivo:</label>
                                                        <div class="input-group">
                                                            <asp:FileUpload ID="fupSubirDocumentacionEquipo" runat="server" EnableTheming="True" CssClass="form-control-sm" TabIndex="90" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:ValidationSummary ID="ValidationSummary6" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarSubirDocumentacionEquipo" />
                            <div class="col-md-4 text-end p-2">
                                <asp:Button ID="btnAceptarSubirDocumentacionEquipo" runat="server" ValidationGroup="vgrpValidarSubirDocumentacionEquipo"
                                    ToolTip="Aceptar Registro" TabIndex="72" Text="Agregar" CssClass="btn btn-outline-facebook btn-sm" />
                                &nbsp;
                                <asp:Button ID="btnCancelarSubirDocumentacionEquipo" runat="server"
                                    ToolTip="Cancelar Registro" TabIndex="73" Text="Cancelar" CssClass="btn btn-outline-google-plus btn-sm" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <script language="javascript" type="text/javascript">
        function popupEmitirEquipoDetalleReporte(IdEquipo) {
            window.open("Informes/frmCmmsEquipoDetalleReporte.aspx?IdEqu=" + IdEquipo, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
        }
        function popupEmitirOrdenFabricacionReporte(IdEquipo) {
            window.open("Informes/frmCmmsOrdenFabricacionReporte.aspx?IdEqu=" + IdEquipo, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
        }
        function popupEmitirOrdenTrabajoReporte(IdEquipo) {
            window.open("Informes/frmCmmsEquipoOrdenTrabajoReporte.aspx?IdEqu=" + IdEquipo, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
        }

        // Variable para almacenar el valor anterior del DropDownList
        var valorAnterior;

        // Función para guardar el valor inicial del DropDownList
        function guardarValorInicial() {
            // Obtener el valor inicial del DropDownList
            valorAnterior = document.getElementById('<%= cboCatalogo.ClientID %>').value;

            // Asignar el valor al HiddenField
            document.getElementById('<%= hfdIdCatalogoAnterior.ClientID %>').value = valorAnterior;
        }

        // Función para guardar el valor actual del DropDownList antes de realizar el postback
        function guardarValor() {
            // Obtener el valor seleccionado del DropDownList
            var selectedValue = document.getElementById('<%= cboCatalogo.ClientID %>').value;

            // Asignar el valor anterior al HiddenField antes del postback
            document.getElementById('<%= hfdIdCatalogoAnterior.ClientID %>').value = valorAnterior;

            // Actualizar el valor anterior con el valor actual
            valorAnterior = selectedValue;

            // Realizar el postback
            __doPostBack('<%= cboCatalogo.ClientID %>', '');
        }

        // Llamar a la función para guardar el valor inicial al cargar la página
        window.onload = guardarValorInicial;

    </script>

    <link href="cssbootstrap/bootstrap-switch.css" rel="stylesheet" />
    <script src="jsbootstrap/bootstrap-switch.js"></script>

    <script src="vendors/anchorjs/anchor.min.js"></script>
    <script src="vendors/glightbox/glightbox.min.js"> </script>
    <script src="vendors/lodash/lodash.min.js"></script>
    <script src="assets/js/theme.js"></script>
</asp:Content>
