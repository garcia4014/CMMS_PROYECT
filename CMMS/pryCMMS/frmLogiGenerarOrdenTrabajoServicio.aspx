<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmLogiGenerarOrdenTrabajoServicio.aspx.vb" Inherits="pryCMMS.frmLogiGenerarOrdenTrabajoServicio" ValidateRequest="false" %>

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

    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
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
            <asp:HiddenField ID="hfdIdClienteSAPEquipo" runat="server" />
            <asp:HiddenField ID="hfdIdEquipoSAPEquipo" runat="server" />
            <asp:HiddenField ID="hfdIdArticuloSAPPrincipal" runat="server" />
            <asp:HiddenField ID="hfdFechaCreacionEquipo" runat="server" />
            <asp:HiddenField ID="hfdOrdenFabricacionReferencia" runat="server" />
            <asp:HiddenField ID="hfdIdUsuarioCreacionOrdenTrabajo" runat="server" />
            <asp:HiddenField ID="hfdIdCatalogoCabecera" runat="server" />
            <asp:HiddenField ID="hfdIdTipoActivoCabecera" runat="server" />
            <asp:HiddenField ID="hfdIdJerarquiaCatalogoCabecera" runat="server" />
            <asp:HiddenField ID="hfdIdTipoControlTiempo" runat="server" />
            <asp:Panel ID="pnlCabecera" runat="server">
                <div class="card mb-3">
                    <div class="card-header">
                        <div class="row flex-between-end">
                            <div class="col-auto align-self-center">
                                <div class="nav nav-pills gap-2" role="tablist">
                                    <h5 class="mb-0" data-anchor="data-anchor">Listado de Ordenes de Trabajo Por Servicio</h5>
                                    <a class="btn btn-falcon-default btn-sm ml-auto float-right" href="frmBienvenida.aspx" aria-expanded="true">
                                        <span class="fas fa-home"></span><span class="nav-link-text ps-1">Inicio</span>
                                    </a>
                                </div>
                            </div>
                            <div class="col-auto ms-auto">
                                <div class="nav gap-2" role="tablist">
                                    <%--<asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-primary btn-sm ml-auto float-right" Text="<span class='fas fa-pen'></span> Editar" ToolTip="Editar Orden de Trabajo" />--%>
                                    <asp:LinkButton ID="btnEditar" runat="server" OnClick="btnEditar_Click" CssClass="btn btn-primary btn-sm ml-auto float-right" Text="<span class='fas fa-pen'></span> Editar" ToolTip="Editar Orden de Trabajo" />
                                    <%--<asp:LinkButton ID="btnNuevo" runat="server" OnClick="btnNuevo_Click" CssClass="btn btn-success btn-sm ml-auto float-right" Text="<span class='fas fa-plus'></span> Procesar" ToolTip="Procesar Orden de Trabajo" />--%>
                                    <asp:LinkButton ID="btnNuevo" runat="server" OnClick="btnNuevo_Click" CssClass="btn btn-success btn-sm ml-auto float-right" Text="Procesar" ToolTip="Procesar Orden de Trabajo" />
                                    
                                    <div class="dropdown font-sans-serif btn-reveal-trigger">
                                        <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesEquipo" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                                        <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesEquipo">
                                            <asp:LinkButton ID="lnkbtnVerOrdenTrabajo" runat="server" ToolTip="Ver Orden de Trabajo" Text="<span class='fas fa-file me-2'></span>Ver O.Trabajo" CssClass="nav-link nav-link-card-details" />
                                            <asp:LinkButton ID="lnkbtnVerDetalleOrdenTrabajoServicio" runat="server" ToolTip="Ver Detalle Orden de Trabajo" Text="<span class='fas fa-file me-2'></span>Ver Detalle O.Trabajo" CssClass="nav-link nav-link-card-details" />
                                            <asp:LinkButton ID="lnkbtnVerCheckList" runat="server" ToolTip="Ver CheckList" Text="<span class='fas fa-file me-2'></span>Ver ChekList" CssClass="nav-link nav-link-card-details" />
                                            <asp:LinkButton ID="lnkbtnTerminarOrdenTrabajo" runat="server" ToolTip="Terminar Orden de Trabajo" Text="<span class='fas fa-file me-2'></span>Terminar O.T." CssClass="nav-link nav-link-card-details" />
                                            <asp:LinkButton ID="lnkbtnRegenerarOrdenTrabajo" runat="server" ToolTip="Regenerar Orden de Trabajo" Text="<span class='fas fa-file me-2'></span>Regenerar O.T." CssClass="nav-link nav-link-card-details" />
                                            <asp:LinkButton ID="lnkbtnSubirDocumentacionOrdenTrabajo" runat="server" ToolTip="Ver documentación de la Orden de trabajo" Text="<span class='fas fa-file-signature me-2'></span>Documentos" CssClass="nav-link nav-link-card-details" />
                                            <asp:LinkButton ID="lnkbtnEliminarOrdenTrabajo" runat="server" ToolTip="Eliminar Orden de trabajo" Text="<span class='fas fa-file-signature me-2'></span>Eliminar OT" CssClass="nav-link nav-link-card-details" />
                                            <asp:LinkButton ID="lnkbtnFirmarOrdenTrabajo" runat="server" ToolTip="Firmar Orden de trabajo" Text="<span class='fas fa-file-signature me-2'></span>Firmar OT" CssClass="nav-link nav-link-card-details" />
                                            <asp:LinkButton ID="lnkbtnConclusionesRecomendacionesOrdenTrabajo" runat="server" ToolTip="Conclusiones y recomendaciones Orden de trabajo" Text="<span class='fas fa-file-signature me-2'></span>Con&Rec" CssClass="nav-link nav-link-card-details" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="row bg-100 p-2">
                            <div class="col-lg-2 col-sm-6">
                                <span class="fas fa-filter"></span>
                                <label class="text-black-50 col-form-label fs--2 m-1">Filtrar por Status:</label>
                                <div class="input-group">
                                    <asp:DropDownList ID="cboFiltroStatus" runat="server" CssClass="form-select form-select-sm js-choice" TabIndex="1" AutoPostBack="True">
                                        <asp:ListItem Value="*">Todos</asp:ListItem>
                                        <asp:ListItem Value="R">Registrado</asp:ListItem>
                                        <asp:ListItem Value="P">Pendiente</asp:ListItem>
                                        <asp:ListItem Value="T">Terminado</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-4 col-sm-6">
                                <span class="fas fa-filter"></span>
                                <label class="text-black-50 col-form-label fs--2 m-1">Filtrar por:</label>
                                <div class="input-group">
                                    <asp:DropDownList ID="cboFiltroOrdenTrabajo" runat="server" CssClass="form-select form-select-sm js-choice" TabIndex="1">
                                        <asp:ListItem>SELECCIONE EL FILTRO</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-6">
                                <span class="far fa-address-card"></span>
                                <label class="text-black-50 col-form-label fs--2 m-1">Buscar por:</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtBuscarOrdenTrabajo" runat="server" CssClass="form-control form-control-sm" TabIndex="2"
                                        Style="text-transform: uppercase" placeholder="Ingrese Busqueda"></asp:TextBox>
                                    &nbsp;
							        <asp:ImageButton ID="imgbtnBuscarOrdenTrabajo" runat="server" Height="18px" TabIndex="3" CssClass="mt-1"
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
                                                <asp:TemplateField HeaderText="Informe" ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small">
                                                    <ItemTemplate>
                                                        <div class="col-12">
                                                            <asp:HyperLink ID="hlnkVerInforme" runat="server" Target="_blank"
                                                                NavigateUrl='<%# IIf(Trim(Eval("NroOrdenTrabajoReferencial")) = "", "", "~/InformesWord/" & Eval("IdEquipo") & "-" & Trim(Eval("NroOrdenTrabajoReferencial")) & ".docx") %>'
                                                                Text="Ver" CssClass="button short green borde_redondo2"></asp:HyperLink>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="FechaInicio" HeaderText="Fec.Inicio" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="IdCliente" HeaderText="Id.Cliente" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="IdClienteSAP" HeaderText="Id. Cliente SAP" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="RucCliente" HeaderText="RUC" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="RazonSocial" HeaderText="Razón Social" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="IdEquipo" HeaderText="Id.Equipo"  HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="IdEquipoSAP" HeaderText="Id.Equipo SAP" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="DescripcionEquipo" HeaderText="Descripción Equipo" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="NumeroSerieEquipo" HeaderText="Nro. Serie Equipo" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="IdArticuloSAPCabecera" HeaderText="Articulo Principal" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="IdTipoMantenimiento" HeaderText="T.Mnt." HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:TemplateField HeaderText="Status" ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEstado" runat="server" Visible='<%# If(Eval("StatusOrdenTrabajo") = "", "false", "true") %>' CssClass='<%#If(Eval("StatusOrdenTrabajo") = "R", "texto_estado short_estado black borde_redondo1", If(Eval("StatusOrdenTrabajo") = "T", "texto_estado short_estado verde borde_redondo1", "texto_estado short_estado orange borde_redondo1")) %>' Text='<%# StrConv(IIf(Eval("StatusOrdenTrabajo") = "P", "PENDIENTE", IIf(Eval("StatusOrdenTrabajo") = "T", "TERMINADO", "REGISTRADO")), VbStrConv.ProperCase) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Doc." ShowHeader="True" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="p-1 m-0 small">
                                                    <ItemTemplate>
                                                        <div class="col-12">
                                                            <asp:LinkButton ID="lnkDocumentos" runat="server" CommandName="VerDocumentos" CommandArgument='<%#  Eval("IdTipoDocumento") & "," & Eval("IdNumeroSerie") & "," & Eval("IdNumeroCorrelativo") & "," & Eval("IdEquipo") %>' Visible='<%# If(Eval("bVisualizarDOC") = "1", "true", "false") %>'>
                                                                <asp:Image ID="imgDocumento" runat="server" ImageUrl="~/Imagenes/documentos.png" AlternateText="Ver Documento" />
                                                            </asp:LinkButton>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:TemplateField HeaderText="Estado" ItemStyle-Width="120px" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                    <ItemTemplate>
                                                        <div class="bootstrap-switch bootstrap-switch-on bootstrap bootstrap-switch-animate bootstrap-switch-mini ">
                                                            <div class="bootstrap-switch-container">
                                                                <asp:LinkButton ID="lnkEstadoDetalleOn" runat="server" CommandName="Activar" CommandArgument='<%# Eval("IdTipoDocumento") & "," & Eval("IdNumeroSerie") & "," & Eval("IdNumeroCorrelativo") %>' CssClass="bootstrap-switch-handle-on bootstrap-switch-primary" Visible='<%# If(Eval("Estado") = "True", "True", "False") %>'>Activado</asp:LinkButton>
                                                                <span class="bootstrap-switch-label">&nbsp;</span>
                                                                <asp:LinkButton ID="lnkEstadoDetalleOff" runat="server" CommandName="Desactivar" CommandArgument='<%# Eval("IdTipoDocumento") & "," & Eval("IdNumeroSerie") & "," & Eval("IdNumeroCorrelativo") %>' CssClass="bootstrap-switch-handle-off bootstrap-switch-danger" Visible='<%# If(Eval("Estado") = "True", "False", "True") %>'>Anulado</asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                
                                                
                                                <asp:BoundField DataField="FechaEmision" HeaderText="Fec.Emisión" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="FechaTermino" HeaderText="Fec.Termino" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
                                                <asp:BoundField DataField="IdNumeroCabeceraCheckListPlantilla" HeaderText="T.Mnt." HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small" />
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
    <asp:UpdatePanel ID="updpnlContenido" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnNuevo" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="pnlContenido" runat="server">
                <asp:HiddenField ID="hfdValoresCheckList" runat="Server"></asp:HiddenField>
                <div id="BarraHTML" class="container-fluid">
                    <div class="row">
                        <div class="col-auto align-self-center mt-2 mb-2">
                            <div class="nav nav-pills gap-2" role="tablist">
                                <asp:LinkButton ID="btnAtras" runat="server" CssClass="btn btn-falcon-default btn-sm ml-auto float-right" Text="<span class='fas fa-step-backward'></span> Atrás" ToolTip="Ir hacia atrás" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row g-0">
                    <div class="col-lg-6 pe-lg-2 mb-3">
                        <div class="card mb-2">
                            <div class="card-header bg-light">
                                <div class="row align-items-center">
                                    <div class="col-6">
                                        <h6 class="mb-0">Datos Generales</h6>
                                    </div>
                                    <div class="col-6">
                                        <%--<asp:HiddenField ID="hfdStatusOrdenTrabajoTarea" runat="Server" Value='<%# Eval("StatusComponente") %>'></asp:HiddenField>--%>
                                        <div class="row">
                                            <asp:LinkButton ID="lnkbtnIniciarOrdenTrabajoTarea" runat="server" CommandName="Iniciar" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdEquipo") & "*" & Eval("IdEquipoComponente") & "*I" %>' Visible="false" ToolTip="Iniciar Actividad" Text="<span class='fas fa-power-off me-2'></span>Iniciar" CssClass="btn btn-outline-primary btn-sm w-75 fs--1 m-0" />
                                        </div>
                                        <%--<div class="row pb-1">
                                            <asp:LinkButton ID="lnkbtnPendienteOrdenTrabajoTarea" runat="server" CommandName="Pendiente" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdEquipo") & "*" & Eval("IdEquipoComponente") & "*P" %>' Visible="false" ToolTip="Actividad Pendiente" Text="<span class='fas fa-tasks me-2'></span>En Espera" CssClass="btn btn-outline-warning btn-sm w-75 fs--1 m-0" />
                                        </div>
                                        <div class="row pb-1">
                                            <asp:LinkButton ID="lnkbtnRetomarOrdenTrabajoTarea" runat="server" CommandName="Retomar" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdEquipo") & "*" & Eval("IdEquipoComponente") & "*R" %>' Visible="false" ToolTip="Retomar Actividad" Text="<span class='fas fa-retweet me-2'></span>Retomar" CssClass="btn btn-outline-danger btn-sm w-75 fs--1" />
                                        </div>
                                        <div class="row pb-1">
                                            <asp:LinkButton ID="lnkbtnFinalizarOrdenTrabajoTarea" runat="server" CommandName="Finalizar" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdEquipo") & "*" & Eval("IdEquipoComponente") & "*F" %>' Visible="false" ToolTip="Finalizar Actividad" Text="<span class='fas fa-check me-2'></span>Finalizar" CssClass="btn btn-outline-success btn-sm w-75 fs--1" />
                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                            <div class="card-body">
                                <div class="row mt-1">
                                    <div class="col-lg-12 col-sm-12">
                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 mt-2">
                                                <span class="far fa-address-card"></span>
                                                <label class="text-black-50 col-form-label fs--2 m-1">Código Equipo:</label>
                                                <div class="input-group">
                                                    <p class="fs--1 m-0">
                                                        <asp:Label ID="lblIdEquipo" runat="server" Text="" CssClass="text-500 small"></asp:Label>
                                                    </p>
                                                </div>
                                            </div>
                                            <div class="col-lg-4 col-md-4 mt-2">
                                                <span class="far fa-address-card"></span>
                                                <label class="text-black-50 col-form-label fs--2 m-1">Descrip.Equipo:</label>
                                                <div class="input-group">
                                                    <p class="fs--1 m-0">
                                                        <asp:Label ID="lblDescripcionEquipo" runat="server" Text="" CssClass="text-500 small"></asp:Label>
                                                    </p>
                                                </div>
                                            </div>
                                            <div class="col-lg-4 col-md-4 col-sm-6 mt-2">
                                                <span class="far fa-address-card"></span>
                                                <label class="text-black-50 col-form-label fs--2 m-1">Plantilla CheckList</label>
                                                <div class="input-group">
                                                    <p class="fs--1 m-0">
                                                        <asp:Label ID="lblPlantillaChecklistEquipo" runat="server" Text="" CssClass="text-500 small"></asp:Label>
                                                    </p>
                                                </div>
                                            </div>
                                            <div class="col-lg-4 col-md-2 mt-2">
                                                <span class="far fa-address-card"></span>
                                                <label class="text-black-50 col-form-label fs--2 m-1">N.Ser.:</label>
                                                <div class="input-group">
                                                    <p class="fs--1 m-0">
                                                        <asp:Label ID="lblNroSerieEquipo" runat="server" Text="" CssClass="text-500 small"></asp:Label>
                                                    </p>
                                                </div>
                                            </div>
                                            <div class="col-lg-4 col-md-2 mt-2">
                                                <span class="far fa-address-card"></span>
                                                <label class="text-black-50 col-form-label fs--2 m-1">#Part:</label>
                                                <div class="input-group">
                                                    <p class="fs--1 m-0">
                                                        <asp:Label ID="lblNroParteEquipo" runat="server" Text="" CssClass="text-500 small"></asp:Label>
                                                    </p>
                                                </div>
                                            </div>
                                            <div class="col-lg-4 col-md-2 mt-2">
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
                                            <div class="col-lg-4 col-md-2 mt-2">
                                                <span class="far fa-clone"></span>
                                                <label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Tipo Activo:</label>
                                                <div class="input-group">
                                                    <p class="fs--1 m-0">
                                                        <asp:Label ID="lblTipoActivoEquipo" runat="server" Text="" CssClass="text-500 small"></asp:Label>
                                                    </p>
                                                </div>
                                            </div>
                                            <div class="col-lg-4 col-md-2 mt-2">
                                                <span class="far fa-clone"></span>
                                                <label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Catalogo Principal:</label>
                                                <div class="input-group">
                                                    <p class="fs--1 m-0">
                                                        <asp:Label ID="lblCatalogoEquipo" runat="server" Text="" CssClass="text-500 small"></asp:Label>
                                                    </p>
                                                </div>
                                            </div>
                                            <div id="divSistemaFuncional" runat="server" class="col-lg-4 col-md-2 mt-2">
                                                <span class="far fa-clone"></span>
                                                <label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Sistema Funcional:</label>
                                                <div class="input-group">
                                                    <p class="fs--1 m-0">
                                                        <asp:Label ID="lblSistemaFuncionalEquipo" runat="server" Text="" CssClass="text-500 small"></asp:Label>
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header bg-light">
                                <div class="row align-items-center">
                                    <div class="col">
                                        <h6 class="mb-0">Componentes</h6>
                                    </div>
                                </div>
                            </div>
                            <div class="card-body h-100">
                                <asp:DataList ID="lstComponentes" runat="server" OnItemCommand="lstComponentes_RowCommand_Botones" RepeatColumns="1" RepeatDirection="Horizontal" CssClass="list-unstyled w-100">
                                    <ItemTemplate>
                                        <div class="col-md-12">
                                            <div class="w-100">
                                                <div class="row border-bottom border-200">
                                                    <div class="col-9">
                                                        <div class="row g-0 align-items-center py-2 position-relative">
                                                            <div class="col ps-card py-1 position-static">
                                                                <div class="d-flex align-items-center">
                                                                    <div class="avatar avatar-xl me-3">
                                                                        <div class="avatar-name rounded-circle bg-soft-primary text-dark"><span class="fs-0 text-primary"><%# Mid(Eval("DescripcionEquipo"), 1, 1) %></span></div>
                                                                    </div>
                                                                    <div class="flex-1">
                                                                        <h6 class="mb-0 d-flex align-items-center">
                                                                            <%--<asp:LinkButton ID="lnkbtnComponente" runat="server" CssClass="text-800 stretched-link" CommandName="AdicionarTareaComponente" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdEquipo") & "*" & Eval("IdEquipoComponente") & "*I" %>' ToolTip="Registrar Tarea" >  <%# Eval("DescripcionEquipo") %></a> </asp:LinkButton> <span class="badge rounded-pill ms-2 bg-200 text-primary"><%# Eval("PorcentajeText") %></span></h6>--%>
                                                                            <asp:LinkButton ID="lnkbtnComponente" runat="server" CssClass="text-800 stretched-link" CommandName="AdicionarTareaComponente" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdEquipo") & "*" & Eval("IdEquipoComponente") & "*" & Eval("StatusComponente") %>' ToolTip="Registrar Tarea">  <%# Eval("DescripcionEquipo") %></a> </asp:LinkButton>
                                                                            <span class="badge rounded-pill ms-2 bg-200 text-primary"><%# Eval("PorcentajeText") %></span></h6>
                                                                        <asp:Label ID="lblPorcentajeText" runat="server" Text='<%# Eval("Porcentaje") %>' Visible="false"></asp:Label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col py-1">
                                                                <div class="row flex-end-center g-0">
                                                                    <div class="col-auto pe-2">
                                                                        <div class="fs--1 fw-semi-bold"><%# TimeSpan.FromSeconds(Eval("Tiempo")).ToString("dd\.hh\:mm\:ss") %></div>
                                                                    </div>
                                                                    <div class="col-5 pe-card ps-2">
                                                                        <div class="progress bg-200 me-2" style="height: 5px;">
                                                                            <div id="spanPorcentajeText" class="progress-bar rounded-pill" role="progressbar" style='<%# "width: " & Eval("Porcentaje").ToString & "%" %>' aria-valuenow='<%# Eval("Porcentaje") %>' aria-valuemin="0" aria-valuemax="100"></div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-3">
                                                        <asp:HiddenField ID="hfdStatusComponenteTarea" runat="Server" Value='<%# Eval("StatusComponente") %>'></asp:HiddenField>
                                                        <div class="row mt-2 pb-1">
                                                            <asp:LinkButton ID="lnkbtnIniciarComponenteTarea" runat="server" CommandName="Iniciar" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdEquipo") & "*" & Eval("IdEquipoComponente") & "*I" %>' Visible="false" ToolTip="Iniciar Actividad" Text="<span class='fas fa-power-off me-2'></span></br>Iniciar" CssClass="btn btn-outline-primary btn-sm w-75 fs--1 m-0" />
                                                        </div>
                                                        <div class="row pb-1">
                                                            <asp:LinkButton ID="lnkbtnPendienteComponenteTarea" runat="server" CommandName="Pendiente" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdEquipo") & "*" & Eval("IdEquipoComponente") & "*P" %>' Visible="false" ToolTip="Actividad Pendiente" Text="<span class='fas fa-tasks me-2'></span></br>En Espera" CssClass="btn btn-outline-warning btn-sm w-75 fs--1 m-0" />
                                                        </div>
                                                        <div class="row pb-1">
                                                            <asp:LinkButton ID="lnkbtnRetomarComponenteTarea" runat="server" CommandName="Retomar" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdEquipo") & "*" & Eval("IdEquipoComponente") & "*R" %>' Visible="false" ToolTip="Retomar Actividad" Text="<span class='fas fa-retweet me-2'></span></br>Retomar" CssClass="btn btn-outline-danger btn-sm w-75 fs--1" />
                                                        </div>
                                                        <div class="row pb-1">
                                                            <asp:LinkButton ID="lnkbtnFinalizarComponenteTarea" runat="server" CommandName="Finalizar" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdEquipo") & "*" & Eval("IdEquipoComponente") & "*F" %>' Visible="false" ToolTip="Finalizar Actividad" Text="<span class='fas fa-check me-2'></span></br>Finalizar" CssClass="btn btn-outline-success btn-sm w-75 fs--1" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:DataList>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-6 ps-lg-2 mb-3">
                        <div class="card h-lg-100">
                            <div class="card-header">
                                <div class="row flex-between-center">
                                    <div class="col-auto">
                                        <h6 class="mb-0">Actividades</h6>
                                    </div>
                                    <div class="col-auto d-flex">
                                        <div class="dropdown font-sans-serif btn-reveal-trigger">
                                            <button class="btn btn-link text-600 btn-sm dropdown-toggle dropdown-caret-none btn-reveal" type="button" id="dropdown-total-sales" data-bs-toggle="dropdown" data-boundary="viewport" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                            <div class="dropdown-menu dropdown-menu-end border py-2" aria-labelledby="dropdown-total-sales">
                                                <asp:LinkButton ID="lnkbtnVerGaleria" runat="server" ToolTip="Ver la galeria por actividades" Text="<span class='fas fa-file me-2'></span>Ver Galeria" CssClass="nav-link nav-link-card-details" />
                                                <asp:LinkButton ID="lnkbtnAddActividad" runat="server" ToolTip="Agregar actividades" Text="<span class='fas fa-file me-2'></span>Agregar Actividad" CssClass="nav-link nav-link-card-details" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="card-body h-100">
                                <asp:DataList ID="lstCheckList" runat="server" DataKeyField="IdActividad" OnItemCommand="lstCheckList_RowCommand_Botones"
                                    RepeatColumns="1" RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <div class="card mb-2 bg-100">
                                            <div class="card-body p-0">
                                                <div class="row">
                                                    <div class="col-xl-2 col-lg-3 col-md-2 col-sm-2 col-3">
                                                        <div class="overflow-hidden">
                                                            <div class="position-relative rounded-top overflow-hidden">
                                                                <div class="p-1">
                                                                    <a href='<%# "Imagenes/check-" & Eval("StatusCheckList") & ".png" %> ' data-gallery="gallery-1">
                                                                        <img class="img-fluid rounded" src='<%# "Imagenes/check-" & Eval("StatusCheckList") & ".png" %> ' alt="" /></a>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-xl-10 col-lg-6 col-md-7 col-sm-7 col-9">
                                                        <div class="mt-2">
                                                            <div class="row">
                                                                <div class="col-12">
                                                                    <h5 class="fs-0">
                                                                        <asp:Label ID="lblActividad" runat="server" Text='<%# Eval("DescripcionActividad") %>' CssClass="text-dark"></asp:Label></h5>
                                                                </div>
                                                            </div>
                                                            <div class="row p-0">
                                                                <div class="col-xl-12 col-md-12">
                                                                    <div class="row">
                                                                        <div class="col-xl-3 col-lg-5">
                                                                            <p class="fs--1 m-0"><strong class="small">Componente:</strong></p>
                                                                        </div>
                                                                        <div class="col-xl-9 col-lg-7">
                                                                            <p class="fs--1 m-0">
                                                                                <asp:Label ID="lblComponente" runat="server" Text='<%# Eval("DescripcionCatalogo") %>' CssClass="text-500 small"></asp:Label>
                                                                            </p>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-xl-12 col-md-12">
                                                                    <div class="row">
                                                                        <div class="col-xl-3 col-lg-5">
                                                                            <p class="fs--1 m-0"><strong class="small">Tipo Activo:</strong></p>
                                                                        </div>
                                                                        <div class="col-xl-9 col-lg-7">
                                                                            <p class="fs--1 m-0">
                                                                                <asp:Label ID="lblTipoActivo" runat="server" Text='<%# Eval("DescripcionTipoActivo") %>' CssClass="text-500 small"></asp:Label>
                                                                            </p>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-xl-12 col-md-12">
                                                                    <div class="row">
                                                                        <div class="col-xl-3 col-lg-5">
                                                                            <p class="fs--1 m-0"><strong class="small">Sist.Funcional:</strong></p>
                                                                        </div>
                                                                        <div class="col-xl-9 col-lg-7">
                                                                            <p class="fs--1 m-0">
                                                                                <asp:Label ID="lblSistemaFuncional" runat="server" Text='<%# Eval("DescripcionSistemaFuncional") %>' CssClass="text-500 small"></asp:Label>
                                                                            </p>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-xl-12 col-md-12">
                                                                    <div class="row">
                                                                        <div class="col-xl-3 col-lg-5">
                                                                            <p class="fs--1 m-0"><strong class="small">Observacion:</strong></p>
                                                                        </div>
                                                                        <div class="col-xl-9 col-lg-7">
                                                                            <p class="fs--1 m-0">
                                                                                <asp:Label ID="lblObservacionActividad" runat="server" Text='<%# Eval("Observacion") %>' CssClass="text-500 small"></asp:Label>
                                                                                <asp:LinkButton ID="lnkbtnAgregarObservacion" runat="server" CommandName="AgregarObservacion" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdActividad") & "*" & Eval("IdEquipo") & "*" & Eval("IdSistemaFuncional") & "*" & Eval("IdTipoActivo") & "*" & Eval("IdEquipoCheckList") %>' Text="<span class='fas fa-tools mx-1'></span>" CssClass="link-600 border-0 me-3 fs--1 text-center" />
                                                                            </p>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="card-footer bg-200">
                                                <div class="row m-0 p-0">
                                                    <div class="col-12">
                                                        <div class="d-flex justify-content-center">
                                                            <div class="p-2">
                                                                <asp:LinkButton ID="lnkbtnEstadoMalo" runat="server" CommandName="EstadoMalo" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdActividad") & "*" & Eval("IdEquipo") & "*" & Eval("IdSistemaFuncional") & "*" & Eval("IdTipoActivo") & "*" & Eval("IdEquipoCheckList") %>' Text="<span class='far fa-frown me-2'></span>" CssClass='<%# If(Eval("StatusActividad") = "M" Or Eval("StatusActividad") = " ", "btn btn-lg btn-danger", "btn btn-lg btn-light") %>' data-bs-toggle="tooltip" data-bs-placement="top" title="Estado de la Actividad : Malo" /></span>
                                                                <asp:LinkButton ID="lnkbtnEstadoRegular" runat="server" CommandName="EstadoRegular" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdActividad") & "*" & Eval("IdEquipo") & "*" & Eval("IdSistemaFuncional") & "*" & Eval("IdTipoActivo") & "*" & Eval("IdEquipoCheckList") %>' Text="<span class='far fa-frown-open me-2'></span>" CssClass='<%# If(Eval("StatusActividad") = "R" Or Eval("StatusActividad") = " ", "btn btn-lg btn-warning", "btn btn-lg btn-light") %>' data-bs-toggle="tooltip" data-bs-placement="top" title="Estado de la Actividad : Regular" /></span>
                                                                <asp:LinkButton ID="LnkbtnEstadoBueno" runat="server" CommandName="EstadoBueno" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdActividad") & "*" & Eval("IdEquipo") & "*" & Eval("IdSistemaFuncional") & "*" & Eval("IdTipoActivo") & "*" & Eval("IdEquipoCheckList") %>' Text="<span class='far fa-grin me-2'></span>" CssClass='<%# If(Eval("StatusActividad") = "B" Or Eval("StatusActividad") = " ", "btn btn-lg btn-success", "btn btn-lg btn-light") %>' data-bs-toggle="tooltip" data-bs-placement="top" title="Estado de la Actividad : Bueno" /></span>
                                                                <asp:LinkButton ID="LnkbtnEstadoNoAplica" runat="server" CommandName="EstadoNoAplica" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdActividad") & "*" & Eval("IdEquipo") & "*" & Eval("IdSistemaFuncional") & "*" & Eval("IdTipoActivo") & "*" & Eval("IdEquipoCheckList") %>' Text="<span class='far fa-dizzy'></span>" CssClass='<%# If(Eval("StatusActividad") = "N" Or Eval("StatusActividad") = " ", "btn btn-lg btn-danger", "btn btn-lg btn-light") %>' data-bs-toggle="tooltip" data-bs-placement="top" title="Estado de la Actividad : No Aplica" /></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="border-top py-2 d-flex justify-content-center">
                                                        <asp:LinkButton ID="lnkbtnAgregarInsumos" runat="server" CommandName="AgregarInsumos" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdActividad") & "*" & Eval("IdEquipo") & "*" & Eval("IdSistemaFuncional") & "*" & Eval("IdTipoActivo") & "*" & Eval("IdEquipoCheckList") %>' ToolTip="Agregar Insumos a la Actividad" Text="<span class='fas fa-mortar-pestle mx-1'></span><div class='d-none d-xl-block'>Insumos</div>" CssClass="link-600 border-0 me-3 fs--1 text-center" />
                                                        <asp:LinkButton ID="lnkbtnAgregarImagenes" runat="server" CommandName="AgregarImagenes" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdActividad") & "*" & Eval("IdEquipo") & "*" & Eval("IdSistemaFuncional") & "*" & Eval("IdTipoActivo") & "*" & Eval("IdEquipoCheckList") %>' ToolTip="Agregar Imagenes a la Actividad" Text="<span class='fas fa-image mx-1'></span><div class='d-none d-xl-block'>Imágenes</div>" CssClass="link-600 border-0 me-3 fs--1 text-center" />
                                                        <asp:LinkButton ID="lnkbtnAgregarDatosAdicionales" runat="server" CommandName="AgregarDatosAdicionales" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdActividad") & "*" & Eval("IdEquipo") & "*" & Eval("IdSistemaFuncional") & "*" & Eval("IdTipoActivo") & "*" & Eval("IdEquipoCheckList") %>' ToolTip="Agregar Datos Adicionales a la Actividad" Text="<span class='fas fa-project-diagram mx-1'></span><div class='d-none d-xl-block'>Datos</div>" CssClass="link-600 border-0 me-3 fs--1 text-center" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:DataList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="position-fixed bottom-0 end-0 p-3">
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidar" />
                </div>
            </asp:Panel>

            <asp:LinkButton ID="lnk_mostrarPanelMantenimientoTareaDetalleOrdenTrabajo" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMantenimientoTareaDetalleOrdenTrabajo_ModalPopupExtender" runat="server"
                BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True"
                CancelControlID="btnCancelarMantenimientoTareaDetalleOrdenTrabajo"
                PopupControlID="pnlMantenimientoTareaDetalleOrdenTrabajo" TargetControlID="lnk_mostrarPanelMantenimientoTareaDetalleOrdenTrabajo">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlMantenimientoTareaDetalleOrdenTrabajo" runat="server" CssClass="container" DefaultButton="btnAceptarMantenimientoTareaDetalleOrdenTrabajo">
                <div class="modal-dialog modal-lg">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header bg-light">
                                    <h4 class="modal-title" id="myModalLabelTareaDetalleOrdenTrabajo">Mantenimiento Tarea por Orden de Trabajo</h4>
                                    <asp:Button ID="imgbtnCancelarMantenimientoTareaDetalleOrdenTrabajoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <asp:UpdatePanel ID="updpnlMantenimientoTareaDetalleOrdenTrabajo" runat="server">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="scrollbar">
                                                    <div class="row">
                                                        <div class="col-md-12 mt-2">
                                                            <div id="divGridPersonalAsignadoMantenimientoTareaDetalleOrdenTrabajo" style="overflow: auto; height: 220px; align: left;">
                                                                <asp:GridView ID="grdPersonalAsignadoMantenimientoTareaDetalleOrdenTrabajo" runat="server" AutoGenerateColumns="False" TabIndex="17"
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
                                                    <div class="row bg-200">
                                                        <div class="col-lg-12 col-md-12 col-sm-12 mt-2 d-flex justify-content-end">
                                                            <%--<span class="far fa-address-card"></span>--%>
                                                            <label class="text-black-50 col-form-label fs--2 m-1">Asignar tareos al personal</label>
                                                                <asp:Button ID="btnIniciarMantenimientoTareaDetalleOrdenTrabajo" runat="server" Text="Asignar Tareo" CssClass="btn btn-primary btn-sm" ToolTip="Iniciar Tareo del Personal" TabIndex="30" />
                                                        </div>
                                                        <div class="col-md-12 mt-2">
                                                            <asp:HiddenField ID="hfdItemSeleccionadoGridPersonalTareoMantenimientoTareaDetalleOrdenTrabajo" runat="Server"></asp:HiddenField>
                                                            <div id="divGridPersonalTareoMantenimientoTareaDetalleOrdenTrabajo" style="overflow: auto; height: 260px; align: left;">
                                                                <asp:GridView ID="grdPersonalTareoMantenimientoTareaDetalleOrdenTrabajo" runat="server" AutoGenerateColumns="False" TabIndex="17"
                                                                    GridLines="None" CssClass="table table-responsive-sm table-striped table-hover small" AllowPaging="True"
                                                                    EmptyDataText="No hay registros a visualizar" ShowHeader="False" PageSize="1000" OnRowCommand="grdPersonalTareoMantenimientoTareaDetalleOrdenTrabajo_RowCommand_Botones">  <%--OnRowDataBound = "grdPersonalTareoMantenimientoTareaDetalleOrdenTrabajo_RowDataBound"--%>
                                                                    <PagerStyle CssClass="pgr" />
                                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <!-- Primera fila: Campos principales -->
                                                                                <div class="row">
                                                                                    <div class="col-md-12">
                                                                                        <div class="d-flex flex-wrap">
                                                                                            <!-- Columna # -->
                                                                                            <div class="p-1 col-1">
                                                                                                <asp:Label ID="lblItem" runat="server" Text='<%# Eval("nIdNumeroItemDetalleTareaOrdenTrabajo") %>' 
                                                                                                          CssClass="form-control form-control-sm text-center"/>
                                                                                            </div>
                                                                                            <!-- Fecha Inicio -->
                                                                                            <div class="p-1 col-3" >
                                                                                                <asp:TextBox ID="txtFechaInicioTareaDetalleOrdenTrabajo" runat="server" 
                                                                                                             Text='<%# Convert.ToDateTime(Eval("dFechaInicioTareaOrdenTrabajo")).ToString("yyyy-MM-dd") %>'
                                                                                                             CssClass="form-control form-control-sm" type="date"/>
                                                                                            </div>
                                                                                             <!-- Hora Inicio -->
                                                                                            <div class="p-1 col-2">
                                                                                                <asp:TextBox ID="txtHoraInicioTareaDetalleOrdenTrabajo" runat="server" 
                                                                                                            Text='<%# Convert.ToDateTime(Eval("dFechaInicioTareaOrdenTrabajo")).ToString("HH:mm") %>' 
                                                                                                            CssClass="form-control form-control-sm" type="time"/>
                                                                                            </div>
                                                                                            <!-- Fecha Fin -->
                                                                                            <div class="p-1 col-3">
                                                                                                <asp:TextBox ID="txtFechaFinTareaDetalleOrdenTrabajo" runat="server" 
                                                                                                            Text='<%#  If(Eval("dFechaFinalTareaOrdenTrabajo") IsNot DBNull.Value, Convert.ToDateTime(Eval("dFechaFinalTareaOrdenTrabajo")).ToString("yyyy-MM-dd"), String.Empty) %>'
                                                                                                            CssClass="form-control form-control-sm" type="date"/>
                                                                                            </div>
                                                                                            <!-- Hora Fin -->
                                                                                            <div class="p-1 col-2">
                                                                                                <asp:TextBox ID="txtHoraFinTareaDetalleOrdenTrabajo" runat="server" 
                                                                                                            Text='<%#  If(Eval("dFechaFinalTareaOrdenTrabajo") IsNot DBNull.Value, Convert.ToDateTime(Eval("dFechaFinalTareaOrdenTrabajo")).ToString("HH:mm"), String.Empty)  %>'
                                                                                                            CssClass="form-control form-control-sm" type="time"/>
                                                                                            </div>
                                                                                            <!-- Columna Eliminar -->
                                                                                            <div class="p-1 col-1">
                                                                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Eliminar" CommandArgument='<%# Eval("cIdTipoDocumentoCabeceraOrdenTrabajo") & "*" & Eval("vIdNumeroSerieCabeceraOrdenTrabajo") & "*" & Eval("vIdNumeroCorrelativoCabeceraOrdenTrabajo") & "*" & Eval("cIdEmpresa") & "*" & Eval("cIdEquipoCabeceraOrdenTrabajo") & "*" & Eval("vIdArticuloSAPCabeceraOrdenTrabajo") & "*" & Eval("nIdNumeroItemDetalleTareaOrdenTrabajo") %>' CssClass="btn btn-danger btn-sm" >
                                                                                                    <i class="fas fa-trash"></i>
                                                                                                </asp:LinkButton>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <!-- Segunda fila: Responsable y Tarea -->
                                                                                <div class="row mt-2">
                                                                                    <div class="col-md-12">
                                                                                        <div class="d-flex flex-wrap">
                                                                                            <!-- Responsable -->
                                                                                            <div class="p-1 flex-grow-1">
                                                                                                <div class="input-group">
                                                                                                    <span class="input-group-text">Responsable(s):</span>
                                                                                                    <div class="form-control form-control-sm bg-white text-dark">
                                                                                                        <asp:Label ID="lblResponsableTareaDetalleOrdenTrabajo" runat="server" 
                                                                                                                   Text='<%# Eval("vDescripcionPersonal") %>'></asp:Label>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                            <!-- Tarea -->
                                                                                            <div class="p-1 flex-grow-1" style="min-width: 250px;">
                                                                                                <div class="input-group">
                                                                                                    <span class="input-group-text">Tarea:</span>
                                                                                                    <asp:TextBox ID="txtDescripcionTareaDetalleOrdenTrabajo" runat="server" 
                                                                                                                Text='<%# Eval("vDescripcionTareaOrdenTrabajo") %>' 
                                                                                                                CssClass="form-control form-control-sm"/>
                                                                                                    <!-- Columna Eliminar -->
                                                                                                    <%--<div class="p-1 col-1">--%>
                                                                                                        <asp:LinkButton ID="lnkAdicionarTarea" runat="server" CommandName="AdicionarTarea" CommandArgument='<%# Eval("cIdTipoDocumentoCabeceraOrdenTrabajo") & "*" & Eval("vIdNumeroSerieCabeceraOrdenTrabajo") & "*" & Eval("vIdNumeroCorrelativoCabeceraOrdenTrabajo") & "*" & Eval("cIdEmpresa") & "*" & Eval("cIdEquipoCabeceraOrdenTrabajo") & "*" & Eval("vIdArticuloSAPCabeceraOrdenTrabajo") & "*" & Eval("nIdNumeroItemDetalleTareaOrdenTrabajo") %>' CssClass="btn btn-primary btn-sm d-flex align-items-center justify-content-center" >
                                                                                                            <i class="fas fa-plus"></i>
                                                                                                        </asp:LinkButton>
                                                                                                    <%--</div>--%>
                                                                                                </div>
                                                                                            </div>
                                                                                            <!-- Comentario -->
                                                                                            <div class="p-1 flex-grow-1" style="min-width: 250px;">
                                                                                                <div class="input-group">
                                                                                                    <span class="input-group-text">Comentario:</span>
                                                                                                    <asp:TextBox ID="txtComentarioTareaDetalleOrdenTrabajo" runat="server" 
                                                                                                                Text='<%# Eval("vComentarioTareaOrdenTrabajo") %>' 
                                                                                                                CssClass="form-control form-control-sm"/>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                           <%-- <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900" />
                                                                            <asp:BoundField DataField="Personal" HeaderText="Descripción" HeaderStyle-CssClass="bg-200 text-900" />
                                                                            <asp:BoundField DataField="Responsable" HeaderText="Responsable" HeaderStyle-CssClass="bg-200 text-900" />--%>

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="thead-dark" />
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <%--<div class="row">
                                                        <div class="col-12 mt-2">
                                                            <span class="far fa-address-card"></span>
                                                            <label class="text-black-50 col-form-label fs--2 m-1">Descripción Tarea</label>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtDescripcionMantenimientoTareaDetalleOrdenTrabajo" runat="server" CssClass="form-control form-control-sm"
                                                                    Style="text-transform: uppercase" TabIndex="18" autocomplete="off"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>--%>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="modal-footer">
                                    <div class="container">
                                        <div class="row">
                                            <div class="col-md-8 mt-2">
                                                <asp:ValidationSummary ID="ValidationSummary14" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarMantenimientoTareaDetalleOrdenTrabajo" />
                                            </div>
                                            <div class="col-md-4 text-end p-2">
                                                <asp:Button ID="btnAceptarMantenimientoTareaDetalleOrdenTrabajo" runat="server" ValidationGroup="vgrpValidarMantenimientoTareaDetalleOrdenTrabajo"
                                                    ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                &nbsp;
                                                <asp:Button ID="btnCancelarMantenimientoTareaDetalleOrdenTrabajo" runat="server"
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




            <asp:LinkButton ID="lnk_mostrarPanelListadoTareasPredefinidas" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelListadoTareasPredefinidas_ModalPopupExtender" runat="server"
                BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True"
                CancelControlID="imgbtnCancelarListadoTareasPredefinidasImagen"
                PopupControlID="pnlListadoTareasPredefinidas" TargetControlID="lnk_mostrarPanelListadoTareasPredefinidas">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlListadoTareasPredefinidas" runat="server" CssClass="container" DefaultButton="btnAceptarListadoTareasPredefinidas">
                <div class="modal-dialog modal-lg">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header bg-light">
                                    <h4 class="modal-title" id="myModalLabelListadoTareasPredefinidas">Listado de Tareas Predefinidas</h4>
                                    <asp:Button ID="imgbtnCancelarListadoTareasPredefinidasImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <asp:UpdatePanel ID="updpnlListadoTareasPredefinidas" runat="server">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="scrollbar">
                                                    <div class="row">
                                                        <div class="col-md-12 mt-2">
                                                            <div id="divGridListadoTareasPredefinidas" style="overflow: auto; height: 220px; align: left;">
                                                                <asp:GridView ID="grdListadoTareasPredefinidas" runat="server" AutoGenerateColumns="False" TabIndex="17"
                                                                    GridLines="None" CssClass="table table-responsive-sm table-striped table-hover small" AllowPaging="True"
                                                                    EmptyDataText="No hay registros a visualizar" PageSize="1000">
                                                                    <PagerStyle CssClass="pgr" />
                                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                    <Columns>
                                                                        <asp:CommandField ShowSelectButton="True" HeaderStyle-Width="70px" />
                                                                        <asp:BoundField DataField="cIdTareaPredefinida" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900" />
                                                                        <asp:BoundField DataField="vDescripcionTareaPredefinida" HeaderText="Descripción" HeaderStyle-CssClass="bg-200 text-900" />
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="thead-dark" />
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="modal-footer">
                                    <div class="container">
                                        <div class="row">
                                            <div class="col-md-8 mt-2">
                                                <asp:ValidationSummary ID="ValidationSummary15" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarListadoTareasPredefinidas" />
                                            </div>
                                            <div class="col-md-4 text-end p-2">
                                                <asp:Button ID="btnAceptarListadoTareasPredefinidas" runat="server" ValidationGroup="vgrpValidarListadoTareasPredefinidas"
                                                    ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                &nbsp;
                                                <asp:Button ID="btnCancelarListadoTareasPredefinidas" runat="server"
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



            <asp:LinkButton ID="lnk_mostrarPanelMantenimientoTarea" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMantenimientoTarea_ModalPopupExtender" runat="server"
                BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True"
                CancelControlID="btnCancelarMantenimientoTarea"
                PopupControlID="pnlMantenimientoTarea" TargetControlID="lnk_mostrarPanelMantenimientoTarea">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlMantenimientoTarea" runat="server" CssClass="container" DefaultButton="btnAceptarMantenimientoTarea">
                <div class="modal-dialog modal-lg">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header bg-light">
                                    <h4 class="modal-title" id="myModalLabelTarea">Mantenimiento Tarea</h4>
                                    <asp:Button ID="imgbtnCancelarTareaImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <asp:UpdatePanel ID="updpnlMantenimientoTarea" runat="server">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="scrollbar">
                                                    <div class="row">
                                                        <div class="col-2 mt-2">
                                                            <span class="far fa-address-card"></span>
                                                            <label class="text-black-50 col-form-label fs--2 m-1">Fecha Tarea</label>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtFechaMantenimientoTarea" runat="server" autocomplete="off"
                                                                    CssClass="form-control form-control-sm" Style="text-transform: uppercase;"
                                                                    placeholder="Fecha Mantenimiento" TabIndex="18" type="text"></asp:TextBox>
                                                                <ajaxToolkit:CalendarExtender ID="txtFechaMantenimientoTarea_CalendarExtender" runat="server"
                                                                    CssClass="calendario" Enabled="True" TargetControlID="txtFechaMantenimientoTarea">
                                                                </ajaxToolkit:CalendarExtender>
                                                            </div>
                                                        </div>
                                                        <div class="col-6 mt-2">
                                                            <span class="far fa-address-card"></span>
                                                            <label class="text-black-50 col-form-label fs--2 m-1">Hora Tarea</label>
                                                            <div class="input-group">
                                                                <asp:DropDownList ID="cboHorasMantenimientoTarea" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                                                    class="form-select form-select-sm js-choice" Style="text-transform: uppercase" TabIndex="11">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="cboMinutosMantenimientoTarea" runat="server" AppendDataBoundItems="True"
                                                                    class="form-select form-select-sm js-choice" Style="text-transform: uppercase" TabIndex="11">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="cboSegundosMantenimientoTarea" runat="server" AppendDataBoundItems="True"
                                                                    class="form-select form-select-sm js-choice" Style="text-transform: uppercase" TabIndex="11">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="cboMeridianoMantenimientoTarea" runat="server" AppendDataBoundItems="True"
                                                                    class="form-select form-select-sm js-choice" Style="text-transform: uppercase" TabIndex="11">
                                                                    <asp:ListItem Value="AM">AM</asp:ListItem>
                                                                    <asp:ListItem Value="PM">PM</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div>
                                                            <asp:Label ID="lbFechaInicio" runat="server" AppendDataBoundItems="True" class="form-label" Style="text-transform: uppercase" TabIndex="11"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row bg-200">
                                                        <div class="col-lg-12 col-md-12 col-sm-12 mt-2">
                                                            <span class="far fa-address-card"></span>
                                                            <label class="text-black-50 col-form-label fs--2 m-1">Personal Asignado</label>
                                                            <div class="input-group">
                                                                <asp:DropDownList ID="cboPersonalAsignadoMantenimientoTarea" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                                                    class="form-select form-select-sm js-choice" Style="text-transform: uppercase"
                                                                    TabIndex="14">
                                                                    <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:Button ID="btnAdicionarPersonalAsignadoMantenimientoTarea" runat="server" Text="Agregar" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Personal" TabIndex="30" />
                                                                <asp:RequiredFieldValidator ID="rfvPersonalAsignadoMantenimientoTarea" runat="server"
                                                                    ControlToValidate="cboPersonalAsignadoMantenimientoTarea" EnableClientScript="False"
                                                                    ErrorMessage="Ingrese el personal asignado" Font-Size="10px" ForeColor="Red"
                                                                    ValidationGroup="vgrpValidarMantenimientoTarea">(*)</asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 mt-2">
                                                            <div id="divGridPersonalAsignadoMantenimientoTarea" style="overflow: auto; height: 220px; align: left;">
                                                                <asp:GridView ID="grdPersonalAsignadoMantenimientoTarea" runat="server" AutoGenerateColumns="False" TabIndex="17"
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
                                                    <div class="row">
                                                        <div class="col-12 mt-2">
                                                            <span class="far fa-address-card"></span>
                                                            <label class="text-black-50 col-form-label fs--2 m-1">Descripción Tarea</label>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtDescripcionMantenimientoTarea" runat="server" CssClass="form-control form-control-sm"
                                                                    Style="text-transform: uppercase" TabIndex="18" autocomplete="off"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="modal-footer">
                                    <div class="container">
                                        <div class="row">
                                            <div class="col-md-8 mt-2">
                                                <asp:ValidationSummary ID="ValidationSummary3" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarMantenimientoTarea" />
                                            </div>
                                            <div class="col-md-4 text-end p-2">
                                                <asp:Button ID="btnAceptarMantenimientoTarea" runat="server" ValidationGroup="vgrpValidarMantenimientoTarea"
                                                    ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                &nbsp;
                                                <asp:Button ID="btnCancelarMantenimientoTarea" runat="server"
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

            <asp:LinkButton ID="lnk_mostrarPanelMantenimientoActividad" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMantenimientoActividad_ModalPopupExtender" runat="server"
                BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True"
                CancelControlID="btnCancelarMantenimientoActividad"
                PopupControlID="pnlMantenimientoActividad" TargetControlID="lnk_mostrarPanelMantenimientoActividad">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlMantenimientoActividad" runat="server" CssClass="container" DefaultButton="btnAceptarMantenimientoActividad">
                <div class="modal-dialog modal-lg">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header bg-light">
                                    <h4 class="modal-title" id="myModalLabelMantenimientoActividad">Mantenimiento Tarea</h4>
                                    <asp:Button ID="imgbtnCancelarMantenimientoActividadImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <asp:UpdatePanel ID="updpnlMantenimientoActividad" runat="server">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="scrollbar">
                                                    <div class="row">
                                                        <div class="col-lg-6 col-md-6 col-sm-12 mt-2">
                                                            <span class="far fa-address-card"></span>
                                                            <label class="text-black-50 col-form-label fs--2 m-1">Equipo</label>
                                                            <div class="input-group">
                                                                <p class="fs--1 m-0">
                                                                    <asp:Label ID="lblDescripcionEquipoMantenimientoActividad" runat="server" Text="" CssClass="text-500 small"></asp:Label>
                                                                </p>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-6 col-md-6 col-sm-12 mt-2">
                                                            <span class="far fa-address-card"></span>
                                                            <label class="text-black-50 col-form-label fs--2 m-1">Componente</label>
                                                            <div class="input-group">
                                                                <p class="fs--1 m-0">
                                                                    <asp:Label ID="lblDescripcionComponenteMantenimientoActividad" runat="server" Text="" CssClass="text-500 small"></asp:Label>
                                                                </p>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-lg-12 col-md-12 col-sm-12 mt-2">
                                                            <span class="far fa-address-card"></span>
                                                            <label class="text-black-50 col-form-label fs--2 m-1">Observación Actividad</label>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtObservacionMantenimientoActividad" runat="server" autocomplete="off"
                                                                    CssClass="form-control form-control-sm" Style="text-transform: uppercase;"
                                                                    placeholder="Observación de actividad" TabIndex="18" type="text"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="modal-footer">
                                    <div class="container">
                                        <div class="row">
                                            <div class="col-md-8 mt-2">
                                                <asp:ValidationSummary ID="ValidationSummary8" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarMantenimientoActividad" />
                                            </div>
                                            <div class="col-md-4 text-end p-2">
                                                <asp:Button ID="btnAceptarMantenimientoActividad" runat="server" ValidationGroup="vgrpValidarMantenimientoActividad"
                                                    ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                &nbsp;
                                                <asp:Button ID="btnCancelarMantenimientoActividad" runat="server"
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

            <asp:LinkButton ID="lnk_mostrarPanelEliminarOrdenTrabajo" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelEliminarOrdenTrabajo_ModalPopupExtender" runat="server"
                BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True"
                CancelControlID="btnNoEliminarOrdenTrabajo"
                PopupControlID="pnlEliminarOrdenTrabajo" TargetControlID="lnk_mostrarPanelEliminarOrdenTrabajo">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlEliminarOrdenTrabajo" runat="server" CssClass="container" DefaultButton="btnSiEliminarOrdenTrabajo">
                <div class="modal-dialog modal-sm">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header bg-light">
                                    <h4 class="modal-title" id="myModalLabelEliminarOrdenTrabajo">Eliminar Orden Trabajo</h4>
                                    <asp:Button ID="imgbtnCancelarEliminarOrdenTrabajoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <asp:UpdatePanel ID="updpnlEliminarOrdenTrabajo" runat="server">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="scrollbar">
                                                    <div class="row">
                                                        <div class="col-lg-12 col-md-12 col-sm-12 mt-2">
                                                            <%--<span class="far fa-address-card"></span>--%>
                                                            <label class="text-black-50 col-form-label fs--2 m-1">Se procederá a eliminar la siguiente orden de trabajo.  ¿Esta seguro?</label>
                                                            <div class="input-group">
                                                                <p class="fs--1 m-0">
                                                                    ==> <asp:Label ID="lblDescripcionEquipoEliminarOrdenTrabajo" runat="server" Text="" CssClass="text-500 small"></asp:Label>
                                                                </p>
                                                            </div>
                                                        </div>
                                                        <%--<div class="col-lg-6 col-md-6 col-sm-12 mt-2">
                                                            <span class="far fa-address-card"></span>
                                                            <label class="text-black-50 col-form-label fs--2 m-1">Componente</label>
                                                            <div class="input-group">
                                                                <p class="fs--1 m-0">
                                                                    <asp:Label ID="lblDescripcionComponenteEliminarOrdenTrabajo" runat="server" Text="" CssClass="text-500 small"></asp:Label>
                                                                </p>
                                                            </div>
                                                        </div>--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="modal-footer">
                                    <div class="container">
                                        <div class="row">
                                            <div class="col-md-8 mt-2">
                                                <asp:ValidationSummary ID="ValidationSummary13" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarEliminarOrdenTrabajo" />
                                            </div>
                                            <div class="col-md-4 text-end p-2 d-flex justify-content-end gap-2">
                                                <asp:Button ID="btnSiEliminarOrdenTrabajo" runat="server" ValidationGroup="vgrpValidarEliminarOrdenTrabajo"
                                                    ToolTip="Aceptar Eliminación" TabIndex="72" Text="Si" CssClass="btn btn-outline-facebook btn-sm" />
                                                &nbsp;
                                                <asp:Button ID="btnNoEliminarOrdenTrabajo" runat="server"
                                                    ToolTip="Cancelar Eliminación" TabIndex="73" Text="No" CssClass="btn btn-outline-google-plus btn-sm" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <asp:LinkButton ID="lnk_mostrarPanelFirmarOrdenTrabajo" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelFirmarOrdenTrabajo_ModalPopupExtender" runat="server"
                BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True"
                CancelControlID="btnNoFirmarOrdenTrabajo"
                PopupControlID="pnlFirmarOrdenTrabajo" TargetControlID="lnk_mostrarPanelFirmarOrdenTrabajo">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlFirmarOrdenTrabajo" runat="server" CssClass="container" DefaultButton="btnSiFirmarOrdenTrabajo">
                <div class="modal-dialog modal-sm">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header bg-light">
                                    <h4 class="modal-title" id="myModalLabelFirmarOrdenTrabajo">Firmar Orden Trabajo</h4>
                                    <asp:Button ID="imgbtnCancelarFirmarOrdenTrabajoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <asp:UpdatePanel ID="updpnlFirmarOrdenTrabajo" runat="server">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="scrollbar">
                                                    <div class="row">
                                                        <div class="col-lg-12 col-md-12 col-sm-12 mt-2">
                                                            <label class="text-black-50 col-form-label fs--2 m-1">Se procederá a Firmar la siguiente orden de trabajo.  ¿Esta seguro?</label>
                                                            <div class="input-group">
                                                                <p class="fs--1 m-0">
                                                                    Orden de Trabajo ==> <asp:Label ID="lblDescripcionEquipoFirmarOrdenTrabajo" runat="server" Text="" CssClass="text-500"></asp:Label>
                                                                </p>
                                                                <p class="fs--1 m-0">
                                                                    Estado ==> <asp:Label ID="lblEstadoFirmaOrdenTrabajo" runat="server" Text="" CssClass="text-500"></asp:Label>
                                                                </p>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="modal-footer">
                                    <div class="container">
                                        <div class="row">
                                            <div class="col-md-8 mt-2">
                                                <asp:ValidationSummary ID="ValidationSummary16" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarFirmarOrdenTrabajo" />
                                            </div>
                                            <div class="col-md-4 text-end p-2 d-flex justify-content-end gap-2">
                                                <asp:Button ID="btnSiFirmarOrdenTrabajo" runat="server" ValidationGroup="vgrpValidarFirmarOrdenTrabajo"
                                                    ToolTip="Aceptar Eliminación" TabIndex="72" Text="Si" CssClass="btn btn-outline-facebook btn-sm" />
                                                &nbsp;
                                                <asp:Button ID="btnNoFirmarOrdenTrabajo" runat="server"
                                                    ToolTip="Cancelar Eliminación" TabIndex="73" Text="No" CssClass="btn btn-outline-google-plus btn-sm" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <asp:LinkButton ID="lnk_mostrarPanelConclusionRecomendacionOrdenTrabajo" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelConclusionRecomendacionOrdenTrabajo_ModalPopupExtender" runat="server"
                BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True"
                CancelControlID="btnCancelarConclusionRecomendacionOrdenTrabajo"
                PopupControlID="pnlConclusionRecomendacionOrdenTrabajo" TargetControlID="lnk_mostrarPanelConclusionRecomendacionOrdenTrabajo">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlConclusionRecomendacionOrdenTrabajo" runat="server" CssClass="container" DefaultButton="btnGuardarConclusionRecomendacionOrdenTrabajo">
                <div class="modal-dialog modal-md">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header bg-light">
                                    <h4 class="modal-title" id="myModalLabelConclusionRecomendacionOrdenTrabajo">Conclusión y Recomendación</h4>
                                    <asp:Button ID="imgbtnCancelarConclusionRecomendacionOrdenTrabajoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <asp:UpdatePanel ID="updpnlConclusionRecomendacionOrdenTrabajo" runat="server">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="scrollbar">
                                                    <div class="row">
                                                        <div class="col-lg-12 col-md-12 col-sm-12 mt-2">
                                                            <label class="text-black-50 col-form-label fs--2 m-1">Registre sus conclusiones y recomendaciones:</label>
                                                            <div class="input-group">
                                                                <asp:TextBox 
                                                                    ID="txtConclusionesConclusionRecomendacionOrdenTrabajo" 
                                                                    runat="server" 
                                                                    CssClass="form-control form-control-sm" 
                                                                    TextMode="MultiLine" 
                                                                    Rows="6" 
                                                                    TabIndex="70"
                                                                    placeholder="Ingrese sus conclusiones">
                                                                </asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="modal-footer">
                                    <div class="container">
                                        <div class="row">
                                            <div class="col-md-8 mt-2">
                                                <asp:ValidationSummary ID="ValidationSummary17" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarConclusionRecomendacionOrdenTrabajo" />
                                            </div>
                                            <div class="col-md-4 text-end p-2 d-flex justify-content-end gap-2">
                                                <asp:Button ID="btnGuardarConclusionRecomendacionOrdenTrabajo" runat="server" ValidationGroup="vgrpValidarConclusionRecomendacionOrdenTrabajo" OnClientClick="return updateCKEditors();"
                                                    ToolTip="Guardar conclusiones y recomendaciones" TabIndex="72" Text="Guardar" CssClass="btn btn-outline-facebook btn-sm" />
                                                &nbsp;
                                                <asp:Button ID="btnCancelarConclusionRecomendacionOrdenTrabajo" runat="server"
                                                    ToolTip="Cancelar Guardar conclusiones y recomendaciones" TabIndex="73" Text="Cancelar" CssClass="btn btn-outline-google-plus btn-sm" />
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGuardarConclusionRecomendacionOrdenTrabajo" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:LinkButton ID="lnk_mostrarPanelSubirImagenActividad" runat="server"></asp:LinkButton>
    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelSubirImagenActividad_ModalPopupExtender" runat="server"
        BackgroundCssClass="FondoAplicacion" DropShadow="False" DynamicServicePath="" Enabled="True"
        CancelControlID="btnCancelarSubirImagenActividad"
        PopupControlID="pnlSeleccionarSubirImagenActividad" TargetControlID="lnk_mostrarPanelSubirImagenActividad">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="pnlSeleccionarSubirImagenActividad" runat="server" CssClass="container">
        <div class="modal-dialog modal-lg">
            <div class="shadow rounded">
                <div class="modal-dialog-scrollable">
                    <div class="modal-content">
                        <div class="modal-header bg-light">
                            <h4 class="modal-title" id="myModalLabelSubirImagenActividad">Agregar Imagen a la Actividad</h4>
                            <asp:Button ID="imgbtnCancelarSubirImagenActividadImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="updpnlSubirImagenActividad" runat="server">
                                <ContentTemplate>
                                    <div class="row mt-0">
                                        <div class="col-12 mt-2">
                                            <label class="text-black-50 col-form-label col-form-label-sm">Título:</label>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtTituloSubirImagenActividad" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="12"
                                                    Style="text-transform: uppercase" placeholder="Titulo de la imagen"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvTituloSubirImagenActividad" runat="server"
                                                    ControlToValidate="txtTituloSubirImagenActividad" EnableClientScript="False"
                                                    ErrorMessage="Ingrese el título de la imagen" Font-Size="10px" ForeColor="Red"
                                                    ValidationGroup="vgrpValidarSubirImagenActividad">(*)</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-12 mt-2">
                                            <label class="text-black-50 col-form-label col-form-label-sm">Descripción:</label>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtDescripcionSubirImagenActividad" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="12"
                                                    Style="text-transform: uppercase" placeholder="Descripción de la imagen"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvDescripcionSubirImagenActividad" runat="server"
                                                    ControlToValidate="txtDescripcionSubirImagenActividad" EnableClientScript="False"
                                                    ErrorMessage="Ingrese la descripción de la imagen" Font-Size="10px" ForeColor="Red"
                                                    ValidationGroup="vgrpValidarSubirImagenActividad">(*)</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-12 mt-2">
                                            <label class="text-black-50 col-form-label col-form-label-sm">Observación:</label>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtObservacionSubirImagenActividad" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="12"
                                                    Style="text-transform: uppercase" placeholder="Observación de la imagen"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvObservacionSubirImagenActividad" runat="server"
                                                    ControlToValidate="txtObservacionSubirImagenActividad" EnableClientScript="False"
                                                    ErrorMessage="Ingrese la observación de la imagen" Font-Size="10px" ForeColor="Red"
                                                    ValidationGroup="vgrpValidarSubirImagenActividad">(*)</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="row">
                                <label class="text-black-50 col-form-label col-form-label-sm">Archivo:</label>
                                <div class="input-group">
                                    <asp:FileUpload ID="fupSubirImagenActividad" runat="server" EnableTheming="True" CssClass="form-control-sm" TabIndex="90" />
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:ValidationSummary ID="ValidationSummary5" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarSubirImagenActividad" />
                            <div class="col-md-4 text-end p-2">
                                <asp:Button ID="btnAceptarSubirImagenActividad" runat="server" ValidationGroup="vgrpValidarSubirImagenActividad"
                                    ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                &nbsp;
                                <asp:Button ID="btnCancelarSubirImagenActividad" runat="server"
                                    ToolTip="Cancelar Registro" TabIndex="73" Text="Cancelar" CssClass="btn btn-outline-google-plus btn-sm" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:UpdatePanel ID="updpnlInsumos" runat="server">
        <ContentTemplate>
            <asp:LinkButton ID="lnk_mostrarPanelAgregarInsumos" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelAgregarInsumos_ModalPopupExtender" runat="server"
                BackgroundCssClass="FondoAplicacion" DropShadow="False" DynamicServicePath="" Enabled="True"
                CancelControlID="btnCancelarAgregarInsumos"
                PopupControlID="pnlSeleccionarAgregarInsumos" TargetControlID="lnk_mostrarPanelAgregarInsumos">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlSeleccionarAgregarInsumos" runat="server" CssClass="container">
                <div class="modal-dialog modal-lg">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header bg-light">
                                    <h4 class="modal-title" id="myModalLabelAgregarInsumos">Agregar Insumos a la Actividad</h4>
                                    <asp:Button ID="imgbtnCancelarAgregarInsumosImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <div class="row mt-0 bg-200">
                                        <div class="col-12 mt-2">
                                            <span class="far fa-clone"></span>
                                            <label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Descripción de la Actividad:</label>
                                            <div class="input-group">
                                                <asp:Label ID="lblDescripcionActividadAgregarInsumos" runat="server" Text="" CssClass="fs-0 mb-2 px-3"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div runat="server" class="row justify-content-between">
                                        <div class="col-lg-12 col-md-12 col-sm-12">
                                            <span class="fas fa-link"></span>
                                            <label class="text-black-50 col-form-label fs--2 m-1">Filtrar por Insumos:</label>
                                            <div class="input-group">
                                                <asp:DropDownList ID="cboInsumosAgregarInsumos" runat="server" AppendDataBoundItems="True" TabIndex="29"
                                                    CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase">
                                                    <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Button ID="btnAdicionarInsumos" runat="server" Text="Agregar" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Insumos a la Actividad" TabIndex="30" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row justify-content-between mt-3">
                                        <div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
                                            <div id="divGridDetalleAgregarInsumos" style="overflow: auto; height: 160px; align: left;" class="scrollbar">
                                                <asp:UpdatePanel ID="updpnlDetalleAgregarInsumos" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlDetalleAgregarInsumos" runat="server" CssClass="bg-light">
                                                            <div class="table-responsive scrollbar">
                                                                <asp:GridView ID="grdDetalleAgregarInsumos" runat="server" AutoGenerateColumns="False" TabIndex="4"
                                                                    GridLines="None" CssClass="table table-striped table-hover small overflow-hidden" AllowPaging="True"
                                                                    EmptyDataText="No hay registros a visualizar" PageSize="3">
                                                                    <PagerStyle CssClass="mGrid" />
                                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                    <Columns>
                                                                        <asp:CommandField ShowDeleteButton="True" HeaderStyle-Width="70px" />
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkRowDetalleAgregarInsumos" runat="server" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-Width="40px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                        <asp:TemplateField HeaderText="Cantidad Consumida" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                            <ItemTemplate>
                                                                                <div class="input-group">
                                                                                    <asp:HiddenField ID="hfdRowNumber" Value='<%# Container.DataItemIndex %>' runat="server" />
                                                                                    <asp:TextBox ID="txtCantidadDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("Cantidad") %>' AutoPostBack="False" Style="text-transform: uppercase" OnTextChanged="txtCantidadDetalle_TextChanged"></asp:TextBox>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="DescripcionUnidadMedida" HeaderText="Uni.Med." HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
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
                                    <asp:ValidationSummary ID="ValidationSummary6" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarAgregarInsumos" />
                                    <div class="col-md-4 text-end p-2">
                                        <asp:Button ID="btnAceptarAgregarInsumos" runat="server" ValidationGroup="vgrpValidarAgregarInsumos"
                                            ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                        &nbsp;
                                        <asp:Button ID="btnCancelarAgregarInsumos" runat="server"
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

    
    <asp:UpdatePanel ID="updpnlGaleriaImagen" runat="server">
        <ContentTemplate>
            <asp:LinkButton ID="lnk_mostrarPanelGaleriaImagenActividad" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelGaleriaImagenActividad_ModalPopupExtender" runat="server"
                BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True"
                CancelControlID="btnCancelarGaleriaImagenActividad"
                PopupControlID="pnlGaleriaImagenActividad" TargetControlID="lnk_mostrarPanelGaleriaImagenActividad">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlGaleriaImagenActividad" runat="server" CssClass="container" DefaultButton="btnAgregarGaleriaImagenActividad">
                <div class="modal-dialog modal-lg">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header bg-light">
                                    <h4 class="modal-title" id="myModalLabelGaleriaImagen">Galería de Imagenes</h4>
                                    <asp:Button ID="imgbtnCancelarGaleriaImagenImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <%--<asp:UpdatePanel ID="updpnlGaleriaImagenActividad" runat="server">
                                        <ContentTemplate>--%>
                                            <div class="row">
                                                <%--<div id="divGridGaleriaActividad" style="overflow: auto; height: 200px; align: left;" class="scrollbar">--%>
                                                <div id="divGridGaleriaActividad" class="viewport table-responsive scrollbar">
                                                    <asp:DataList ID="lstGaleriaActividad" runat="server" DataKeyField="DocumentoRef"
                                                        RepeatColumns="5" RepeatDirection="Horizontal" OnItemCommand="lstGaleriaActividad_ItemCommand">
                                                        <ItemTemplate>
                                                            <div class="row">
                                                                <div class="mb-4 col-12">
                                                                    <div class="border rounded-1 h-100 d-flex flex-column justify-content-between pb-3">
                                                                        <div class="overflow-hidden">
                                                                            <div class="position-relative rounded-top overflow-hidden">
                                                                                <asp:HiddenField ID="hfdUrlGaleriaActividad" runat="Server" Value='<%# Trim(Eval("NombreArchivo")) %>'></asp:HiddenField>
                                                                                <div class="col-12 p-1">
                                                                                    <asp:LinkButton ID="lnkbtnVerGaleriaImagenesActividadPersonal" runat="server" CommandName="VerFotoGaleria" data-gallery="gallery-1"><img class="img-fluid rounded" src='<%# "Imagenes/Actividadsmall/" & Trim(Eval("NombreArchivo")) %> ' alt="" /></asp:LinkButton>
                                                                                </div>
                                                                            </div>
                                                                            <div class="p-3">
                                                                                <h5 class="fs-0">
                                                                                    <asp:Label ID="lblCodigo" runat="server" Text='<%# Eval("Titulo") %>' CssClass="text-dark"></asp:Label></h5>
                                                                                <p class="fs--1 mb-3">
                                                                                    <asp:Label ID="lblDescripcion" runat="server" Text='<%# Eval("Descripcion") %>' CssClass="mgrid_small text-500"></asp:Label>
                                                                                </p>
                                                                                <p class="fs--1 mb-1">
                                                                                    <asp:Label ID="lblObservacion" runat="server" CssClass="mgrid_tipo_afectacion" Text='<%# Eval("Observacion") %>'></asp:Label>
                                                                                </p>
                                                                                <!-- Botón Eliminar -->
                                                                                <asp:LinkButton ID="lnkEliminar" runat="server" CommandName="Eliminar" CssClass="btn btn-danger btn-sm mt-2"
                                                                                                CommandArgument='<%# Eval("NombreArchivo") %>' OnClientClick="return confirm('¿Estás seguro de eliminar esta imagen?');">
                                                                                    Eliminar
                                                                                </asp:LinkButton>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </div>
                                            </div>
                                        <%--</ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </div>
                                <div class="modal-footer">
                                    <div class="container">
                                        <div class="row">
                                            <div class="col-md-6 mt-2">
                                                <asp:ValidationSummary ID="ValidationSummary4" runat="server" role="alert" CssClass="alert alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarGaleriaImagenActividad" />
                                            </div>
                                            <div class="col-md-6 text-end p-2">
                                                <asp:Button ID="btnAgregarGaleriaImagenActividad" runat="server" ValidationGroup="vgrpValidarGaleriaImagenActividad"
                                                    ToolTip="Agregar Imagen" TabIndex="72" Text="Agregar Imagen" CssClass="btn btn-outline-facebook btn-sm" />
                                                &nbsp;
        <%--                                        <asp:Button ID="btnAceptarGaleriaImagenActividad" runat="server" ValidationGroup="vgrpValidarGaleriaImagenActividad"
                                                    ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                &nbsp;--%>
                                                <asp:Button ID="btnCancelarGaleriaImagenActividad" runat="server"
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

    <asp:LinkButton ID="lnk_mostrarPanelAddActividades" runat="server"></asp:LinkButton>
    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelAddActividades_ModalPopupExtender" runat="server"
        BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True"
        PopupControlID="pnlAddActividades" TargetControlID="lnk_mostrarPanelAddActividades">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="pnlAddActividades" runat="server" CssClass="container" DefaultButton="btnAceptarAddActividades">
        <div class="modal-dialog modal-lg">
            <div class="shadow rounded">
                <div class="modal-dialog-scrollable">
                    <div class="modal-content">
                        <div class="modal-header bg-light">
                            <h4 class="modal-title" id="myModalLabelAddActividades">Actividades</h4>
                            <%--<asp:Button ID="Button1" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />--%>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="updpnlAddActividades" runat="server" class="row justify-content-between">
                                <ContentTemplate>
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <asp:UpdatePanel ID="updpnlActividadCatalogoComponente" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                            <ContentTemplate>
                                                <span class="fas fa-link"></span>
                                                <label class="text-black-50 col-form-label fs--2 m-1">Actividades:</label>
                                                <div class="input-group">
                                                    <asp:DropDownList ID="cboActividadCatalogoComponente" runat="server" AppendDataBoundItems="True" TabIndex="29"
                                                        CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase">
                                                        <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btnAdicionarActividadCatalogoComponente" runat="server" Text="Agregar" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Caracteristica al equipo principal" TabIndex="30" />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="row justify-content-between mt-3">
                                        <div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
                                            <asp:UpdatePanel ID="updpnlDetalleActividadCatalogoComponente" runat="server">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlDetalleActividadCatalogoComponente" runat="server" CssClass="bg-light">
                                                        <div id="divGridActividadCatalogoComponente" style="overflow: auto; height: 220px; align: left;" class="scrollbar">
                                                            <asp:GridView ID="grdDetalleActividadCatalogoComponente" runat="server" AutoGenerateColumns="False" TabIndex="4"
                                                                GridLines="None" CssClass="table table-striped table-hover small overflow-hidden" AllowPaging="True"
                                                                EmptyDataText="No hay registros a visualizar" PageSize="14">
                                                                <PagerStyle CssClass="mGrid" />
                                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                <Columns>
                                                                    <asp:CommandField ShowDeleteButton="True" HeaderStyle-Width="70px" />
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkRowDetalleActividadCatalogoComponente" runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Item" HeaderText="#" ControlStyle-Width="25px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                    <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-Width="40px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                    <asp:BoundField DataField="IdCatalogo" HeaderText="IdCatalogo" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                    <asp:BoundField DataField="IdJerarquia" HeaderText="IdJerarquia" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                    <asp:BoundField DataField="DescripcionComponente" HeaderText="Componente" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                </Columns>
                                                                <HeaderStyle CssClass="thead-dark" />
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </ContentTemplate>

                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <div class="container">
                                <div class="row">
                                    <div class="col-md-8 mt-2">
                                        <asp:ValidationSummary ID="ValidationSummary11" runat="server" role="alert" CssClass="alert alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarGaleriaImagenActividad" />
                                    </div>
                                    <div class="col-md-4 text-end p-2">
                                        <asp:Button ID="btnAceptarAddActividades" runat="server" ValidationGroup="vgrpValidarGaleriaImagenActividad"
                                            ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                        &nbsp;
                                        <asp:Button ID="btnCancelarAddActividades" runat="server"
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

    <%--Así se debe de hacer: 21/05/2023--%>
    <asp:UpdatePanel ID="updpnlVerImagenActividad" runat="server">
        <ContentTemplate>
            <asp:LinkButton ID="lnk_mostrarPanelVerImagenActividad" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelVerImagenActividad_ModalPopupExtender" runat="server"
                BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True"
                CancelControlID="imgbtnCancelarVerImagenActividadImagen"
                PopupControlID="pnlVerImagenActividad" TargetControlID="lnk_mostrarPanelVerImagenActividad">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlVerImagenActividad" runat="server" CssClass="container">
                <div class="modal-dialog modal-dialog-centered modal-md shadow rounded">
                    <div class="modal-content">
                        <div class="modal-header bg-light">
                            <h4 class="modal-title" id="myModalLabelVerImagenActividadImagen">Ver Imagen
                            </h4>
                            <asp:Button ID="imgbtnCancelarVerImagenActividadImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-12">
                                    <asp:Image ID="imgVerImagenActividad" runat="server" class="img-fluid rounded" alt="" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <%--Así se debe de hacer: 21/05/2023--%>
    <asp:UpdatePanel ID="updpnlIngresarOtrosDatos" runat="server">
        <ContentTemplate>
            <asp:LinkButton ID="lnk_mostrarPanelOtrosDatos" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelOtrosDatos_ModalPopupExtender" runat="server"
                BackgroundCssClass="FondoAplicacion" DropShadow="false" Enabled="True"
                CancelControlID="btnCancelarOtrosDatos"
                PopupControlID="pnlIngresarOtrosDatos" TargetControlID="lnk_mostrarPanelOtrosDatos">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlIngresarOtrosDatos" runat="server" CssClass="container" DefaultButton="btnAceptarOtrosDatos">
                <div class="modal-dialog modal-lg">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title" id="myModalLabelOtrosDatos">Mantenimiento Otros Datos</h4>
                                    <asp:Button ID="imgbtnCancelarOtrosDatosImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
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
                                                    <div class="col-12 mt-2">
                                                        <span class="far fa-address-card"></span>
                                                        <label class="text-black-50 col-form-label fs--2 m-1">Descripción Equipo:</label>
                                                        <div class="input-group">
                                                            <p class="fs--1 m-0">
                                                                <asp:Label ID="lblDescripcionEquipoCaracteristicaOtrosDatos" runat="server" Text="" CssClass="text-500 small"></asp:Label>
                                                            </p>
                                                        </div>
                                                    </div>
                                                    <div class="col-12 mt-2">
                                                        <span class="far fa-address-card"></span>
                                                        <label class="text-black-50 col-form-label fs--2 m-1">Descripción Actividad:</label>
                                                        <div class="input-group">
                                                            <p class="fs--1 m-0">
                                                                <asp:Label ID="lblDescripcionActividadCaracteristicaOtrosDatos" runat="server" Text="" CssClass="text-500 small"></asp:Label>
                                                            </p>
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
                                                <div id="divFiltrarCaracteristicaOtrosDatos" runat="server" class="row justify-content-between">
                                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                                        <asp:UpdatePanel ID="updpnlGeneral" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                                            <ContentTemplate>
                                                                <span class="fas fa-link"></span>
                                                                <label class="text-black-50 col-form-label fs--2 m-1">Filtrar Caracteristica:</label>
                                                                <div class="input-group">
                                                                    <asp:DropDownList ID="cboCaracteristicaOtrosDatos" runat="server" AppendDataBoundItems="True" TabIndex="29"
                                                                        CssClass="form-select form-select-sm js-choice" Style="text-transform: uppercase">
                                                                        <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:Button ID="btnAdicionarCaracteristicaOtrosDatos" runat="server" Text="Agregar" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Caracteristica" TabIndex="30" />
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="row justify-content-between mt-3">
                                                    <div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
                                                        <asp:UpdatePanel ID="updpnlDetalleCaracteristicaOtrosDatos" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="pnlDetalleCaracteristicaOtrosDatos" runat="server" CssClass="bg-light">
                                                                    <div class="table-responsive scrollbar">
                                                                        <asp:GridView ID="grdDetalleCaracteristicaOtrosDatos" runat="server" AutoGenerateColumns="False" TabIndex="4"
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
                                                                                <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-Width="40px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />

                                                                                <asp:TemplateField HeaderText="Valor" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                                    <ItemTemplate>
                                                                                        <div class="input-group">
                                                                                            <asp:HiddenField ID="hfdRowNumber" Value='<%# Container.DataItemIndex %>' runat="server" />
                                                                                            <asp:TextBox ID="txtValorDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("ValorReferencial") %>' AutoPostBack="False" Style="text-transform: uppercase" OnTextChanged="txtValorDetalleCaracteristicaOtrosDatos_TextChanged"></asp:TextBox>
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
                                    <asp:ValidationSummary ID="ValidationSummary7" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarOtrosDatos" />
                                    <div class="col-md-4 text-end p-2">
                                        <asp:Button ID="btnAceptarOtrosDatos" runat="server" ValidationGroup="vgrpValidarOtrosDatos" OnClick="btnAceptarOtrosDatos_Click"
                                            ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                        &nbsp;
                                        <asp:Button ID="btnCancelarOtrosDatos" runat="server"
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

    <asp:UpdatePanel ID="updpnlVerDocumentosEquipo" runat="server">
        <ContentTemplate>
            <asp:LinkButton ID="lnk_mostrarPanelVerDocumentosEquipo" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelVerDocumentosEquipo_ModalPopupExtender" runat="server"
                BackgroundCssClass="FondoAplicacion" DropShadow="false" Enabled="True"
                CancelControlID="btnCancelarVerDocumentosEquipo"
                PopupControlID="pnlVerDocumentosEquipo" TargetControlID="lnk_mostrarPanelVerDocumentosEquipo">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlVerDocumentosEquipo" runat="server" CssClass="container" DefaultButton="btnCancelarVerDocumentosEquipo">
                <div class="modal-dialog modal-sm">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title" id="myModalLabelVerDocumentosEquipo">Ver Documentos</h4>
                                    <asp:Button ID="imgbtnCancelarVerDocumentosEquipo" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <div class="form-group">
                                        <div class="row mt-1">
                                            <div class="col-lg-12 col-sm-12">
                                                <div id="divGridVerDocumentosEquipo" style="overflow: auto; height: 220px; align: left;" class="scrollbar">
                                                    <asp:GridView ID="grdListaVerDocumentosEquipo" runat="server" AutoGenerateColumns="False" TabIndex="4" DataKeyNames="NombreArchivo"
                                                        GridLines="None" CssClass="table table-responsive-sm table-hover small" AllowPaging="True"
                                                        EmptyDataText="No hay registros a visualizar" PageSize="14" OnRowCommand="grdLista_RowCommand">
                                                        <PagerStyle CssClass="pgr" />
                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Detalle" ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small">
                                                                <ItemTemplate>
                                                                    <div class="col-12">
                                                                        <asp:Label ID="lblTituloVerDocumento" runat="server" Text='<%# Eval("Titulo") %>'></asp:Label>
                                                                    </div>
                                                                    <div class="col-12">
                                                                        <asp:Label ID="lblDescripcionVerDocumento" runat="server" Text='<%# Eval("Descripcion") %>'></asp:Label>
                                                                    </div>
                                                                    <div class="col-12">
                                                                        <asp:HyperLink ID="hlnkVerDocumento" runat="server" Target="_blank"
                                                                            NavigateUrl='<%# IIf(Trim(Eval("NombreArchivo")) = "", "", "~/Downloads/Equipo/" & Eval("NombreArchivo")) %>'
                                                                            Text='<%# Eval("NombreArchivo") %>' CssClass="button short green borde_redondo2"></asp:HyperLink>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="thead-dark" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:ValidationSummary ID="ValidationSummary9" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarOtrosDatos" />
                                    <div class="col-md-4 text-end p-2">
                                        <asp:Button ID="btnCancelarVerDocumentosEquipo" runat="server"
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
                                    <%--<asp:Button ID="imgbtnCancelarOrdenTrabajoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />--%>
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
                                                        <asp:RequiredFieldValidator ID="rfvTipoControlTiempoPlanificadaMantenimientoOrdenTrabajo" runat="server"
                                                            ControlToValidate="cboTipoMantenimientoMantenimientoOrdenTrabajo" EnableClientScript="False"
                                                            ErrorMessage="Ingrese el tipo control de tiempo de mantenimiento" Font-Size="10px" ForeColor="Red"
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
                                                    <label class="text-black-50 col-form-label fs--2 m-1">Control de Tiemposss</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboTipoControlTiempoMantenimientoOrdenTrabajo" runat="server" AppendDataBoundItems="True"
                                                            class="form-select form-select-sm js-choice" Style="text-transform: uppercase"
                                                            TabIndex="14">
                                                            <asp:ListItem Value="D">POR ORDEN DE TRABAJO</asp:ListItem>
                                                            <asp:ListItem Value="C">POR COMPONENTES</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvTipoControlTiempoMantenimientoOrdenTrabajo" runat="server"
                                                            ControlToValidate="cboTipoControlTiempoMantenimientoOrdenTrabajo" EnableClientScript="False"
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
                                                <asp:ValidationSummary ID="ValidationSummary12" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarMantenimientoOrdenTrabajo" />
                                            </div>
                                            <div class="col-md-4 text-end p-2">
                                                <asp:Button ID="btnAceptarMantenimientoOrdenTrabajo" runat="server" ValidationGroup="vgrpValidarMantenimientoOrdenTrabajo" OnClientClick="deshabilitar(this.id)"
                                                    ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                &nbsp;
                                                <asp:Button ID="btnCancelarMantenimientoOrdenTrabajo" runat="server"
                                                    ToolTip="Cancelar Registro" TabIndex="73" Text="Cancelar" CssClass="btn btn-outline-google-plus btn-sm"
                                                    
                                                    />
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









    <asp:LinkButton ID="lnk_mostrarPanelSubirDocumentacionOrdenTrabajo" runat="server"></asp:LinkButton>
    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelSubirDocumentacionOrdenTrabajo_ModalPopupExtender" runat="server"
        BackgroundCssClass="FondoAplicacion" DropShadow="False" DynamicServicePath="" Enabled="True"
        CancelControlID="btnCancelarSubirDocumentacionOrdenTrabajo"
        PopupControlID="pnlSeleccionarSubirDocumentacionOrdenTrabajo" TargetControlID="lnk_mostrarPanelSubirDocumentacionOrdenTrabajo">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlSeleccionarSubirDocumentacionOrdenTrabajo" runat="server" CssClass="container">
        <div class="modal-dialog modal-lg">
            <div class="shadow rounded">
                <div class="modal-dialog-scrollable">
                    <div class="modal-content">
                        <div class="modal-header bg-light">
                            <h4 class="modal-title" id="myModalLabelSubirDocumentacionOrdenTrabajo">Ver Documentación de la Orden de Trabajo</h4>
                            <asp:Button ID="imgbtnCancelarSubirDocumentacionOrdenTrabajoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-5 mt-2">
                                    <asp:UpdatePanel ID="updpnlSubirDocumentacionOrdenTrabajoImagen" runat="server">
                                        <ContentTemplate>
                                            <div id="divGridVerDocumentosOrdenTrabajo" style="overflow: auto; height: 220px; align: left;" class="scrollbar">
                                                <asp:GridView ID="grdListaVerDocumentosOrdenTrabajo" runat="server" AutoGenerateColumns="False" TabIndex="4" DataKeyNames="NombreArchivo"
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
                                                                        NavigateUrl='<%# IIf(Trim(Eval("NombreArchivo")) = "", "", "~/Downloads/OrdenTrabajo/" & Eval("NombreArchivo")) %>'
                                                                        Text='<%# Eval("NombreArchivo") %>' CssClass="button short green borde_redondo2"></asp:HyperLink>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mant." ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEliminarDocumento" runat="server" CommandName="EliminarDocumento" Text="Eliminar" CommandArgument='<%# Eval("Item") & "," & Eval("Tipo") & "," & Eval("Serie") & "," & Eval("Correlativo") %>' ImageUrl="imagenes/eliminar.png" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="thead-dark" />
                                                </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:Button ID="btnAgregarVerDocumentosOrdenTrabajo" runat="server" ValidationGroup="vgrpValidarSubirDocumentacionOrdenTrabajo"
                                        ToolTip="Nuevo Documento" TabIndex="72" Text="Nuevo Documento" CssClass="btn btn-outline-facebook btn-sm" />
                                </div>
                                <div class="col-7">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <div id="divDetalleVerDocumentosOrdenTrabajo" runat="server" class="scrollbar">
                                                <div class="row">
                                                    <div class="col-12 mt-2">
                                                        <label class="text-black-50 col-form-label col-form-label-sm">Título:</label>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtTituloSubirDocumentacionOrdenTrabajo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="12"
                                                                Style="text-transform: uppercase" placeholder="Titulo del archivo"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvTituloSubirDocumentacionOrdenTrabajo" runat="server"
                                                                ControlToValidate="txtTituloSubirDocumentacionOrdenTrabajo" EnableClientScript="False"
                                                                ErrorMessage="Ingrese el título del documento" Font-Size="10px" ForeColor="Red"
                                                                ValidationGroup="vgrpValidarSubirDocumentacionOrdenTrabajo">(*)</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-12 mt-2">
                                                        <label class="text-black-50 col-form-label col-form-label-sm">Descripción:</label>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtDescripcionSubirDocumentacionOrdenTrabajo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="12"
                                                                Style="text-transform: uppercase" placeholder="Descripción del archivo"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvDescripcionSubirDocumentacionOrdenTrabajo" runat="server"
                                                                ControlToValidate="txtDescripcionSubirDocumentacionOrdenTrabajo" EnableClientScript="False"
                                                                ErrorMessage="Ingrese la descripción del documento" Font-Size="10px" ForeColor="Red"
                                                                ValidationGroup="vgrpValidarSubirDocumentacionOrdenTrabajo">(*)</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-12 mt-2">
                                                        <label class="text-black-50 col-form-label col-form-label-sm">Archivo:</label>
                                                        <div class="input-group">
                                                            <asp:FileUpload ID="fupSubirDocumentacionOrdenTrabajo" runat="server" EnableTheming="True" CssClass="form-control-sm" TabIndex="90" />
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
                            <asp:ValidationSummary ID="ValidationSummary10" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarSubirDocumentacionOrdenTrabajo" />
                            <div class="col-md-4 text-end p-2">
                                <asp:Button ID="btnAceptarSubirDocumentacionOrdenTrabajo" runat="server" ValidationGroup="vgrpValidarSubirDocumentacionOrdenTrabajo"
                                    ToolTip="Aceptar Registro" TabIndex="72" Text="Agregar" CssClass="btn btn-outline-facebook btn-sm" />
                                &nbsp;
                                <asp:Button ID="btnCancelarSubirDocumentacionOrdenTrabajo" runat="server"
                                    ToolTip="Cancelar Registro" TabIndex="73" Text="Cancelar" CssClass="btn btn-outline-google-plus btn-sm" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

















    <script language="javascript" type="text/javascript">
        function popupEmitirOrdenTrabajoReporte(TipDoc, NroSer, NroDoc) {
            window.open("Informes/frmCmmsOrdenTrabajoReporte.aspx?TipDoc=" + TipDoc + "&NroSerie=" + NroSer + "&NroDoc=" + NroDoc, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
        }
        function popupEmitirOrdenTrabajoCheckListReporte(TipDoc, NroSer, NroDoc) {
            window.open("Informes/frmCmmsOrdenTrabajoCheckListClienteReporte.aspx?TipDoc=" + TipDoc + "&NroSerie=" + NroSer + "&NroDoc=" + NroDoc, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
        }

        function popupEmitirOrdenTrabajoCheckListReporte(TipDoc, NroSer, NroDoc) {
            window.open("Informes/frmCmmsOrdenTrabajoCheckListClienteReporte.aspx?TipDoc=" + TipDoc + "&NroSerie=" + NroSer + "&NroDoc=" + NroDoc, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
        }

        function popupEmitirDetalleOrdenTrabajoServicioReporte(TipDoc, NroSer, NroDoc) {
            window.open("Informes/frmCmmsOrdenTrabajoServicioReporte.aspx?TipDoc=" + TipDoc + "&NroSerie=" + NroSer + "&NroDoc=" + NroDoc, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
        }
    </script>

    <script type="text/javascript">
        $(function () {
            // Inicialización normal al cargar la página
            CKEDITOR.replace('<%= txtConclusionesConclusionRecomendacionOrdenTrabajo.ClientID %>', {
                toolbar: 'Full',
                language: 'es',
                height: 300,
                width: '100%'
            });
        });

        // Re-inicializar tras postback asincrónico
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            if (CKEDITOR.instances['<%= txtConclusionesConclusionRecomendacionOrdenTrabajo.ClientID %>']) {
                CKEDITOR.instances['<%= txtConclusionesConclusionRecomendacionOrdenTrabajo.ClientID %>'].destroy(true);
            }
            CKEDITOR.replace('<%= txtConclusionesConclusionRecomendacionOrdenTrabajo.ClientID %>', {
                toolbar: 'Full',
                language: 'es',
                height: 300,
                width: '100%'
            });
        });

        function updateCKEditors() {
            for (var instanceName in CKEDITOR.instances) {
                CKEDITOR.instances[instanceName].updateElement();
            }
            return true; // <- MUY IMPORTANTE
        }
    </script>

    <link href="cssbootstrap/bootstrap-switch.css" rel="stylesheet" />
    <script src="jsbootstrap/bootstrap-switch.js"></script>

    <script src="vendors/anchorjs/anchor.min.js"></script>
    <script src="vendors/glightbox/glightbox.min.js"> </script>
    <script src="vendors/lodash/lodash.min.js"></script>
    <script src="assets/js/theme.js"></script>

    <script src="<%= ResolveUrl("~/Scripts/ckeditor/ckeditor.js") %>"></script>
    <script src="<%= ResolveUrl("~/Scripts/ckeditor/adapters/jquery.js") %>"></script>
</asp:Content>
