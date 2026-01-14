<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/PM_Cmms.Master"
    CodeBehind="frmLogiGenerarContrato_v3.aspx.vb" Inherits="pryCMMS.frmLogiGenerarContrato_v3" %>

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


    <!-- NUEVOS ESTILOS PARA LA SOLUCIÓN DEFINITIVA -->
    <style>
        /* CONTENEDORES PRINCIPALES */
        .dual-table-container {
            display: flex;
            width: 100%;
            border: 1px solid #dee2e6;
            border-radius: 5px;
            margin-top: 15px;
            overflow: hidden;
        }

        .fixed-columns-container {
            width: 530px;
            background-color: #fff;
            box-shadow: 3px 0 5px rgba(0,0,0,0.1);
            z-index: 10;
            /*overflow: visible !important;*/ /* Cambio crucial */
            overflow: auto !important; /* Cambio crucial */
            max-height: 400px; /* Altura fija para permitir scroll */
        }

        .scrollable-columns-container {
            flex: 1;
            overflow-x: auto;
            max-height: 400px; /* Altura fija para permitir scroll */
        }

        /* ESTILOS DE TABLA - MÁXIMA PRIORIDAD */
        .split-table {
            border-collapse: collapse !important;
            width: 100% !important;
            table-layout: auto !important; /* Cambiado a auto */
        }

        .split-table th,
        .split-table td {
            white-space: nowrap;
            vertical-align: middle; /* Cambiado de top a middle */
            padding: 4px 6px !important;
            border: 1px solid #dee2e6 !important; /* Borde visible para debug */
            background-color: inherit;
            font-size: 0.75rem !important;
            overflow: visible !important;
            text-overflow: clip !important;
            display: table-cell !important;
            visibility: visible !important;
            /*line-height: 2.4;*/
        }

        /* Asegurar altura consistente para filas */
        .split-table tr {
            /* Altura mínima basada en line-height y padding */
            min-height: 45px; /* (0.75rem * 2.4) + (4px * 2) */
            height: 45px;
            /*display: flex;*/
            /*align-items: stretch;*/
        }

        .split-table th {
            position: sticky;
            top: 0;
            background-color: #f8f9fa !important;
            z-index: 20;
        }

        /* ESPECÍFICO PARA GRID FIJADO */
        #grdSireFixed {
            width: auto !important;
            min-width: 100%;
        }

        #grdSireFixed th,
        #grdSireFixed td {
            min-width: 50px !important; /* Ancho mínimo garantizado */
            box-sizing: border-box;
        }


        /* Aplica a todos los linkbuttons dentro del grid */
        .grid-linkbutton {
            display: inline-block;
            padding: 0 2px;      /* menos espacio interno */
            margin: 0;           /* sin espacio externo */
            line-height: 1;      /* altura de línea ajustada */
            font-size: 11px;     /* letra más compacta */
            white-space: nowrap; /* evita que se parta en varias líneas */
        }

        /* Aplica al label de estado */
        .grid-label {
            display: inline-block;
            padding: 3px 3px;
            margin: 0;
            line-height: 1;
            font-size: 10px;
            white-space: nowrap;
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
                                    <h5 class="mb-0" data-anchor="data-anchor">Listado de Contratos (Planificación)</h5>
                                    <a class="btn btn-falcon-default btn-sm ml-auto float-right"
                                        href="frmBienvenida.aspx" aria-expanded="true">
                                        <span class="fas fa-home"></span><span
                                            class="nav-link-text ps-1">Inicio</span>
                                    </a>
                                </div>
                            </div>
                            <div class="col-auto ms-auto">
                                <div class="nav gap-2" role="tablist">
                                    <div hidden>
                                        <asp:LinkButton ID="btnNuevo" runat="server" OnClick="btnNuevo_Click" Visible="false"
                                        CssClass="btn btn-success btn-sm ml-auto float-right"
                                        Text="<span class='fas fa-plus'></span> Nuevo"
                                        ToolTip="Nuevo Contrato" />
                                    </div>
                                    <asp:LinkButton ID="btnEditar" runat="server" OnClick="btnEditar_Click"
                                        CssClass="btn btn-primary btn-sm ml-auto float-right"
                                        Text="<span class='fas fa-eye'></span> Ver"
                                        ToolTip="Ver Contrato" />
                                    <div class="dropdown font-sans-serif btn-reveal-trigger" hidden>
                                        <button class="btn btn-sm btn-reveal py-0 px-2" type="button"
                                            id="OpcionesContrato" data-bs-toggle="dropdown" aria-haspopup="true"
                                            aria-expanded="false">
                                            <span
                                                class="fas fa-ellipsis-h"></span>
                                        </button>
                                        <div class="dropdown-menu dropdown-menu-end py-0"
                                            aria-labelledby="OpcionesContrato">
                                            <asp:LinkButton ID="lnkbtnVerContrato" runat="server"
                                                ToolTip="Ver Contrato"
                                                Text="<span class='fas fa-file me-2'></span>Ver Contrato"
                                                CssClass="nav-link nav-link-card-details" />
                                            <asp:LinkButton ID="lnkbtnVerProgramacion" runat="server"
                                                ToolTip="Ver Programación"
                                                Text="<span class='fas fa-file me-2'></span>Ver Programación"
                                                CssClass="nav-link nav-link-card-details" />
                                        </div>
                                    </div>
                                </div>
                                <asp:LinkButton ID="lnk_mostrarPanelImprimirProgramacion" runat="server">
                                </asp:LinkButton>
                                <ajaxToolkit:ModalPopupExtender
                                    ID="lnk_mostrarPanelImprimirProgramacion_ModalPopupExtender" runat="server"
                                    BackgroundCssClass="FondoAplicacion"
                                    CancelControlID="btnCancelarImprimirProgramacion" DropShadow="False"
                                    Enabled="True" PopupControlID="pnlIngresarImprimirProgramacion"
                                    TargetControlID="lnk_mostrarPanelImprimirProgramacion">
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:UpdatePanel ID="updpnlImprimirProgramacion" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlIngresarImprimirProgramacion" runat="server"
                                            CssClass="container">
                                            <div class="modal-dialog modal-xl">
                                                <div class="shadow rounded">
                                                    <div class="modal-dialog-scrollable">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <h4 class="modal-title"
                                                                    id="myModalLabelCatalogo">Datos a Imprimir
                                                                </h4>
                                                                <asp:Button
                                                                    ID="imgbtnCancelarImprimirProgramacionImagen"
                                                                    runat="server"
                                                                    CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base"
                                                                    data-dismiss="modal" aria-label="Close" />
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="form-group">
                                                                    <div class="row mt-1">
                                                                        <div class="col-lg-6 col-sm-12">
                                                                            <div class="row mt-1">
                                                                                <div
                                                                                    class="col-12 mb-0 bg-100 d-none d-md-block">
                                                                                    <h6 class="mt-2 mb-1">FILTROS</h6>
                                                                                    <hr class="bg-300 m-0" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div
                                                                                    class="col-lg-4 col-md-4 mt-2">
                                                                                    <span
                                                                                        class="far fa-calendar-alt"></span>
                                                                                    <label
                                                                                        class="text-black-50 col-form-label fs--2 m-1">
                                                                                        Fecha
                                                                                                Inicial:</label>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox
                                                                                            ID="txtFechaInicialImprimirProgramacion"
                                                                                            runat="server"
                                                                                            autocomplete="off"
                                                                                            CssClass="form-control form-control-sm"
                                                                                            Style="text-transform: uppercase;"
                                                                                            placeholder="Periodo Inicial"
                                                                                            TabIndex="4"
                                                                                            type="date">
                                                                                        </asp:TextBox>
                                                                                        <asp:RequiredFieldValidator
                                                                                            ID="rfvFechaInicialImprimirProgramacion"
                                                                                            runat="server"
                                                                                            ControlToValidate="txtFechaInicialImprimirProgramacion"
                                                                                            EnableClientScript="True"
                                                                                            ErrorMessage="Ingrese el periodo inicial"
                                                                                            Font-Size="10px"
                                                                                            ForeColor="Red"
                                                                                            ValidationGroup="vgrpValidar">
                                                                                                    (*)
                                                                                        </asp:RequiredFieldValidator>
                                                                                    </div>
                                                                                </div>
                                                                                <div
                                                                                    class="col-lg-4 col-md-4 mt-2">
                                                                                    <span
                                                                                        class="far fa-calendar-alt"></span>
                                                                                    <label
                                                                                        class="text-black-50 col-form-label fs--2 m-1">
                                                                                        Fecha
                                                                                                Final:</label>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox
                                                                                            ID="txtFechaFinalImprimirProgramacion"
                                                                                            runat="server"
                                                                                            autocomplete="off"
                                                                                            CssClass="form-control form-control-sm datetimepicker"
                                                                                            Style="text-transform: uppercase;"
                                                                                            placeholder="Periodo Final"
                                                                                            TabIndex="4"
                                                                                            type="date">
                                                                                        </asp:TextBox>
                                                                                        <asp:RequiredFieldValidator
                                                                                            ID="rfvFechaFinalImprimirProgramacion"
                                                                                            runat="server"
                                                                                            ControlToValidate="txtFechaFinalImprimirProgramacion"
                                                                                            EnableClientScript="True"
                                                                                            ErrorMessage="Ingrese el periodo final"
                                                                                            Font-Size="10px"
                                                                                            ForeColor="Red"
                                                                                            ValidationGroup="vgrpValidar">
                                                                                                    (*)
                                                                                        </asp:RequiredFieldValidator>
                                                                                    </div>
                                                                                </div>
                                                                                <div
                                                                                    class="col-lg-4 col-md-4 mt-2">
                                                                                    <span
                                                                                        class="far fa-address-card"></span>
                                                                                    <label
                                                                                        class="text-black-50 col-form-label fs--2 m-1">
                                                                                        Marcar
                                                                                                Todos:</label>
                                                                                    <div class="input-group">
                                                                                        <asp:CheckBox
                                                                                            ID="chkTodosImprimirProgramacion"
                                                                                            runat="server"
                                                                                            TabIndex="5"
                                                                                            CssClass="form-check"
                                                                                            AutoPostBack="True" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div
                                                                            class="col-lg-6 col-sm-12 mt-2 bg-200">
                                                                            <div class="row mt-1">
                                                                                <div
                                                                                    class="col-12 mb-0 d-none d-md-block">
                                                                                    <h6 class="mt-2 mb-1">LISTADO DE EQUIPOS</h6>
                                                                                    <hr class="bg-300 m-0" />
                                                                                </div>
                                                                            </div>
                                                                            <div
                                                                                class="row justify-content-between mt-3">
                                                                                <div
                                                                                    class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
                                                                                    <asp:Panel
                                                                                        ID="pnlDetalleEquipoImprimiProgramacion"
                                                                                        runat="server"
                                                                                        CssClass="bg-light">
                                                                                        <div
                                                                                            class="table-responsive scrollbar">
                                                                                            <asp:GridView
                                                                                                ID="grdDetalleEquipoImprimirProgramacion"
                                                                                                runat="server"
                                                                                                AutoGenerateColumns="False"
                                                                                                TabIndex="4"
                                                                                                GridLines="None"
                                                                                                CssClass="table table-striped table-hover small overflow-hidden"
                                                                                                AllowPaging="True"
                                                                                                EmptyDataText="No hay registros a visualizar"
                                                                                                PageSize="50">
                                                                                                <PagerStyle
                                                                                                    CssClass="mGrid" />
                                                                                                <SelectedRowStyle
                                                                                                    BackColor="#E2DED6"
                                                                                                    Font-Bold="True"
                                                                                                    ForeColor="#333333" />
                                                                                                <Columns>
                                                                                                    <asp:TemplateField>
                                                                                                        <ItemTemplate>
                                                                                                            <asp:CheckBox
                                                                                                                ID="chkRowDetalleEquipo"
                                                                                                                runat="server"
                                                                                                                Checked='<%# Eval("Seleccionar") %>'
                                                                                                                AutoPostBack="true"
                                                                                                                OnCheckedChanged="chkRowDetalleEquipo_CheckedChanged" />
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:BoundField
                                                                                                        DataField="Item"
                                                                                                        HeaderText="#"
                                                                                                        ControlStyle-Width="25px"
                                                                                                        HeaderStyle-CssClass="p-1 m-1"
                                                                                                        ItemStyle-CssClass="p-1 m-1" />
                                                                                                    <asp:BoundField
                                                                                                        DataField="IdEquipo"
                                                                                                        HeaderText="Código"
                                                                                                        HeaderStyle-Width="40px"
                                                                                                        HeaderStyle-CssClass="p-1 m-1"
                                                                                                        ItemStyle-CssClass="p-1 m-1" />
                                                                                                    <asp:BoundField
                                                                                                        DataField="DescripcionEquipo"
                                                                                                        HeaderText="Descripción"
                                                                                                        HeaderStyle-CssClass="p-1 m-1"
                                                                                                        ItemStyle-CssClass="p-1 m-1" />
                                                                                                </Columns>
                                                                                                <HeaderStyle
                                                                                                    CssClass="thead-dark" />
                                                                                            </asp:GridView>
                                                                                        </div>
                                                                                    </asp:Panel>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:ValidationSummary ID="ValidationSummary3"
                                                                    runat="server" role="alert"
                                                                    CssClass="toast alert-warning alert-dismissible fade show"
                                                                    ValidationGroup="vgrpValidarComponente" />
                                                                <div class="col-md-4 text-end p-2">
                                                                    <asp:Button
                                                                        ID="btnAceptarImprimirProgramacion"
                                                                        runat="server"
                                                                        ValidationGroup="vgrpValidarComponente"
                                                                        ToolTip="Aceptar Registro" TabIndex="72"
                                                                        Text="Imprimir"
                                                                        CssClass="btn btn-outline-facebook btn-sm" />
                                                                    &nbsp;
                                                                            <asp:Button
                                                                                ID="btnCancelarImprimirProgramacion"
                                                                                runat="server"
                                                                                ToolTip="Cancelar Registro"
                                                                                TabIndex="73" Text="Cancelar"
                                                                                CssClass="btn btn-outline-google-plus btn-sm" />
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
                    </div>
                    <div class="card-body">
                        <div class="row bg-100 p-2">
                            <div class="col-lg-6 col-sm-6">
                                <span class="fas fa-filter"></span>
                                <label class="text-black-50 col-form-label fs--2 m-1">Filtrar por:</label>
                                <div class="input-group">
                                    <asp:DropDownList ID="cboFiltroContrato" runat="server"
                                        CssClass="form-select form-select-sm js-choice" TabIndex="1">
                                        <asp:ListItem>SELECCIONE EL FILTRO</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-6">
                                <span class="far fa-address-card"></span>
                                <label class="text-black-50 col-form-label fs--2 m-1">Buscar por:</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtBuscarContrato" runat="server"
                                        CssClass="form-control form-control-sm" TabIndex="2"
                                        Style="text-transform: uppercase" placeholder="Ingrese Busqueda">
                                    </asp:TextBox>
                                    &nbsp;
                                            <asp:ImageButton ID="imgbtnBuscarContrato" runat="server" Height="18px"
                                                TabIndex="3" CssClass="mt-1" ImageUrl="~/Imagenes/buscar.png"
                                                ToolTip="Buscar Registro" Width="21px" />
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

    <asp:UpdatePanel ID="updpnlContenido" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnNuevo" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnEditar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSiMensajeDocumento" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="pnlContenido" runat="server">
                <asp:HiddenField ID="hfdValoresCheckList" runat="Server"></asp:HiddenField>
                <div id="BarraHTML" class="container-fluid">
                    <div class="row">
                        <div class="col-auto align-self-center mt-2 mb-2">
                            <div class="nav nav-pills gap-2" role="tablist">
                                <asp:LinkButton ID="btnAtras" runat="server"
                                    CssClass="btn btn-falcon-default btn-sm ml-auto float-right"
                                    Text="<span class='fas fa-step-backward'></span> Atrás"
                                    ToolTip="Ir hacia atrás" />
                            </div>
                        </div>
                        <div class="col-auto ms-auto">
                            <div class="nav nav-pills gap-2" role="tablist">
                                <asp:LinkButton ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Visible="false"
                                    CssClass="btn btn-success btn-sm ml-auto float-right"
                                    Text="<span class='fas fa-plus'></span> Guardar"
                                    ToolTip="Guardar Registro" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card mb-2" hidden>
                    <div class="card-body">
                        <div class="row mt-0">
                            <div class="col-lg-12 col-sm-12">
                                <div class="row">
                                    <div class="col-lg-3 col-md-4">
                                        <span class="far fa-address-card"></span>
                                        <label class="text-black-50 col-form-label fs--2 m-1">
                                            Código Contrato:</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtIdContrato" runat="server"
                                                CssClass="form-control form-control-sm" TabIndex="10"
                                                placeholder="Id. del equipo"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-5 col-md-4">
                                        <span class="far fa-address-card"></span>
                                        <label class="text-black-50 col-form-label fs--2 m-1">Descripción Contrato:</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtDescripcionContrato" runat="server"
                                                CssClass="form-control form-control-sm" TabIndex="11"
                                                Style="text-transform: uppercase"
                                                placeholder="Descripción del contrato" autocomplete="off">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDescripcionContrato" runat="server"
                                                ControlToValidate="txtDescripcionContrato"
                                                EnableClientScript="False"
                                                ErrorMessage="Ingrese la descripción del contrato" Font-Size="10px"
                                                ForeColor="Red" ValidationGroup="vgrpValidar">(*)
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-4">
                                        <span class="far fa-address-card"></span>
                                        <label
                                            class="text-black-50 col-form-label fs--2 m-1">
                                            Nro.Licitación:</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtNroLicitacionContrato" runat="server"
                                                CssClass="form-control form-control-sm" TabIndex="11"
                                                Style="text-transform: uppercase"
                                                placeholder="Descripción del contrato" autocomplete="off">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                                <div class="row" style="margin-top: 10px;">
                                    <div class="col-lg-4 col-md-4">
                                        <span class="far fa-address-card"></span>
                                        <label class="text-black-50 col-form-label fs--2 m-1">Fecha Emisión:</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtFechaEmisionContrato" runat="server"
                                                CssClass="form-control form-control-sm" Enabled="False"
                                                Style="text-transform: uppercase" TabIndex="5" AutoPostBack="True">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFechaEmisionContrato" runat="server"
                                                ControlToValidate="txtFechaEmisionContrato"
                                                EnableClientScript="False"
                                                ErrorMessage="Ingrese la fecha de emisión" Font-Size="10px"
                                                ForeColor="Red" ValidationGroup="vgrpValidar">(*)
                                            </asp:RequiredFieldValidator>
                                            <ajaxToolkit:CalendarExtender
                                                ID="txtFechaEmisionContrato_CalendarExtender" runat="server"
                                                CssClass="calendario" Enabled="True"
                                                TargetControlID="txtFechaEmisionContrato">
                                            </ajaxToolkit:CalendarExtender>
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-4">
                                        <span class="far fa-address-card"></span>
                                        <label class="text-black-50 col-form-label fs--2 m-1">
                                            Fecha Vigencia
                                                Inicio:</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtFechaVigenciaInicio" runat="server"
                                                CssClass="form-control form-control-sm" Enabled="False"
                                                Style="text-transform: uppercase" TabIndex="5" AutoPostBack="True">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFechaVigenciaInicio" runat="server"
                                                ControlToValidate="txtFechaVigenciaInicio"
                                                EnableClientScript="False"
                                                ErrorMessage="Ingrese la fecha de vigencia inicial" Font-Size="10px"
                                                ForeColor="Red" ValidationGroup="vgrpValidar">(*)
                                            </asp:RequiredFieldValidator>
                                            <ajaxToolkit:CalendarExtender
                                                ID="txtFechaVigenciaInicio_CalendarExtender" runat="server"
                                                CssClass="calendario" Enabled="True"
                                                TargetControlID="txtFechaVigenciaInicio">
                                            </ajaxToolkit:CalendarExtender>
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-4">
                                        <span class="far fa-address-card"></span>
                                        <label class="text-black-50 col-form-label fs--2 m-1">
                                            Fecha Vigencia
                                                Final:</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtFechaVigenciaFinal" runat="server"
                                                CssClass="form-control form-control-sm" Enabled="False"
                                                Style="text-transform: uppercase" TabIndex="5" AutoPostBack="True">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFechaVigenciaFinal" runat="server"
                                                ControlToValidate="txtFechaVigenciaFinal" EnableClientScript="False"
                                                ErrorMessage="Ingrese la fecha de vigencia final" Font-Size="10px"
                                                ForeColor="Red" ValidationGroup="vgrpValidar">(*)
                                            </asp:RequiredFieldValidator>
                                            <ajaxToolkit:CalendarExtender
                                                ID="txtFechaVigenciaFinal_CalendarExtender" runat="server"
                                                CssClass="calendario" Enabled="True"
                                                TargetControlID="txtFechaVigenciaFinal">
                                            </ajaxToolkit:CalendarExtender>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-3 col-md-4 mt-2">
                                <span class="far fa-address-card"></span>
                                <label class="text-black-50 col-form-label fs--2 m-1">Código Cliente:</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtIdCliente" runat="server"
                                        CssClass="form-control form-control-sm" TabIndex="17"></asp:TextBox><%--<
                                            /td>--%>
                                    <asp:LinkButton ID="btnAdicionarCliente" runat="server"
                                        CssClass="btn btn-primary btn-sm"
                                        Text="<span class='fas fa-binoculars'></span>"
                                        ToolTip="Crear Cliente" />
                                    <asp:RequiredFieldValidator ID="rfvIdCliente" runat="server"
                                        ControlToValidate="txtIdCliente" EnableClientScript="False"
                                        ErrorMessage="Ingrese el código del cliente" Font-Size="10px"
                                        ForeColor="Red" ValidationGroup="vgrpValidar">(*)
                                    </asp:RequiredFieldValidator>
                                    <input type="hidden" runat="server" id="hfdIdTipoDocumentoCliente" />
                                    <asp:HiddenField ID="hfdIdAuxiliar" runat="server" />
                                    <asp:HiddenField ID="hfdIdTipoPersonaCliente" runat="server" />
                                    <asp:HiddenField ID="hfdIdTipoCliente" runat="server" />
                                    <asp:HiddenField ID="hfdIdUbicacionGeograficaCliente" runat="server" />
                                    <asp:HiddenField ID="hfdIdUbicacionGeograficaClienteUbicacion"
                                        runat="server" />
                                    <asp:HiddenField ID="hfdNroDocumentoCliente" runat="server" />
                                    <asp:HiddenField ID="hfdDireccionFiscalCliente" runat="server" />
                                    <asp:HiddenField ID="hfdTelefonoContactoCliente" runat="server" />
                                    <asp:LinkButton ID="lnk_mostrarPanelMensajeCliente" runat="server">
                                    </asp:LinkButton>
                                    <ajaxToolkit:ModalPopupExtender
                                        ID="lnk_mostrarPanelMensajeCliente_ModalPopupExtender" runat="server"
                                        BackgroundCssClass="FondoAplicacion"
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
                                                            <h4 class="modal-title"
                                                                id="myModalLabelMensajeCliente">Mensaje
                                                                        Informativo</h4>
                                                            <asp:Button ID="imgbtnCancelarMensajeClienteImagen"
                                                                runat="server"
                                                                CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base"
                                                                data-dismiss="modal" aria-label="Close" />
                                                        </div>
                                                        <div class="modal-body">
                                                            <div class="row pt-2 pb-2">
                                                                <asp:Label ID="lblTituloMensajeCliente"
                                                                    runat="server"
                                                                    CssClass="col-sm-12 col-form-label-sm text-center">
                                                                            Se creó el siguiente número de cliente
                                                                </asp:Label>
                                                                <div class="col-sm-12 text-center">
                                                                    <h1>
                                                                        <asp:Label
                                                                            ID="lblNroClienteMensajeCliente"
                                                                            runat="server" Text="00000"
                                                                            CssClass="col-form-label-sm h4">
                                                                        </asp:Label>
                                                                    </h1>
                                                                </div>
                                                                <div class="col-sm-12 text-center">
                                                                    <asp:Label
                                                                        ID="lblDatoInformativoMensajeCliente"
                                                                        runat="server" Text=""
                                                                        CssClass="texto_estado short_estado light-orange borde_redondo1">
                                                                    </asp:Label>
                                                                </div>
                                                                <asp:UpdatePanel ID="UpdatePanel9"
                                                                    runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="row pt-1">
                                                                            <label
                                                                                class="col-md-4 text-black-50 col-form-label col-form-label-sm">
                                                                                País:</label>
                                                                            <div class="col-md-8">
                                                                                <div class="input-group">
                                                                                    <asp:DropDownList
                                                                                        ID="cboPaisMensajeCliente"
                                                                                        runat="server"
                                                                                        AppendDataBoundItems="True"
                                                                                        TabIndex="100"
                                                                                        CssClass="form-select form-select-sm js-choice"
                                                                                        Style="text-transform: uppercase"
                                                                                        AutoPostBack="True">
                                                                                        <asp:ListItem>SELECCIONE EL PAIS</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator
                                                                                        ID="rfvPaisMensajeCliente"
                                                                                        runat="server"
                                                                                        ControlToValidate="cboPaisMensajeCliente"
                                                                                        EnableClientScript="False"
                                                                                        ErrorMessage="Ingrese el País"
                                                                                        Font-Size="10px"
                                                                                        ForeColor="Red"
                                                                                        ValidationGroup="vgrpValidarCliente">
                                                                                                (*)
                                                                                    </asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row pt-1">
                                                                            <label
                                                                                class="col-md-4 text-black-50 col-form-label col-form-label-sm">
                                                                                Departamento:</label>
                                                                            <div class="col-md-8">
                                                                                <div class="input-group">
                                                                                    <asp:DropDownList
                                                                                        ID="cboDepartamentoMensajeCliente"
                                                                                        runat="server"
                                                                                        AppendDataBoundItems="True"
                                                                                        CssClass="form-select form-select-sm js-choice"
                                                                                        Style="text-transform: uppercase"
                                                                                        TabIndex="101"
                                                                                        AutoPostBack="True">
                                                                                        <asp:ListItem>SELECCIONE
                                                                                                    EL DEPARTAMENTO
                                                                                        </asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator
                                                                                        ID="rfvDepartamentoMensajeCliente"
                                                                                        runat="server"
                                                                                        ControlToValidate="cboDepartamentoMensajeCliente"
                                                                                        EnableClientScript="False"
                                                                                        ErrorMessage="Ingrese el Departamento"
                                                                                        Font-Size="10px"
                                                                                        ForeColor="Red"
                                                                                        ValidationGroup="vgrpValidarCliente">
                                                                                                (*)
                                                                                    </asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row pt-1">
                                                                            <label
                                                                                class="col-md-4 text-black-50 col-form-label col-form-label-sm">
                                                                                Provincia:</label>
                                                                            <div class="col-md-8">
                                                                                <div class="input-group">
                                                                                    <asp:DropDownList
                                                                                        ID="cboProvinciaMensajeCliente"
                                                                                        runat="server"
                                                                                        AppendDataBoundItems="True"
                                                                                        TabIndex="102"
                                                                                        CssClass="form-select form-select-sm js-choice"
                                                                                        Style="text-transform: uppercase"
                                                                                        AutoPostBack="True">
                                                                                        <asp:ListItem>SELECCIONE
                                                                                                    LA PROVINCIA
                                                                                        </asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator
                                                                                        ID="rfvProvinciaMensajeCliente"
                                                                                        runat="server"
                                                                                        ControlToValidate="cboProvinciaMensajeCliente"
                                                                                        EnableClientScript="False"
                                                                                        ErrorMessage="Ingrese la Provincia"
                                                                                        Font-Size="10px"
                                                                                        ForeColor="Red"
                                                                                        ValidationGroup="vgrpValidarCliente">
                                                                                                (*)
                                                                                    </asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row pt-1">
                                                                            <label
                                                                                class="col-md-4 text-black-50 col-form-label col-form-label-sm">
                                                                                Distrito:</label>
                                                                            <div class="col-md-8">
                                                                                <div class="input-group">
                                                                                    <asp:DropDownList
                                                                                        ID="cboDistritoMensajeCliente"
                                                                                        runat="server"
                                                                                        AppendDataBoundItems="True"
                                                                                        CssClass="form-select form-select-sm js-choice"
                                                                                        Style="text-transform: uppercase"
                                                                                        TabIndex="103"
                                                                                        AutoPostBack="False">
                                                                                        <asp:ListItem>SELECCIONE
                                                                                                    EL DISTRITO
                                                                                        </asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator
                                                                                        ID="rfvDistritoMensajeCliente"
                                                                                        runat="server"
                                                                                        ControlToValidate="cboDistritoMensajeCliente"
                                                                                        EnableClientScript="False"
                                                                                        ErrorMessage="Ingrese el Distrito"
                                                                                        Font-Size="10px"
                                                                                        ForeColor="Red"
                                                                                        ValidationGroup="vgrpValidarCliente">
                                                                                                (*)
                                                                                    </asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>

                                                                <div class="row pt-1">
                                                                    <div class="col-sm-12">
                                                                        <asp:Label
                                                                            ID="lblDireccionAdicionalMensajeCliente"
                                                                            runat="server" Text="Dirección:"
                                                                            CssClass="text-black-50 col-form-label col-form-label-sm">
                                                                        </asp:Label>
                                                                        <asp:TextBox
                                                                            ID="txtDireccionAdicionalMensajeCliente"
                                                                            runat="server"
                                                                            CssClass="form-control form-control-sm"
                                                                            Style="text-transform: uppercase;"
                                                                            TabIndex="104"></asp:TextBox>
                                                                    </div>
                                                                    <div class="col-sm-12">
                                                                        <asp:Label
                                                                            ID="lblCorreoAdicionalMensajeCliente"
                                                                            runat="server" Text="Correo:"
                                                                            CssClass="text-black-50 col-form-label col-form-label-sm">
                                                                        </asp:Label>
                                                                        <asp:TextBox
                                                                            ID="txtCorreoAdicionalMensajeCliente"
                                                                            runat="server"
                                                                            CssClass="form-control form-control-sm"
                                                                            Style="text-transform: uppercase;"
                                                                            TabIndex="105" TextMode="Email">
                                                                        </asp:TextBox>
                                                                    </div>
                                                                    <div class="col-sm-12">
                                                                        <asp:Label
                                                                            ID="lblTelefonoMensajeCliente"
                                                                            runat="server" Text="Teléfono:"
                                                                            CssClass="text-black-50 col-form-label col-form-label-sm">
                                                                        </asp:Label>
                                                                        <asp:TextBox
                                                                            ID="txtTelefonoMensajeCliente"
                                                                            runat="server"
                                                                            CssClass="form-control form-control-sm"
                                                                            Style="text-transform: uppercase;"
                                                                            TabIndex="106"></asp:TextBox>
                                                                    </div>
                                                                    <div class="col-md-12">
                                                                        <asp:Label ID="lblMensajeMensajeCliente"
                                                                            runat="server"
                                                                            Style="text-align: center;"
                                                                            CssClass="col-form-label col-form-label-sm text-danger">
                                                                        </asp:Label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="modal-footer">
                                                            <asp:Button ID="btnAceptarMensajeClienteImagen"
                                                                runat="server" Text="Aceptar"
                                                                CssClass="btn btn-primary btn-sm me-0 mb-0"
                                                                ToolTip="Aceptar Información"
                                                                ValidationGroup="vgrpValidarCliente" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <asp:LinkButton ID="lnk_mostrarPanelCliente" runat="server">
                                    </asp:LinkButton>
                                    <ajaxToolkit:ModalPopupExtender
                                        ID="lnk_mostrarPanelCliente_ModalPopupExtender" runat="server"
                                        BackgroundCssClass="FondoAplicacion" DropShadow="False"
                                        DynamicServicePath="" Enabled="True"
                                        CancelControlID="btnCancelarCliente"
                                        PopupControlID="pnlSeleccionarCliente"
                                        TargetControlID="lnk_mostrarPanelCliente">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="pnlSeleccionarCliente" runat="server" CssClass="container"
                                        DefaultButton="imgbtnBuscarCliente">
                                        <div class="modal-dialog modal-xl">
                                            <div class="shadow rounded">
                                                <div class="modal-dialog-scrollable">
                                                    <div class="modal-content">
                                                        <div class="modal-header bg-light">
                                                            <h4 class="modal-title" id="myModalLabelCliente">Seleccionar Cliente</h4>
                                                            <asp:Button ID="imgbtnCancelarClienteImagen"
                                                                runat="server"
                                                                CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base"
                                                                data-dismiss="modal" aria-label="Close" />
                                                        </div>
                                                        <div class="modal-body">
                                                            <div class="row pt-2 pb-2">
                                                                <span class="col-sm-2 col-form-label-sm">Buscar
                                                                            por</span>
                                                                <div class="col-sm-4">
                                                                    <asp:DropDownList ID="cboFiltroCliente"
                                                                        runat="server"
                                                                        CssClass="form-select form-select-sm js-choice"
                                                                        TabIndex="74">
                                                                        <asp:ListItem
                                                                            Value="vRazonSocialCliente">RAZ.SOC.
                                                                        </asp:ListItem>
                                                                        <asp:ListItem Value="cIdCliente">CODIGO
                                                                        </asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div class="col-sm-6">
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtBuscarCliente"
                                                                            runat="server"
                                                                            CssClass="form-control form-control-sm"
                                                                            TabIndex="61"
                                                                            Style="text-transform: uppercase"
                                                                            placeholder="Ingrese Busqueda"
                                                                            onfocus="fnSetFocus('ctl00$cuerpo$txtBuscarCliente')">
                                                                        </asp:TextBox>
                                                                        &nbsp;
                                                                                <asp:ImageButton
                                                                                    ID="imgbtnBuscarCliente"
                                                                                    runat="server" Height="18px"
                                                                                    TabIndex="3" CssClass="mt-1"
                                                                                    ImageUrl="~/Imagenes/buscar.png"
                                                                                    ToolTip="Buscar Registro"
                                                                                    Width="21px"
                                                                                    onfocus="fnSetFocus('ctl00$cuerpo$txtBuscarCliente')" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-12 mt-3">
                                                                    <div style="overflow: auto">
                                                                        <asp:GridView ID="grdListaCliente"
                                                                            runat="server"
                                                                            AutoGenerateColumns="False"
                                                                            TabIndex="63" GridLines="None"
                                                                            CssClass="table table-responsive-sm table-striped table-hover small"
                                                                            AllowPaging="True"
                                                                            EmptyDataText="No hay registros a visualizar"
                                                                            PageSize="6">
                                                                            <PagerStyle CssClass="pgr" />
                                                                            <SelectedRowStyle
                                                                                BackColor="#E2DED6"
                                                                                Font-Bold="True"
                                                                                ForeColor="#333333" />
                                                                            <AlternatingRowStyle
                                                                                CssClass="alt" />
                                                                            <Columns>
                                                                                <asp:CommandField
                                                                                    ShowSelectButton="True"
                                                                                    HeaderStyle-CssClass="bg-200 text-900" />
                                                                                <asp:BoundField
                                                                                    DataField="Codigo"
                                                                                    HeaderText="Código"
                                                                                    HeaderStyle-CssClass="bg-200 text-900" />
                                                                                <asp:BoundField DataField="DNI"
                                                                                    HeaderText="DNI"
                                                                                    HeaderStyle-CssClass="bg-200 text-900" />
                                                                                <asp:BoundField DataField="RUC"
                                                                                    HeaderText="RUC"
                                                                                    HeaderStyle-CssClass="bg-200 text-900" />
                                                                                <asp:BoundField
                                                                                    DataField="Descripcion"
                                                                                    HeaderText="Razón Social"
                                                                                    HeaderStyle-CssClass="bg-200 text-900" />
                                                                                <asp:BoundField
                                                                                    DataField="Estado"
                                                                                    HeaderText="Estado"
                                                                                    HeaderStyle-CssClass="bg-200 text-900" />
                                                                                <asp:BoundField
                                                                                    DataField="Direccion"
                                                                                    HeaderText="Dirección"
                                                                                    HeaderStyle-CssClass="bg-200 text-900" />
                                                                            </Columns>
                                                                            <HeaderStyle
                                                                                CssClass="thead-dark" />
                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="modal-footer">
                                                            <asp:Label ID="lblMensajeCliente" runat="server"
                                                                CssClass="col-form-label col-form-label-sm text-danger">
                                                            </asp:Label>
                                                            <asp:Button ID="btnAceptarCliente" runat="server"
                                                                ValidationGroup="vgrpValidarBusqueda"
                                                                ToolTip="Aceptar Registro" TabIndex="72"
                                                                Text="Aceptar"
                                                                CssClass="btn btn-outline-facebook btn-sm d-block" />
                                                            &nbsp;
                                                                    <asp:Button ID="btnCancelarCliente" runat="server"
                                                                        ToolTip="Cancelar Registro" TabIndex="73"
                                                                        Text="Cancelar"
                                                                        CssClass="btn btn-outline-google-plus btn-sm d-block" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                            <div class="col-lg-5 col-md-6 mt-2">
                                <span class="far fa-address-card"></span>
                                <label class="text-black-50 col-form-label fs--2 m-1">Razón Social (Cliente):</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtRazonSocial" runat="server"
                                        CssClass="form-control form-control-sm" Enabled="False" TabIndex="18"
                                        Style="text-transform: uppercase" placeholder="Razón Social"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvRazonSocial" runat="server"
                                        ControlToValidate="txtRazonSocial" EnableClientScript="False"
                                        ErrorMessage="Ingrese la razón social" Font-Size="10px" ForeColor="Red"
                                        ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="card">
                    <div class="card-header bg-light">
                        <div class="row align-items-center">
                            <div class="row">
                                <div class="col-lg-5 col-md-4 mt-2">
                                    <span class="fas fa-calendar"></span>
                                    <label class="text-black-50 col-form-label fs--2 m-1">Periodo:</label>
                                    <div class="input-group">
                                        <asp:DropDownList ID="cboPeriodo" runat="server"
                                            AppendDataBoundItems="True" AutoPostBack="True"
                                            CssClass="form-select form-select-sm js-choice"
                                            Style="text-transform: uppercase" Width="170px" TabIndex="2">
                                            <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-lg-5 col-md-4 mt-2">
                                    <span class="fas fa-calendar-week"></span>
                                    <label class="text-black-50 col-form-label fs--2 m-1">Mes:</label>

                                    <div class="input-group">
                                        <asp:DropDownList ID="cboMes" runat="server"  AppendDataBoundItems="True" AutoPostBack="True"
                                            CssClass="form-select form-select-sm js-choice"
                                            Style="text-transform: uppercase" Width="50px" TabIndex="3">
                                            <%--<asp:ListItem Value="1">01 - ENERO</asp:ListItem>
                                            <asp:ListItem Value="2">02 - FEBRERO</asp:ListItem>
                                            <asp:ListItem Value="3">03 - MARZO</asp:ListItem>
                                            <asp:ListItem Value="4">04 - ABRIL</asp:ListItem>
                                            <asp:ListItem Value="5">05 - MAYO</asp:ListItem>
                                            <asp:ListItem Value="6">06 - JUNIO</asp:ListItem>
                                            <asp:ListItem Value="7">07 - JULIO</asp:ListItem>
                                            <asp:ListItem Value="8">08 - AGOSTO</asp:ListItem>
                                            <asp:ListItem Value="9">09 - SEPTIEMBRE</asp:ListItem>
                                            <asp:ListItem Value="10">10 - OCTUBRE</asp:ListItem>
                                            <asp:ListItem Value="11">11 - NOVIEMBRE</asp:ListItem>
                                            <asp:ListItem Value="12">12 - DICIEMBRE</asp:ListItem>--%>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-lg-2 col-md-4 mt-2">
                                    <div class="dropdown font-sans-serif btn-reveal-trigger">
                                        <span class="fas fa-cog"></span>
                                        <label class="text-black-50 col-form-label fs--2 m-1">Opciones:</label>
                                        <button class="btn btn-sm btn-reveal py-0 px-2" type="button"
                                            id="OpcionesComponentesCatalogo" data-bs-toggle="dropdown"
                                            aria-haspopup="true" aria-expanded="false">
                                            <span
                                                class="fas fa-ellipsis-h"></span>
                                        </button>
                                        <div class="dropdown-menu dropdown-menu-end py-0"
                                            aria-labelledby="OpcionesComponentesCatalogo">
                                            <asp:LinkButton ID="lnkbtnAgregarEquipo" runat="server"
                                                ToolTip="Agregar equipo al contrato"
                                                Text="<span class='fas fa-file-signature me-2'></span>Agregar Equipo"
                                                CssClass="nav-link nav-link-card-details" />
                                        </div>
                                        <asp:LinkButton ID="lnk_mostrarPanelSeleccionarEquipo" runat="server">
                                        </asp:LinkButton>
                                        <ajaxToolkit:ModalPopupExtender
                                            ID="lnk_mostrarPanelSeleccionarEquipo_ModalPopupExtender" runat="server"
                                            BackgroundCssClass="FondoAplicacion" DropShadow="False"
                                            DynamicServicePath="" Enabled="True"
                                            CancelControlID="btnCancelarSeleccionarEquipo"
                                            PopupControlID="pnlSeleccionarEquipo"
                                            TargetControlID="lnk_mostrarPanelSeleccionarEquipo">
                                        </ajaxToolkit:ModalPopupExtender>
                                        <asp:Panel ID="pnlSeleccionarEquipo" runat="server" CssClass="container"
                                            DefaultButton="imgbtnBuscarSeleccionarEquipo">
                                            <div class="modal-dialog modal-xl">
                                                <div class="shadow rounded">
                                                    <div class="modal-dialog-scrollable">
                                                        <div class="modal-content">
                                                            <div class="modal-header bg-light">
                                                                <h4 class="modal-title"
                                                                    id="myModalLabelSeleccionarEquipo">Seleccionar
                                                                        Equipo</h4>
                                                                <asp:Button
                                                                    ID="imgbtnCancelarSeleccionarEquipoImagen"
                                                                    runat="server"
                                                                    CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base"
                                                                    data-dismiss="modal" aria-label="Close" />
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row pt-2 pb-2">
                                                                    <span class="col-sm-2 col-form-label-sm">Buscar
                                                                            por</span>
                                                                    <div class="col-sm-4">
                                                                        <asp:DropDownList
                                                                            ID="cboFiltroSeleccionarEquipo"
                                                                            runat="server"
                                                                            CssClass="form-select form-select-sm js-choice"
                                                                            TabIndex="74">
                                                                            <asp:ListItem
                                                                                Value="vDescripcionEquipo">NOMBRE
                                                                                    EQUIPO</asp:ListItem>
                                                                            <asp:ListItem Value="cIdEquipo">CODIGO
                                                                            </asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="col-sm-6">
                                                                        <div class="input-group">
                                                                            <asp:TextBox
                                                                                ID="txtBuscarSeleccionarEquipo"
                                                                                runat="server"
                                                                                CssClass="form-control form-control-sm"
                                                                                TabIndex="61"
                                                                                Style="text-transform: uppercase"
                                                                                placeholder="Ingrese Busqueda"
                                                                                onfocus="fnSetFocus('ctl00$cuerpo$txtBuscarSeleccionarEquipo')">
                                                                            </asp:TextBox>
                                                                            &nbsp;
                                                                                <asp:ImageButton
                                                                                    ID="imgbtnBuscarSeleccionarEquipo"
                                                                                    runat="server" Height="18px"
                                                                                    TabIndex="3" CssClass="mt-1"
                                                                                    ImageUrl="~/Imagenes/buscar.png"
                                                                                    ToolTip="Buscar Registro"
                                                                                    Width="21px"
                                                                                    onfocus="fnSetFocus('ctl00$cuerpo$txtBuscarSeleccionarEquipo')" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-12 mt-3">
                                                                        <div style="overflow: auto">
                                                                            <asp:GridView
                                                                                ID="grdListaSeleccionarEquipo"
                                                                                runat="server"
                                                                                AutoGenerateColumns="False"
                                                                                TabIndex="63" GridLines="None"
                                                                                CssClass="table table-responsive-sm table-striped table-hover small"
                                                                                AllowPaging="True"
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
                                                                                    <asp:CommandField
                                                                                        ShowSelectButton="True"
                                                                                        HeaderStyle-CssClass="bg-200 text-900" />
                                                                                    <asp:BoundField
                                                                                        DataField="Codigo"
                                                                                        HeaderText="Código"
                                                                                        HeaderStyle-CssClass="bg-200 text-900" />
                                                                                   
                                                                                    <asp:BoundField
                                                                                        DataField="IdTipoActivo"
                                                                                        HeaderText="Id.Tipo Activo"
                                                                                        HeaderStyle-CssClass="bg-200 text-900" />
                                                                                    <asp:BoundField
                                                                                        DataField="IdCatalogo"
                                                                                        HeaderText="Id.Catálogo"
                                                                                        HeaderStyle-CssClass="bg-200 text-900" />
                                                                                    <asp:BoundField
                                                                                        DataField="Descripcion"
                                                                                        HeaderText="Equipo"
                                                                                        HeaderStyle-CssClass="bg-200 text-900" />
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
                                                                <asp:Button ID="btnAceptarSeleccionarEquipo"
                                                                    runat="server"
                                                                    ValidationGroup="vgrpValidarBusqueda"
                                                                    ToolTip="Aceptar Registro" TabIndex="72"
                                                                    Text="Aceptar"
                                                                    CssClass="btn btn-outline-facebook btn-sm d-block" />
                                                                &nbsp;
                                                                    <asp:Button ID="btnCancelarSeleccionarEquipo"
                                                                        runat="server" ToolTip="Cancelar Registro"
                                                                        TabIndex="73" Text="Cancelar"
                                                                        CssClass="btn btn-outline-google-plus btn-sm d-block" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>

                            </div>

                        </div>
                    </div>
                    <asp:LinkButton ID="lnk_mostrarPanelMantenimientoOrdenTrabajo" runat="server"></asp:LinkButton>
                    <ajaxToolkit:ModalPopupExtender
                        ID="lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender" runat="server"
                        BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True"
                        CancelControlID="btnCancelarMantenimientoOrdenTrabajo"
                        PopupControlID="pnlMantenimientoOrdenTrabajo"
                        TargetControlID="lnk_mostrarPanelMantenimientoOrdenTrabajo">
                    </ajaxToolkit:ModalPopupExtender>
                    <asp:Panel ID="pnlMantenimientoOrdenTrabajo" runat="server" CssClass="container"
                        DefaultButton="btnAceptarMantenimientoOrdenTrabajo">
                        <div class="modal-dialog modal-xl">
                            <div class="shadow rounded">
                                <div class="modal-dialog-scrollable">
                                    <div class="modal-content">
                                        <div class="modal-header bg-light">
                                            <h4 class="modal-title" id="myModalLabelOrdenTrabajo">Mantenimiento Orden de Trabajo <asp:Label ID="lblTitleOT" runat="server"></asp:Label>
                                            </h4>
                                            <asp:Button ID="imgbtnCancelarOrdenTrabajoImagen" runat="server"
                                                CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base"
                                                data-dismiss="modal" aria-label="Close" />
                                        </div>
                                        <div class="modal-body">
                                            <div class="row">
                                                <div class="scrollbar" style="height: 380px;">
                                                    <div class="row">
                                                        <div class="col-lg-2 col-md-5 col-sm-6 mt-2">
                                                            <span class="far fa-address-card"></span>
                                                            <label
                                                                class="text-black-50 col-form-label fs--2 m-1">
                                                                Fecha Inicio</label>
                                                            <div class="input-group">
                                                                <asp:TextBox
                                                                    ID="txtFechaInicioPlanificadaMantenimientoOrdenTrabajo"
                                                                    runat="server"
                                                                    CssClass="form-control form-control-sm"
                                                                    AutoPostBack="true"
                                                                    Style="text-transform: uppercase" TabIndex="18"
                                                                    autocomplete="off"></asp:TextBox>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator6" runat="server"
                                                                    ControlToValidate="txtFechaInicioPlanificadaMantenimientoOrdenTrabajo"
                                                                    EnableClientScript="False"
                                                                    ErrorMessage="Ingrese la fecha de inicio de planificación"
                                                                    Font-Size="10px" ForeColor="Red"
                                                                    ValidationGroup="vgrpValidarCrearProducto">(*)
                                                                </asp:RequiredFieldValidator>
                                                                <ajaxToolkit:CalendarExtender
                                                                    ID="txtFechaInicioPlanificadaMantenimientoOrdenTrabajo_CalendarExtender"
                                                                    runat="server" CssClass="calendario"
                                                                    Enabled="True"
                                                                    TargetControlID="txtFechaInicioPlanificadaMantenimientoOrdenTrabajo">
                                                                </ajaxToolkit:CalendarExtender>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-2 col-md-5 col-sm-6 mt-2">
                                                            <span class="far fa-address-card"></span>
                                                            <label
                                                                class="text-black-50 col-form-label fs--2 m-1">
                                                                Fecha
                                                                    Termino</label>
                                                            <div class="input-group">
                                                                <asp:TextBox
                                                                    ID="txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo"
                                                                    runat="server"
                                                                    CssClass="form-control form-control-sm"
                                                                    AutoPostBack="true"
                                                                    Style="text-transform: uppercase" TabIndex="18"
                                                                    autocomplete="off"></asp:TextBox>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator1" runat="server"
                                                                    ControlToValidate="txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo"
                                                                    EnableClientScript="False"
                                                                    ErrorMessage="Ingrese la fecha de inicio de planificación"
                                                                    Font-Size="10px" ForeColor="Red"
                                                                    ValidationGroup="vgrpValidarCrearProducto">(*)
                                                                </asp:RequiredFieldValidator>
                                                                <ajaxToolkit:CalendarExtender
                                                                    ID="txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo_CalendarExtender"
                                                                    runat="server" CssClass="calendario"
                                                                    Enabled="True"
                                                                    TargetControlID="txtFechaTerminoPlanificadaMantenimientoOrdenTrabajo">
                                                                </ajaxToolkit:CalendarExtender>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-4 col-md-7 col-sm-6 mt-2">
                                                            <span class="far fa-address-card"></span>
                                                            <label
                                                                class="text-black-50 col-form-label fs--2 m-1">
                                                                Tipo
                                                                    de Mantenimiento</label>
                                                            <div class="input-group">
                                                                <asp:DropDownList
                                                                    ID="cboTipoMantenimientoMantenimientoOrdenTrabajo"
                                                                    runat="server" AutoPostBack="True"
                                                                    class="form-select form-select-sm js-choice"
                                                                    Style="text-transform: uppercase" TabIndex="11">
                                                                    <asp:ListItem Value="SELECCIONE DATO">SELECCIONE
                                                                            DATO</asp:ListItem>
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
                                                        <div class="col-lg-4 col-md-7 col-sm-6 mt-2">
                                                            <span class="far fa-address-card"></span>
                                                            <label
                                                                class="text-black-50 col-form-label fs--2 m-1">
                                                                Plantilla
                                                                    CheckList</label>
                                                            <div class="input-group">
                                                                <asp:DropDownList
                                                                    ID="cboListadoCheckListMantenimientoOrdenTrabajo"
                                                                    runat="server" AutoPostBack="True"
                                                                    class="form-select form-select-sm js-choice"
                                                                    Style="text-transform: uppercase" TabIndex="12">
                                                                    <asp:ListItem Value="SELECCIONE DATO">SELECCIONE
                                                                            DATO</asp:ListItem>
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
                                                        <div class="col-lg-4 col-md-12 col-sm-6 mt-2 mb-2">
                                                            <span class="far fa-address-card"></span>
                                                            <label
                                                                class="text-black-50 col-form-label fs--2 m-1">
                                                                Responsable</label>
                                                            <div class="input-group">
                                                                <asp:DropDownList
                                                                    ID="cboPersonalResponsableMantenimientoOrdenTrabajo"
                                                                    runat="server" AppendDataBoundItems="True"
                                                                    AutoPostBack="True"
                                                                    class="form-select form-select-sm js-choice"
                                                                    Style="text-transform: uppercase" TabIndex="13">
                                                                    <asp:ListItem Value="SELECCIONE DATO">SELECCIONE
                                                                            DATO</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator
                                                                    ID="rfvPersonalResponsableMantenimientoOrdenTrabajo"
                                                                    runat="server"
                                                                    ControlToValidate="cboPersonalResponsableMantenimientoOrdenTrabajo"
                                                                    EnableClientScript="False"
                                                                    ErrorMessage="Ingrese el personal responsable"
                                                                    Font-Size="10px" ForeColor="Red"
                                                                    ValidationGroup="vgrpValidarMantenimientoOrdenTrabajo">
                                                                        (*)</asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-4 col-md-12 col-sm-6 mt-2 mb-2">
                                                            <span class="far fa-address-card"></span>
                                                            <label class="text-black-50 col-form-label fs--2 m-1">Control de Tiempo</label>
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
                                                        <div class="col-lg-4 col-md-12 col-sm-6 mt-2 mb-2">
                                                            <span class="far fa-address-card"></span>
                                                            <label class="text-black-50 col-form-label fs--2 m-1">Frecuencia (días)</label>

                                                            <div class="row g-2">
                                                                <!-- Textbox más pequeño -->
                                                                <div class="col-4">
                                                                    <asp:TextBox
                                                                        ID="txtPeriodicidadDiasMantenimientoOrdenTrabajo"
                                                                        runat="server"
                                                                        CssClass="form-control form-control-sm"
                                                                        Style="text-transform: uppercase"
                                                                        TabIndex="18"
                                                                        autocomplete="off"></asp:TextBox>
                                                                </div>

                                                                <!-- Checkbox ocupa el resto -->
                                                                <div class="col d-flex align-items-center">
                                                                    <div class="form-check mb-0">
                                                                        <input ID="chkReajustarProgramacion" type="checkbox" runat="server" CssClass="form-check-input" TabIndex="20" />
                                                                        <asp:Label ID="lblReajustarProgramacion" runat="server"
                                                                            AssociatedControlID="chkReajustarProgramacion"
                                                                            CssClass="form-check-label text-black-50 col-form-label-sm ms-1 fs--2"
                                                                            Text="Reajustar programación"></asp:Label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row bg-200">
                                                        <div class="col-lg-12 col-md-12 col-sm-12 mt-2">
                                                            <span class="far fa-address-card"></span>
                                                            <label
                                                                class="text-black-50 col-form-label fs--2 m-1">
                                                                Personal
                                                                    Asignado</label>
                                                            <div class="input-group">
                                                                <asp:DropDownList
                                                                    ID="cboPersonalAsignadoMantenimientoOrdenTrabajo"
                                                                    runat="server" AppendDataBoundItems="True"
                                                                    AutoPostBack="True"
                                                                    class="form-select form-select-sm js-choice"
                                                                    Style="text-transform: uppercase" TabIndex="14">
                                                                    <asp:ListItem Value="SELECCIONE DATO">SELECCIONE
                                                                            DATO</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:Button
                                                                    ID="btnAdicionarPersonalAsignadoMantenimientoOrdenTrabajo"
                                                                    runat="server" Text="Agregar"
                                                                    CssClass="btn btn-primary btn-sm"
                                                                    ToolTip="Adicionar Personal" TabIndex="30" />
                                                                <asp:RequiredFieldValidator
                                                                    ID="rfvPersonalAsignadoMantenimientoOrdenTrabajo"
                                                                    runat="server"
                                                                    ControlToValidate="cboPersonalAsignadoMantenimientoOrdenTrabajo"
                                                                    EnableClientScript="False"
                                                                    ErrorMessage="Ingrese el personal asignado"
                                                                    Font-Size="10px" ForeColor="Red"
                                                                    ValidationGroup="vgrpValidarMantenimientoOrdenTrabajo">
                                                                        (*)</asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 mt-2">
                                                            <div id="divGridPersonalAsignado"
                                                                style="overflow: auto; height: 220px; align: left;">
                                                                <asp:GridView
                                                                    ID="grdPersonalAsignadoMantenimientoOrdenTrabajo"
                                                                    runat="server" AutoGenerateColumns="False"
                                                                    TabIndex="17" GridLines="None"
                                                                    CssClass="table table-responsive-sm table-striped table-hover small"
                                                                    AllowPaging="True"
                                                                    EmptyDataText="No hay registros a visualizar"
                                                                    PageSize="1000">
                                                                    <PagerStyle CssClass="pgr" />
                                                                    <SelectedRowStyle BackColor="#E2DED6"
                                                                        Font-Bold="True" ForeColor="#333333" />
                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                    <Columns>
                                                                        <asp:CommandField ShowDeleteButton="True"
                                                                            HeaderStyle-Width="70px" />
                                                                        <asp:BoundField DataField="Codigo"
                                                                            HeaderText="Código"
                                                                            HeaderStyle-CssClass="bg-200 text-900" />
                                                                        <asp:BoundField DataField="Personal"
                                                                            HeaderText="Descripción"
                                                                            HeaderStyle-CssClass="bg-200 text-900" />
                                                                        <asp:BoundField DataField="Responsable"
                                                                            HeaderText="Responsable"
                                                                            HeaderStyle-CssClass="bg-200 text-900" />
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
                                            <asp:Label ID="lblMensajeMantenimientoOrdenTrabajo" runat="server"
                                                CssClass="col-form-label col-form-label-sm text-danger"></asp:Label>

                                            <asp:Button ID="btnAceptarMantenimientoOrdenTrabajo" runat="server"
                                                ValidationGroup="vgrpValidarMantenimientoOrdenTrabajo"
                                                ToolTip="Aceptar Registro" TabIndex="72" Text="Aceptar"
                                                CssClass="btn btn-outline-facebook btn-sm" />
                                            &nbsp;
                                                <asp:Button ID="btnCancelarMantenimientoOrdenTrabajo" runat="server"
                                                    ToolTip="Cancelar Registro" TabIndex="73" Text="Cancelar"
                                                    CssClass="btn btn-outline-google-plus btn-sm" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="card-body h-100">
                        <div class="row justify-content-between mt-3">
                            <div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
                                <asp:Panel ID="pnlDetalleEquipos" runat="server">
                                    <div class="dual-table-container">
                                        <div class="fixed-columns-container" id="divFixed">
                                            <asp:GridView ID="grdDetalleEquiposFixed" runat="server"
                                                AutoGenerateColumns="False" TabIndex="10" GridLines="None"
                                                CssClass="split-table"
                                                OnRowCommand="grdDetalleEquiposFixed_RowCommand_Botones"
                                                EmptyDataText="No hay registros a visualizar">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Mant."
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="imgbtnGenerarOrdenTrabajo"
                                                                runat="server" CommandName="GenerarOT"
                                                                CssClass="btn btn-primary btn-sm"
                                                                CommandArgument='<%#  Eval("D1") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>'
                                                                Text="<span class='fas fa-calendar-day'></span>" />
                                                            <asp:LinkButton ID="imgbtnEliminarOrdenTrabajo"
                                                                runat="server" CommandName="EliminarEquipo"
                                                                CssClass="btn btn-danger btn-sm"
                                                                CommandArgument='<%#  Eval("D1") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>'
                                                                Text="<span class='fas fa-calendar-times'></span>" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Código" ShowHeader="false"
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small sort">
                                                        <ItemTemplate>
                                                            <div class="col-12">
                                                                <asp:LinkButton ID="lnkbtnVerEquipo" runat="server" CommandName="VerEquipo" ToolTip="Ver Equipo" CommandArgument='<%# Eval("IdEquipo") %>' Text='<%# Eval("IdEquipo") %>' CssClass="p-1 m-0" ValidationGroup="vgrpValidar" />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="DescripcionEquipo"
                                                        HeaderText="Descripción"
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" />
                                                </Columns>
                                                <HeaderStyle CssClass="thead-dark" />
                                            </asp:GridView>
                                        </div>
                                        <div class="scrollable-columns-container" id="divScrollable">
                                            <asp:GridView ID="grdDetalleEquiposScrollable" runat="server"
                                                AutoGenerateColumns="False" TabIndex="10" GridLines="None"
                                                CssClass="split-table"
                                                EmptyDataText="No hay registros a visualizar"
                                                OnRowCommand="grdDetalleEquiposFixed_RowCommand_Botones">
                                                <PagerStyle CssClass="mGrid" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True"
                                                    ForeColor="#333333" />
                                                <Columns>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD1" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D1") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D1").ToString()),
                                                                                            "",
                                                                                            (Function(s)
                                                                                                 Dim p = s.ToString().Split("|"c)
                                                                                                 Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                                 Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                                 Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                             End Function)(Eval("D1"))
                                                                                        ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D1") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD1" runat="server" 
                                                                Visible='<%# (Eval("D1") IsNot Nothing AndAlso Eval("D1").ToString().Split("|"c).Length > 6 AndAlso Eval("D1").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D1") IsNot Nothing AndAlso Eval("D1").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D1").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D1").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D1") IsNot Nothing AndAlso Eval("D1").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D1").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D1").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD2" runat="server" CssClass="grid-linkbutton" 
                                                                Text='<%# If(Eval("D2") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D2").ToString()),
                                                                                            "",
                                                                                            (Function(s)
                                                                                                 Dim p = s.ToString().Split("|"c)
                                                                                                 Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                                 Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                                 Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                             End Function)(Eval("D2"))
                                                                                        ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D2") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD2" runat="server" 
                                                                Visible='<%# (Eval("D2") IsNot Nothing AndAlso Eval("D2").ToString().Split("|"c).Length > 6 AndAlso Eval("D2").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D2") IsNot Nothing AndAlso Eval("D2").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D2").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D2").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D2") IsNot Nothing AndAlso Eval("D2").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D2").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D2").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD3" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D3") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D3").ToString()),
                                                                                            "",
                                                                                            (Function(s)
                                                                                                 Dim p = s.ToString().Split("|"c)
                                                                                                 Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                                 Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                                 Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                             End Function)(Eval("D3"))
                                                                                        ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D3") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD3" runat="server" 
                                                                Visible='<%# (Eval("D3") IsNot Nothing AndAlso Eval("D3").ToString().Split("|"c).Length > 6 AndAlso Eval("D3").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D3") IsNot Nothing AndAlso Eval("D3").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D3").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D3").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D3") IsNot Nothing AndAlso Eval("D3").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D3").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D3").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD4" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D4") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D4").ToString()),
                                                                                        "",
                                                                                        (Function(s)
                                                                                             Dim p = s.ToString().Split("|"c)
                                                                                             Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                             Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                             Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                         End Function)(Eval("D4"))
                                                                                    ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D4") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD4" runat="server" 
                                                                Visible='<%# (Eval("D4") IsNot Nothing AndAlso Eval("D4").ToString().Split("|"c).Length > 6 AndAlso Eval("D4").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D4") IsNot Nothing AndAlso Eval("D4").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D4").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D4").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D4") IsNot Nothing AndAlso Eval("D4").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D4").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D4").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD5" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D5") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D5").ToString()),
                                                                                        "",
                                                                                        (Function(s)
                                                                                             Dim p = s.ToString().Split("|"c)
                                                                                             Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                             Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                             Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                         End Function)(Eval("D5"))
                                                                                    ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D5") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD5" runat="server" 
                                                                Visible='<%# (Eval("D5") IsNot Nothing AndAlso Eval("D5").ToString().Split("|"c).Length > 6 AndAlso Eval("D5").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D5") IsNot Nothing AndAlso Eval("D5").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D5").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D5").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D5") IsNot Nothing AndAlso Eval("D5").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D5").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D5").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD6" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D6") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D6").ToString()),
                                                                                        "",
                                                                                        (Function(s)
                                                                                             Dim p = s.ToString().Split("|"c)
                                                                                             Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                             Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                             Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                         End Function)(Eval("D6"))
                                                                                    ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D6") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD6" runat="server" 
                                                                Visible='<%# (Eval("D6") IsNot Nothing AndAlso Eval("D6").ToString().Split("|"c).Length > 6 AndAlso Eval("D6").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D6") IsNot Nothing AndAlso Eval("D6").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D6").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D6").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D6") IsNot Nothing AndAlso Eval("D6").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D6").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D6").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                         </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD7" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D7") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D7").ToString()),
                                                                                            "",
                                                                                            (Function(s)
                                                                                                 Dim p = s.ToString().Split("|"c)
                                                                                                 Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                                 Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                                 Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                             End Function)(Eval("D7"))
                                                                                        ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D7") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD7" runat="server" 
                                                                Visible='<%# (Eval("D7") IsNot Nothing AndAlso Eval("D7").ToString().Split("|"c).Length > 6 AndAlso Eval("D7").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D7") IsNot Nothing AndAlso Eval("D7").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D7").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D7").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D7") IsNot Nothing AndAlso Eval("D7").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D7").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D7").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD8" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D8") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D8").ToString()),
                                                                                            "",
                                                                                            (Function(s)
                                                                                                 Dim p = s.ToString().Split("|"c)
                                                                                                 Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                                 Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                                 Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                             End Function)(Eval("D8"))
                                                                                        ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D8") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD8" runat="server" 
                                                                Visible='<%# (Eval("D8") IsNot Nothing AndAlso Eval("D8").ToString().Split("|"c).Length > 6 AndAlso Eval("D8").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D8") IsNot Nothing AndAlso Eval("D8").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D8").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D8").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D8") IsNot Nothing AndAlso Eval("D8").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D8").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D8").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD9" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D9") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D9").ToString()),
                                                                                    "",
                                                                                    (Function(s)
                                                                                         Dim p = s.ToString().Split("|"c)
                                                                                         Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                         Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                         Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                     End Function)(Eval("D9"))
                                                                                ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D9") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD9" runat="server" 
                                                                Visible='<%# (Eval("D9") IsNot Nothing AndAlso Eval("D9").ToString().Split("|"c).Length > 6 AndAlso Eval("D9").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D9") IsNot Nothing AndAlso Eval("D9").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D9").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D9").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D9") IsNot Nothing AndAlso Eval("D9").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D9").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D9").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD10" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D10") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D10").ToString()),
                                                                                    "",
                                                                                    (Function(s)
                                                                                         Dim p = s.ToString().Split("|"c)
                                                                                         Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                         Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                         Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                     End Function)(Eval("D10"))
                                                                                ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D10") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD10" runat="server" 
                                                                Visible='<%# (Eval("D10") IsNot Nothing AndAlso Eval("D10").ToString().Split("|"c).Length > 6 AndAlso Eval("D10").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D10") IsNot Nothing AndAlso Eval("D10").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D10").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D10").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D10") IsNot Nothing AndAlso Eval("D10").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D10").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D10").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD11" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D11") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D11").ToString()),
                                                                                    "",
                                                                                    (Function(s)
                                                                                         Dim p = s.ToString().Split("|"c)
                                                                                         Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                         Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                         Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                     End Function)(Eval("D11"))
                                                                                ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D11") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD11" runat="server" 
                                                                Visible='<%# (Eval("D11") IsNot Nothing AndAlso Eval("D11").ToString().Split("|"c).Length > 6 AndAlso Eval("D11").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D11") IsNot Nothing AndAlso Eval("D11").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D11").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D11").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D11") IsNot Nothing AndAlso Eval("D11").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D11").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D11").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD12" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D12") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D12").ToString()),
                                                                                            "",
                                                                                            (Function(s)
                                                                                                 Dim p = s.ToString().Split("|"c)
                                                                                                 Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                                 Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                                 Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                             End Function)(Eval("D12"))
                                                                                        ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D12") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD12" runat="server" 
                                                                Visible='<%# (Eval("D12") IsNot Nothing AndAlso Eval("D12").ToString().Split("|"c).Length > 6 AndAlso Eval("D12").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D12") IsNot Nothing AndAlso Eval("D12").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D12").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D12").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D12") IsNot Nothing AndAlso Eval("D12").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D12").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D12").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD13" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D13") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D13").ToString()),
                                                                                        "",
                                                                                        (Function(s)
                                                                                             Dim p = s.ToString().Split("|"c)
                                                                                             Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                             Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                             Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                         End Function)(Eval("D13"))
                                                                                    ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D13") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD13" runat="server" 
                                                                Visible='<%# (Eval("D13") IsNot Nothing AndAlso Eval("D13").ToString().Split("|"c).Length > 6 AndAlso Eval("D13").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D13") IsNot Nothing AndAlso Eval("D13").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D13").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D13").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D13") IsNot Nothing AndAlso Eval("D13").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D13").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D13").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD14" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D14") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D14").ToString()),
                                                                                            "",
                                                                                            (Function(s)
                                                                                                 Dim p = s.ToString().Split("|"c)
                                                                                                 Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                                 Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                                 Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                             End Function)(Eval("D14"))
                                                                                        ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D14") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD14" runat="server" 
                                                                Visible='<%# (Eval("D14") IsNot Nothing AndAlso Eval("D14").ToString().Split("|"c).Length > 6 AndAlso Eval("D14").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D14") IsNot Nothing AndAlso Eval("D14").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D14").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D14").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D14") IsNot Nothing AndAlso Eval("D14").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D14").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D14").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD15" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D15") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D15").ToString()),
                                                                                    "",
                                                                                    (Function(s)
                                                                                         Dim p = s.ToString().Split("|"c)
                                                                                         Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                         Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                         Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                     End Function)(Eval("D15"))
                                                                                ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D15") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD15" runat="server" 
                                                                Visible='<%# (Eval("D15") IsNot Nothing AndAlso Eval("D15").ToString().Split("|"c).Length > 6 AndAlso Eval("D15").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D15") IsNot Nothing AndAlso Eval("D15").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D15").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D15").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D15") IsNot Nothing AndAlso Eval("D15").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D15").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D15").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD16" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D16") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D16").ToString()),
                                                                                        "",
                                                                                        (Function(s)
                                                                                             Dim p = s.ToString().Split("|"c)
                                                                                             Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                             Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                             Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                         End Function)(Eval("D16"))
                                                                                    ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D16") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD16" runat="server" 
                                                                Visible='<%# (Eval("D16") IsNot Nothing AndAlso Eval("D16").ToString().Split("|"c).Length > 6 AndAlso Eval("D16").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D16") IsNot Nothing AndAlso Eval("D16").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D16").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D16").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D16") IsNot Nothing AndAlso Eval("D16").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D16").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D16").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD17" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D17") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D17").ToString()),
                                                                                            "",
                                                                                            (Function(s)
                                                                                                 Dim p = s.ToString().Split("|"c)
                                                                                                 Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                                 Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                                 Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                             End Function)(Eval("D17"))
                                                                                        ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D17") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD17" runat="server" 
                                                                Visible='<%# (Eval("D17") IsNot Nothing AndAlso Eval("D17").ToString().Split("|"c).Length > 6 AndAlso Eval("D17").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D17") IsNot Nothing AndAlso Eval("D17").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D17").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D17").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D17") IsNot Nothing AndAlso Eval("D17").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D17").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D17").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD18" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D18") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D18").ToString()),
                                                                                    "",
                                                                                    (Function(s)
                                                                                         Dim p = s.ToString().Split("|"c)
                                                                                         Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                         Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                         Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                     End Function)(Eval("D18"))
                                                                                ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D18") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD18" runat="server" 
                                                                Visible='<%# (Eval("D18") IsNot Nothing AndAlso Eval("D18").ToString().Split("|"c).Length > 6 AndAlso Eval("D18").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D18") IsNot Nothing AndAlso Eval("D18").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D18").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D18").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D18") IsNot Nothing AndAlso Eval("D18").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D18").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D18").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD19" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D19") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D19").ToString()),
                                                                                    "",
                                                                                    (Function(s)
                                                                                         Dim p = s.ToString().Split("|"c)
                                                                                         Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                         Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                         Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                     End Function)(Eval("D19"))
                                                                                ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D19") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD19" runat="server" 
                                                                Visible='<%# (Eval("D19") IsNot Nothing AndAlso Eval("D19").ToString().Split("|"c).Length > 6 AndAlso Eval("D19").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D19") IsNot Nothing AndAlso Eval("D19").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D19").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D19").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D19") IsNot Nothing AndAlso Eval("D19").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D19").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D19").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD20" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D20") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D20").ToString()),
                                                                                        "",
                                                                                        (Function(s)
                                                                                             Dim p = s.ToString().Split("|"c)
                                                                                             Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                             Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                             Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                         End Function)(Eval("D20"))
                                                                                    ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D20") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD20" runat="server" 
                                                                Visible='<%# (Eval("D20") IsNot Nothing AndAlso Eval("D20").ToString().Split("|"c).Length > 6 AndAlso Eval("D20").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D20") IsNot Nothing AndAlso Eval("D20").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D20").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D20").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D20") IsNot Nothing AndAlso Eval("D20").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D20").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D20").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD21" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D21") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D21").ToString()),
                                                                                            "",
                                                                                            (Function(s)
                                                                                                 Dim p = s.ToString().Split("|"c)
                                                                                                 Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                                 Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                                 Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                             End Function)(Eval("D21"))
                                                                                        ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D21") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD21" runat="server" 
                                                                Visible='<%# (Eval("D21") IsNot Nothing AndAlso Eval("D21").ToString().Split("|"c).Length > 6 AndAlso Eval("D21").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D21") IsNot Nothing AndAlso Eval("D21").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D21").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D21").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D21") IsNot Nothing AndAlso Eval("D21").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D21").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D21").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD22" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D22") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D22").ToString()),
                                                                                        "",
                                                                                        (Function(s)
                                                                                             Dim p = s.ToString().Split("|"c)
                                                                                             Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                             Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                             Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                         End Function)(Eval("D22"))
                                                                                    ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D22") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD22" runat="server" 
                                                                Visible='<%# (Eval("D22") IsNot Nothing AndAlso Eval("D22").ToString().Split("|"c).Length > 6 AndAlso Eval("D22").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D22") IsNot Nothing AndAlso Eval("D22").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D22").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D22").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D22") IsNot Nothing AndAlso Eval("D22").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D22").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D22").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD23" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D23") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D23").ToString()),
                                                                                            "",
                                                                                            (Function(s)
                                                                                                 Dim p = s.ToString().Split("|"c)
                                                                                                 Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                                 Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                                 Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                             End Function)(Eval("D23"))
                                                                                        ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D23") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD23" runat="server" 
                                                                Visible='<%# (Eval("D23") IsNot Nothing AndAlso Eval("D23").ToString().Split("|"c).Length > 6 AndAlso Eval("D23").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D23") IsNot Nothing AndAlso Eval("D23").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D23").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D23").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D23") IsNot Nothing AndAlso Eval("D23").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D23").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D23").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD24" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D24") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D24").ToString()),
                                                                                            "",
                                                                                            (Function(s)
                                                                                                 Dim p = s.ToString().Split("|"c)
                                                                                                 Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                                 Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                                 Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                             End Function)(Eval("D24"))
                                                                                        ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D24") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD24" runat="server" 
                                                                Visible='<%# (Eval("D24") IsNot Nothing AndAlso Eval("D24").ToString().Split("|"c).Length > 6 AndAlso Eval("D24").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D24") IsNot Nothing AndAlso Eval("D24").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D24").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D24").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D24") IsNot Nothing AndAlso Eval("D24").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D24").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D24").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD25" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D25") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D25").ToString()),
                                                                                            "",
                                                                                            (Function(s)
                                                                                                 Dim p = s.ToString().Split("|"c)
                                                                                                 Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                                 Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                                 Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                             End Function)(Eval("D25"))
                                                                                        ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D25") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD25" runat="server" 
                                                                Visible='<%# (Eval("D25") IsNot Nothing AndAlso Eval("D25").ToString().Split("|"c).Length > 6 AndAlso Eval("D25").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D25") IsNot Nothing AndAlso Eval("D25").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D25").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D25").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D25") IsNot Nothing AndAlso Eval("D25").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D25").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D25").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD26" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D26") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D26").ToString()),
                                                                                            "",
                                                                                            (Function(s)
                                                                                                 Dim p = s.ToString().Split("|"c)
                                                                                                 Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                                 Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                                 Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                             End Function)(Eval("D26"))
                                                                                        ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D26") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD26" runat="server" 
                                                                Visible='<%# (Eval("D26") IsNot Nothing AndAlso Eval("D26").ToString().Split("|"c).Length > 6 AndAlso Eval("D26").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D26") IsNot Nothing AndAlso Eval("D26").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D26").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D26").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D26") IsNot Nothing AndAlso Eval("D26").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D26").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D26").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD27" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D27") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D27").ToString()),
                                                                                            "",
                                                                                            (Function(s)
                                                                                                 Dim p = s.ToString().Split("|"c)
                                                                                                 Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                                 Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                                 Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                             End Function)(Eval("D27"))
                                                                                        ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D27") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD27" runat="server" 
                                                                Visible='<%# (Eval("D27") IsNot Nothing AndAlso Eval("D27").ToString().Split("|"c).Length > 6 AndAlso Eval("D27").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D27") IsNot Nothing AndAlso Eval("D27").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D27").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D27").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D27") IsNot Nothing AndAlso Eval("D27").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D27").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D27").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD28" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D28") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D28").ToString()),
                                                                                            "",
                                                                                            (Function(s)
                                                                                                 Dim p = s.ToString().Split("|"c)
                                                                                                 Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                                 Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                                 Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                             End Function)(Eval("D28"))
                                                                                        ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D28") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD28" runat="server" 
                                                                Visible='<%# (Eval("D28") IsNot Nothing AndAlso Eval("D28").ToString().Split("|"c).Length > 6 AndAlso Eval("D28").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D28") IsNot Nothing AndAlso Eval("D28").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D28").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D28").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D28") IsNot Nothing AndAlso Eval("D28").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D28").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D28").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD29" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D29") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D29").ToString()),
                                                                                            "",
                                                                                            (Function(s)
                                                                                                 Dim p = s.ToString().Split("|"c)
                                                                                                 Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                                 Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                                 Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                             End Function)(Eval("D29"))
                                                                                        ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D29") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD29" runat="server" 
                                                                Visible='<%# (Eval("D29") IsNot Nothing AndAlso Eval("D29").ToString().Split("|"c).Length > 6 AndAlso Eval("D29").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D29") IsNot Nothing AndAlso Eval("D29").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D29").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D29").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D29") IsNot Nothing AndAlso Eval("D29").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D29").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D29").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD30" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D30") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D30").ToString()),
                                                                                            "",
                                                                                            (Function(s)
                                                                                                 Dim p = s.ToString().Split("|"c)
                                                                                                 Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                                 Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                                 Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                             End Function)(Eval("D30"))
                                                                                        ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D30") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD30" runat="server" 
                                                                Visible='<%# (Eval("D30") IsNot Nothing AndAlso Eval("D30").ToString().Split("|"c).Length > 6 AndAlso Eval("D30").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D30") IsNot Nothing AndAlso Eval("D30").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D30").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D30").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D30") IsNot Nothing AndAlso Eval("D30").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D30").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D30").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField
                                                        HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnD31" runat="server" CssClass="grid-linkbutton"
                                                                Text='<%# If(Eval("D31") Is Nothing OrElse String.IsNullOrWhiteSpace(Eval("D31").ToString()),
                                                                                            "",
                                                                                            (Function(s)
                                                                                                 Dim p = s.ToString().Split("|"c)
                                                                                                 Dim p0 = If(p.Length > 0, p(0).Trim(), "")
                                                                                                 Dim p4 = If(p.Length > 4, p(4).Trim(), "")
                                                                                                 Return (p0 & If(p0 <> "" AndAlso p4 <> "", " ", "") & p4).Trim()
                                                                                             End Function)(Eval("D31"))
                                                                                        ) %>'
                                                                ToolTip="Editar Mantenimiento"
                                                                CommandName="VerMantenimientoPlanificacionEquipo"
                                                                CommandArgument='<%# Eval("D31") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
                                                            <br style="line-height: 1px; height: 1px; margin: 0; padding: 0;" />
                                                            <asp:Label ID="lblEstadoD31" runat="server" 
                                                                Visible='<%# (Eval("D31") IsNot Nothing AndAlso Eval("D31").ToString().Split("|"c).Length > 6 AndAlso Eval("D31").ToString().Split("|"c)(6) <> "") %>' 
                                                                CssClass='<%# If(Eval("D31") IsNot Nothing AndAlso Eval("D31").ToString().Split("|"c).Length > 6,
                                                                                             If(Split(Eval("D31").ToString(), "|")(6) = "R", "texto_estado short_estado black borde_redondo1 grid-label",
                                                                                                If(Split(Eval("D31").ToString(), "|")(6) = "T", "texto_estado short_estado verde borde_redondo1 grid-label",
                                                                                                   "texto_estado short_estado orange borde_redondo1 grid-label")),
                                                                                             "") %>'
                                                                Text='<%# If(Eval("D31") IsNot Nothing AndAlso Eval("D31").ToString().Split("|"c).Length > 6,
                                                                                  StrConv(IIf(Split(Eval("D31").ToString(), "|")(6) = "P", "PENDIENTE",
                                                                                        IIf(Split(Eval("D31").ToString(), "|")(6) = "T", "TERMINADO", "REGISTRADO")),
                                                                                  VbStrConv.ProperCase),
                                                                                  "") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="thead-dark" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>

                
                <div class="position-fixed bottom-0 end-0 p-3">
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" role="alert"
                        CssClass="toast alert-warning alert-dismissible fade show"
                        ValidationGroup="vgrpValidar" />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="updpnlMensajeDocumento" runat="server">
        <ContentTemplate>
            <asp:LinkButton ID="lnk_mostrarPanelMensajeDocumento" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMensajeDocumento_ModalPopupExtender"
                runat="server" BackgroundCssClass="FondoAplicacion"
                CancelControlID="imgbtnCancelarDocumentoImagen" DropShadow="False" DynamicServicePath=""
                Enabled="True" PopupControlID="pnlMensajeDocumento"
                TargetControlID="lnk_mostrarPanelMensajeDocumento">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlMensajeDocumento" runat="server" CssClass="container">
                <div class="modal-dialog modal-sm">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title" id="myModalLabelMensajeDocumento">Mensaje
                                                Informativo</h4>
                                    <asp:Button ID="imgbtnCancelarDocumentoImagen" runat="server"
                                        CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base"
                                        data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <div class="form-group row pt-2">
                                        <span class="col-sm-12 col-form-label-sm text-center text-900">
                                            <asp:Label ID="lblDescripcionMensajeDocumento" runat="server"
                                                Text="Se creó el siguiente número de documento"></asp:Label>
                                        </span>
                                        <h3 class="col-sm-12 text-center">
                                            <asp:Label ID="lblNroDocumentoMensajeDocumento" runat="server"
                                                Text="00000" CssClass="text-info"></asp:Label>
                                        </h3>
                                        <div class="col-sm-12 text-center">
                                            <asp:Label ID="lblMensajeDocumento" runat="server" Text=""
                                                CssClass="texto_estado short_estado light-orange borde_redondo1">
                                            </asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnSiMensajeDocumento" runat="server" Text="Si"
                                        CssClass="btn btn-outline-facebook btn-sm d-block"
                                        ToolTip="Confirmar Eliminación" />
                                    &nbsp;&nbsp;
                                            <asp:Button ID="btnNoMensajeDocumento" runat="server" Text="No"
                                                CssClass="btn btn-outline-facebook btn-sm d-block"
                                                ToolTip="Cancelar Eliminación" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">
        function popupEmitirContratoReporte(TipDoc, NroSer, NroDoc) {
            window.open("Informes/frmCmmsContratoReporte.aspx?TipDoc=" + TipDoc + "&NroSerie=" + NroSer + "&NroDoc=" + NroDoc, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
        }
        function popupEmitirContratoProgramacionReporte(TipDoc, NroSer, NroDoc, FecIni, FecFin, NroEquipos) {
            window.open("Informes/frmCmmsContratoProgramacionReporte.aspx?TipDoc=" + TipDoc + "&NroSerie=" + NroSer + "&NroDoc=" + NroDoc + "&FecIni=" + FecIni + "&FecFin=" + FecFin + "&NroEquipos=" + NroEquipos, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
        }

    </script>

    <script type="text/javascript">
        function syncScroll() {
            const fixedGrid = document.getElementById('<%= grdDetalleEquiposFixed.ClientID %>');

            if (fixedGrid) {
                // Forzar visibilidad
                fixedGrid.style.visibility = 'visible';
                fixedGrid.style.display = 'table';

                // Forzar visibilidad de celdas
                const cells = fixedGrid.querySelectorAll('th, td');
                cells.forEach(cell => {
                    cell.style.display = 'table-cell';
                    cell.style.visibility = 'visible';
                    cell.style.width = 'auto';
                });

                // Asegurar que el contenedor muestre todo
                const container = document.querySelector('.fixed-columns-container');
                container.style.overflow = 'visible';
                container.style.width = 'auto';
            }

            const fixed = document.getElementById("divFixed");
            const scrollable = document.getElementById("divScrollable");

            if (!fixed || !scrollable) return;

            // Remover listeners previos para evitar duplicados
            fixed.removeEventListener("scroll", scrollHandler);
            scrollable.removeEventListener("scroll", scrollHandler);

            function scrollHandler(e) {
                // Detener la propagación para evitar bucles
                e.stopPropagation();
                const source = e.currentTarget;
                const target = source === fixed ? scrollable : fixed;

                // Sincronizar posición vertical
                target.scrollTop = source.scrollTop;
            }

            // Agregar nuevos listeners
            fixed.addEventListener("scroll", scrollHandler);
            scrollable.addEventListener("scroll", scrollHandler);
        }

        // Inicializar y manejar postbacks
        document.addEventListener('DOMContentLoaded', syncScroll);
        if (typeof Sys !== 'undefined') {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(syncScroll);
        }
    </script>
    <link href="cssbootstrap/bootstrap-switch.css" rel="stylesheet" />
    <script src="jsbootstrap/bootstrap-switch.js"></script>
</asp:Content>
