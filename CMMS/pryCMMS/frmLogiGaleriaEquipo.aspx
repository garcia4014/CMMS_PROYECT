<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmLogiGaleriaEquipo.aspx.vb" Inherits="pryCMMS.frmLogiGaleriaEquipo" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- ===============================================-->
    <!--    Stylesheets-->
    <!-- ===============================================-->
    <%--Inicio: Este es para la galeria--%>
    <link href="vendors/flatpickr/flatpickr.min.css" rel="stylesheet">
    <link href="vendors/glightbox/glightbox.min.css" rel="stylesheet">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cuerpo" runat="server">
    <link href="Content/CuadroDialogo.css" rel="stylesheet" type="text/css"/>
    <script src="Scripts/jsUpdateProgress.js" type="text/javascript"></script>
    <link href="Content/AutoCompletar.css" rel="stylesheet" type="text/css"/>

    <asp:UpdatePanel ID ="updpnlContent" runat="server">
        <ContentTemplate>
            <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
            </ajaxToolkit:ToolkitScriptManager>
            <div class="row g-0">
                <div class="col-lg-12 mb-3">
                  <div class="row g-0 h-100 align-items-stretch">
                    <div class="col-12">
                      <div class="card h-100">
                        <div class="card-body overflow-hidden">
                          <p>Galería de imagenes.</p>
                          <div class="row mx-n1">
                            <asp:DataList ID="lstGaleriaEquipo" runat="server" DataKeyField="Codigo" 
                                    RepeatColumns="3" RepeatDirection="Horizontal" >
                                <ItemTemplate>
                                    <div class="row">
                                        <div class="mb-4 col-12">
                                            <div class="border rounded-1 h-100 d-flex flex-column justify-content-between pb-3">
                                                <div class="overflow-hidden">
                                                    <div class="position-relative rounded-top overflow-hidden">
                                                        <div class="col-12 p-1"><a href='<%# "Imagenes/Equipo/" & Eval("NombreArchivo") %> ' data-gallery="gallery-1"><img class="img-fluid rounded" src='<%# "Imagenes/Equipo/" & Eval("NombreArchivo") %> ' alt="" /></a></div>
                                                    </div>
                                                    <div class="p-3">
                                                        <h5 class="fs-0"><asp:Label ID="lblTitulo" runat="server" Text='<%# Eval("Titulo") %>' CssClass="text-dark"></asp:Label></h5>
                                                        <p class="fs--1 mb-3"><asp:Label ID="lblCodigo" runat="server" Text='<%# Eval("Codigo") %>' CssClass="mgrid_small text-500"></asp:Label></p>
                                                        <p class="fs--1 mb-1"><asp:Label ID="lblDescripcion" runat="server" Text='<%# Eval("Descripcion") %>'></asp:Label></p>
                                                        <p class="fs--1 mb-1"><asp:Label ID="lblObservacion" runat="server" CssClass="mgrid_tipo_afectacion" Text='<%# Eval("Observacion") %>'></asp:Label></p>
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
                    </div>
                  </div>
                </div>
            </div>
            <div class="container" data-layout="container">
                <script>
                    var isFluid = JSON.parse(localStorage.getItem('isFluid'));
                    if (isFluid) {
                        var container = document.querySelector('[data-layout]');
                        container.classList.remove('container');
                        container.classList.add('container-fluid');
                    }
                </script>
                <nav class="navbar navbar-light navbar-vertical navbar-expand-xl">
                  <script>
                      var navbarStyle = localStorage.getItem("navbarStyle");
                      if (navbarStyle && navbarStyle !== 'transparent') {
                          document.querySelector('.navbar-vertical').classList.add(`navbar-${navbarStyle}`);
                      }
                  </script>          
                </nav>

            </div>
            <div class="position-fixed bottom-0 end-0 p-3">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" role="alert" CssClass="toast alert-warning alert-dismissible fade show" ValidationGroup="vgrpValidar" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <link href="cssbootstrap/bootstrap-switch.css" rel="stylesheet" />
    <script src="jsbootstrap/bootstrap-switch.js"></script>

    <!-- ===============================================-->
    <!--    JavaScripts Galeria de imagenes-->
    <!-- ===============================================-->
    <script src="vendors/anchorjs/anchor.min.js"></script>
    <script src="vendors/glightbox/glightbox.min.js"> </script>
    <script src="vendors/lodash/lodash.min.js"></script>
    <script src="assets/js/theme.js"></script>
</asp:Content>
