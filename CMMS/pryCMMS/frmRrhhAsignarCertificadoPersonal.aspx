<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmRrhhAsignarCertificadoPersonal.aspx.vb" Inherits="pryCMMS.frmRrhhAsignarCertificadoPersonal" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

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
            <asp:HiddenField ID="hfdOperacionDetalle" runat="server" />
            <asp:HiddenField ID="hfdFocusObjeto" runat="server" />
            <asp:HiddenField ID="hfdEstado" runat="server" />            
            <asp:Panel ID="pnlCabecera" runat="server">
                <div class="card mb-3">
                    <div class="card-header">
                      <div class="row flex-between-end">
                        <div class="col-auto align-self-center">
                            <div class="nav nav-pills gap-2" role="tablist">
                                <h5 class="mb-0" data-anchor="data-anchor">Listado de Personal</h5>
                                <a class="btn btn-falcon-default btn-sm ml-auto float-right" href="frmBienvenida.aspx" aria-expanded="true">
                                    <span class="fas fa-home"></span><span class="nav-link-text ps-1">Inicio</span>
                                </a>
                            </div>
                        </div>
                        <div class="col-auto ms-auto">
                          <div class="nav gap-2" role="tablist">
							<asp:LinkButton ID="btnNuevo" runat="server" OnClick="btnNuevo_Click" CssClass="btn btn-success btn-sm ml-auto float-right" Text="<span class='fas fa-plus'></span> Asignar Certificados" ToolTip="Asignar los certificados al trabajador" />
                            <div class="dropdown font-sans-serif btn-reveal-trigger">
                              <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesEquipo" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                              <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesEquipo">
                                  <asp:LinkButton ID="lnkbtnVerPersonal" runat="server" ToolTip="Ver certificados del personal" Text="<span class='fas fa-file me-2'></span>Ver Cert.Personal" CssClass="nav-link nav-link-card-details" />
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
						            <asp:DropDownList ID="cboFiltroPersonal" runat="server" CssClass="form-select form-select-sm js-choice" TabIndex="1">
							            <asp:ListItem>SELECCIONE EL FILTRO</asp:ListItem>
						            </asp:DropDownList>
					            </div>
					        </div>
					        <div class="col-lg-6 col-sm-6">
                                <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Buscar por:</label>
						        <div class="input-group">
							        <asp:TextBox ID="txtBuscarPersonal" runat="server" CssClass="form-control form-control-sm" TabIndex="2"
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
                                        <asp:GridView ID="grdLista" runat="server" AutoGenerateColumns="False" TabIndex="4" DataKeyNames="Codigo"
                                            GridLines="None" CssClass="table table-responsive-sm table-hover small" AllowPaging="True" 
                                            EmptyDataText="No hay registros a visualizar" PageSize="14" >
                                            <PagerStyle CssClass="pgr" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <AlternatingRowStyle CssClass="alt" />
                                            <Columns>
                                                <asp:BoundField DataField="Codigo" HeaderText="Código" />
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                                                <asp:BoundField DataField="Fecha_Ing" HeaderText="Fecha Ing." />
                                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                                <asp:BoundField DataField="Genero" HeaderText="Género" />
                                                <asp:BoundField DataField="Estado" HeaderText="Estado" />
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
            <asp:AsyncPostBackTrigger controlid="btnAceptarAgregarCertificados" eventname="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="pnlContenido" runat="server">
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
                <div class="row g-0">
                    <div class="col-lg-4 pe-lg-2 mb-3">
                      <div class="card mb-2">
                        <div class="card-header">
                          <div class="row align-items-center">
                            <div class="col">
                              <h6 class="mb-0">Datos Generales</h6>
                            </div>
                          </div>
                        </div>
                        <div class="card-body">
                          <div class="row">
                            <div class="col-lg-12 col-sm-12">
                              <div class="row">
                                <div class="col-lg-4 col-md-4 mt-2">
                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Id.Personal:</label>
                                    <div class="input-group">
                                        <p class="fs--1 m-0"><asp:Label ID="lblIdPersonal" runat="server" Text="" CssClass="text-500"></asp:Label></p>
                                    </div>
                                </div>
                                <div class="col-lg-8 col-md-8 mt-2">
                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Nombre Completo:</label>
                                    <div class="input-group">
                                        <p class="fs--1 m-0"><asp:Label ID="lblNombreCompletoPersonal" runat="server" Text="" CssClass="text-500"></asp:Label></p>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-md-2 mt-2">
                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">T.Doc:</label>
                                    <div class="input-group">
                                        <p class="fs--1 m-0">  
                                            <asp:DropDownList ID="cboTipoDocumentoPersonal" runat="server" AppendDataBoundItems="True" 
                                                CssClass="form-select form-select-sm js-choice" 
                                                style="text-transform : uppercase" TabIndex="13">
                                                <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                            </asp:DropDownList>
                                        </p>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-md-2 mt-2">
                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Nro.Doc:</label>
                                    <div class="input-group">
                                        <p class="fs--1 m-0"><asp:Label ID="lblNumeroDocumentoPersonal" runat="server" Text="" CssClass="text-500"></asp:Label></p>
                                    </div>
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                    <div class="col-lg-8 ps-lg-2 mb-3">
                      <div class="card h-lg-100">
                        <div class="card-header">
                          <div class="row flex-between-center">
                            <div class="col-auto">
                              <h6 class="mb-0">Certificados</h6>
                            </div>
                            <div class="col-auto d-flex">
                                <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                                    <div class="dropdown font-sans-serif btn-reveal-trigger">
                                        <span class="fas fa-cog"></span><label class="text-black-50 col-form-label fs--2 m-1">Opciones:</label>
                                        <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesCertificados" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                                        <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesCertificados">
                                            <asp:LinkButton ID="lnkbtnNuevoCertificado" runat="server" OnClick="lnkbtnNuevoCertificado_Click" ToolTip="Crear un certificado" Text="<span class='fas fa-file me-2'></span>Nuevo" CssClass="nav-link nav-link-card-details" />
                                            <asp:LinkButton ID="lnkbtnEditarCertificado" runat="server" OnClick="lnkbtnEditarCertificado_Click" ToolTip="Editar un certificado" Text="<span class='fas fa-file-signature me-2'></span>Editar" CssClass="nav-link nav-link-card-details" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                          </div>
                        </div>
                        <div class="card-body h-100">
                            <div runat="server" class="row justify-content-between">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <span class="fas fa-link"></span><label class="text-black-50 col-form-label fs--2 m-1">Seleccione el certificado a adicionar:</label>
                                    <div class="input-group">
                                        <asp:DropDownList ID="cboCertificado" runat="server" AppendDataBoundItems="True" TabIndex="29"
										    CssClass="form-select form-select-sm js-choice" style="text-transform :uppercase">
										    <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
									    </asp:DropDownList>
                                        <asp:Button ID="btnAdicionarCertificado" runat="server" Text="Agregar" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Certificado" TabIndex="30"/>
                                    </div>
                                </div>
                            </div>
                            <div class="kanban-items-container scrollbar">
                                <div class="row">
                                    <div class="col-md-12 mt-3">
                                        <div class="table-responsive">
                                            <asp:HiddenField ID="hfdValoresCertificado" runat="Server"></asp:HiddenField>
                                            <div class="table-responsive scrollbar">
                                                <asp:GridView ID="grdListaCertificados" runat="server" AutoGenerateColumns="False" TabIndex="4" 
                                                    GridLines="None" CssClass="table table-striped table-hover small overflow-hidden" AllowPaging="True" 
                                                    EmptyDataText="No hay registros a visualizar" PageSize="14" OnRowCommand="grdListaCertificados_RowCommand_Botones" >
                                                    <PagerStyle CssClass="pgr" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:BoundField DataField="NroCertificado" HeaderText="Nro.Certificado" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="p-1 m-0 small" />
                                                        <asp:BoundField DataField="Descripcion" HeaderText="Certificado" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="p-1 m-0 small" />
                                                        <asp:BoundField DataField="FechaVigenciaInicio" HeaderText="Fec. Inicio" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="p-1 m-0 small" />
                                                        <asp:BoundField DataField="FechaVigenciaFinal" HeaderText="Fec. Final" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="p-1 m-0 small" />
                                                        <asp:TemplateField HeaderText="Estado" ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="p-1 m-0 small">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEstado" runat="server" Visible='<%# If(Eval("Status") = "", "false", "true") %>' CssClass='<%#If(Eval("Status") = "POR VENCER", "texto_estado short_estado black borde_redondo1", If(Eval("Status") = "EN VIGENCIA", "texto_estado short_estado verde borde_redondo1", "texto_estado short_estado orange borde_redondo1")) %>' Text='<%# StrConv(Eval("Status"), VbStrConv.ProperCase) %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="PDF" ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="p-1 m-0 small">
                                                            <ItemTemplate>
                                                                <div class="col-12">
                                                                    <asp:HyperLink ID="hlnkPDF" runat="server" Target="_blank" ImageUrl="Imagenes/doc-pdf.png"
                                                                        NavigateUrl='<%# "~/DocCertificadoPDF/" & Eval("UrlDescarga", "{0}") %>' 
                                                                        Text="" CssClass="button short green borde_redondo2" Visible='<%# If(Eval("UrlDescarga").ToString.Length > 0, "true", "false") %>' ></asp:HyperLink>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Acciones" ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="p-1 m-0 small" >
                                                            <ItemTemplate>
                                                                <div class="dropdown font-sans-serif position-static">
                                                                    <button class="btn btn-link text-600 btn-sm dropdown-toggle btn-reveal" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h fs--1"></span></button>
                                                                    <div class="dropdown-menu dropdown-menu-end border py-0">
                                                                        <div class="bg-white py-2">
                                                                            <div class="d-grid gap-0">
                                                                                <div class="col-12">
                                                                                    <ul class="nav flex-lg-column fs--1">
                                                                                        <li class="nav-item me-2 me-lg-0">
                                                                                            <asp:LinkButton ID="lnkbtnEliminar" runat="server"  CommandName="Eliminar" ToolTip="Eliminar Certificado" CommandArgument='<%# Eval("Codigo") & "*" & Eval("NroCertificado") & "*" & Eval("Status") & "*" & Eval("FechaVigenciaInicio") %>' Text="<span class='fas fa-file-archive me-2'></span>Eliminar" CssClass="nav-link nav-link-card-details" ValidationGroup="vgrpValidar" />
                                                                                            <asp:LinkButton ID="lnkbtnAdjuntar" runat="server"  CommandName="Adjuntar" ToolTip="Adjuntar Certificado" CommandArgument='<%# Eval("Codigo") & "*" & Eval("NroCertificado") & "*" & Eval("Status") & "*" & Eval("FechaVigenciaInicio") %>' Text="<span class='fas fa-file-archive me-2'></span>Adjuntar Certificado" CssClass="nav-link nav-link-card-details" ValidationGroup="vgrpValidar" />
                                                                                        </li>
                                                                                    </ul>
                                                                                </div>
                                                                            </div>
                                                                        </div>
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
                                                                <span class="col-sm-12 col-form-label-sm text-center text-900">¿Seguro desea eliminar este certificado asignado?</span>
                                                            </div>
                                                        </div>
                                                        <div class="modal-footer">
                                                            <asp:Button ID="btnSiMensajeDocumentoValidacion" runat="server" Text="Si" CssClass="btn btn-outline-facebook btn-sm d-block" ToolTip="Si desea eliminar el certificado" />
                                                            &nbsp;&nbsp;<asp:Button ID="btnNoMensajeDocumentoValidacion" runat="server" Text="No" CssClass="btn btn-outline-google-plus btn-sm d-block" ToolTip="No desea eliminar el certificado" />
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
                    </div>
                </div>
                <div class="position-fixed bottom-0 end-0 p-3">
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidar" />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="updpnlMantenimientoCertificado" runat="server">
        <ContentTemplate>
            <asp:LinkButton ID="lnk_mostrarPanelMantenimientoCertificado" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelMantenimientoCertificado_ModalPopupExtender" runat="server" 
                BackgroundCssClass="FondoAplicacion" DropShadow="False" Enabled="True" 
                CancelControlID="btnCancelarMantenimientoCertificado" 
                PopupControlID="pnlMantenimientoCertificado" TargetControlID="lnk_mostrarPanelMantenimientoCertificado">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlMantenimientoCertificado" runat="server" CssClass="container" DefaultButton="btnAceptarMantenimientoCertificado" >
                <div class="modal-dialog modal-lg">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header bg-light">
                                    <h4 class="modal-title" id="myModalLabelTarea">
                                        Mantenimiento Certificado</h4>
                                    <asp:Button ID="imgbtnCancelarCertificadoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="scrollbar">
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 mt-2">
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Código</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtIdMantenimientoCertificado" runat="server" CssClass="form-control form-control-sm" Enabled="false" 
                                                            style="text-transform :uppercase" TabIndex="18" autocomplete="off" ></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-12 mt-2">
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtDescripcionMantenimientoCertificado" runat="server" CssClass="form-control form-control-sm" 
                                                            style="text-transform :uppercase" TabIndex="18" autocomplete="off" ></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvDescripcionMantenimientoCertificado" runat="server" 
                                                            ControlToValidate="txtDescripcionMantenimientoCertificado" EnableClientScript="False" 
                                                            ErrorMessage="Ingrese la descripcion del certificado" Font-Size="10px" ForeColor="Red" 
                                                            ValidationGroup="vgrpValidarMantenimientoCertificado">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-12 mt-2">
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Descripción Abreviada</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtDescripcionAbreviadaMantenimientoCertificado" runat="server" CssClass="form-control form-control-sm" 
                                                            style="text-transform :uppercase" TabIndex="18" autocomplete="off" ></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvDescripcionAbreviadaMantenimientoCertificado" runat="server" 
                                                            ControlToValidate="txtDescripcionAbreviadaMantenimientoCertificado" EnableClientScript="False" 
                                                            ErrorMessage="Ingrese la descripcion abreviada del certificado" Font-Size="10px" ForeColor="Red" 
                                                            ValidationGroup="vgrpValidarMantenimientoCertificado">(*)</asp:RequiredFieldValidator>
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
                                                <asp:ValidationSummary ID="ValidationSummary3" runat="server" role="alert" CssClass="alert alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarMantenimientoCertificado" />
                                            </div>
                                            <div class="col-md-4 text-end p-2">
                                                <asp:Button ID="btnAceptarMantenimientoCertificado" runat="server" ValidationGroup="vgrpValidarMantenimientoCertificado"
                                                    ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                                &nbsp;
                                                <asp:Button ID="btnCancelarMantenimientoCertificado" runat="server" 
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

    <asp:UpdatePanel ID="updpnlAgregarCertificados" runat="server">
        <ContentTemplate>
            <asp:LinkButton ID="lnk_mostrarPanelAgregarCertificados" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelAgregarCertificados_ModalPopupExtender" runat="server" 
                BackgroundCssClass="FondoAplicacion" DropShadow="False" DynamicServicePath="" Enabled="True" 
                CancelControlID="btnCancelarAgregarCertificados" 
                PopupControlID="pnlSeleccionarAgregarCertificados" TargetControlID="lnk_mostrarPanelAgregarCertificados">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlSeleccionarAgregarCertificados" runat="server" CssClass="container" >
                <div class="modal-dialog modal-lg">
                    <div class="shadow rounded">
                        <div class="modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header bg-light">
                                    <h4 class="modal-title" id="myModalLabelAgregarCertificados">
                                        Agregar Certificados</h4>
                                    <asp:Button ID="imgbtnCancelarAgregarCertificadosImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="scrollbar">
                                            <div class="row mt-0 bg-200">
                                                <div class="col-12 mt-2">
                                                    <span class="far fa-clone"></span><label class="text-black-50 col-form-label col-form-label-sm fs--2 m-1">Descripción del Certificado:</label>
                                                    <div class="input-group">
                                                        <asp:Label ID="lblDescripcionAgregarCertificados" runat="server" Text="" CssClass="fs-0 mb-2 px-3"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-4 col-md-6 mt-2">
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Nro. Certificado:</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtNroCertificadoAgregarCertificados" runat="server" CssClass="form-control form-control-sm" 
                                                            TabIndex="11" style="text-transform :uppercase" placeholder="Número de certificado"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvNroCertificadoAgregarCertificados" runat="server" 
                                                            ControlToValidate="txtNroCertificadoAgregarCertificados" EnableClientScript="False" 
                                                            ErrorMessage="Ingrese el número del certificado" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidarAgregarCertificados">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-lg-4 col-md-6 mt-2">
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Fecha Vigencia Inicio:</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtFechaInicioAgregarCertificados" runat="server" CssClass="form-control form-control-sm" 
                                                            TabIndex="11" style="text-transform :uppercase" type="date" placeholder="Fecha de vigencia inicial"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvFechaInicioAgregarCertificados" runat="server" 
                                                            ControlToValidate="txtFechaInicioAgregarCertificados" EnableClientScript="False" 
                                                            ErrorMessage="Ingrese la fecha de emisión inicial" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidarAgregarCertificados">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                    <asp:CompareValidator ID="cvFechaInicioAgregarCertificados" runat="server" ValidationGroup="vgrpValidarAgregarCertificados"
                                                        ControlToCompare="txtFechaInicioAgregarCertificados" ControlToValidate="txtFechaFinalAgregarCertificados" 
                                                        ErrorMessage="Fecha inicial debe ser menor que fecha final de vigencia" Font-Size="10px" 
                                                        ForeColor="Red" Operator="GreaterThanEqual" Type="Date">(*)</asp:CompareValidator>
                                                </div>
                                                <div class="col-lg-4 col-md-12 mt-2">
                                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Fecha Vigencia Final:</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtFechaFinalAgregarCertificados" runat="server" CssClass="form-control form-control-sm" 
                                                            TabIndex="11" style="text-transform :uppercase" type="date" placeholder="Fecha de vigencial final"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvFechaFinalAgregarCertificados" runat="server" 
                                                            ControlToValidate="txtFechaFinalAgregarCertificados" EnableClientScript="False" 
                                                            ErrorMessage="Ingrese la fecha de vigencia final" Font-Size="10px" ForeColor="Red" ValidationGroup="vgrpValidarAgregarCertificados">(*)</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:ValidationSummary ID="ValidationSummary6" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarAgregarCertificados" />
                                    <div class="col-md-4 text-end p-2">
                                        <asp:Button ID="btnAceptarAgregarCertificados" runat="server" ValidationGroup="vgrpValidarAgregarCertificados"
                                            ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                        &nbsp;
                                        <asp:Button ID="btnCancelarAgregarCertificados" runat="server" 
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

    <asp:LinkButton ID="lnk_mostrarPanelSubirCertificado" runat="server"></asp:LinkButton>
    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelSubirCertificado_ModalPopupExtender" runat="server" 
        BackgroundCssClass="FondoAplicacion" DropShadow="False" DynamicServicePath="" Enabled="True" 
        CancelControlID="imgbtnCancelarSubirCertificadoImagen" 
        PopupControlID="pnlSeleccionarSubirCertificado" TargetControlID="lnk_mostrarPanelSubirCertificado">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="pnlSeleccionarSubirCertificado" runat="server" CssClass="container" >
        <div class="modal-dialog modal-lg">
            <div class="shadow rounded">
                <div class="modal-dialog-scrollable">
                    <div class="modal-content">
                        <div class="modal-header bg-light">
                            <h4 class="modal-title" id="myModalLabelSubirCertificado">
                                Agregar Imagen al Equipo</h4>
                            <asp:Button ID="imgbtnCancelarSubirCertificadoImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <label class="text-black-50 col-form-label col-form-label-sm">Archivo:</label>
								<div class="input-group">
									<asp:FileUpload ID="fupSubirCertificado" runat="server" EnableTheming="True" CssClass="form-control-sm" TabIndex="90" />
								</div>
							</div>
                        </div>
                        <div class="modal-footer">
                            <asp:ValidationSummary ID="ValidationSummary5" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarSubirCertificado" />
                            <div class="col-md-4 text-end p-2">
                                <asp:Button ID="btnAceptarSubirCertificado" runat="server" ValidationGroup="vgrpValidarSubirCertificado"
                                    ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                &nbsp;
                                <asp:Button ID="btnCancelarSubirCertificado" runat="server" 
                                    ToolTip="Cancelar Registro" TabIndex ="73" Text="Cancelar" cssclass="btn btn-outline-google-plus btn-sm"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <script language="javascript" type="text/javascript">
        function popupEmitirCertificadoPersonalReporte(IdPer) {
            window.open("Informes/frmCmmsCertificadoPersonalReporte.aspx?IdPersonal=" + IdPer, "", "toolbar=0,location=0,status=0,width=900,height=600,scrollbars=yes");
        }
    </script>

    <link href="cssbootstrap/bootstrap-switch.css" rel="stylesheet" />
    <script src="jsbootstrap/bootstrap-switch.js"></script>
</asp:Content>
