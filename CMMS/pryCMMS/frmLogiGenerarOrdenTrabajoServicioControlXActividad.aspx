<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmLogiGenerarOrdenTrabajoServicio_20231102_1008.aspx.vb" Inherits="pryCMMS.frmLogiGenerarOrdenTrabajoServicioControlXActividad" %>
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

    <%--<link href="vendors/flatpickr/flatpickr.min.css" rel="stylesheet">--%>

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

    <%-- <link href="vendors/flatpickr/flatpickr.min.css" rel="stylesheet">
    <div class="col-lg-2 col-md-3 col-sm-4 mt-2">
                        <label class="text-black-50 col-form-label col-form-label-sm fs--2">Fecha Final:</label>
                        <div class="input-group">
                            <asp:TextBox ID="txtFechaFinal" runat="server" autocomplete="off"
                                CssClass="form-control form-control-sm datetimepicker" style="text-transform :uppercase;" 
                                placeholder="Periodo Final" TabIndex="4" type="text" 
                                data-options='{"enableTime":false,"dateFormat":"d/m/Y","locale":"en","disableMobile":true}'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFechaFinal" runat="server" 
                                ControlToValidate="txtFechaFinal" EnableClientScript="True" 
                                ErrorMessage="Ingrese el periodo final" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidar">(*)</asp:RequiredFieldValidator>
                        </div>
                    </div>




    <label class="form-label" for="timepicker1">Start Time</label>
<input class="form-control datetimepicker" id="timepicker1" type="text" placeholder="H:i" data-options='{"enableTime":true,"noCalendar":true,"dateFormat":"H:i","disableMobile":true}' />


    <script src="assets/js/flatpickr.js"></script>--%>
    <%--<div class="container">
  <div class="row">
    <div class="form-group col-md-6">
      <label for="date">Date</label>
      <div class="input-group date" id="datePicker">
        <input type="text" class="form-control">
        <span class="input-group-addon"><i class="fa fa-calendar" aria-hidden="true"></i></span>
      </div>
    </div>
    <div class="form-group col-md-6">
      <label for="time">Time</label>
      <div class="input-group date" id="timePicker">
        <input type="text" class="form-control timePicker">
        <span class="input-group-addon"><i class="fa fa-clock-o" aria-hidden="true"></i></span>
      </div>
    </div>
  </div>
</div>--%>

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
            <asp:HiddenField ID="hfdOrdenFabricacionReferencia" runat="server" />
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
							<asp:LinkButton ID="btnNuevo" runat="server" OnClick="btnNuevo_Click" CssClass="btn btn-success btn-sm ml-auto float-right" Text="<span class='fas fa-plus'></span> Procesar" ToolTip="Procesar Orden de Trabajo" />
                            <div class="dropdown font-sans-serif btn-reveal-trigger">
                              <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesEquipo" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                              <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesEquipo">
                                  <asp:LinkButton ID="lnkbtnVerOrdenTrabajo" runat="server" ToolTip="Ver Orden de Trabajo" Text="<span class='fas fa-file me-2'></span>Ver O.Trabajo" CssClass="nav-link nav-link-card-details" />
                                  <asp:LinkButton ID="lnkbtnTerminarOrdenTrabajo" runat="server" ToolTip="Terminar Orden de Trabajo" Text="<span class='fas fa-file me-2'></span>Terminar O.T." CssClass="nav-link nav-link-card-details" />
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
						            <asp:DropDownList ID="cboFiltroOrdenTrabajo" runat="server" CssClass="form-select form-select-sm js-choice" TabIndex="1">
							            <asp:ListItem>SELECCIONE EL FILTRO</asp:ListItem>
						            </asp:DropDownList>
					            </div>
					        </div>
					        <div class="col-lg-6 col-sm-6">
                                <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Buscar por:</label>
						        <div class="input-group">
							        <asp:TextBox ID="txtBuscarOrdenTrabajo" runat="server" CssClass="form-control form-control-sm" TabIndex="2"
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
                                                <asp:BoundField DataField="IdClienteSAP" HeaderText="Id. Cliente SAP" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>
                                                <asp:BoundField DataField="RucCliente" HeaderText="RUC" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>
                                                <asp:BoundField DataField="RazonSocial" HeaderText="Razón Social" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>
                                                <asp:BoundField DataField="IdEquipo" HeaderText="Id.Equipo" DataFormatString="{0:dd/MM/yyyy}"  HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>
                                                <asp:BoundField DataField="IdEquipoSAP" HeaderText="Id.Equipo SAP" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>
                                                <asp:BoundField DataField="DescripcionEquipo" HeaderText="Descripción Equipo" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>
                                                <asp:BoundField DataField="NumeroSerieEquipo" HeaderText="Nro. Serie Equipo" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>
                                                <asp:BoundField DataField="IdArticuloSAPCabecera" HeaderText="Articulo Principal" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small"/>
                                                <asp:TemplateField HeaderText="Estado" ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="small">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEstado" runat="server" Visible='<%# If(Eval("StatusOrdenTrabajo") = "", "false", "true") %>' CssClass='<%#If(Eval("StatusOrdenTrabajo") = "R", "texto_estado short_estado black borde_redondo1", If(Eval("StatusOrdenTrabajo") = "T", "texto_estado short_estado verde borde_redondo1", "texto_estado short_estado orange borde_redondo1")) %>' Text='<%# StrConv(IIf(Eval("StatusOrdenTrabajo") = "P", "PENDIENTE", IIf(Eval("StatusOrdenTrabajo") = "T", "TERMINADO", "REGISTRADO")), VbStrConv.ProperCase) %>'></asp:Label>
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
                            <div class="col">
                              <h6 class="mb-0">Datos Generales</h6>
                            </div>
                          </div>
                        </div>
                        <div class="card-body">
                          <div class="row mt-1">
                            <div class="col-lg-12 col-sm-12">
                              <div class="row">
                                <div class="col-lg-4 col-md-4 mt-2">
                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Código Equipo:</label>
                                    <div class="input-group">
                                        <p class="fs--1 m-0"><asp:Label ID="lblIdEquipo" runat="server" Text="" CssClass="text-500 small"></asp:Label></p>
                                    </div>
                                </div>
                                <div class="col-lg-8 col-md-8 mt-2">
                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción Equipo:</label>
                                    <div class="input-group">
                                        <p class="fs--1 m-0"><asp:Label ID="lblDescripcionEquipo" runat="server" Text="" CssClass="text-500 small"></asp:Label></p>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-md-2 mt-2">
                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">N.Ser.:</label>
                                    <div class="input-group">
                                        <p class="fs--1 m-0"><asp:Label ID="lblNroSerieEquipo" runat="server" Text="" CssClass="text-500 small"></asp:Label></p>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-md-2 mt-2">
                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">#Part:</label>
                                    <div class="input-group">
                                        <p class="fs--1 m-0"><asp:Label ID="lblNroParteEquipo" runat="server" Text="" CssClass="text-500 small"></asp:Label></p>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-md-2 mt-2">
                                    <div class="mt-1"><asp:Label ID="lblIdentificadorComponente" runat="server" CssClass="text-black-50 col-form-label col-form-label-sm fs--2 fw-semi-bold" Text="¿Es un componente?:"></asp:Label></div>
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
                                    <span class="far fa-clone"></span><label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Tipo Activo:</label>
                                    <div class="input-group">
                                        <p class="fs--1 m-0"><asp:Label ID="lblTipoActivoEquipo" runat="server" Text="" CssClass="text-500 small"></asp:Label></p>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-md-2 mt-2">
                                    <span class="far fa-clone"></span><label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Catalogo Principal:</label>
                                    <div class="input-group">
                                        <p class="fs--1 m-0"><asp:Label ID="lblCatalogoEquipo" runat="server" Text="" CssClass="text-500 small"></asp:Label></p>
                                    </div>
                                </div>
                                <div id="divSistemaFuncional" runat="server" class="col-lg-4 col-md-2 mt-2">
                                    <span class="far fa-clone"></span><label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Sistema Funcional:</label>
                                    <div class="input-group">
                                        <p class="fs--1 m-0"><asp:Label ID="lblSistemaFuncionalEquipo" runat="server" Text="" CssClass="text-500 small"></asp:Label></p>
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
                        <div class="card-body">
                            <asp:ListView ID="lstComponentes" runat="server">
                                <ItemTemplate>
                                  <div class="row g-0 align-items-center py-2 position-relative border-bottom border-200">
								    <div class="col ps-card py-1 position-static">
								      <div class="d-flex align-items-center">
									    <div class="avatar avatar-xl me-3">
									      <div class="avatar-name rounded-circle bg-soft-primary text-dark"><span class="fs-0 text-primary"><%# Mid(Eval("Descripcion"), 1, 1) %></span></div>
									    </div>
									    <div class="flex-1">
									      <h6 class="mb-0 d-flex align-items-center"><a class="text-800 stretched-link" href="#!"><%# Eval("Descripcion") %></a><span class="badge rounded-pill ms-2 bg-200 text-primary"><%# Eval("PorcentajeText") %></span></h6>
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
                                              <div class="progress-bar rounded-pill" role="progressbar" style='<%# "width: " & Eval("Porcentaje").ToString & "%" %>' aria-valuenow='<%# Eval("Porcentaje") %>' aria-valuemin="0" aria-valuemax="100"></div>
									      </div>
									    </div>
								      </div>
								    </div>
							      </div>
                                </ItemTemplate>
                            </asp:ListView>
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
                                <div class="dropdown-menu dropdown-menu-end border py-2" aria-labelledby="dropdown-total-sales"><%--<a class="dropdown-item" href="#!">Ver Galeria</a>--%><%--<a class="dropdown-item" href="#!">Export</a>
                                  <div class="dropdown-divider"></div><a class="dropdown-item text-danger" href="#!">Remove</a>--%>
                                  <asp:LinkButton ID="lnkbtnVerGaleria" runat="server" ToolTip="Ver la galeria por actividades" Text="<span class='fas fa-file me-2'></span>Ver Galeria" CssClass="nav-link nav-link-card-details" />
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                        <div class="card-body h-100">
                            <asp:DataList ID="lstCheckList" runat="server" DataKeyField="IdActividad" OnItemCommand="lstCheckList_RowCommand_Botones"
                                RepeatColumns="1" RepeatDirection="Horizontal" >
                                <ItemTemplate>
                                    <div class="card mb-2 bg-100">
                                        <div class="card-body p-0">
                                          <div class="row">
                                            <div class="col-3 col-xl-2 col-lg-3 col-md-2 col-sm-2">
                                                <div class="overflow-hidden">
                                                    <div class="position-relative rounded-top overflow-hidden">
                                                        <div class="p-1"><a href='<%# "Imagenes/check-" & Eval("StatusCheckList") & ".png" %> ' data-gallery="gallery-1"><img class="img-fluid rounded" src='<%# "Imagenes/check-" & Eval("StatusCheckList") & ".png" %> ' alt="" /></a></div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-5 col-xl-7 col-lg-6 col-md-7 col-sm-7">
                                                <div class="mt-2">
                                                    <div class="row">
                                                        <div class="col-12">
                                                            <h5 class="fs-0"><asp:Label ID="lblActividad" runat="server" Text='<%# Eval("DescripcionActividad") %>' CssClass="text-dark"></asp:Label></h5>
                                                        </div>
                                                    </div>
                                                    <div class="row p-0">
                                                        <div class="col-xl-12 col-md-12">
                                                            <div class="row">
                                                                <div class="col-xl-4 col-lg-5">
                                                                    <p class="fs--1 m-0"><strong class="small">Componente:</strong></p>
                                                                </div>
                                                                <div class="col-xl-8 col-lg-7">
                                                                    <p class="fs--1 m-0"><asp:Label ID="lblComponente" runat="server" Text='<%# Eval("DescripcionCatalogo") %>' CssClass="text-500 small"></asp:Label></p>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-xl-12 col-md-12">
                                                            <div class="row">
                                                                <div class="col-xl-4 col-lg-5">
                                                                    <p class="fs--1 m-0"><strong class="small">Tipo Activo:</strong></p>
                                                                </div>
                                                                <div class="col-xl-8 col-lg-7">
                                                                    <p class="fs--1 m-0"><asp:Label ID="lblTipoActivo" runat="server" Text='<%# Eval("DescripcionTipoActivo") %>' CssClass="text-500 small"></asp:Label></p>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-xl-12 col-md-12">
                                                            <div class="row">
                                                                <div class="col-xl-4 col-lg-5">
                                                                    <p class="fs--1 m-0"><strong class="small">Sist.Funcional:</strong></p>
                                                                </div>
                                                                <div class="col-xl-8 col-lg-7">
                                                                    <p class="fs--1 m-0"><asp:Label ID="lblSistemaFuncional" runat="server" Text='<%# Eval("DescripcionSistemaFuncional") %>' CssClass="text-500 small"></asp:Label></p>                                                    
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-4 col-xl-3 col-lg-3 col-md-3 col-sm-3">
                                                <asp:HiddenField ID="hfdStatusCheckList" runat="Server" Value='<%# Eval("StatusCheckList") %>'></asp:HiddenField>
                                                <div class="row mt-2 pb-1">
                                                    <asp:LinkButton ID="lnkbtnIniciarCheckList" runat="server" CommandName="Iniciar" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdActividad") & "*" & Eval("IdEquipo") & "*" & Eval("IdSistemaFuncional") & "*" & Eval("IdTipoActivo") & "*" & Eval("IdEquipoCheckList") & "*I" %>' Visible="false" ToolTip="Iniciar Actividad" Text="<span class='fas fa-power-off me-2'></span></br>Iniciar" CssClass="btn btn-outline-primary btn-sm w-75 fs--1 m-0" />
                                                </div>
                                                <div class="row pb-1">
                                                    <asp:LinkButton ID="lnkbtnPendienteCheckList" runat="server" CommandName="Pendiente" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdActividad") & "*" & Eval("IdEquipo") & "*" & Eval("IdSistemaFuncional") & "*" & Eval("IdTipoActivo") & "*" & Eval("IdEquipoCheckList") & "*P" %>' Visible="false" ToolTip="Actividad Pendiente" Text="<span class='fas fa-tasks me-2'></span></br>En Espera" CssClass="btn btn-outline-warning btn-sm w-75 fs--1 m-0" />
                                                </div>
                                                <div class="row pb-1">
                                                    <asp:LinkButton ID="lnkbtnRetomarCheckList" runat="server" CommandName="Retomar" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdActividad") & "*" & Eval("IdEquipo") & "*" & Eval("IdSistemaFuncional") & "*" & Eval("IdTipoActivo") & "*" & Eval("IdEquipoCheckList") & "*E" %>' Visible="false" ToolTip="Retomar Actividad" Text="<span class='fas fa-retweet me-2'></span></br>Retomar" CssClass="btn btn-outline-danger btn-sm w-75 fs--1" />
                                                </div>
                                                <div class="row pb-1">
                                                    <asp:LinkButton ID="lnkbtnFinalizarCheckList" runat="server" CommandName="Finalizar" CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdActividad") & "*" & Eval("IdEquipo") & "*" & Eval("IdSistemaFuncional") & "*" & Eval("IdTipoActivo") & "*" & Eval("IdEquipoCheckList") & "*F" %>' Visible="false" ToolTip="Finalizar Actividad" Text="<span class='fas fa-check me-2'></span></br>Finalizar" CssClass="btn btn-outline-success btn-sm w-75 fs--1" />
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
                                                <%--<asp:LinkButton ID="lnkbtnAgregarComentario" runat="server" CommandName="AgregarComentario"  CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdActividad") & "*" & Eval("IdEquipo") & "*" & Eval("IdSistemaFuncional") & "*" & Eval("IdTipoActivo") & "*" & Eval("IdEquipoCheckList") %>' ToolTip="Agregar Comentario a la Actividad" Text="<span class='fas fa-pencil-alt mx-1'></span><div class='d-none d-xl-block'>Comentario</div>" CssClass="link-600 border-0 me-3 fs--1 text-center" />--%>
                                                <asp:LinkButton ID="lnkbtnAgregarDatosAdicionales" runat="server" CommandName="AgregarDatosAdicionales"  CommandArgument='<%# Eval("IdCatalogo") & "*" & Eval("IdJerarquiaCatalogo") & "*" & Eval("IdArticuloSAPCabecera") & "*" & Eval("IdActividad") & "*" & Eval("IdEquipo") & "*" & Eval("IdSistemaFuncional") & "*" & Eval("IdTipoActivo") & "*" & Eval("IdEquipoCheckList") %>' ToolTip="Agregar Datos Adicionales a la Actividad" Text="<span class='fas fa-project-diagram mx-1'></span><div class='d-none d-xl-block'>Datos</div>" CssClass="link-600 border-0 me-3 fs--1 text-center" />
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

            <asp:LinkButton ID="lnk_mostrarPanelMantenimientoTarea" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMantenimientoTarea_ModalPopupExtender" runat="server" 
                BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True" 
                CancelControlID="btnCancelarMantenimientoTarea" 
                PopupControlID="pnlMantenimientoTarea" TargetControlID="lnk_mostrarPanelMantenimientoTarea">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlMantenimientoTarea" runat="server" CssClass="container" DefaultButton="btnAceptarMantenimientoTarea" >
                <div class="modal-dialog modal-lg" >
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header bg-light">
                                    <h4 class="modal-title" id="myModalLabelTarea">
                                        Mantenimiento Tarea</h4>
                                    <asp:Button ID="imgbtnCancelarTareaImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body" >
                                    <asp:UpdatePanel ID="updpnlMantenimientoTarea" runat="server">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="scrollbar">
                                                    <div class="row">
                                                        <div class="col-2 mt-2">
                                                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Fecha Tarea</label>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtFechaMantenimientoTarea"  runat="server" autocomplete="off"
                                                                    CssClass="form-control form-control-sm" style="text-transform :uppercase;" 
                                                                    placeholder="Fecha Mantenimiento" TabIndex="18" type="text"></asp:TextBox>
                                                                <ajaxToolkit:CalendarExtender ID="txtFechaMantenimientoTarea_CalendarExtender" runat="server" 
                                                                    CssClass="calendario" Enabled="True" TargetControlID="txtFechaMantenimientoTarea">
                                                                </ajaxToolkit:CalendarExtender>
                                                            </div>
                                                        </div>
                                                        <div class="col-6 mt-2">
                                                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Hora Tarea</label>
                                                            <div class="input-group">
                                                                <asp:DropDownList ID="cboHorasMantenimientoTarea" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                                                    class="form-select form-select-sm js-choice" style="text-transform: uppercase" TabIndex="11">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="cboMinutosMantenimientoTarea" runat="server" AppendDataBoundItems="True"
                                                                    class="form-select form-select-sm js-choice" style="text-transform: uppercase" TabIndex="11">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="cboSegundosMantenimientoTarea" runat="server" AppendDataBoundItems="True"
                                                                    class="form-select form-select-sm js-choice" style="text-transform: uppercase" TabIndex="11">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="cboMeridianoMantenimientoTarea" runat="server" AppendDataBoundItems="True"
                                                                    class="form-select form-select-sm js-choice" style="text-transform: uppercase" TabIndex="11">
                                                                    <asp:ListItem Value="AM">AM</asp:ListItem>
                                                                    <asp:ListItem Value="PM">PM</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-lg-12 col-md-12 col-sm-12 mt-2">
                                                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Personal Asignado a la Tarea</label>
                                                            <div class="input-group">
                                                                <asp:DropDownList ID="cboPersonalAsignadoMantenimientoTarea" runat="server" AppendDataBoundItems="True" AutoPostBack="True" 
                                                                    class="form-select form-select-sm js-choice" style="text-transform : uppercase" 
                                                                    TabIndex="11">
                                                                    <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                                                </asp:DropDownList>
												                <asp:RequiredFieldValidator ID="rfvPersonalAsignadoMantenimientoTarea" runat="server" 
													                ControlToValidate="cboPersonalAsignadoMantenimientoTarea" EnableClientScript="False" 
													                ErrorMessage="Ingrese el personal asignado" Font-Size="10px" ForeColor="Red" 
													                ValidationGroup="vgrpValidarMantenimientoTarea">(*)</asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-12 mt-2">
                                                            <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción Tarea</label>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtDescripcionMantenimientoTarea" runat="server" CssClass="form-control form-control-sm" 
                                                                    style="text-transform :uppercase" TabIndex="18" autocomplete="off" ></asp:TextBox>
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
                                                    ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                &nbsp;
                                                <asp:Button ID="btnCancelarMantenimientoTarea" runat="server" 
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



    <asp:LinkButton ID="lnk_mostrarPanelSubirImagenActividad" runat="server"></asp:LinkButton>
    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelSubirImagenActividad_ModalPopupExtender" runat="server" 
        BackgroundCssClass="FondoAplicacion" DropShadow="False" DynamicServicePath="" Enabled="True" 
        CancelControlID="btnCancelarSubirImagenActividad" 
        PopupControlID="pnlSeleccionarSubirImagenActividad" TargetControlID="lnk_mostrarPanelSubirImagenActividad">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="pnlSeleccionarSubirImagenActividad" runat="server" CssClass="container" >
        <div class="modal-dialog modal-lg">
            <div class="shadow rounded">
                <div class="modal-dialog-scrollable">
                    <div class="modal-content">
                        <div class="modal-header bg-light">
                            <h4 class="modal-title" id="myModalLabelSubirImagenActividad">
                                Agregar Imagen a la Actividad</h4>
                            <asp:Button ID="imgbtnCancelarSubirImagenActividadImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                        </div>
                        <div class="modal-body">
                            <div class="row mt-0">
                                <div class="col-12 mt-2">
                                    <label class="text-black-50 col-form-label col-form-label-sm">Título:</label>
									<div class="input-group">
                                        <asp:TextBox ID="txtTituloSubirImagenActividad" runat="server" CssClass="form-control form-control-sm" autocomplete="off" TabIndex="12"
                                            style="text-transform :uppercase" placeholder="Titulo de la imagen"></asp:TextBox>
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
                                        style="text-transform :uppercase" placeholder="Descripción de la imagen"></asp:TextBox>
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
                                        style="text-transform :uppercase" placeholder="Observación de la imagen"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvObservacionSubirImagenActividad" runat="server" 
                                        ControlToValidate="txtObservacionSubirImagenActividad" EnableClientScript="False" 
                                        ErrorMessage="Ingrese la observación de la imagen" Font-Size="10px" ForeColor="Red" 
                                        ValidationGroup="vgrpValidarSubirImagenActividad">(*)</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
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
                                    ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                &nbsp;
                                <asp:Button ID="btnCancelarSubirImagenActividad" runat="server" 
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



    <asp:UpdatePanel ID="updpnlInsumos" runat="server">
        <ContentTemplate>
    <asp:LinkButton ID="lnk_mostrarPanelAgregarInsumos" runat="server"></asp:LinkButton>
    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelAgregarInsumos_ModalPopupExtender" runat="server" 
        BackgroundCssClass="FondoAplicacion" DropShadow="False" DynamicServicePath="" Enabled="True" 
        CancelControlID="btnCancelarAgregarInsumos" 
        PopupControlID="pnlSeleccionarAgregarInsumos" TargetControlID="lnk_mostrarPanelAgregarInsumos">
    </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlSeleccionarAgregarInsumos" runat="server" CssClass="container" >
                <div class="modal-dialog modal-lg">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header bg-light">
                                    <h4 class="modal-title" id="myModalLabelAgregarInsumos">
                                        Agregar Insumos a la Actividad</h4>
                                    <asp:Button ID="imgbtnCancelarAgregarInsumosImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <div class="row mt-0 bg-200">
                                        <div class="col-12 mt-2">
                                            <span class="far fa-clone"></span><label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Descripción de la Actividad:</label>
                                            <div class="input-group">
                                                <asp:Label ID="lblDescripcionActividadAgregarInsumos" runat="server" Text="" CssClass="fs-0 mb-2 px-3"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div runat="server" class="row justify-content-between">
                                        <div class="col-lg-12 col-md-12 col-sm-12">
                                            <%--<asp:UpdatePanel ID="updpnlInsumos" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">--%>
                                            <%--<asp:UpdatePanel ID="updpnlInsumos" runat="server">--%>
                                                <span class="fas fa-link"></span><label class="text-black-50 col-form-label fs--2 m-1">Filtrar por Insumos:</label>
                                                <div class="input-group">
                                                    <asp:DropDownList ID="cboInsumosAgregarInsumos" runat="server" AppendDataBoundItems="True" TabIndex="29"
										                CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase">
										                <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
									                </asp:DropDownList>
                                                    <asp:Button ID="btnAdicionarInsumos" runat="server" Text="Agregar" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Insumos a la Actividad" TabIndex="30"/>
                                                </div>
                                            <%--</asp:UpdatePanel>--%>
                                        </div>
                                    </div>
							        <div class="row justify-content-between mt-3"> 
								        <div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
									        <div id="divGridDetalleAgregarInsumos" style="overflow:auto; height:160px; align:left;" class="scrollbar">
										        <asp:UpdatePanel ID="updpnlDetalleAgregarInsumos" runat="server">
										        <ContentTemplate>
											        <asp:Panel ID="pnlDetalleAgregarInsumos" runat="server" CssClass="bg-light" >
												        <div class="table-responsive scrollbar">
												        <asp:GridView ID="grdDetalleAgregarInsumos" runat="server" AutoGenerateColumns="False" TabIndex="4" 
													        GridLines="None" CssClass="table table-striped table-hover small overflow-hidden" AllowPaging="True" 
													        EmptyDataText="No hay registros a visualizar" PageSize="3" >
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
																	        <asp:TextBox ID="txtCantidadDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("Cantidad") %>' AutoPostBack="False" style="text-transform :uppercase" OnTextChanged="txtCantidadDetalle_TextChanged" ></asp:TextBox>
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
                                            ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                        &nbsp;
                                        <asp:Button ID="btnCancelarAgregarInsumos" runat="server" 
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

    <asp:LinkButton ID="lnk_mostrarPanelGaleriaImagenActividad" runat="server"></asp:LinkButton>
    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelGaleriaImagenActividad_ModalPopupExtender" runat="server" 
        BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True" 
        CancelControlID="btnCancelarGaleriaImagenActividad" 
        PopupControlID="pnlGaleriaImagenActividad" TargetControlID="lnk_mostrarPanelGaleriaImagenActividad">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="pnlGaleriaImagenActividad" runat="server" CssClass="container" DefaultButton="btnAceptarGaleriaImagenActividad" >
        <div class="modal-dialog modal-lg">
            <div class="shadow rounded">
                <div class="modal-dialog-scrollable">
                    <div class="modal-content">
                        <div class="modal-header bg-light">
                            <h4 class="modal-title" id="myModalLabelGaleriaImagen">
                                Galería de Imagenes</h4>
                            <asp:Button ID="imgbtnCancelarGaleriaImagenImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="updpnlGaleriaImagenActividad" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="scrollbar">
                                            <asp:DataList ID="lstGaleriaActividad" runat="server" DataKeyField="DocumentoRef" 
                                                RepeatColumns="5" RepeatDirection="Horizontal" onitemcommand="lstGaleriaActividad_ItemCommand" >
                                                <ItemTemplate>
                                                    <div class="row">
                                                        <div class="mb-4 col-12">
                                                            <div class="border rounded-1 h-100 d-flex flex-column justify-content-between pb-3">
                                                                <div class="overflow-hidden">
                                                                    <div class="position-relative rounded-top overflow-hidden">
                                                                        <%--<div class="col-12 p-1"><a href='<%# "Imagenes/Actividadsmall/" & Eval("DocumentoRef") & "-" & Eval("NumeroItem") & ".jpg" %> ' data-gallery="gallery-1"><img class="img-fluid rounded" src='<%# "Imagenes/Equipo/" & Eval("Codigo") & "-" & Eval("NumeroItem") & ".jpg" %> ' alt="" /></a></div>--%>
                                                                        <%--<div class="col-12 p-1"><asp:LinkButton ID="lnkbtnVerGaleriaImagenesActividadPersonal" runat="server" href='<%# "Imagenes/Actividad/" & Eval("DocumentoRef") & "-" & Eval("NumeroItem") & ".jpg" %> ' data-gallery="gallery-1"><img class="img-fluid rounded" src='<%# "Imagenes/Actividadsmall/" & Eval("DocumentoRef") & "-" & Eval("NumeroItem") & ".jpg" %> ' alt="" /></asp:LinkButton></div>--%>
                                                                        <asp:HiddenField ID="hfdUrlGaleriaActividad" runat="Server" Value='<%# Eval("DocumentoRef") & "-" & Eval("NumeroItem") %>'></asp:HiddenField>
                                                                        <div class="col-12 p-1"><asp:LinkButton ID="lnkbtnVerGaleriaImagenesActividadPersonal" runat="server" CommandName="VerFotoGaleria" data-gallery="gallery-1"><img class="img-fluid rounded" src='<%# "Imagenes/Actividadsmall/" & Eval("DocumentoRef") & "-" & Eval("NumeroItem") & ".jpg" %> ' alt="" /></asp:LinkButton></div>
                                                                    </div>
                                                                    <div class="p-3">
                                                                        <h5 class="fs-0"><asp:Label ID="lblCodigo" runat="server" Text='<%# Eval("Titulo") %>' CssClass="text-dark"></asp:Label></h5>
                                                                        <p class="fs--1 mb-3"><asp:Label ID="lblDescripcion" runat="server" Text='<%# Eval("Descripcion") %>' CssClass="mgrid_small text-500"></asp:Label></p>
                                                                        <p class="fs--1 mb-1"><asp:Label ID="lblObservacion" runat="server" CssClass="mgrid_tipo_afectacion" Text='<%# Eval("Observacion") %>'></asp:Label></p>
                                                                       <%-- <p class="fs--1 mb-1"><asp:Label ID="lbl" runat="server" CssClass="mgrid_tipo_afectacion" Text='<%# Eval("Observacion") %>'></asp:Label></p>--%>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <div class="container">
                                <div class="row">
                                    <div class="col-md-8 mt-2">
                                        <asp:ValidationSummary ID="ValidationSummary4" runat="server" role="alert" CssClass="alert alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarGaleriaImagenActividad" />
                                    </div>
                                    <div class="col-md-4 text-end p-2">
                                        <asp:Button ID="btnAceptarGaleriaImagenActividad" runat="server" ValidationGroup="vgrpValidarGaleriaImagenActividad"
                                            ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                        &nbsp;
                                        <asp:Button ID="btnCancelarGaleriaImagenActividad" runat="server" 
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
                <div class="modal-dialog modal-sm">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header bg-light">
                                    <h4 class="modal-title" id="myModalLabelVerImagenActividadImagen">
                                        Ver Imagen</h4>
                                    <asp:Button ID="imgbtnCancelarVerImagenActividadImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <div class="row">
								        <div class="col-12">
                                            <%--<asp:UpdatePanel ID="updpnlVerImagenActividad" runat="server">
                                                <ContentTemplate>--%>
                                    
                                                    <asp:Image ID="imgVerImagenActividad" runat="server" class="img-fluid rounded" alt="" />
                                                <%--</ContentTemplate>
                                            </asp:UpdatePanel>--%>
                                        
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
                                    <h4 class="modal-title" id="myModalLabelOtrosDatos">
                                        Mantenimiento Otros Datos</h4>
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
                                                        <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción Equipo:</label>
                                                        <div class="input-group">
                                                            <p class="fs--1 m-0"><asp:Label ID="lblDescripcionEquipoCaracteristicaOtrosDatos" runat="server" Text="" CssClass="text-500 small"></asp:Label></p>
                                                        </div>
                                                    </div>
                                                    <div class="col-12 mt-2">
                                                        <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción Actividad:</label>
                                                        <div class="input-group">
                                                            <p class="fs--1 m-0"><asp:Label ID="lblDescripcionActividadCaracteristicaOtrosDatos" runat="server" Text="" CssClass="text-500 small"></asp:Label></p>
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
                                                            <span class="fas fa-link"></span><label class="text-black-50 col-form-label fs--2 m-1">Filtrar Caracteristica:</label>
                                                            <div class="input-group">
                                                                <asp:DropDownList ID="cboCaracteristicaOtrosDatos" runat="server" AppendDataBoundItems="True" TabIndex="29"
												                    CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase">
												                    <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
											                    </asp:DropDownList>
                                                                <asp:Button ID="btnAdicionarCaracteristicaOtrosDatos" runat="server" Text="Agregar" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Caracteristica" TabIndex="30"/>
                                                            </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="row justify-content-between mt-3"> 
                                                    <div class="col-md-12 col-lg-12 col-xl-12 mb-4 mb-lg-0">
                                                        <asp:UpdatePanel ID="updpnlDetalleCaracteristicaOtrosDatos" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="pnlDetalleCaracteristicaOtrosDatos" runat="server" CssClass="bg-light" >
                                                                <div class="table-responsive scrollbar">
                                                                <asp:GridView ID="grdDetalleCaracteristicaOtrosDatos" runat="server" AutoGenerateColumns="False" TabIndex="4" 
                                                                    GridLines="None" CssClass="table table-striped table-hover small overflow-hidden" AllowPaging="True" 
                                                                    EmptyDataText="No hay registros a visualizar" PageSize="50" >
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
																		            <asp:TextBox ID="txtValorDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("ValorReferencial") %>' AutoPostBack="False" style="text-transform :uppercase" OnTextChanged="txtValorDetalleCaracteristicaOtrosDatos_TextChanged"></asp:TextBox>
																	            </div>
																            </ItemTemplate>
															            </asp:TemplateField>
                                                                        <%--<asp:TemplateField HeaderText="Id. Ref. SAP" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                            <ItemTemplate>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtIdReferenciaSAPDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("IdReferenciaSAP") %>' CommandName="ButtonField" AutoPostBack="False" style="text-transform :uppercase" ></asp:TextBox>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Campo SAP" ItemStyle-Width="120px" HeaderStyle-CssClass="p-1 m-1" ItemStyle-CssClass="p-1 m-1">
                                                                            <ItemTemplate>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="txtDescripcionCampoSAPDetalle" runat="server" CssClass="form-control form-control-sm text-right small" Text='<%# Eval("DescripcionCampoSAP") %>' CommandName="ButtonField" AutoPostBack="False" style="text-transform :uppercase" ></asp:TextBox>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>--%>
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
                                            ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                        &nbsp;
                                        <asp:Button ID="btnCancelarOtrosDatos" runat="server" 
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

    <script language="javascript" type="text/javascript">
        //function popupEmitirEquipoDetalleReporte(IdEquipo) {
        //    window.open("Informes/frmCmmsEquipoDetalleReporte.aspx?IdEqu=" + IdEquipo, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
        //}
        function popupEmitirOrdenTrabajoReporte(TipDoc, NroSer, NroDoc) {
            window.open("Informes/frmCmmsOrdenTrabajoReporte.aspx?TipDoc=" + TipDoc + "&NroSerie=" + NroSer + "&NroDoc=" + NroDoc, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
        }

        <%--function pageLoad(sender, args) {
            // Está dentro de un pageload para que puede ejecutar esto dentro de un <asp:UpdatePanel>
            // Obtiene una referencia al DropDownList de minutos por su ID
            var ddlHoras = document.getElementById('<%= cboHorasMantenimientoTarea.ClientID %>');
            var ddlMinutos = document.getElementById('<%= cboMinutosMantenimientoTarea.ClientID %>');
            var ddlSegundos = document.getElementById('<%= cboSegundosMantenimientoTarea.ClientID %>');

            // Llena los DropDownList de horas, minutos y segundos
            for (var i = 0; i < 24; i++) {
                var hora = i.toString().padStart(2, '0');
                var option = document.createElement('option');
                option.value = hora;
                option.text = hora;
                ddlHoras.appendChild(option);
            }

            for (var i = 0; i < 60; i++) {
                var minutoSegundo = i.toString().padStart(2, '0');
                var optionMinutos = document.createElement('option');
                optionMinutos.value = minutoSegundo;
                optionMinutos.text = minutoSegundo;
                ddlMinutos.appendChild(optionMinutos);

                var optionSegundos = document.createElement('option');
                optionSegundos.value = minutoSegundo;
                optionSegundos.text = minutoSegundo;
                ddlSegundos.appendChild(optionSegundos);
            }

            function actualizarMeridiano() {
                var horas = parseInt(document.getElementById('<%= cboHorasMantenimientoTarea.ClientID %>').value);

                var ddlMeridiano = document.getElementById('<%= cboMeridianoMantenimientoTarea.ClientID %>');

                if (horas >= 12) {
                    ddlMeridiano.value = "PM";
                } else {
                    ddlMeridiano.value = "AM";
                }
            }
        }--%>
    </script>


    <link href="cssbootstrap/bootstrap-switch.css" rel="stylesheet" />
    <script src="jsbootstrap/bootstrap-switch.js"></script>

    

</asp:Content>
