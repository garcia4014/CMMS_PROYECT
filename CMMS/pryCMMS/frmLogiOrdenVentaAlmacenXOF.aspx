<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmLogiSolicitudServicioAlmacenXOF.aspx.vb" Inherits="pryCMMS.frmLogiSolicitudServicioAlmacenXOF" %>
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
            <asp:HiddenField ID="hfdIdClienteSAPEquipo" runat="server" />
            <asp:HiddenField ID="hfdIdEquipoSAPEquipo" runat="server" />
            <asp:HiddenField ID="hfdIdArticuloSAPPrincipal" runat="server" />
            <asp:HiddenField ID="hfdFechaCreacionEquipo" runat="server" />
            <asp:HiddenField ID="hfdIdUsuarioCreacionEquipo" runat="server" />
            <asp:Panel ID="pnlCabecera" runat="server">
                <div class="card mb-3">
                    <div class="card-header">
                      <div class="row flex-between-end">
                        <div class="col-auto align-self-center">
                            <div class="nav nav-pills gap-2" role="tablist">
                                <h5 class="mb-0" data-anchor="data-anchor">Listado de Ordenes</h5>
                                <a class="btn btn-falcon-default btn-sm ml-auto float-right" href="frmBienvenida.aspx" aria-expanded="true">
                                    <span class="fas fa-home"></span><span class="nav-link-text ps-1">Inicio</span>
                                </a>
                            </div>
                        </div>
                        <div class="col-auto ms-auto">
                          <div class="nav gap-2" role="tablist">
                            <asp:LinkButton ID="btnGenerar" runat="server" OnClick="btnGenerar_Click" CssClass="btn btn-success btn-sm ml-auto float-right" Text="<span class='fas fa-plus'></span> Ver Detalle OF" ToolTip="Control de existencias por orden de fabricación" />
                            <asp:LinkButton ID="btnAsignarAsesor" runat="server" CssClass="btn btn-success btn-sm ml-auto float-right" Text="<span class='fas fa-plus'></span> Asignar Responsable" ToolTip="Asignar responsable para la orden de fabricación" />
                            <div class="dropdown font-sans-serif btn-reveal-trigger">
                              <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesEquipo" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                              <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesEquipo">
                                  <%--<asp:LinkButton ID="lnkbtnAsignarAsesor" runat="server" ToolTip="Asignar Asesor a la Orden" Text="<span class='fas fa-file-signature me-2'></span>Asignar Asesor" CssClass="nav-link nav-link-card-details" />--%>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                    <div class="card-body">
                        <div class="row bg-100 p-2">
					        <div class="col-lg-6 col-sm-6">
                                <span class="fas fa-filter"></span><label class="text-black-50 col-form-label fs--2 m-1">Filtrar por:</label>
                                <div class="input-group">
						            <asp:DropDownList ID="cboFiltroEquipo" runat="server" CssClass="form-select form-select-sm js-choice" TabIndex="1">
							            <asp:ListItem>SELECCIONE EL FILTRO</asp:ListItem>
						            </asp:DropDownList>
					            </div>
					        </div>
					        <div class="col-lg-6 col-sm-6">
                                <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Buscar por:</label>
						        <div class="input-group">
							        <asp:TextBox ID="txtBuscarEquipo" runat="server" CssClass="form-control form-control-sm" TabIndex="2"
								        style="text-transform :uppercase" placeholder="Ingrese Busqueda"></asp:TextBox>
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
                                                <asp:BoundField DataField="IdTipoDocumento" HeaderText="Tip.Doc." HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:BoundField DataField="IdNumeroSerie" HeaderText="Nro.Serie" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:BoundField DataField="IdNumeroCorrelativo" HeaderText="Nro.Correlativo" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:BoundField DataField="OrdenCompra" HeaderText="O.C." HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:BoundField DataField="OrdenVenta" HeaderText="O.V." HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:BoundField DataField="FechaOrdenVenta" HeaderText="Fec.Hor.O.V." DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:BoundField DataField="OrdenFabricacion" HeaderText="O.F." HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:BoundField DataField="FechaEmision" HeaderText="Fec.Hor.O.F." DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:BoundField DataField="DescripcionOrdenFabricacion" HeaderText="Descripción de Orden" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:BoundField DataField="CantidadItems" HeaderText="Cant. Items" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:BoundField DataField="IdCliente" HeaderText="Id.Cliente" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:BoundField DataField="IdClienteSAP" HeaderText="Id. Cliente SAP" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:TemplateField HeaderText="Cliente" ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small">
                                                    <ItemTemplate>
                                                        <div class="col-12">
                                                            <asp:Label ID="lblRucCliente" runat="server" Visible='<%# If(Eval("RucCliente") = "", "false", "true") %>' Text='<%# Eval("RucCliente") %>'></asp:Label>
                                                        </div>
                                                        <div class="col-12">
                                                            <asp:Label ID="lblRazonSocialCliente" runat="server" Visible='<%# If(Eval("RazonSocial") = "", "false", "true") %>' Text='<%# Eval("RazonSocial") %>'></asp:Label>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="NombreCompletoAuxiliar" HeaderText="Asesor Auxiliar" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:BoundField DataField="FechaAsignacionAuxiliar" HeaderText="Fec.Asignación" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:BoundField DataField="FechaProgramacionAlmacen" HeaderText="Fec.Asignación" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:TemplateField HeaderText="Estado" ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEstado" runat="server" Visible='<%# If(Eval("StatusOrdenFabricacion") = "", "false", "true") %>' CssClass='<%#If(Eval("StatusOrdenFabricacion") = "R", "texto_estado short_estado black borde_redondo1", If(Eval("StatusOrdenFabricacion") = "T", "texto_estado short_estado verde borde_redondo1", "texto_estado short_estado orange borde_redondo1")) %>' Text='<%# StrConv(IIf(Eval("StatusOrdenFabricacion") = "P", "PENDIENTE", IIf(Eval("StatusOrdenFabricacion") = "T", "TERMINADO", "REGISTRADO")), VbStrConv.ProperCase) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Estado" HeaderText="Estado" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:BoundField DataField="IdEquipoSAP" HeaderText="Id.Equipo SAP" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:BoundField DataField="IdUnidadTrabajo" HeaderText="Unid.Trabajo" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:BoundField DataField="IdEquipo" HeaderText="IdEquipo" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:BoundField DataField="NumeroSerieEquipo" HeaderText="IdEquipo" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <asp:BoundField DataField="IdArticuloSAPCabecera" HeaderText="IdArticuloSAP" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small"/>
                                                <%--<asp:TemplateField HeaderText="Ord.Ref.SAP" ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small">
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
                                                </asp:TemplateField>--%>
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

            <asp:Panel ID="pnlControlDespacho" runat="server" CssClass="container">
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
                      <h6 class="mt-2 mb-1">DATOS PRINCIPALES</h6>
                      <hr class="bg-300 m-0" />
                    </div>
                  </div>
                  <div class="row mt-1">
                    <div class="col-12 mb-1">
                      <div class="row">
                        <%--<div class="col-lg-12 col-md-12 mt-2">
                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción Orden:</label>
                            <div class="input-group">
                                <asp:TextBox ID="txtDescripcionOrden" runat="server" CssClass="form-control form-control-sm" TabIndex="10" placeholder="Descripción de la orden"></asp:TextBox>
                                <asp:HiddenField ID="hfdFechaEquipo" runat="server" />
                            </div>
                        </div>--%>
                        <div class="col-lg-4 col-md-4 mt-2">
                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Nro. de Orden:</label>
                            <div class="input-group">
                                <p class="fs--1 m-0"><asp:Label ID="lblNroOrden" runat="server" Text="" CssClass="text-500 small"></asp:Label></p>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-4 mt-2">
                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Orden Venta:</label>
                            <div class="input-group">
                                <p class="fs--1 m-0"><asp:Label ID="lblNroOrdenVenta" runat="server" Text="" CssClass="text-500 small"></asp:Label></p>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-4 mt-2">
                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción Orden:</label>
                            <div class="input-group">
                                <p class="fs--1 m-0"><asp:Label ID="lblDescripcionOrden" runat="server" Text="" CssClass="text-500 small"></asp:Label></p>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-4 mt-2">
                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Ruc:</label>
                            <div class="input-group">
                                <p class="fs--1 m-0"><asp:Label ID="lblRucCliente" runat="server" Text="" CssClass="text-500 small"></asp:Label></p>
                            </div>
                        </div>

                        <div class="col-lg-4 col-md-4 mt-2">
                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Razón Social:</label>
                            <div class="input-group">
                                <p class="fs--1 m-0"><asp:Label ID="lblRazonSocialCliente" runat="server" Text="" CssClass="text-500 small"></asp:Label></p>
                            </div>
                        </div>

                        <%--<div class="col-lg-8 col-md-6 mt-2">
                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción Equipo:</label>
                            <div class="input-group">
                                <asp:TextBox ID="txtDescripcionEquipo" runat="server" CssClass="form-control form-control-sm" 
                                    TabIndex="11" style="text-transform :uppercase" placeholder="Descripción del equipo" autocomplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDescripcionEquipo" runat="server" 
                                    ControlToValidate="txtDescripcionEquipo" EnableClientScript="False" 
                                    ErrorMessage="Ingrese la descripción del equipo" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                            </div>
                        </div>--%>
                      </div>
                      <%--<div class="row">
                        <div class="col-lg-4 col-md-6 mt-2">
                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">N.Ser.:</label>
                            <div class="input-group">
                                <asp:TextBox ID="txtNroSerieEquipo" runat="server" CssClass="form-control form-control-sm" 
                                    TabIndex="12" style="text-transform :uppercase" placeholder="Número de serie del equipo" autocomplete="off" AutoPostBack="True"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-6 mt-2">
                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">#Part:</label>
                            <div class="input-group">
                                <asp:TextBox ID="txtNroParteEquipo" runat="server" CssClass="form-control form-control-sm" 
                                    TabIndex="13" style="text-transform :uppercase" placeholder="Número de parte del equipo" autocomplete="off" AutoPostBack="True"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-6 mt-2">
                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Tag:</label>
                            <div class="input-group">
                                <asp:TextBox ID="txtTagEquipo" runat="server" CssClass="form-control form-control-sm" 
                                    TabIndex="14" style="text-transform :uppercase" placeholder="Tag del equipo" autocomplete="off" AutoPostBack="True"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-6 mt-2">
                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Capacidad:</label>
                            <div class="input-group">
                                <asp:TextBox ID="txtCapacidadEquipo" runat="server" CssClass="form-control form-control-sm" 
                                    TabIndex="15" style="text-transform :uppercase" placeholder="Tag del equipo" autocomplete="off" AutoPostBack="True"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-8 col-md-6 mt-2">
                            <span class="far fa-clone"></span><label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Tipo Equipo:</label>
                            <div class="input-group">
                                <asp:DropDownList ID="cboTipoEquipo" runat="server" AppendDataBoundItems="True" 
                                    AutoPostBack="True" CssClass="form-select form-select-sm js-choice" 
                                    style="text-transform : uppercase" TabIndex="16">
                                    <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                </asp:DropDownList>
							</div>
                        </div>	
                      </div>--%>
                    </div>
                    <div class="col-12 bg-200">
                        <div class="row mt-1">
                            <div class="col-12 mb-0 d-none d-md-block">
                                <h6 class="mt-2 mb-1">CONTROL DE DESPACHO</h6>
                                <hr class="bg-300 m-0" />
                            </div>
                        </div>
                        <asp:UpdatePanel ID="updpnlDetalleControlDespacho" runat="server">
                                    <ContentTemplate>
                        <div class="row justify-content-between mt-3"> 
                            <div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
                                <%--Inicio:Se utiliza para que al seleccionar la grilla no se posicione al inicio SNIF SNIF NO FUNCIONAAAAAAAA REVISAR--%>
								<script type="text/javascript">
                                    var xPos, yPos;
                                    var xPos1, yPos1;
                                    var xPos2, yPos2;
                                    var prm = Sys.WebForms.PageRequestManager.getInstance();
                                    prm.add_beginRequest(BeginRequestHandler);
                                    prm.add_endRequest(EndRequestHandler);
                                    function BeginRequestHandler(sender, args) {
                                        xPos = $get("divGridDetalleControlDespacho").scrollLeft;
                                        yPos = $get("divGridDetalleControlDespacho").scrollTop;
                                    }
                                    function EndRequestHandler(sender, args) {
                                        $get("divGridDetalleControlDespacho").scrollLeft = xPos;
                                        $get("divGridDetalleControlDespacho").scrollTop = yPos;
                                    }
                                </script>
								<%--Final:Se utiliza para que al seleccionar la grilla no se posicione al inicio--%>

                                <div id="divGridDetalleControlDespacho" style="overflow:auto; height:450px; align:left;" class="scrollbar">
                                    <%--<asp:UpdatePanel ID="updpnlDetalleControlDespacho" runat="server">
                                    <ContentTemplate>--%>
                                        <%--<asp:Panel ID="pnlDetalleControlDespacho" runat="server" CssClass="bg-light" >
                                            <div class="table-responsive scrollbar">--%>
                                            <asp:GridView ID="grdDetalleControlDespacho" runat="server" AutoGenerateColumns="False" TabIndex="22" 
                                                GridLines="None" CssClass="table table-striped table-hover small overflow-hidden" AllowPaging="True" 
                                                EmptyDataText="No hay registros a visualizar" PageSize="50" >
                                                <PagerStyle CssClass="mGrid" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <Columns>
                                                    <%--<asp:CommandField ShowDeleteButton="True" HeaderStyle-Width="70px" />--%>
                                                    <%--<asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkRowDetalleCaracteristica" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <%--<asp:BoundField DataField="Item" HeaderText="#" ControlStyle-Width="25px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />--%>
                                                    <asp:TemplateField HeaderText="Articulo" ShowHeader="false" HeaderStyle-CssClass="text-900 p-1 m-1 small" ItemStyle-CssClass="p-1 m-1">
                                                        <ItemTemplate>
                                                            <div class="col-12">
                                                                <asp:Label ID="lblCodigoArticulo" runat="server" Visible='<%# If(Eval("Codigo") = "", "false", "true") %>' Text='<%# Eval("Codigo") %>'></asp:Label>
                                                            </div>
                                                            <div class="col-12">
                                                                <asp:Label ID="lblDescripcionArticulo" runat="server" Visible='<%# If(Eval("Descripcion") = "", "false", "true") %>' Text='<%# Eval("Descripcion") %>'></asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="Codigo" HeaderText="Código" HeaderStyle-Width="40px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />
                                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1" />--%>
                                                    <asp:BoundField DataField="CantidadOrigen" HeaderText="Cant. Origen" HeaderStyle-CssClass="text-900 p-1 m-1 small" ItemStyle-CssClass="p-1 m-1" />
                                                    <asp:TemplateField HeaderText="Cant.Encontrada" ItemStyle-Width="120px" HeaderStyle-CssClass="text-900 p-1 m-1 small" ItemStyle-CssClass="p-1 m-1">
												        <ItemTemplate>
													        <div class="input-group">
														        <asp:TextBox ID="txtValorDetalleControlDespacho" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("CantidadEncontrada") %>' AutoPostBack="True" style="text-transform :uppercase" OnTextChanged="txtValorDetalleControlDespacho_TextChanged" ></asp:TextBox>
													        </div>
												        </ItemTemplate>
											        </asp:TemplateField>
                                                    <asp:BoundField DataField="CantidadSaldo" HeaderText="Cant. Saldo" HeaderStyle-CssClass="text-900 p-1 m-1 small" ItemStyle-CssClass="p-1 m-1" />
                                                </Columns>
                                                <HeaderStyle CssClass="thead-dark" />
                                            </asp:GridView>
                                            <%--</div>
                                        </asp:Panel>--%>
                                   <%-- </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </div>
                            </div>
                        </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
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
    <asp:UpdatePanel ID="updpnlMantenimientoAsignarAsesorAuxiliar" runat="server">
        <ContentTemplate>
            <asp:LinkButton ID="lnk_mostrarPanelMantenimientoAsesorAuxiliar" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMantenimientoAsesorAuxiliar_ModalPopupExtender" runat="server" 
                BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True" 
                CancelControlID="btnCancelarMantenimientoAsesorAuxiliar" 
                PopupControlID="pnlMantenimientoAsesorAuxiliar" TargetControlID="lnk_mostrarPanelMantenimientoAsesorAuxiliar">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlMantenimientoAsesorAuxiliar" runat="server" CssClass="container" DefaultButton="btnAceptarMantenimientoAsesorAuxiliar" >
                <div class="modal-dialog modal-lg">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header bg-light">
                                    <h4 class="modal-title" id="myModalLabelAsesorAuxiliar">
                                        Mantenimiento Asignar Asesor / Auxiliar</h4>
                                    <asp:Button ID="imgbtnCancelarAsesorAuxiliarImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="scrollbar">
                                            <div class="row">
                                                <div class="col-12 mt-2">
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1 mb-0">Descripción Orden</label>
                                                    <div class="input-group">
                                                        <asp:Label ID="lblDescripcionMantenimientoAsesorAuxiliar" runat="server" CssClass="col-form-label col-form-label-sm text-800 fs-0 mt-0 mb-2" ></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-3 mt-2">
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Fecha Inicio</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar" runat="server" CssClass="form-control form-control-sm" 
                                                            style="text-transform :uppercase" TabIndex="18" autocomplete="off" ></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvTxtFechaInicioPlanificadaMantenimientoAsesorAuxiliar" runat="server" 
                                                            ControlToValidate="txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar" EnableClientScript="False" 
                                                            ErrorMessage="Ingrese la fecha de inicio de planificación" Font-Size="10px" ForeColor="Red"
                                                            ValidationGroup="vgrpValidarCrearProducto">(*)</asp:RequiredFieldValidator>
                                                        <ajaxToolkit:CalendarExtender ID="txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar_CalendarExtender" runat="server" 
                                                            CssClass="calendario" Enabled="True" TargetControlID="txtFechaInicioPlanificadaMantenimientoAsesorAuxiliar">
                                                        </ajaxToolkit:CalendarExtender>
                                                    </div>
                                                </div>
                                                <div class="col-9 mt-2">
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Hora Tarea</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboHorasMantenimientoAsesorAuxiliar" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                                            class="form-select form-select-sm js-choice" style="text-transform: uppercase" TabIndex="11">
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="cboMinutosMantenimientoAsesorAuxiliar" runat="server" AppendDataBoundItems="True"
                                                            class="form-select form-select-sm js-choice" style="text-transform: uppercase" TabIndex="11">
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="cboSegundosMantenimientoAsesorAuxiliar" runat="server" AppendDataBoundItems="True"
                                                            class="form-select form-select-sm js-choice" style="text-transform: uppercase" TabIndex="11">
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="cboMeridianoMantenimientoAsesorAuxiliar" runat="server" AppendDataBoundItems="True"
                                                            class="form-select form-select-sm js-choice" style="text-transform: uppercase" TabIndex="11">
                                                            <asp:ListItem Value="AM">AM</asp:ListItem>
                                                            <asp:ListItem Value="PM">PM</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-lg-12 mt-2">
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Asesor Auxiliar</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboAuxiliarMantenimientoAsesorAuxiliar" runat="server" 
                                                            class="form-select form-select-sm js-choice" style="text-transform : uppercase" 
                                                            TabIndex="11">
                                                            <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                        </asp:DropDownList>
												        <asp:RequiredFieldValidator ID="rfvAuxiliarMantenimientoAsesorAuxiliar" runat="server" 
													        ControlToValidate="cboAuxiliarMantenimientoAsesorAuxiliar" EnableClientScript="False" 
													        ErrorMessage="Ingrese el auxiliar" Font-Size="10px" ForeColor="Red" 
													        ValidationGroup="vgrpValidarMantenimientoAsesorAuxiliar">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-6 col-sm-6 mt-2">
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Unidad de Trabajo</label>
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboUnidadTrabajoMantenimientoAsesorAuxiliar" runat="server" 
                                                            class="form-select form-select-sm js-choice" style="text-transform : uppercase" 
                                                            TabIndex="11">
                                                            <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                        </asp:DropDownList>
												        <asp:RequiredFieldValidator ID="rfvUnidadTrabajoMantenimientoAsesorAuxiliar" runat="server" 
													        ControlToValidate="cboUnidadTrabajoMantenimientoAsesorAuxiliar" EnableClientScript="False" 
													        ErrorMessage="Ingrese el auxiliar" Font-Size="10px" ForeColor="Red" 
													        ValidationGroup="vgrpValidarMantenimientoAsesorAuxiliar">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6 col-sm-6 mt-2">
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Orden Venta</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtOrdenVentaMantenimientoAsesorAuxiliar" runat="server" CssClass="form-control form-control-sm" 
                                                            style="text-transform :uppercase" TabIndex="18" autocomplete="off" ></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6 col-sm-6 mt-2">
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Orden Compra</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtOrdenCompraMantenimientoAsesorAuxiliar" runat="server" CssClass="form-control form-control-sm" 
                                                            style="text-transform :uppercase" TabIndex="18" autocomplete="off" ></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6 col-sm-6 mt-2">
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Orden Fabricación</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtOrdenFabricacionMantenimientoAsesorAuxiliar" runat="server" CssClass="form-control form-control-sm" 
                                                            style="text-transform :uppercase" TabIndex="18" autocomplete="off" ></asp:TextBox>
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
                                                <asp:ValidationSummary ID="ValidationSummary2" runat="server" role="alert" CssClass="alert alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarMantenimientoAsesorAuxiliar" />
                                            </div>
                                            <div class="col-md-4 text-end p-2">
                                                <asp:Button ID="btnAceptarMantenimientoAsesorAuxiliar" runat="server" ValidationGroup="vgrpValidarMantenimientoAsesorAuxiliar"
                                                    ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                &nbsp;
                                                <asp:Button ID="btnCancelarMantenimientoAsesorAuxiliar" runat="server" 
                                                    ToolTip="Cancelar Registro" TabIndex ="73" Text="Cancelar" cssclass="btn btn-outline-google-plus btn-sm"/>
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
    </script>
    <link href="cssbootstrap/bootstrap-switch.css" rel="stylesheet" />
    <script src="jsbootstrap/bootstrap-switch.js"></script>

    <script src="vendors/anchorjs/anchor.min.js"></script>
    <script src="vendors/glightbox/glightbox.min.js"> </script>
    <script src="vendors/lodash/lodash.min.js"></script>
    <script src="assets/js/theme.js"></script>
</asp:Content>
