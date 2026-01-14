<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WebForm1.aspx.vb" Inherits="pryCMMS.WebForm1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>


    <!-- ===============================================-->
    <!--    Stylesheets-->
    <!-- ===============================================-->
    <link href="vendors/flatpickr/flatpickr.min.css" rel="stylesheet">    <link href="vendors/glightbox/glightbox.min.css" rel="stylesheet">
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
    <form id="form1" runat="server">
    <asp:UpdatePanel ID ="updpnlContent" runat="server">
        <ContentTemplate>
            <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
            </ajaxToolkit:ToolkitScriptManager>


            <div class="row g-0">
                <div class="col-lg-8 pe-lg-2 mb-3">
                  <div class="row g-0 h-100 align-items-stretch">
                    <div class="col-12">
                      <div class="card h-100">
                        
                        <div class="card-body overflow-hidden">
                          <p>Rowan Sebastian Atkinson CBE is an English actor, comedian and screenwriter best known for his work on the sitcoms Blackadder and Mr. Bean.</p>
                          <div class="row mx-n1">
                            <%--<div class="col-6 p-1"><a href="assets/img/generic/4.jpg" data-gallery="gallery-1"><img class="img-fluid rounded" src="assets/img/generic/4.jpg" alt="" /></a></div>
                            <div class="col-6 p-1"><a href="assets/img/generic/5.jpg" data-gallery="gallery-1"><img class="img-fluid rounded" src="assets/img/generic/5.jpg" alt="" /></a></div>
                            <div class="col-4 p-1"><a href="assets/img/gallery/4.jpg" data-gallery="gallery-1"><img class="img-fluid rounded" src="assets/img/gallery/4.jpg" alt="" /></a></div>
                            <div class="col-4 p-1"><a href="assets/img/gallery/5.jpg" data-gallery="gallery-1"><img class="img-fluid rounded" src="assets/img/gallery/5.jpg" alt="" /></a></div>
                            <div class="col-4 p-1"><a href="assets/img/gallery/3.jpg" data-gallery="gallery-1"><img class="img-fluid rounded" src="assets/img/gallery/3.jpg" alt="" /></a></div>--%>

                              <asp:DataList ID="lstGaleriaEquipo" runat="server" DataKeyField="Codigo" 
                                    RepeatColumns="2" RepeatDirection="Horizontal" >
                                    <ItemTemplate>
                                        <div class="row">
                                            <div class="mb-4 col-12">
                                                <div class="border rounded-1 h-100 d-flex flex-column justify-content-between pb-3">
                                                    <div class="overflow-hidden">
                                                        <div class="position-relative rounded-top overflow-hidden">
                                                            <%--<a class="d-block" href="WebForm1.aspx">--%>
                                                                <div class="col-12 p-1"><a href='<%# "Imagenes/Equipo/" & Eval("Codigo") & "-" & Eval("NumeroItem") & ".jpg" %> ' data-gallery="gallery-1"><img class="img-fluid rounded" src='<%# "Imagenes/Equipo/" & Eval("Codigo") & "-" & Eval("NumeroItem") & ".jpg" %> ' alt="" /></a></div>
                                                            <%--</a>--%>
                                                        </div>
                                                        <div class="p-3">
                                                            <h5 class="fs-0"><asp:Label ID="lblCodigo" runat="server" Text='<%# Eval("Titulo") %>' CssClass="text-dark"></asp:Label></h5>
                                                            <p class="fs--1 mb-3"><asp:Label ID="lblDescripcion" runat="server" Text='<%# Eval("Codigo") %>' CssClass="mgrid_small text-500"></asp:Label></p>
                                                            <p class="fs--1 mb-1"><asp:Label ID="lblTipoAfectacionDetalle" runat="server" Text='<%# Eval("Descripcion") %>'></asp:Label></p>
                                                            <p class="fs--1 mb-1"><asp:Label ID="Label1" runat="server" CssClass="mgrid_tipo_afectacion" Text='<%# Eval("Observacion") %>'></asp:Label></p>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%--<div class="row">
                                            <div class="mb-4 col-12">
                                                <div class="border rounded-1 h-100 d-flex flex-column justify-content-between pb-3">
                                                    <div class="overflow-hidden">
                                                        <div class="position-relative rounded-top overflow-hidden">--%>
                                                           <%-- <a class="d-block" href="../../../app/e-commerce/product/product-details.html">--%>
                                                                <%--<asp:Image ID="imgProducto" runat="server" CssClass="img-fluid rounded-top" ImageUrl='<%# "Imagenes/Equipo/" & Eval("Codigo") & "-" & Eval("NumeroItem") & ".jpg" %> '/>--%>
                                                                <%--<div class="col-4 p-1"><a href='<%# "Imagenes/Equipo/" & Eval("Codigo") & "-" & Eval("NumeroItem") & ".jpg" %> ' data-gallery="gallery-1"><img class="img-fluid rounded" src='<%# "Imagenes/Equipo/" & Eval("Codigo") & "-" & Eval("NumeroItem") & ".jpg" %> ' alt="" /></a></div>--%>
                                                           <%-- </a>--%>
                                                        <%--</div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>--%>
                                    </ItemTemplate>
                                </asp:DataList>


                          </div>
                        </div>
                        
                      </div>
                    </div>
                  </div>
                </div>
            </div>
            



        </ContentTemplate>
    </asp:UpdatePanel>
    
    <link href="cssbootstrap/bootstrap-switch.css" rel="stylesheet" />
    <script src="jsbootstrap/bootstrap-switch.js"></script>

    <script src="vendors/anchorjs/anchor.min.js"></script>
    <script src="vendors/glightbox/glightbox.min.js"> </script>
    <script src="vendors/lodash/lodash.min.js"></script>
    <script src="assets/js/theme.js"></script>
    </form>
</body>
</html>
