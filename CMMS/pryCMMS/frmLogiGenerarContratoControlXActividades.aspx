<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmLogiGenerarContratoControlXActividades.aspx.vb" Inherits="pryCMMS.frmLogiGenerarContratoControlXActividades" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

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
        #BarraHTML ul{
           list-style-type: none;
        }
        #BarraHTML li{
           display: inline;
           text-align: center;
           margin: 0 0 0 0;
        }
        #BarraHTML li a {
           padding: 2px 7px 2px 7px;
           text-decoration: none;
        }
        #BarraHTML li a:hover{
           background-color: #333333;
           color: #ffffff;
        }
        #texto{
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
    <link href="Content/Calendario.css" rel="stylesheet" type="text/css"/>
    <link href="Content/CuadroDialogo.css" rel="stylesheet" type="text/css"/>
    <script src="Scripts/jsUpdateProgress.js" type="text/javascript"></script>
    <link href="Content/AutoCompletar.css" rel="stylesheet" type="text/css"/>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
    </ajaxToolkit:ToolkitScriptManager>
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
            <asp:HiddenField ID="hfdFechaCreacionContrato" runat="server" />
            <asp:HiddenField ID="hfdIdUsuarioCreacionContrato" runat="server" />

            <asp:HiddenField ID="hfdIdEquipo" runat="server" />
            <asp:HiddenField ID="hfdIdEquipoSAPEquipo" runat="server" />
            <asp:HiddenField ID="hfdIdCatalogoEquipo" runat="server" />
            <asp:HiddenField ID="hfdIdTipoActivoEquipo" runat="server" />
            <asp:HiddenField ID="hfdJerarquiaEquipo" runat="server" />
            <%--<asp:HiddenField ID="hfdIdContrato" runat="server" />--%>
            <asp:HiddenField ID="hfdIdOrdenTrabajo" runat="server" />
            <asp:HiddenField ID="hfdIdOrdenTrabajoCliente" runat="server" />
            <asp:HiddenField ID="hfdFechaPlanificacion" runat="server" />
            <asp:Panel ID="pnlCabecera" runat="server">
                <div class="card mb-3">
                    <div class="card-header">
                      <div class="row flex-between-end">
                        <div class="col-auto align-self-center">
                            <div class="nav nav-pills gap-2" role="tablist">
                                <h5 class="mb-0" data-anchor="data-anchor">Listado de Contratos</h5>
                                <a class="btn btn-falcon-default btn-sm ml-auto float-right" href="frmBienvenida.aspx" aria-expanded="true">
                                    <span class="fas fa-home"></span><span class="nav-link-text ps-1">Inicio</span>
                                </a>
                            </div>
                        </div>
                        <div class="col-auto ms-auto">
                          <div class="nav gap-2" role="tablist">
                            <asp:LinkButton ID="btnNuevo" runat="server" OnClick="btnNuevo_Click" CssClass="btn btn-success btn-sm ml-auto float-right" Text="<span class='fas fa-plus'></span> Nuevo" ToolTip="Nuevo Contrato" />
							<asp:LinkButton ID="btnEditar" runat="server" OnClick="btnEditar_Click" CssClass="btn btn-primary btn-sm ml-auto float-right" Text="<span class='fas fa-pencil-alt'></span> Editar" ToolTip="Editar Contrato" />
                            <div class="dropdown font-sans-serif btn-reveal-trigger">
                              <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesContrato" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                              <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesContrato">
                                  <asp:LinkButton ID="lnkbtnVerContrato" runat="server" ToolTip="Ver Contrato" Text="<span class='fas fa-file me-2'></span>Ver Contrato" CssClass="nav-link nav-link-card-details" />
                                  <asp:LinkButton ID="lnkbtnVerProgramacion" runat="server" ToolTip="Ver Programación" Text="<span class='fas fa-file me-2'></span>Ver Programación" CssClass="nav-link nav-link-card-details" />
                              </div>
                            </div>
                          </div>
                          
                          <asp:LinkButton ID="lnk_mostrarPanelImprimirProgramacion" runat="server"></asp:LinkButton>
	                      <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelImprimirProgramacion_ModalPopupExtender" 
		                    runat="server" BackgroundCssClass="FondoAplicacion" 
		                    CancelControlID="btnCancelarImprimirProgramacion" DropShadow="False" Enabled="True" 
		                    PopupControlID="pnlIngresarImprimirProgramacion" 
		                    TargetControlID="lnk_mostrarPanelImprimirProgramacion">
	                      </ajaxToolkit:ModalPopupExtender>
                          <asp:UpdatePanel ID="updpnlImprimirProgramacion" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlIngresarImprimirProgramacion" runat="server" CssClass="container">
                                    <div class="modal-dialog modal-xl">
                                        <div class="shadow rounded">
                                            <div class="modal-dialog-scrollable">
                                                <div class="modal-content">
                                                    <div class="modal-header">
                                                        <h4 class="modal-title" id="myModalLabelCatalogo">
                                                            Datos a Imprimir</h4>
                                                        <asp:Button ID="imgbtnCancelarImprimirProgramacionImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                    </div>
                                                    <div class="modal-body">
                                                        <div class="form-group">
                                                            <div class="row mt-1">
                                                                <div class="col-lg-6 col-sm-12">
                                                                    <div class="row mt-1">
                                                                        <div class="col-12 mb-0 bg-100 d-none d-md-block">
                                                                            <h6 class="mt-2 mb-1">FILTROS</h6>
                                                                            <hr class="bg-300 m-0" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-lg-4 col-md-4 mt-2">
                                                                            <span class="far fa-calendar-alt"></span><label class="text-black-50 col-form-label fs--2 m-1">Fecha Inicial:</label>
                                                                            <div class="input-group">
                                                                                <asp:TextBox ID="txtFechaInicialImprimirProgramacion" runat="server" autocomplete="off"
                                                                                    CssClass="form-control form-control-sm" style="text-transform :uppercase;" 
                                                                                    placeholder="Periodo Inicial" TabIndex="4" type="date"></asp:TextBox>
                                                                                <asp:RequiredFieldValidator ID="rfvFechaInicialImprimirProgramacion" runat="server" 
                                                                                    ControlToValidate="txtFechaInicialImprimirProgramacion" EnableClientScript="True" 
                                                                                    ErrorMessage="Ingrese el periodo inicial" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-lg-4 col-md-4 mt-2">
                                                                            <span class="far fa-calendar-alt"></span><label class="text-black-50 col-form-label fs--2 m-1">Fecha Final:</label>
                                                                            <div class="input-group">
                                                                                <asp:TextBox ID="txtFechaFinalImprimirProgramacion" runat="server" autocomplete="off"
                                                                                    CssClass="form-control form-control-sm datetimepicker" style="text-transform :uppercase;" 
                                                                                    placeholder="Periodo Final" TabIndex="4" type="date"></asp:TextBox>
                                                                                <asp:RequiredFieldValidator ID="rfvFechaFinalImprimirProgramacion" runat="server" 
                                                                                    ControlToValidate="txtFechaFinalImprimirProgramacion" EnableClientScript="True" 
                                                                                    ErrorMessage="Ingrese el periodo final" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-lg-4 col-md-4 mt-2">
                                                                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Marcar Todos:</label>
                                                                            <div class="input-group">
                                                                                <asp:CheckBox ID="chkTodosImprimirProgramacion" runat="server" TabIndex="5" CssClass="form-check" AutoPostBack="True" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-lg-6 col-sm-12 mt-2 bg-200">
                                                                    <div class="row mt-1">
                                                                        <div class="col-12 mb-0 d-none d-md-block">
                                                                            <h6 class="mt-2 mb-1">LISTADO DE EQUIPOS</h6>
                                                                            <hr class="bg-300 m-0" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="row justify-content-between mt-3"> 
                                                                        <div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
                                                                            <asp:Panel ID="pnlDetalleEquipoImprimiProgramacion" runat="server" CssClass="bg-light" >
                                                                                <div class="table-responsive scrollbar">
                                                                                <asp:GridView ID="grdDetalleEquipoImprimirProgramacion" runat="server" AutoGenerateColumns="False" TabIndex="4" 
                                                                                    GridLines="None" CssClass="table table-striped table-hover small overflow-hidden" AllowPaging="True" 
                                                                                    EmptyDataText="No hay registros a visualizar" PageSize="50" >
                                                                                    <PagerStyle CssClass="mGrid" />
                                                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField>
                                                                                            <ItemTemplate>
                                                                                                <asp:CheckBox ID="chkRowDetalleEquipo" runat="server" Checked='<%# Eval("Seleccionar") %>' AutoPostBack="true" OnCheckedChanged="chkRowDetalleEquipo_CheckedChanged" />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:BoundField DataField="Item" HeaderText="#" ControlStyle-Width="25px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                                        <asp:BoundField DataField="IdEquipo" HeaderText="Código" HeaderStyle-Width="40px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                                        <asp:BoundField DataField="DescripcionEquipo" HeaderText="Descripción" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                                                    </Columns>
                                                                                    <HeaderStyle CssClass="thead-dark" />
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
                                                        <asp:ValidationSummary ID="ValidationSummary3" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarComponente" />
                                                        <div class="col-md-4 text-end p-2">
                                                            <asp:Button ID="btnAceptarImprimirProgramacion" runat="server" ValidationGroup="vgrpValidarComponente"
                                                                ToolTip="Aceptar Registro" TabIndex ="72" Text="Imprimir" CssClass="btn btn-outline-facebook btn-sm" />
                                                            &nbsp;
                                                            <asp:Button ID="btnCancelarImprimirProgramacion" runat="server" 
                                                                ToolTip="Cancelar Registro" TabIndex ="73" Text="Cancelar" cssclass="btn btn-outline-google-plus btn-sm"/>
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
                                <span class="fas fa-filter"></span><label class="text-black-50 col-form-label fs--2 m-1">Filtrar por:</label>
                                <div class="input-group">
						            <asp:DropDownList ID="cboFiltroContrato" runat="server" CssClass="form-select form-select-sm js-choice" TabIndex="1">
							            <asp:ListItem>SELECCIONE EL FILTRO</asp:ListItem>
						            </asp:DropDownList>
					            </div>
					        </div>
					        <div class="col-lg-6 col-sm-6">
                                <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Buscar por:</label>
						        <div class="input-group">
							        <asp:TextBox ID="txtBuscarContrato" runat="server" CssClass="form-control form-control-sm" TabIndex="2"
								        style="text-transform :uppercase" placeholder="Ingrese Busqueda"></asp:TextBox>
							        &nbsp;
							        <asp:ImageButton ID="imgbtnBuscarContrato" runat="server" Height="18px" TabIndex="3" CssClass="mt-1"
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
                                            EmptyDataText="No hay registros a visualizar" PageSize="14" >
                                            <PagerStyle CssClass="pgr" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <AlternatingRowStyle CssClass="alt" />
                                            <Columns>
                                                <asp:BoundField DataField="IdTipoDocumento" HeaderText="Tip.Doc." HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>
                                                <asp:BoundField DataField="IdNumeroSerie" HeaderText="Nro.Serie" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>
                                                <asp:BoundField DataField="IdNumeroCorrelativo" HeaderText="Nro.Correlativo" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>
                                                <asp:BoundField DataField="FechaEmision" HeaderText="Fec.Emisión" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>
                                                <asp:BoundField DataField="IdCliente" HeaderText="Id.Cliente" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>
                                               <%-- <asp:BoundField DataField="IdClienteSAP" HeaderText="Id. Cliente SAP" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>--%>
                                                <asp:BoundField DataField="RucCliente" HeaderText="RUC" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>
                                                <asp:BoundField DataField="RazonSocialCliente" HeaderText="Razón Social" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>
                                                <asp:TemplateField HeaderText="Estado" ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEstado" runat="server" Visible='<%# If(Eval("StatusContrato") = "", "false", "true") %>' CssClass='<%#If(Eval("StatusContrato") = "R", "texto_estado short_estado black borde_redondo1", If(Eval("StatusContrato") = "T", "texto_estado short_estado verde borde_redondo1", "texto_estado short_estado orange borde_redondo1")) %>' Text='<%# StrConv(IIf(Eval("StatusContrato") = "P", "PENDIENTE", IIf(Eval("StatusContrato") = "T", "TERMINADO", "REGISTRADO")), VbStrConv.ProperCase) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>
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
    <asp:UpdatePanel ID ="updpnlContenido" runat="server" updatemode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger controlid="btnNuevo" eventname="Click" />
            <asp:AsyncPostBackTrigger controlid="btnEditar" eventname="Click" />
            <asp:AsyncPostBackTrigger controlid="btnSiMensajeDocumento" eventname="Click" />
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
                    <div class="col-auto ms-auto">
                        <div class="nav nav-pills gap-2" role="tablist">
						    <asp:LinkButton ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" CssClass="btn btn-success btn-sm ml-auto float-right" Text="<span class='fas fa-plus'></span> Guardar" ToolTip="Guardar Registro" />
                        </div>
                    </div>
                </div>
              </div>
              <div class="card mb-2">
                <div class="card-header bg-light">
                    <div class="row align-items-center">
                    <div class="col">
                        <h6 class="mb-0">Datos Generales</h6>
                    </div>
                    </div>
                </div>
                <div class="card-body">
                    <div class="row mt-0">
                        <div class="col-lg-12 col-sm-12">
                            <div class="row">
                            <div class="col-lg-2 col-md-4">
                                <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Código Contrato:</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtIdContrato" runat="server" CssClass="form-control form-control-sm" TabIndex="10" placeholder="Id. del equipo"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-2 col-md-8">
                                <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción Contrato:</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtDescripcionContrato" runat="server" CssClass="form-control form-control-sm" 
                                        TabIndex="11" style="text-transform :uppercase" placeholder="Descripción del contrato" autocomplete="off"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDescripcionContrato" runat="server" 
                                        ControlToValidate="txtDescripcionContrato" EnableClientScript="False" 
                                        ErrorMessage="Ingrese la descripción del contrato" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-lg-2 col-md-2">
                                <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Nro.Licitación:</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtNroLicitacionContrato" runat="server" CssClass="form-control form-control-sm" 
                                        TabIndex="11" style="text-transform :uppercase" placeholder="Descripción del contrato" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-2 col-md-2">
                                <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Fecha Emisión:</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtFechaEmisionContrato" runat="server" CssClass="form-control form-control-sm" 
                                        Enabled="False" style="text-transform : uppercase" TabIndex="5" AutoPostBack="True"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvFechaEmisionContrato" runat="server" 
                                        ControlToValidate="txtFechaEmisionContrato" EnableClientScript="False" 
                                        ErrorMessage="Ingrese la fecha de emisión" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                    <ajaxToolkit:CalendarExtender ID="txtFechaEmisionContrato_CalendarExtender" runat="server" 
                                        CssClass="calendario" Enabled="True" TargetControlID="txtFechaEmisionContrato">
                                    </ajaxToolkit:CalendarExtender>
                                </div>
                            </div>
                            <div class="col-lg-2 col-md-2">
                                <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Fecha Vigencia Inicio:</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtFechaVigenciaInicio" runat="server" CssClass="form-control form-control-sm" 
                                        Enabled="False" style="text-transform : uppercase" TabIndex="5" AutoPostBack="True"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvFechaVigenciaInicio" runat="server" 
                                        ControlToValidate="txtFechaVigenciaInicio" EnableClientScript="False" 
                                        ErrorMessage="Ingrese la fecha de vigencia inicial" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                    <ajaxToolkit:CalendarExtender ID="txtFechaVigenciaInicio_CalendarExtender" runat="server" 
                                        CssClass="calendario" Enabled="True" TargetControlID="txtFechaVigenciaInicio">
                                    </ajaxToolkit:CalendarExtender>
                                </div>
                            </div>
                            <div class="col-lg-2 col-md-2">
                                <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Fecha Vigencia Final:</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtFechaVigenciaFinal" runat="server" CssClass="form-control form-control-sm" 
                                        Enabled="False" style="text-transform : uppercase" TabIndex="5" AutoPostBack="True"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvFechaVigenciaFinal" runat="server" 
                                        ControlToValidate="txtFechaVigenciaFinal" EnableClientScript="False" 
                                        ErrorMessage="Ingrese la fecha de vigencia final" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                    <ajaxToolkit:CalendarExtender ID="txtFechaVigenciaFinal_CalendarExtender" runat="server" 
                                        CssClass="calendario" Enabled="True" TargetControlID="txtFechaVigenciaFinal">
                                    </ajaxToolkit:CalendarExtender>
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
                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Código Cliente:</label>
                            <div class="input-group">
                                <asp:TextBox ID="txtIdCliente" runat="server" CssClass="form-control form-control-sm" TabIndex="17"></asp:TextBox><%--</td>--%>
                                <asp:LinkButton ID="btnAdicionarCliente" runat="server" CssClass="btn btn-primary btn-sm" Text="<span class='fas fa-binoculars'></span>" ToolTip="Crear Cliente" />
                                <asp:RequiredFieldValidator ID="rfvIdCliente" runat="server" 
                                    ControlToValidate="txtIdCliente" EnableClientScript="False" 
                                    ErrorMessage="Ingrese el código del cliente" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                                <input type=hidden runat=server id=hfdIdTipoDocumentoCliente/>
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
                                <asp:Panel ID="pnlMensajeCliente" runat="server" CssClass="container" >
                                    <div class="modal-dialog modal-sm">
                                        <div class="shadow rounded">
                                            <div class= "modal-dialog-scrollable">
                                                <div class="modal-content">
                                                    <div class="modal-header">
                                                        <h4 class="modal-title" id="myModalLabelMensajeCliente">
                                                            Mensaje Informativo</h4>
                                                        <asp:Button ID="imgbtnCancelarMensajeClienteImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                    </div>
                                                    <div class="modal-body">
                                                        <div class="row pt-2 pb-2">
                                                            <asp:Label ID="lblTituloMensajeCliente" runat="server" CssClass="col-sm-12 col-form-label-sm text-center">Se creó el siguiente número de cliente</asp:Label>
                                                            <div class="col-sm-12 text-center">
                                                                <h1><asp:Label ID="lblNroClienteMensajeCliente" runat="server" Text="00000" CssClass="col-form-label-sm h4" ></asp:Label></h1>
                                                            </div>
                                                            <div class="col-sm-12 text-center">
                                                                <asp:Label ID="lblDatoInformativoMensajeCliente" runat="server" Text="" CssClass="texto_estado short_estado light-orange borde_redondo1" ></asp:Label>
                                                            </div>
                                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                                <ContentTemplate>
                                                                    <div class="row pt-1">
                                                                        <label class="col-md-4 text-black-50 col-form-label col-form-label-sm">País:</label>
                                                                        <div class="col-md-8">
                                                                            <div class="input-group">
                                                                                <asp:DropDownList ID="cboPaisMensajeCliente" runat="server" AppendDataBoundItems="True" TabIndex="100" 
                                                                                    CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase" 
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
                                                                                    CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase" TabIndex="101"
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
                                                                                    CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase" 
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
                                                                                    CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase" TabIndex="103"
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
                                                                    <asp:Label ID="lblDireccionAdicionalMensajeCliente" runat="server" Text="Dirección:" CssClass="text-black-50 col-form-label col-form-label-sm" ></asp:Label>
                                                                    <asp:TextBox ID="txtDireccionAdicionalMensajeCliente" runat="server" CssClass="form-control form-control-sm" style="text-transform :uppercase;" TabIndex="104" ></asp:TextBox>
                                                                </div>
                                                                <div class="col-sm-12">
                                                                    <asp:Label ID="lblCorreoAdicionalMensajeCliente" runat="server" Text="Correo:" CssClass="text-black-50 col-form-label col-form-label-sm" ></asp:Label>
                                                                    <asp:TextBox ID="txtCorreoAdicionalMensajeCliente" runat="server" CssClass="form-control form-control-sm" style="text-transform :uppercase;" TabIndex="105" TextMode="Email"></asp:TextBox>
                                                                </div>
                                                                <div class="col-sm-12">
                                                                    <asp:Label ID="lblTelefonoMensajeCliente" runat="server" Text="Teléfono:" CssClass="text-black-50 col-form-label col-form-label-sm" ></asp:Label>
                                                                    <asp:TextBox ID="txtTelefonoMensajeCliente" runat="server" CssClass="form-control form-control-sm" style="text-transform :uppercase;" TabIndex="106" ></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <asp:Label ID="lblMensajeMensajeCliente" runat="server" style="text-align:center;" CssClass="col-form-label col-form-label-sm text-danger"></asp:Label>
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
                                <%--<asp:UpdatePanel ID="updpnlSeleccionarCliente" runat="server">
                                    <ContentTemplate>--%>
                                        <asp:Panel ID="pnlSeleccionarCliente" runat="server" CssClass="container" DefaultButton="imgbtnBuscarCliente" >
                                            <div class="modal-dialog modal-xl">
                                                <div class="shadow rounded">
                                                    <div class="modal-dialog-scrollable">
                                                        <div class="modal-content">
                                                            <div class="modal-header bg-light">
                                                                <h4 class="modal-title" id="myModalLabelCliente">
                                                                    Seleccionar Cliente</h4>
                                                                <asp:Button ID="imgbtnCancelarClienteImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row pt-2 pb-2">
                                                                    <span class="col-sm-2 col-form-label-sm">Buscar por</span>
                                                                    <div class="col-sm-4">
                                                                        <asp:DropDownList ID="cboFiltroCliente" runat="server" CssClass="form-select form-select-sm js-choice"
                                                                            TabIndex ="74">
                                                                            <asp:ListItem Value="vRazonSocialCliente">RAZ.SOC.</asp:ListItem>
                                                                            <asp:ListItem Value="cIdCliente">CODIGO</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="col-sm-6">
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtBuscarCliente" runat="server" CssClass="form-control form-control-sm" TabIndex="61"
                                                                                style="text-transform :uppercase" placeholder="Ingrese Busqueda" onfocus="fnSetFocus('ctl00$cuerpo$txtBuscarCliente')" ></asp:TextBox>
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
                                                                                EmptyDataText="No hay registros a visualizar" PageSize="6" >
                                                                                <PagerStyle CssClass="pgr" />
                                                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                                <AlternatingRowStyle CssClass="alt" />
                                                                                <Columns>
                                                                                    <asp:CommandField ShowSelectButton="True" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                                    <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                                    <asp:BoundField DataField="DNI" HeaderText="DNI" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                                    <asp:BoundField DataField="RUC" HeaderText="RUC" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                                    <asp:BoundField DataField="Descripcion" HeaderText="Razón Social" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                                    <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                                    <asp:BoundField DataField="Direccion" HeaderText="Dirección" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                                </Columns>
                                                                                <HeaderStyle CssClass="thead-dark" />
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:Label ID="lblMensajeCliente" runat="server" CssClass="col-form-label col-form-label-sm text-danger"></asp:Label>
                                                                <asp:Button ID="btnAceptarCliente" runat="server" ValidationGroup="vgrpValidarBusqueda"
                                                                    ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm d-block" />
                                                                &nbsp;
                                                                <asp:Button ID="btnCancelarCliente" runat="server" 
                                                                    ToolTip="Cancelar Registro" TabIndex ="73" Text="Cancelar" cssclass="btn btn-outline-google-plus btn-sm d-block"/>
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
                        <div class="col-lg-5 col-md-4 mt-2">
                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Razón Social:</label>
                            <div class="input-group">
                                <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="form-control form-control-sm" 
                                    Enabled="False" TabIndex="18" style="text-transform :uppercase" placeholder="Razón Social"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvRazonSocial" runat="server" 
                                    ControlToValidate="txtRazonSocial" EnableClientScript="False" 
                                    ErrorMessage="Ingrese la razón social" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>
              </div>
              <div class="card">
                <div class="card-header bg-light">
                    <div class="row align-items-center">
                        <div class="col-5">
                            <h6 class="mb-0">Lista de Equipos Asignados</h6>
                        </div>
                        <div class="col-5">
                            <div class="row">
                                <div class="col-lg-3 col-md-2 mt-2">
                                    <span class="fas fa-calendar"></span><label class="text-black-50 col-form-label fs--2 m-1">Periodo:</label>
                                </div>
                                <div class="col-lg-3 col-md-2 mt-2">
                                    <div class="input-group">
                                        <asp:DropDownList ID="cboPeriodo" runat="server" AppendDataBoundItems="True" 
                                            AutoPostBack="True" CssClass="form-select form-select-sm js-choice" 
                                            style="text-transform : uppercase" Width="170px" TabIndex="2">
                                            <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-lg-2 col-md-2 mt-2">
                                    <span class="fas fa-calendar-week"></span><label class="text-black-50 col-form-label fs--2 m-1">Mes:</label>
                                </div>
                                <div class="col-lg-4 col-md-2 mt-2">
                                    <div class="input-group">
                                        <asp:DropDownList ID="cboMes" runat="server" CssClass="form-select form-select-sm js-choice" style="text-transform : uppercase" Width="50px" TabIndex="3" AutoPostBack="True">
                                            <asp:ListItem Value="1">01 - ENERO</asp:ListItem>
                                            <asp:ListItem Value="2">02 - FEBRERO</asp:ListItem>
                                            <asp:ListItem Value="3">03 - MARZO</asp:ListItem>
                                            <asp:ListItem Value="4">04 - ABRIL</asp:ListItem>
                                            <asp:ListItem Value="5">05 - MAYO</asp:ListItem>
                                            <asp:ListItem Value="6">06 - JUNIO</asp:ListItem>
                                            <asp:ListItem Value="7">07 - JULIO</asp:ListItem>
                                            <asp:ListItem Value="8">08 - AGOSTO</asp:ListItem>
                                            <asp:ListItem Value="9">09 - SETIEMBRE</asp:ListItem>
                                            <asp:ListItem Value="10">10 - OCTUBRE</asp:ListItem>
                                            <asp:ListItem Value="11">11 - NOVIEMBRE</asp:ListItem>
                                            <asp:ListItem Value="12">12 - DICIEMBRE</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-2">
                            <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                                <div class="dropdown font-sans-serif btn-reveal-trigger">
                                    <span class="fas fa-cog"></span><label class="text-black-50 col-form-label fs--2 m-1">Opciones:</label>
                                    <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesComponentesCatalogo" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                                    <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesComponentesCatalogo">
                                        <asp:LinkButton ID="lnkbtnAgregarEquipo" runat="server" ToolTip="Agregar equipo al contrato" Text="<span class='fas fa-file-signature me-2'></span>Agregar Equipo" CssClass="nav-link nav-link-card-details" />
                                    </div>


                                    <asp:LinkButton ID="lnk_mostrarPanelSeleccionarEquipo" runat="server"></asp:LinkButton>
                                    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelSeleccionarEquipo_ModalPopupExtender" runat="server" 
                                        BackgroundCssClass="FondoAplicacion" DropShadow="False" DynamicServicePath="" Enabled="True" 
                                        CancelControlID="btnCancelarSeleccionarEquipo" 
                                        PopupControlID="pnlSeleccionarEquipo" TargetControlID="lnk_mostrarPanelSeleccionarEquipo">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="pnlSeleccionarEquipo" runat="server" CssClass="container" DefaultButton="imgbtnBuscarSeleccionarEquipo" >
                                        <div class="modal-dialog modal-xl">
                                            <div class="shadow rounded">
                                                <div class="modal-dialog-scrollable">
                                                    <div class="modal-content">
                                                        <div class="modal-header bg-light">
                                                            <h4 class="modal-title" id="myModalLabelSeleccionarEquipo">
                                                                Seleccionar Equipo</h4>
                                                            <asp:Button ID="imgbtnCancelarSeleccionarEquipoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                                        </div>
                                                        <div class="modal-body">
                                                            <div class="row pt-2 pb-2">
                                                                <span class="col-sm-2 col-form-label-sm">Buscar por</span>
                                                                <div class="col-sm-4">
                                                                    <asp:DropDownList ID="cboFiltroSeleccionarEquipo" runat="server" CssClass="form-select form-select-sm js-choice"
                                                                        TabIndex ="74">
                                                                        <asp:ListItem Value="vDescripcionEquipo">NOMBRE EQUIPO</asp:ListItem>
                                                                        <asp:ListItem Value="cIdEquipo">CODIGO</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div class="col-sm-6">
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtBuscarSeleccionarEquipo" runat="server" CssClass="form-control form-control-sm" TabIndex="61"
                                                                            style="text-transform :uppercase" placeholder="Ingrese Busqueda" onfocus="fnSetFocus('ctl00$cuerpo$txtBuscarSeleccionarEquipo')" ></asp:TextBox>
                                                                        &nbsp;
                                                                        <asp:ImageButton ID="imgbtnBuscarSeleccionarEquipo" runat="server" Height="18px" TabIndex="3" CssClass="mt-1"
                                                                            ImageUrl="~/Imagenes/buscar.png" ToolTip="Buscar Registro" Width="21px" onfocus="fnSetFocus('ctl00$cuerpo$txtBuscarSeleccionarEquipo')" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-12 mt-3">
                                                                    <div style="overflow: auto">
                                                                        <asp:GridView ID="grdListaSeleccionarEquipo" runat="server" AutoGenerateColumns="False" TabIndex="63"
                                                                            GridLines="None" CssClass="table table-responsive-sm table-striped table-hover small" AllowPaging="True" 
                                                                            EmptyDataText="No hay registros a visualizar" PageSize="6" >
                                                                            <PagerStyle CssClass="pgr" />
                                                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                            <AlternatingRowStyle CssClass="alt" />
                                                                            <Columns>
                                                                                <asp:CommandField ShowSelectButton="True" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                                <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                                <asp:BoundField DataField="IdTipoActivo" HeaderText="Id.Tipo Activo" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                                <asp:BoundField DataField="IdCatalogo" HeaderText="Id.Catálogo" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                                <asp:BoundField DataField="Descripcion" HeaderText="Equipo" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                                <%--<asp:BoundField DataField="DNI" HeaderText="DNI" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                                <asp:BoundField DataField="RUC" HeaderText="RUC" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                                <asp:BoundField DataField="Descripcion" HeaderText="Razón Social" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                                <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900"/>
                                                                                <asp:BoundField DataField="Direccion" HeaderText="Dirección" HeaderStyle-CssClass="bg-200 text-900"/>--%>
                                                                            </Columns>
                                                                            <HeaderStyle CssClass="thead-dark" />
                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="modal-footer">
                                                            <asp:Label ID="lblMensajeSeleccionarEquipo" runat="server" CssClass="col-form-label col-form-label-sm text-danger"></asp:Label>
                                                            <asp:Button ID="btnAceptarSeleccionarEquipo" runat="server" ValidationGroup="vgrpValidarBusqueda"
                                                                ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm d-block" />
                                                            &nbsp;
                                                            <asp:Button ID="btnCancelarSeleccionarEquipo" runat="server" 
                                                                ToolTip="Cancelar Registro" TabIndex ="73" Text="Cancelar" cssclass="btn btn-outline-google-plus btn-sm d-block"/>
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



                      <%--<asp:UpdatePanel ID="updpnlMantenimientoOrdenTrabajo" runat="server">--%><%-- updatemode="Conditional">--%>
        <%--<Triggers>
            <asp:AsyncPostBackTrigger controlid="btnAceptarMantenimientoOrdenTrabajo" eventname="Click" />
        </Triggers>--%>
        <%--<ContentTemplate>--%>
            <asp:LinkButton ID="lnk_mostrarPanelMantenimientoOrdenTrabajo" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMantenimientoOrdenTrabajo_ModalPopupExtender" runat="server" 
                BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True" 
                CancelControlID="btnCancelarMantenimientoOrdenTrabajo" 
                PopupControlID="pnlMantenimientoOrdenTrabajo" TargetControlID="lnk_mostrarPanelMantenimientoOrdenTrabajo">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlMantenimientoOrdenTrabajo" runat="server" CssClass="container" DefaultButton="btnAceptarMantenimientoOrdenTrabajo" >
                <div class="modal-dialog modal-xl">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header bg-light">
                                    <h4 class="modal-title" id="myModalLabelOrdenTrabajo">
                                        Mantenimiento Orden de Trabajo</h4>
                                    <asp:Button ID="imgbtnCancelarOrdenTrabajoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="scrollbar" style="height:380px;">
                                            <div class="row">
                                                <div class="col-lg-2 col-md-5 col-sm-6 mt-2">
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Fecha Inicio</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtFechaInicioPlanificadaMantenimientoOrdenTrabajo" runat="server" CssClass="form-control form-control-sm" AutoPostBack="true"
                                                            style="text-transform :uppercase" TabIndex="18" autocomplete="off" ></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                                            ControlToValidate="txtFechaInicioPlanificadaMantenimientoOrdenTrabajo" EnableClientScript="False" 
                                                            ErrorMessage="Ingrese la fecha de inicio de planificación" Font-Size="10px" ForeColor="Red"
                                                            ValidationGroup="vgrpValidarCrearProducto">(*)</asp:RequiredFieldValidator>
                                                        <ajaxToolkit:CalendarExtender ID="txtFechaInicioPlanificadaMantenimientoOrdenTrabajo_CalendarExtender" runat="server" 
                                                            CssClass="calendario" Enabled="True" TargetControlID="txtFechaInicioPlanificadaMantenimientoOrdenTrabajo">
                                                        </ajaxToolkit:CalendarExtender>
                                                    </div>
                                                </div>

                                                <div class="col-lg-2 col-md-2 mt-2">
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Nro.Ord.Trab.Cliente:</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtNroOTClienteMantenimientoOrdenTrabajo" runat="server" CssClass="form-control form-control-sm" 
                                                            TabIndex="11" style="text-transform :uppercase" placeholder="Número del contrato asignado por el cliente"></asp:TextBox>
                                                    </div>
                                                </div>


                                                <div class="col-lg-4 col-md-7 col-sm-6 mt-2">
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Tipo de Mantenimiento</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboTipoMantenimientoMantenimientoOrdenTrabajo" runat="server" AutoPostBack="True" 
                                                            class="form-select form-select-sm js-choice" style="text-transform : uppercase" 
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
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Plantilla CheckList</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboListadoCheckListMantenimientoOrdenTrabajo" runat="server" AutoPostBack="True" 
                                                            class="form-select form-select-sm js-choice" style="text-transform : uppercase" 
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
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Responsable</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboPersonalResponsableMantenimientoOrdenTrabajo" runat="server" AppendDataBoundItems="True" AutoPostBack="True" 
                                                            class="form-select form-select-sm js-choice" style="text-transform : uppercase" 
                                                            TabIndex="13">
                                                            <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                        </asp:DropDownList>
												        <asp:RequiredFieldValidator ID="rfvPersonalResponsableMantenimientoOrdenTrabajo" runat="server" 
													        ControlToValidate="cboPersonalResponsableMantenimientoOrdenTrabajo" EnableClientScript="False" 
													        ErrorMessage="Ingrese el personal responsable" Font-Size="10px" ForeColor="Red" 
													        ValidationGroup="vgrpValidarMantenimientoOrdenTrabajo">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row bg-200">
                                                <div class="col-lg-12 col-md-12 col-sm-12 mt-2">
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Personal Asignado</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboPersonalAsignadoMantenimientoOrdenTrabajo" runat="server" AppendDataBoundItems="True" AutoPostBack="True" 
                                                            class="form-select form-select-sm js-choice" style="text-transform : uppercase" 
                                                            TabIndex="14">
                                                            <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btnAdicionarPersonalAsignadoMantenimientoOrdenTrabajo" runat="server" Text="Agregar" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Personal" TabIndex="30"/>
												        <asp:RequiredFieldValidator ID="rfvPersonalAsignadoMantenimientoOrdenTrabajo" runat="server" 
													        ControlToValidate="cboPersonalAsignadoMantenimientoOrdenTrabajo" EnableClientScript="False" 
													        ErrorMessage="Ingrese el personal asignado" Font-Size="10px" ForeColor="Red" 
													        ValidationGroup="vgrpValidarMantenimientoOrdenTrabajo">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 mt-2">
                                                    <div id="divGridPersonalAsignado" style="overflow:auto; height:220px; align:left;">
                                                        <asp:GridView ID="grdPersonalAsignadoMantenimientoOrdenTrabajo" runat="server" AutoGenerateColumns="False" TabIndex="17"
                                                            GridLines="None" CssClass="table table-responsive-sm table-striped table-hover small" AllowPaging="True" 
                                                            EmptyDataText="No hay registros a visualizar" PageSize="1000">
                                                            <PagerStyle CssClass="pgr" />
                                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <%--<asp:CommandField ShowSelectButton="True" HeaderStyle-CssClass="bg-200 text-900"/>--%>
                                                                <asp:CommandField ShowDeleteButton="True" HeaderStyle-Width="70px" />
                                                                <asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900"/>  
                                                                <asp:BoundField DataField="Personal" HeaderText="Descripción" HeaderStyle-CssClass="bg-200 text-900"/>  
                                                                <asp:BoundField DataField="Responsable" HeaderText="Responsable" HeaderStyle-CssClass="bg-200 text-900"/>  
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
                                    <%--<div class="container">
                                        <div class="row">
                                            <div class="col-md-8 mt-2">
                                                <asp:ValidationSummary ID="ValidationSummary3" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarMantenimientoOrdenTrabajo" />
                                            </div>
                                            <div class="col-md-4 text-end p-2">--%>
                                                <asp:Label ID="lblMensajeMantenimientoOrdenTrabajo" runat="server" CssClass="col-form-label col-form-label-sm text-danger"></asp:Label>

                                                <asp:Button ID="btnAceptarMantenimientoOrdenTrabajo" runat="server" ValidationGroup="vgrpValidarMantenimientoOrdenTrabajo"
                                                    ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                &nbsp;
                                                <asp:Button ID="btnCancelarMantenimientoOrdenTrabajo" runat="server" 
                                                    ToolTip="Cancelar Registro" TabIndex ="73" Text="Cancelar" cssclass="btn btn-outline-google-plus btn-sm"/>
                                            <%--</div>
                                        </div>
                                    </div>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>




                <div class="card-body h-100">
                    <div class="row justify-content-between mt-3"> 
						<div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
							<div id="divGridDetalleAgregarEquipos" style="overflow:auto; height:160px; align:left;" class="scrollbar">
								<asp:Panel ID="pnlDetalleEquipos" runat="server" CssClass="bg-light" >
									<div class="table-responsive scrollbar">
									<asp:GridView ID="grdDetalleEquipos" runat="server" AutoGenerateColumns="False" TabIndex="4" 
										GridLines="None" CssClass="table table-striped table-hover small overflow-hidden" AllowPaging="True" 
										EmptyDataText="No hay registros a visualizar" PageSize="3" OnRowCommand="grdDetalleEquipos_RowCommand_Botones" >
										<PagerStyle CssClass="mGrid" />
										<SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
										<Columns>
                                            <asp:TemplateField HeaderText="Mant." HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="imgbtnGenerarOrdenTrabajo" runat="server" CommandName="GenerarOT" CssClass="btn btn-primary btn-sm" CommandArgument='<%#  Eval("D1") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' Text="<span class='fas fa-calendar-day'></span>"/>
													<asp:LinkButton ID="imgbtnEliminarOrdenTrabajo" runat="server" CommandName="EliminarEquipo" CssClass="btn btn-danger btn-sm" CommandArgument='<%#  Eval("D1") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' Text="<span class='fas fa-calendar-times'></span>"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="IdEquipo" HeaderText="Código" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" />
                                            <asp:BoundField DataField="DescripcionEquipo" HeaderText="Descripción" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" />
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <%--<asp:LinkButton ID="lnkbtnD1" runat="server" Text='<%# IIf(IsNothing(Eval("D1")), "||||", Mid(Eval("D1").ToString, 1, 3)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D1") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />--%>
                                                    <asp:LinkButton ID="lnkbtnD1" runat="server" Text='<%# IIf(IsNothing(Eval("D1")), "", Split(Eval("D1").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D1") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <%--<asp:LinkButton ID="lnkbtnD2" runat="server" Text='<%# IIf(Eval("D2") = "||||", "", Mid(Eval("D2").ToString, 1, 3)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D2") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />--%>
                                                    <asp:LinkButton ID="lnkbtnD2" runat="server" Text='<%# IIf(IsNothing(Eval("D2")), "", Split(Eval("D2").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D2") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD3" runat="server" Text='<%# IIf(IsNothing(Eval("D3")), "", Split(Eval("D3").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D3") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD4" runat="server" Text='<%# IIf(IsNothing(Eval("D4")), "", Split(Eval("D4").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D4") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD5" runat="server" Text='<%# IIf(IsNothing(Eval("D5")), "", Split(Eval("D5").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D5") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD6" runat="server" Text='<%# IIf(IsNothing(Eval("D6")), "", Split(Eval("D6").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D6") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD7" runat="server" Text='<%# IIf(IsNothing(Eval("D7")), "", Split(Eval("D7").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D7") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD8" runat="server" Text='<%# IIf(IsNothing(Eval("D8")), "", Split(Eval("D8").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D8") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD9" runat="server" Text='<%# IIf(IsNothing(Eval("D9")), "", Split(Eval("D9").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D9") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD10" runat="server" Text='<%# IIf(IsNothing(Eval("D10")), "", Split(Eval("D10").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D10") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD11" runat="server" Text='<%# IIf(IsNothing(Eval("D11")), "", Split(Eval("D11").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D11") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD12" runat="server" Text='<%# IIf(IsNothing(Eval("D12")), "", Split(Eval("D12").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D12") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD13" runat="server" Text='<%# IIf(IsNothing(Eval("D13")), "", Split(Eval("D13").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D13") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD14" runat="server" Text='<%# IIf(IsNothing(Eval("D14")), "", Split(Eval("D14").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D14") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD15" runat="server" Text='<%# IIf(IsNothing(Eval("D15")), "", Split(Eval("D15").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D15") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD16" runat="server" Text='<%# IIf(IsNothing(Eval("D16")), "", Split(Eval("D16").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D16") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD17" runat="server" Text='<%# IIf(IsNothing(Eval("D17")), "", Split(Eval("D17").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D17") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD18" runat="server" Text='<%# IIf(IsNothing(Eval("D18")), "", Split(Eval("D18").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D18") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD19" runat="server" Text='<%# IIf(IsNothing(Eval("D19")), "", Split(Eval("D19").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D19") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD20" runat="server" Text='<%# IIf(IsNothing(Eval("D20")), "", Split(Eval("D20").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D20") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD21" runat="server" Text='<%# IIf(IsNothing(Eval("D21")), "", Split(Eval("D21").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D21") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD22" runat="server" Text='<%# IIf(IsNothing(Eval("D22")), "", Split(Eval("D22").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D22") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD23" runat="server" Text='<%# IIf(IsNothing(Eval("D23")), "", Split(Eval("D23").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D23") & "*" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD24" runat="server" Text='<%# IIf(IsNothing(Eval("D24")), "", Split(Eval("D24").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D24") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD25" runat="server" Text='<%# IIf(IsNothing(Eval("D25")), "", Split(Eval("D25").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D25") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD26" runat="server" Text='<%# IIf(IsNothing(Eval("D26")), "", Split(Eval("D26").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D26") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD27" runat="server" Text='<%# IIf(IsNothing(Eval("D27")), "", Split(Eval("D27").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D27") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD28" runat="server" Text='<%# IIf(IsNothing(Eval("D28")), "", Split(Eval("D28").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D28") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD29" runat="server" Text='<%# IIf(IsNothing(Eval("D29")), "", Split(Eval("D29").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D29") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD30" runat="server" Text='<%# IIf(IsNothing(Eval("D30")), "", Split(Eval("D30").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D30") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small">
												<ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnD31" runat="server" Text='<%# IIf(IsNothing(Eval("D31")), "", Split(Eval("D31").ToString, "|")(0)) %>' ToolTip="Editar Mantenimiento" CommandName="VerMantenimientoPlanificacionEquipo" CommandArgument='<%# Eval("D31") & "|" & Container.DataItemIndex & "|" & Eval("IdEquipo") %>' />
												</ItemTemplate>
											</asp:TemplateField>
										</Columns>
										<HeaderStyle CssClass="thead-dark" />
									</asp:GridView>
									</div>
								</asp:Panel>
							</div>
						</div>
					</div>
                </div>
              </div>
              <div class="position-fixed bottom-0 end-0 p-3">
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidar" />
              </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    

    <asp:UpdatePanel ID="updpnlMensajeDocumento" runat="server">
        <ContentTemplate>
            <asp:LinkButton ID="lnk_mostrarPanelMensajeDocumento" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMensajeDocumento_ModalPopupExtender" 
                runat="server" BackgroundCssClass="FondoAplicacion" 
                CancelControlID="imgbtnCancelarDocumentoImagen" DropShadow="False" 
                DynamicServicePath="" Enabled="True" PopupControlID="pnlMensajeDocumento" 
                TargetControlID="lnk_mostrarPanelMensajeDocumento">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlMensajeDocumento" runat="server" CssClass="container">
                <div class="modal-dialog modal-sm">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title" id="myModalLabelMensajeDocumento">
                                        Mensaje Informativo</h4>
                                    <asp:Button ID="imgbtnCancelarDocumentoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                 <div class="modal-body">
                                    <div class="form-group row pt-2">
                                        <span class="col-sm-12 col-form-label-sm text-center text-900"><asp:Label ID="lblDescripcionMensajeDocumento" runat="server" Text="Se creó el siguiente número de documento"></asp:Label></span>
                                        <h3 class="col-sm-12 text-center"><asp:Label ID="lblNroDocumentoMensajeDocumento" runat="server" Text="00000" CssClass="text-info" ></asp:Label></h3>
                                        <div class="col-sm-12 text-center">
                                            <asp:Label ID="lblMensajeDocumento" runat="server" Text="" CssClass="texto_estado short_estado light-orange borde_redondo1" ></asp:Label>
                                        </div>
                                    </div>       
                                </div>       
                                <div class="modal-footer">
                                    <%--<asp:Button ID="btnImprimirMensajeDocumento" runat="server" Text="Imprimir" CssClass="btn btn-outline-facebook btn-sm d-block" ToolTip="Imprimir Contrato" />
                                    &nbsp;&nbsp;<asp:Button ID="btnNuevoMensajeDocumento" runat="server" Text="Nuevo Contrato" CssClass="btn btn-outline-facebook btn-sm d-block" ToolTip="Nuevo Comprobante" />--%>
                                    <asp:Button ID="btnSiMensajeDocumento" runat="server" Text="Si" CssClass="btn btn-outline-facebook btn-sm d-block" ToolTip="Confirmar Eliminación" />
                                    &nbsp;&nbsp;<asp:Button ID="btnNoMensajeDocumento" runat="server" Text="No" CssClass="btn btn-outline-facebook btn-sm d-block" ToolTip="Cancelar Eliminación" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">
        //function popupEmitirEquipoDetalleReporte(IdEquipo) {
        //    window.open("Informes/frmCmmsEquipoDetalleReporte.aspx?IdEqu=" + IdEquipo, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
        //}
        function popupEmitirContratoReporte(TipDoc, NroSer, NroDoc) {
            window.open("Informes/frmCmmsContratoReporte.aspx?TipDoc=" + TipDoc + "&NroSerie=" + NroSer + "&NroDoc=" + NroDoc, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
        }
        //function popupEmitirContratoProgramacionReporte(TipDoc, NroSer, NroDoc, NroEquipos) {
        function popupEmitirContratoProgramacionReporte(TipDoc, NroSer, NroDoc, FecIni, FecFin, NroEquipos) {
            //window.open("Informes/frmCmmsContratoProgramacionReporte.aspx?TipDoc=" + TipDoc + "&NroSerie=" + NroSer + "&NroDoc=" + NroDoc + "&NroEquipos=" + NroEquipos, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
            window.open("Informes/frmCmmsContratoProgramacionReporte.aspx?TipDoc=" + TipDoc + "&NroSerie=" + NroSer + "&NroDoc=" + NroDoc + "&FecIni=" + FecIni + "&FecFin=" + FecFin + "&NroEquipos=" + NroEquipos, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
        }

    </script>

    <link href="cssbootstrap/bootstrap-switch.css" rel="stylesheet" />
    <script src="jsbootstrap/bootstrap-switch.js"></script>
</asp:Content>
