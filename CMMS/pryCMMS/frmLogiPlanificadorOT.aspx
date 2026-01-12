<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmLogiPlanificadorOT.aspx.vb" Inherits="pryCMMS.frmLogiPlanificadorOT" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- ===============================================-->
    <!--    Favicons FileUpload-->
    <!-- ===============================================-->

    <!-- ===============================================-->
    <!--    Stylesheets-->
    <!-- ===============================================-->
    <%--Inicio: Este es para la galeria--%>
    <link href="../vendors/fullcalendar/main.min.css" rel="stylesheet">
    <link href="vendors/flatpickr/flatpickr.min.css" rel="stylesheet">
    <%--Final: Este es para la galeria--%>
    
    <%--Inicio: Este gadget es para cargar los archivos--%>
    
    <%--Final: Este gadget es para cargar los archivos--%>
    
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

            <div class="content">
              <nav class="navbar navbar-light navbar-glass navbar-top navbar-expand">
                <button class="btn navbar-toggler-humburger-icon navbar-toggler me-1 me-sm-3" type="button" data-bs-toggle="collapse" data-bs-target="#navbarVerticalCollapse" aria-controls="navbarVerticalCollapse" aria-expanded="false" aria-label="Toggle Navigation"><span class="navbar-toggle-icon"><span class="toggle-line"></span></span></button>
                <a class="navbar-brand me-1 me-sm-3" href="../index.html">
                  <div class="d-flex align-items-center"><img class="me-2" src="../assets/img/icons/spot-illustrations/falcon.png" alt="" width="40" /><span class="font-sans-serif">falcon</span>
                  </div>
                </a>
                <ul class="navbar-nav align-items-center d-none d-lg-block">
                  <li class="nav-item">
                    <div class="search-box" data-list='{"valueNames":["title"]}'>
                      <form class="position-relative" data-bs-toggle="search" data-bs-display="static">
                        <input class="form-control search-input fuzzy-search" type="search" placeholder="Search..." aria-label="Search" />
                        <span class="fas fa-search search-box-icon"></span>

                      </form>
                      <div class="btn-close-falcon-container position-absolute end-0 top-50 translate-middle shadow-none" data-bs-dismiss="search">
                        <div class="btn-close-falcon" aria-label="Close"></div>
                      </div>
                      <div class="dropdown-menu border font-base start-0 mt-2 py-0 overflow-hidden w-100">
                        <div class="scrollbar list py-3" style="max-height: 24rem;">
                          <h6 class="dropdown-header fw-medium text-uppercase px-card fs--2 pt-0 pb-2">Recently Browsed</h6><a class="dropdown-item fs--1 px-card py-1 hover-primary" href="../app/events/event-detail.html">
                            <div class="d-flex align-items-center">
                              <span class="fas fa-circle me-2 text-300 fs--2"></span>

                              <div class="fw-normal title">Pages <span class="fas fa-chevron-right mx-1 text-500 fs--2" data-fa-transform="shrink-2"></span> Events</div>
                            </div>
                          </a>
                          <a class="dropdown-item fs--1 px-card py-1 hover-primary" href="../app/e-commerce/customers.html">
                            <div class="d-flex align-items-center">
                              <span class="fas fa-circle me-2 text-300 fs--2"></span>

                              <div class="fw-normal title">E-commerce <span class="fas fa-chevron-right mx-1 text-500 fs--2" data-fa-transform="shrink-2"></span> Customers</div>
                            </div>
                          </a>

                          <hr class="bg-200 dark__bg-900" />
                          <h6 class="dropdown-header fw-medium text-uppercase px-card fs--2 pt-0 pb-2">Suggested Filter</h6><a class="dropdown-item px-card py-1 fs-0" href="../app/e-commerce/customers.html">
                            <div class="d-flex align-items-center"><span class="badge fw-medium text-decoration-none me-2 badge-soft-warning">customers:</span>
                              <div class="flex-1 fs--1 title">All customers list</div>
                            </div>
                          </a>
                          <a class="dropdown-item px-card py-1 fs-0" href="../app/events/event-detail.html">
                            <div class="d-flex align-items-center"><span class="badge fw-medium text-decoration-none me-2 badge-soft-success">events:</span>
                              <div class="flex-1 fs--1 title">Latest events in current month</div>
                            </div>
                          </a>
                          <a class="dropdown-item px-card py-1 fs-0" href="../app/e-commerce/product/product-grid.html">
                            <div class="d-flex align-items-center"><span class="badge fw-medium text-decoration-none me-2 badge-soft-info">products:</span>
                              <div class="flex-1 fs--1 title">Most popular products</div>
                            </div>
                          </a>

                          <hr class="bg-200 dark__bg-900" />
                          <h6 class="dropdown-header fw-medium text-uppercase px-card fs--2 pt-0 pb-2">Files</h6><a class="dropdown-item px-card py-2" href="#!">
                            <div class="d-flex align-items-center">
                              <div class="file-thumbnail me-2"><img class="border h-100 w-100 fit-cover rounded-3" src="../assets/img/products/3-thumb.png" alt="" /></div>
                              <div class="flex-1">
                                <h6 class="mb-0 title">iPhone</h6>
                                <p class="fs--2 mb-0 d-flex"><span class="fw-semi-bold">Antony</span><span class="fw-medium text-600 ms-2">27 Sep at 10:30 AM</span></p>
                              </div>
                            </div>
                          </a>
                          <a class="dropdown-item px-card py-2" href="#!">
                            <div class="d-flex align-items-center">
                              <div class="file-thumbnail me-2"><img class="img-fluid" src="../assets/img/icons/zip.png" alt="" /></div>
                              <div class="flex-1">
                                <h6 class="mb-0 title">Falcon v1.8.2</h6>
                                <p class="fs--2 mb-0 d-flex"><span class="fw-semi-bold">John</span><span class="fw-medium text-600 ms-2">30 Sep at 12:30 PM</span></p>
                              </div>
                            </div>
                          </a>

                          <hr class="bg-200 dark__bg-900" />
                          <h6 class="dropdown-header fw-medium text-uppercase px-card fs--2 pt-0 pb-2">Members</h6><a class="dropdown-item px-card py-2" href="../pages/user/profile.html">
                            <div class="d-flex align-items-center">
                              <div class="avatar avatar-l status-online me-2">
                                <img class="rounded-circle" src="../assets/img/team/1.jpg" alt="" />

                              </div>
                              <div class="flex-1">
                                <h6 class="mb-0 title">Anna Karinina</h6>
                                <p class="fs--2 mb-0 d-flex">Technext Limited</p>
                              </div>
                            </div>
                          </a>
                          <a class="dropdown-item px-card py-2" href="../pages/user/profile.html">
                            <div class="d-flex align-items-center">
                              <div class="avatar avatar-l me-2">
                                <img class="rounded-circle" src="../assets/img/team/2.jpg" alt="" />

                              </div>
                              <div class="flex-1">
                                <h6 class="mb-0 title">Antony Hopkins</h6>
                                <p class="fs--2 mb-0 d-flex">Brain Trust</p>
                              </div>
                            </div>
                          </a>
                          <a class="dropdown-item px-card py-2" href="../pages/user/profile.html">
                            <div class="d-flex align-items-center">
                              <div class="avatar avatar-l me-2">
                                <img class="rounded-circle" src="../assets/img/team/3.jpg" alt="" />

                              </div>
                              <div class="flex-1">
                                <h6 class="mb-0 title">Emma Watson</h6>
                                <p class="fs--2 mb-0 d-flex">Google</p>
                              </div>
                            </div>
                          </a>

                        </div>
                        <div class="text-center mt-n3">
                          <p class="fallback fw-bold fs-1 d-none">No Result Found.</p>
                        </div>
                      </div>
                    </div>
                  </li>
                </ul>
                <ul class="navbar-nav navbar-nav-icons ms-auto flex-row align-items-center">
                  <li class="nav-item">
                    <div class="theme-control-toggle fa-icon-wait px-2">
                      <input class="form-check-input ms-0 theme-control-toggle-input" id="themeControlToggle" type="checkbox" data-theme-control="theme" value="dark" />
                      <label class="mb-0 theme-control-toggle-label theme-control-toggle-light" for="themeControlToggle" data-bs-toggle="tooltip" data-bs-placement="left" title="Switch to light theme"><span class="fas fa-sun fs-0"></span></label>
                      <label class="mb-0 theme-control-toggle-label theme-control-toggle-dark" for="themeControlToggle" data-bs-toggle="tooltip" data-bs-placement="left" title="Switch to dark theme"><span class="fas fa-moon fs-0"></span></label>
                    </div>
                  </li>
                  <li class="nav-item">
                    <a class="nav-link px-0 notification-indicator notification-indicator-warning notification-indicator-fill fa-icon-wait" href="../app/e-commerce/shopping-cart.html"><span class="fas fa-shopping-cart" data-fa-transform="shrink-7" style="font-size: 33px;"></span><span class="notification-indicator-number">1</span></a>

                  </li>
                  <li class="nav-item dropdown">
                    <a class="nav-link notification-indicator notification-indicator-primary px-0 fa-icon-wait" id="navbarDropdownNotification" href="#" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="fas fa-bell" data-fa-transform="shrink-6" style="font-size: 33px;"></span></a>
                    <div class="dropdown-menu dropdown-menu-end dropdown-menu-card dropdown-menu-notification" aria-labelledby="navbarDropdownNotification">
                      <div class="card card-notification shadow-none">
                        <div class="card-header">
                          <div class="row justify-content-between align-items-center">
                            <div class="col-auto">
                              <h6 class="card-header-title mb-0">Notifications</h6>
                            </div>
                            <div class="col-auto ps-0 ps-sm-3"><a class="card-link fw-normal" href="#">Mark all as read</a></div>
                          </div>
                        </div>
                        <div class="scrollbar-overlay" style="max-height:19rem">
                          <div class="list-group list-group-flush fw-normal fs--1">
                            <div class="list-group-title border-bottom">NEW</div>
                            <div class="list-group-item">
                              <a class="notification notification-flush notification-unread" href="#!">
                                <div class="notification-avatar">
                                  <div class="avatar avatar-2xl me-3">
                                    <img class="rounded-circle" src="../assets/img/team/1-thumb.png" alt="" />

                                  </div>
                                </div>
                                <div class="notification-body">
                                  <p class="mb-1"><strong>Emma Watson</strong> replied to your comment : "Hello world 😍"</p>
                                  <span class="notification-time"><span class="me-2" role="img" aria-label="Emoji">💬</span>Just now</span>

                                </div>
                              </a>

                            </div>
                            <div class="list-group-item">
                              <a class="notification notification-flush notification-unread" href="#!">
                                <div class="notification-avatar">
                                  <div class="avatar avatar-2xl me-3">
                                    <div class="avatar-name rounded-circle"><span>AB</span></div>
                                  </div>
                                </div>
                                <div class="notification-body">
                                  <p class="mb-1"><strong>Albert Brooks</strong> reacted to <strong>Mia Khalifa's</strong> status</p>
                                  <span class="notification-time"><span class="me-2 fab fa-gratipay text-danger"></span>9hr</span>

                                </div>
                              </a>

                            </div>
                            <div class="list-group-title border-bottom">EARLIER</div>
                            <div class="list-group-item">
                              <a class="notification notification-flush" href="#!">
                                <div class="notification-avatar">
                                  <div class="avatar avatar-2xl me-3">
                                    <img class="rounded-circle" src="../assets/img/icons/weather-sm.jpg" alt="" />

                                  </div>
                                </div>
                                <div class="notification-body">
                                  <p class="mb-1">The forecast today shows a low of 20&#8451; in California. See today's weather.</p>
                                  <span class="notification-time"><span class="me-2" role="img" aria-label="Emoji">🌤️</span>1d</span>

                                </div>
                              </a>

                            </div>
                            <div class="list-group-item">
                              <a class="border-bottom-0 notification-unread  notification notification-flush" href="#!">
                                <div class="notification-avatar">
                                  <div class="avatar avatar-xl me-3">
                                    <img class="rounded-circle" src="../assets/img/logos/oxford.png" alt="" />

                                  </div>
                                </div>
                                <div class="notification-body">
                                  <p class="mb-1"><strong>University of Oxford</strong> created an event : "Causal Inference Hilary 2019"</p>
                                  <span class="notification-time"><span class="me-2" role="img" aria-label="Emoji">✌️</span>1w</span>

                                </div>
                              </a>

                            </div>
                            <div class="list-group-item">
                              <a class="border-bottom-0 notification notification-flush" href="#!">
                                <div class="notification-avatar">
                                  <div class="avatar avatar-xl me-3">
                                    <img class="rounded-circle" src="../assets/img/team/10.jpg" alt="" />

                                  </div>
                                </div>
                                <div class="notification-body">
                                  <p class="mb-1"><strong>James Cameron</strong> invited to join the group: United Nations International Children's Fund</p>
                                  <span class="notification-time"><span class="me-2" role="img" aria-label="Emoji">🙋‍</span>2d</span>

                                </div>
                              </a>

                            </div>
                          </div>
                        </div>
                        <div class="card-footer text-center border-top"><a class="card-link d-block" href="../app/social/notifications.html">View all</a></div>
                      </div>
                    </div>

                  </li>
                  <li class="nav-item dropdown"><a class="nav-link pe-0" id="navbarDropdownUser" href="#" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                      <div class="avatar avatar-xl">
                        <img class="rounded-circle" src="../assets/img/team/3-thumb.png" alt="" />

                      </div>
                    </a>
                    <div class="dropdown-menu dropdown-menu-end py-0" aria-labelledby="navbarDropdownUser">
                      <div class="bg-white dark__bg-1000 rounded-2 py-2">
                        <a class="dropdown-item fw-bold text-warning" href="#!"><span class="fas fa-crown me-1"></span><span>Go Pro</span></a>

                        <div class="dropdown-divider"></div>
                        <a class="dropdown-item" href="#!">Set status</a>
                        <a class="dropdown-item" href="../pages/user/profile.html">Profile &amp; account</a>
                        <a class="dropdown-item" href="#!">Feedback</a>

                        <div class="dropdown-divider"></div>
                        <a class="dropdown-item" href="../pages/user/settings.html">Settings</a>
                        <a class="dropdown-item" href="../pages/authentication/card/logout.html">Logout</a>
                      </div>
                    </div>
                  </li>
                </ul>
              </nav>
              <div class="card mb-3 overflow-hidden">
                <div class="card-header">
                  <div class="row gx-0 align-items-center">
                    <div class="col-auto d-flex justify-content-end order-md-1">
                      <button class="btn icon-item icon-item-sm shadow-none p-0 me-1 ms-md-2" type="button" data-event="prev" data-bs-toggle="tooltip" title="Previous"><span class="fas fa-arrow-left"></span></button>
                      <button class="btn icon-item icon-item-sm shadow-none p-0 me-1 me-lg-2" type="button" data-event="next" data-bs-toggle="tooltip" title="Next"><span class="fas fa-arrow-right"></span></button>
                    </div>
                    <div class="col-auto col-md-auto order-md-2">
                      <h4 class="mb-0 fs-0 fs-sm-1 fs-lg-2 calendar-title"></h4>
                    </div>
                    <div class="col col-md-auto d-flex justify-content-end order-md-3">
                      <button class="btn btn-falcon-primary btn-sm" type="button" data-event="today">Today</button>
                    </div>
                    <div class="col-md-auto d-md-none">
                      <hr />
                    </div>
                    <div class="col-auto d-flex order-md-0">
                      <button class="btn btn-primary btn-sm" type="button" data-bs-toggle="modal" data-bs-target="#addEventModal"> <span class="fas fa-plus me-2"></span>Add Schedule</button>
                    </div>
                    <div class="col d-flex justify-content-end order-md-2">
                      <div class="dropdown font-sans-serif me-md-2">
                        <button class="btn btn-falcon-default text-600 btn-sm dropdown-toggle dropdown-caret-none" type="button" id="email-filter" data-bs-toggle="dropdown" data-boundary="viewport" aria-haspopup="true" aria-expanded="false"><span data-view-title="data-view-title">Month View</span><span class="fas fa-sort ms-2 fs--1"></span></button>
                        <div class="dropdown-menu dropdown-menu-end border py-2" aria-labelledby="email-filter"><a class="active dropdown-item d-flex justify-content-between" href="#!" data-fc-view="dayGridMonth">Month View<span class="icon-check"><span class="fas fa-check" data-fa-transform="down-4 shrink-4"></span></span></a><a class="dropdown-item d-flex justify-content-between" href="#!" data-fc-view="timeGridWeek">Week View<span class="icon-check"><span class="fas fa-check" data-fa-transform="down-4 shrink-4"></span></span></a><a class="dropdown-item d-flex justify-content-between" href="#!" data-fc-view="timeGridDay">Day View<span class="icon-check"><span class="fas fa-check" data-fa-transform="down-4 shrink-4"></span></span></a><a class="dropdown-item d-flex justify-content-between" href="#!" data-fc-view="listWeek">List View<span class="icon-check"><span class="fas fa-check" data-fa-transform="down-4 shrink-4"></span></span></a><a class="dropdown-item d-flex justify-content-between" href="#!" data-fc-view="listYear">Year View<span class="icon-check"><span class="fas fa-check" data-fa-transform="down-4 shrink-4"></span></span></a>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="card-body p-0">
                  <div class="calendar-outline" id="appCalendar"></div>
                </div>
              </div>
              <footer class="footer">
                <div class="row g-0 justify-content-between fs--1 mt-4 mb-3">
                  <div class="col-12 col-sm-auto text-center">
                    <p class="mb-0 text-600">Thank you for creating with Falcon <span class="d-none d-sm-inline-block">| </span><br class="d-sm-none" /> 2021 &copy; <a href="https://themewagon.com">Themewagon</a></p>
                  </div>
                  <div class="col-12 col-sm-auto text-center">
                    <p class="mb-0 text-600">v3.4.0</p>
                  </div>
                </div>
              </footer>
            </div>

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
                                                        <div class="col-12 p-1"><a href='<%# "Imagenes/Equipo/" & Eval("Codigo") & "-" & Eval("NumeroItem") & ".jpg" %> ' data-gallery="gallery-1"><img class="img-fluid rounded" src='<%# "Imagenes/Equipo/" & Eval("Codigo") & "-" & Eval("NumeroItem") & ".jpg" %> ' alt="" /></a></div>
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
    <!--    JavaScripts-->
    <!-- ===============================================-->
    <script src="vendors/popper/popper.min.js"></script>
    <script src="vendors/bootstrap/bootstrap.min.js"></script>
    <script src="vendors/anchorjs/anchor.min.js"></script>
    <script src="vendors/is/is.min.js"></script>
    <script src="vendors/fullcalendar/main.min.js"></script>
    <script src="assets/js/flatpickr.js"></script>
    <script src="vendors/dayjs/dayjs.min.js"></script>
    <script src="vendors/fontawesome/all.min.js"></script>
    <script src="vendors/lodash/lodash.min.js"></script>
    <script src="https://polyfill.io/v3/polyfill.min.js?features=window.scroll"></script>
    <script src="vendors/list.js/list.min.js"></script>
    <script src="assets/js/theme.js"></script>
</asp:Content>
