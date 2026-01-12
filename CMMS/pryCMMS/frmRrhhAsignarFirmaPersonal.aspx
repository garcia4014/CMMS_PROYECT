<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmRrhhAsignarFirmaPersonal.aspx.vb" Inherits="pryCMMS.frmRrhhAsignarFirmaPersonal" %>
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
                                <h5 class="mb-0" data-anchor="data-anchor">Listado de Firmas</h5>
                                <a class="btn btn-falcon-default btn-sm ml-auto float-right" href="frmBienvenida.aspx" aria-expanded="true">
                                    <span class="fas fa-home"></span><span class="nav-link-text ps-1">Inicio</span>
                                </a>
                            </div>
                        </div>
                        <div class="col-auto ms-auto">
                          <div class="nav gap-2" role="tablist">
							<asp:LinkButton ID="btnNuevo" runat="server" OnClick="btnNuevo_Click" CssClass="btn btn-success btn-sm ml-auto float-right" Text="<span class='fas fa-plus'></span> Nueva Firma" ToolTip="Crear firma" />
                            <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-primary btn-sm ml-auto float-right" Text="<span class='fas fa-pencil-alt'></span> Editar" ToolTip="Editar firma" />
                            <%--<div class="dropdown font-sans-serif btn-reveal-trigger">
                              <button class="btn btn-sm btn-reveal py-0 px-2" type="button" id="OpcionesEquipo" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h"></span></button>
                              <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="OpcionesEquipo">
                                  <asp:LinkButton ID="lnkbtnVerPersonal" runat="server" ToolTip="Ver certificados del personal" Text="<span class='fas fa-file me-2'></span>Ver Cert.Personal" CssClass="nav-link nav-link-card-details" />
                              </div>
                            </div>--%>
                          </div>
                        </div>
                      </div>
                    </div>
                    <div class="card-body">
                        <div class="row bg-100 p-2">
					        <div class="col-lg-6 col-sm-6">
                                <span class="fas fa-filter"></span><label class="text-black-50 col-form-label fs--2 m-1">Filtrar por:</label>
                                <div class="input-group">
						            <asp:DropDownList ID="cboFiltroFirma" runat="server" CssClass="form-select form-select-sm js-choice" TabIndex="1">
							            <asp:ListItem>SELECCIONE EL FILTRO</asp:ListItem>
						            </asp:DropDownList>
					            </div>
					        </div>
					        <div class="col-lg-6 col-sm-6">
                                <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Buscar por:</label>
						        <div class="input-group">
							        <asp:TextBox ID="txtBuscarFirma" runat="server" CssClass="form-control form-control-sm" TabIndex="2"
								        style="text-transform :uppercase" placeholder="Ingrese Busqueda"></asp:TextBox>
							        &nbsp;
							        <asp:ImageButton ID="imgbtnBuscarFirma" runat="server" Height="18px" TabIndex="3" CssClass="mt-1"
								        ImageUrl="~/Imagenes/buscar.png" ToolTip="Buscar Registro" Width="21px" />
						        </div>
					        </div>
                        </div>
                        <div class="kanban-items-container scrollbar">
                            <div class="row">
                                <div class="col-md-12 mt-3">
                                    <div class="table-responsive">
                                        <asp:GridView ID="grdLista" runat="server" AutoGenerateColumns="False" TabIndex="4" DataKeyNames="IdUsuario,IdItem"
                                            GridLines="None" CssClass="table table-responsive-sm table-hover small" AllowPaging="True" 
                                            EmptyDataText="No hay registros a visualizar" PageSize="14" >
                                            <PagerStyle CssClass="pgr" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <AlternatingRowStyle CssClass="alt" />
                                            <Columns>
                                                <asp:BoundField DataField="IdUsuario" HeaderText="Código" />
                                                <asp:BoundField DataField="NombreCompleto" HeaderText="Descripción" />
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
                    </div>
                </div>
                <div class="row g-0">
                    <div class="col-lg-12 pe-lg-2 mb-3">
                      <div class="card mb-2">
                        <div class="card-header">
                          <div class="row align-items-center">
                            <div class="col">
                              <h6 class="mb-0">Datos Generales</h6>
                            </div>
                          </div>
                        </div>
                        <div class="card-body">
                          <div class="row justify-content-between">
                            <div class="col-lg-12 col-sm-12">
                               <div class="col-lg-12 col-md-12 mt-2">
                                    <span class="far fa-address-card"></span><label class="text-black-50 col-form-label fs--2 m-1">Nombre Completo:</label>
                                    <div class="input-group">
                                        <asp:DropDownList ID="cboUsuario" runat="server" AppendDataBoundItems="True" 
                                            CssClass="form-select form-select-sm js-choice" 
                                            style="text-transform : uppercase" TabIndex="13">
                                            <asp:ListItem Value="SELECCIONE DATO">SELECCIONE DATO</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Button ID="btnAdicionarFirma" runat="server" Text="Agregar" CssClass="btn btn-primary btn-sm" ToolTip="Adicionar Firma" TabIndex="30"/>
                                    </div>
                                </div>
                            </div>
                          </div>
                          <div class="kanban-items-container scrollbar">
                            <div class="row">
                                <div class="col-md-12 mt-3">
                                    <div class="table-responsive">
                                        <asp:HiddenField ID="hfdValoresFirma" runat="Server"></asp:HiddenField>
                                        <div class="table-responsive scrollbar">
                                            <asp:GridView ID="grdListaFirmas" runat="server" AutoGenerateColumns="False" TabIndex="4" 
                                                GridLines="None" CssClass="table table-striped table-hover small overflow-hidden" AllowPaging="True" 
                                                EmptyDataText="No hay registros a visualizar" PageSize="14" OnRowCommand="grdListaFirmas_RowCommand_Botones" >
                                                <PagerStyle CssClass="pgr" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <AlternatingRowStyle CssClass="alt" />
                                                <Columns>
                                                    <asp:BoundField DataField="Item" HeaderText="Nro.Firma" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="p-1 m-0 small" />
                                                    <asp:BoundField DataField="Url" HeaderText="Firma" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="p-1 m-0 small" />
                                                    <asp:BoundField DataField="FechaRegistro" HeaderText="Fec. Inicio" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="p-1 m-0 small" />
                                                    <asp:TemplateField HeaderText="Estado" ShowHeader="false" HeaderStyle-CssClass="bg-200 text-900 p-1 m-0 small" ItemStyle-CssClass="p-1 m-0 small">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEstado" runat="server" CssClass='<%#If(Eval("Estado") = True, "texto_estado short_estado verde borde_redondo1", "texto_estado short_estado red borde_redondo1") %>' Text='<%# StrConv(If(Eval("Estado") = True, "Activo", "De Baja"), VbStrConv.ProperCase) %>'></asp:Label>
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
                                                                                        <asp:LinkButton ID="lnkbtnActivar" runat="server"  CommandName="Activar" ToolTip="Activar Firma" CommandArgument='<%# Eval("Item") & "*" & Eval("CodUsu") & "*" & Eval("FechaRegistro") %>' Text="<span class='fas fa-file-archive me-2'></span>Activar" CssClass="nav-link nav-link-card-details" ValidationGroup="vgrpValidar" />
                                                                                        <asp:LinkButton ID="lnkbtnVerFirma" runat="server"  CommandName="VerFirma" ToolTip="Ver Firma" CommandArgument='<%# Eval("Item") & "*" & Eval("CodUsu") & "*" & Eval("FechaRegistro") & "*" & Eval("Url") %>' Text="<span class='fas fa-file-archive me-2'></span>Ver Firma" CssClass="nav-link nav-link-card-details" ValidationGroup="vgrpValidar" />
                                                                                        <%--<asp:LinkButton ID="lnkbtnAdjuntar" runat="server"  CommandName="Adjuntar" ToolTip="Adjuntar Firma" CommandArgument='<%# Eval("Item") & "*" & Eval("CodUsu") & "*" & Eval("FechaRegistro") %>' Text="<span class='fas fa-file-archive me-2'></span>Adjuntar Firma" CssClass="nav-link nav-link-card-details" ValidationGroup="vgrpValidar" />--%>
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
                                                     <span class="col-sm-12 col-form-label-sm text-center text-900">¿Seguro desea activar esta firma, recuerde que se desactivaran el resto de firmas asignadas a este usuario?</span>
                                                 </div>
                                             </div>
                                             <div class="modal-footer">
                                                 <asp:Button ID="btnSiMensajeDocumentoValidacion" runat="server" Text="Si" CssClass="btn btn-outline-facebook btn-sm d-block" ToolTip="Si desea activar la firma" />
                                                 &nbsp;&nbsp;<asp:Button ID="btnNoMensajeDocumentoValidacion" runat="server" Text="No" CssClass="btn btn-outline-google-plus btn-sm d-block" ToolTip="No desea activar la firma" />
                                             </div>
                                         </div>
                                     </div>
                                 </div>
                             </div>
                          </asp:Panel>
                          <br />


                            <asp:LinkButton ID="lnk_mostrarPanelVerImagenFirma" runat="server"></asp:LinkButton>
                            <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelVerImagenFirma_ModalPopupExtender" runat="server"
                                BackgroundCssClass="FondoAplicacion" DropShadow="False" DynamicServicePath="" Enabled="True"
                                CancelControlID="imgbtnCancelarVerImagenFirmaImagen"
                                PopupControlID="pnlVerImagenFirma" TargetControlID="lnk_mostrarPanelVerImagenFirma">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Panel ID="pnlVerImagenFirma" runat="server" CssClass="container">
                                <div class="modal-dialog modal-dialog-centered modal-md shadow rounded">
                                    <div class="modal-content">
                                        <div class="modal-header bg-light">
                                            <h4 class="modal-title" id="myModalLabelVerImagenFirmaImagen">Ver Imagen
                                            </h4>
                                            <asp:Button ID="imgbtnCancelarVerImagenFirmaImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                                        </div>
                                        <div class="modal-body">
                                            <div class="row">
                                                <div class="col-12">
                                                    <asp:Image ID="imgVerImagenFirma" runat="server" class="img-fluid rounded" alt="" />
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
                <div class="position-fixed bottom-0 end-0 p-3">
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidar" />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:LinkButton ID="lnk_mostrarPanelSubirFirma" runat="server"></asp:LinkButton>
    <ajaxToolkit:ModalPopupExtender ID="lnk_mostrarPanelSubirFirma_ModalPopupExtender" runat="server" 
        BackgroundCssClass="FondoAplicacion" DropShadow="False" DynamicServicePath="" Enabled="True" 
        CancelControlID="imgbtnCancelarSubirFirmaImagen" 
        PopupControlID="pnlSeleccionarSubirFirma" TargetControlID="lnk_mostrarPanelSubirFirma">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="pnlSeleccionarSubirFirma" runat="server" CssClass="container" >
        <div class="modal-dialog modal-lg">
            <div class="shadow rounded">
                <div class="modal-dialog-scrollable">
                    <div class="modal-content">
                        <div class="modal-header bg-light">
                            <h4 class="modal-title" id="myModalLabelSubirFirma">
                                Agregar Imagen de la firma</h4>
                            <asp:Button ID="imgbtnCancelarSubirFirmaImagen" runat="server" CssClass="btn-close btn btn-sm btn-circle d-flex flex-center transition-base" data-dismiss="modal" aria-label="Close" />
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <label class="text-black-50 col-form-label col-form-label-sm">Archivo:</label>
								<div class="input-group">
									<asp:FileUpload ID="fupSubirFirma" runat="server" EnableTheming="True" CssClass="form-control-sm" TabIndex="90" />
								</div>
							</div>
                        </div>
                        <div class="modal-footer">
                            <asp:ValidationSummary ID="ValidationSummary4" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidarSubirFirma" />
                            <div class="col-md-4 text-end p-2">
                                <asp:Button ID="btnAceptarSubirFirma" runat="server" ValidationGroup="vgrpValidarSubirFirma"
                                    ToolTip="Aceptar Registro" TabIndex ="72" Text="Aceptar" CssClass="btn btn-outline-facebook btn-sm" />
                                &nbsp;
                                <asp:Button ID="btnCancelarSubirFirma" runat="server" 
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
    <%--<script type="text/javascript">
        //Esta función es solo utilizada para que centre las imagenes al dar previsualizar
        //Se desactiva el sombreado porque genera conflicto en las imagenes
        function CenterModal(popupId) {
            var modal = document.getElementById(popupId);
            if (!modal) return;

            // Mostrar el modal si está oculto
            modal.style.display = 'block';

            var dialog = modal.querySelector(".modal-dialog");
            if (!dialog) return;

            // Forzar el centrado
            dialog.style.position = 'fixed';
            dialog.style.top = '50%';
            dialog.style.left = '50%';
            dialog.style.transform = 'translate(-50%, -50%)';
            dialog.style.zIndex = '10000';
        }

        Sys.Application.add_load(function () {
            // Centrar todos los modales al cargarse
            var modals = ['<%= pnlVerImagenFirma.ClientID %>',
                        '<%= pnlMensajeDocumentoValidacion.ClientID %>',
                        '<%= pnlSeleccionarSubirFirma.ClientID %>'];

            modals.forEach(function (modalId) {
                var modalExt = $find(modalId.replace('pnl', 'lnk_mostrarPanel') + '_ModalPopupExtender');
                if (modalExt) {
                    modalExt.add_shown(function () {
                        CenterModal(modalId);
                    });
                }
            });
        });

    </script>--%>
    <link href="cssbootstrap/bootstrap-switch.css" rel="stylesheet" />
    <script src="jsbootstrap/bootstrap-switch.js"></script>
</asp:Content>
