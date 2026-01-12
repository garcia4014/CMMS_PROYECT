<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PM_Cmms.Master" CodeBehind="frmBienvenida.aspx.vb" Inherits="pryCMMS.frmBienvenida" %>
<%@ Register assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        //var jsondatasemanal = [];
        //var jsondataventasmensual = [];
        //var jsondataventas4semanas = [];
        //var jsondatas = [];
        //var jsondataventastotal1 = [];
        //var jsondataventastotal2 = [];
        var jsondataOrdenTrabajoWeek = [];
        var jsondataOrdenTrabajoMonth = [];
        var jsondataOrdenTrabajoYear = [];
        var strData1 = [];
        var strData2 = [];
        var strData3 = [];
        var strData4 = [];
        var strData5 = [];
        var strData6 = [];
        var strOrdTrabajoNuevaTerminada = [];
    </script>
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cuerpo" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
            </ajaxToolkit:ToolkitScriptManager>
            <asp:Panel ID="pnlDashBoard" runat="server">  
                <div class="row g-3 mb-3">
                    <div class="col-md-3 col-xxl-3">
                        <div class="card h-md-100">
                            <div class="card-body">
                                <div class="row h-100 justify-content-between g-0">
                                    <div class="col-12 col-sm-12 col-xxl pe-2">
                                        <h6 class="mt-1">Ordenes de Trabajo Registradas<span class="ms-1 text-400" data-bs-toggle="tooltip" data-bs-placement="top" title="Ordenes de Trabajo que se encuentran registradas sin actividad"><span class="far fa-question-circle" data-fa-transform="shrink-1"></span></span></h6>

                                        <div class="d-flex justify-content-center">
                                          <div class="p-2">
                                              <asp:Label ID="lblOrdTraRegistrada" runat="server" CssClass="h1 text-primary mb-2" Text="1"></asp:Label>
                                          </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-xxl-3">
                        <div class="card h-md-100">
                            <div class="card-body">
                                <div class="row h-100 justify-content-between g-0">
                                    <div class="col-12 col-sm-12 col-xxl pe-2">
                                        <h6 class="mt-1">Ordenes de Trabajo Pendiente<span class="ms-1 text-400" data-bs-toggle="tooltip" data-bs-placement="top" title="Ordenes de Trabajo que se encuentra en proceso de ejecución"><span class="far fa-question-circle" data-fa-transform="shrink-1"></span></span></h6>
                                        <div class="d-flex justify-content-center">
                                          <div class="p-2">
                                              <asp:Label ID="lblOrdTraEnProceso" runat="server" CssClass="h1 text-primary mb-2" Text="2"></asp:Label>
                                          </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-xxl-3">
                        <div class="card h-md-100">
                            <div class="card-body">
                                <div class="row h-100 justify-content-between g-0">
                                    <div class="col-12 col-sm-12 col-xxl pe-2">
                                        <h6 class="mt-1">Ordenes de Trabajo Finalizadas<span class="ms-1 text-400" data-bs-toggle="tooltip" data-bs-placement="top" title="Ordenes de Trabajo que ya se encuentra finalizadas"><span class="far fa-question-circle" data-fa-transform="shrink-1"></span></span></h6>
                                        <div class="d-flex justify-content-center">
                                          <div class="p-2">
                                              <asp:Label ID="lblOrdTraFinalizada" runat="server" CssClass="h1 text-primary mb-2" Text="3"></asp:Label>
                                          </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-xxl-3">
                        <div class="card h-md-100">
                            <div class="card-body">
                                <div class="row h-100 justify-content-between g-0">
                                    <div class="col-12 col-sm-12 col-xxl pe-2">
                                        <h6 class="mt-1">Tareas Pendientes<span class="ms-1 text-400" data-bs-toggle="tooltip" data-bs-placement="top" title="Cantidad de Tareas Pendientes de las Ordenes de Trabajo que se encuentran en Proceso"><span class="far fa-question-circle" data-fa-transform="shrink-1"></span></span></h6>
                                        <div class="d-flex justify-content-center">
                                          <div class="p-2">
                                              <asp:Label ID="lblTareasPendientes" runat="server" CssClass="h1 text-primary mb-2" Text="4"></asp:Label>
                                          </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6 col-xxl-4">
                        <div class="card echart-session-by-browser-card h-100">
                        <div class="card-header d-flex flex-between-center bg-light py-2">
                            <h6 class="mb-0">Porcentaje de Cumplimiento</h6>
                            <div class="dropdown font-sans-serif btn-reveal-trigger">
                                <button class="btn btn-link text-600 btn-sm dropdown-toggle dropdown-caret-none btn-reveal" type="button" id="dropdown-session-by-browser" data-bs-toggle="dropdown" data-boundary="viewport" aria-haspopup="true" aria-expanded="false"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                <div class="dropdown-menu dropdown-menu-end border py-2" aria-labelledby="dropdown-session-by-browser">
                                    <a class="dropdown-item" href="#!">Ver % Cumplimiento</a>
                                </div>
                            </div>
                        </div>
                        <div class="card-body d-flex flex-column justify-content-between py-0">
                            <div class="my-auto py-5 py-md-0">
                            <!-- Find the JS file for the following chart at: src/js/charts/echarts/session-by-browser.js-->
                            <!-- If you are not using gulp based workflow, you can find the transpiled code at: public/assets/js/theme.js-->
                            <div class="echart-session-by-browser h-100" data-echart-responsive="true"></div>
                            </div>
                            <div class="border-top">
                            <table class="table table-sm mb-0">
                                <tbody>
                                <tr>
                                    <td class="py-3">
                                    <div class="d-flex align-items-center"><img src="Imagenes/ord_trabajo-R.png" alt="" width="16" />
                                        <h6 class="text-600 mb-0 ms-2">O.T. Registradas</h6>
                                    </div>
                                    </td>
                                    <td class="py-3">
                                    <div class="d-flex align-items-center"><span class="fas fa-circle fs--2 me-2 text-primary"></span>
                                        <asp:Label ID="lblPorcentajeOTRegistradas" runat="server" Text="Label" ClientIDMode="Static" CssClass="fw-normal text-700 mb-0"></asp:Label>
                                    </div>
                                    </td>
                                    <td class="py-3">
                                    <div class="d-flex align-items-center justify-content-end"><span class="fas fa-caret-down text-danger"></span>
                                        <h6 class="fs--2 mb-0 ms-2 text-700">Cantidad: <asp:Label ID="lblCantidadOTRegistradas" runat="server" Text="0" ClientIDMode="Static"></asp:Label></h6>
                                    </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="py-3">
                                    <div class="d-flex align-items-center"><img src="Imagenes/ord_trabajo-P.png" alt="" width="16" />
                                        <h6 class="text-600 mb-0 ms-2">O.T. En Proceso</h6>
                                    </div>
                                    </td>
                                    <td class="py-3">
                                    <div class="d-flex align-items-center"><span class="fas fa-circle fs--2 me-2 text-success"></span>
                                        <asp:Label ID="lblPorcentajeOTEnProceso" runat="server" Text="Label" ClientIDMode="Static" CssClass="fw-normal text-700 mb-0"></asp:Label>
                                    </div>
                                    </td>
                                    <td class="py-3">
                                    <div class="d-flex align-items-center justify-content-end"><span class="fas fa-caret-down text-danger"></span>
                                        <h6 class="fs--2 mb-0 ms-2 text-700">Cantidad: <asp:Label ID="lblCantidadOTEnProceso" runat="server" Text="0" ClientIDMode="Static"></asp:Label></h6>
                                    </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="py-3">
                                    <div class="d-flex align-items-center"><img src="Imagenes/ord_trabajo-T.png" alt="" width="16" />
                                        <h6 class="text-600 mb-0 ms-2">O.T. Finalizadas</h6>
                                    </div>
                                    </td>
                                    <td class="py-3">
                                    <div class="d-flex align-items-center"><span class="fas fa-circle fs--2 me-2 text-info"></span>
                                        <asp:Label ID="lblPorcentajeOTFinalizadas" runat="server" Text="Label" ClientIDMode="Static" CssClass="fw-normal text-700 mb-0"></asp:Label>
                                    </div>
                                    </td>
                                    <td class="py-3">
                                    <div class="d-flex align-items-center justify-content-end"><span class="fas fa-caret-down text-danger"></span>
                                        <h6 class="fs--2 mb-0 ms-2 text-700">Cantidad: <asp:Label ID="lblCantidadOTFinalizadas" runat="server" Text="0" ClientIDMode="Static"></asp:Label></h6>
                                    </div>
                                    </td>
                                </tr>
                                </tbody>
                            </table>
                            </div>
                        </div>
                        <div class="card-footer bg-light py-2">
                            <div class="row flex-between-center g-0">
                                <div class="col-auto">
                                    <select class="form-select form-select-sm" data-target=".echart-session-by-browser" onchange="DisplaySport(this.selectedIndex);">
                                    <option value="week" selected="selected">Los últimos 7 días</option>
                                    <option value="month">El último mes</option>
                                    <option value="year">El último año</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        </div>
                    </div>

                    <%--El otro recuadro--%>
                    <div class="col-md-6 col-xxl-4 order-xxl-1">
                        <div class="card h-100">
                        <div class="card-header bg-light">
                            <div class="row justify-content-between">
                            <div class="col-auto">
                                <h6>O.T.Actual VS O.T.Mes Anterior</h6>
                                <div class="d-flex align-items-center">
                                </div>
                            </div>
                            <div class="col-auto">
                                <select class="form-select form-select-sm pe-4" id="select-returning-customer-month">
                                <option value="0">Ene</option>
                                <option value="1">Feb</option>
                                <option value="2">Mar</option>
                                <option value="3">Abr</option>
                                <option value="4">May</option>
                                <option value="5">Jun</option>
                                <option value="6">Jul</option>
                                <option value="7">Ago</option>
                                <option value="8">Sep</option>
                                <option value="9">Oct</option>
                                <option value="10">Nov</option>
                                <option value="11">Dic</option>
                                </select>
                            </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <!-- Find the JS file for the following chart at: src/js/charts/echarts/returning-customer-rate.js-->
                            <!-- If you are not using gulp based workflow, you can find the transpiled code at: public/assets/js/theme.js-->
                            <div class="echart-line-returning-customer-rate h-100" data-echart-responsive="true" data-options='{"target":"returning-customer-rate-footer","monthSelect":"select-returning-customer-month","optionOne":"newMonth","optionTwo":"returningMonth"}'></div>
                        </div>
                        <div class="card-footer border-top py-2">
                            <div class="row align-items-center gx-0" id="returning-customer-rate-footer">
                            <div class="col-auto me-2">
                                <div class="btn btn-sm btn-text d-flex align-items-center p-0 shadow-none" id="newMonth"><span class="fas fa-circle text-primary fs--2 me-1"></span>O.T. Mes Actual </div>
                            </div>
                            <div class="col-auto">
                                <div class="btn btn-sm btn-text d-flex align-items-center p-0 shadow-none" id="returningMonth"><span class="fas fa-circle text-warning fs--2 me-1"></span>O.T. Mes Anterior </div>
                            </div>
                            </div>
                        </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            
        <script type="text/javascript" language="javascript">
            function DisplaySport(x) {
                if (x == 0) {
                    document.getElementById("lblPorcentajeOTRegistradas").innerHTML = strData1[0];
                    document.getElementById("lblPorcentajeOTEnProceso").innerHTML = strData1[1];
                    document.getElementById("lblPorcentajeOTFinalizadas").innerHTML = strData1[2];
                    document.getElementById("lblCantidadOTRegistradas").innerHTML = strData4[0];
                    document.getElementById("lblCantidadOTEnProceso").innerHTML = strData4[1];
                    document.getElementById("lblCantidadOTFinalizadas").innerHTML = strData4[2];
                }
                if (x == 1) {
                    document.getElementById("lblPorcentajeOTRegistradas").innerHTML = strData2[0];
                    document.getElementById("lblPorcentajeOTEnProceso").innerHTML = strData2[1];
                    document.getElementById("lblPorcentajeOTFinalizadas").innerHTML = strData2[2];
                    document.getElementById("lblCantidadOTRegistradas").innerHTML = strData5[0];
                    document.getElementById("lblCantidadOTEnProceso").innerHTML = strData5[1];
                    document.getElementById("lblCantidadOTFinalizadas").innerHTML = strData5[2];
                }
                if (x == 2) {
                    document.getElementById("lblPorcentajeOTRegistradas").innerHTML = strData3[0];
                    document.getElementById("lblPorcentajeOTEnProceso").innerHTML = strData3[1];
                    document.getElementById("lblPorcentajeOTFinalizadas").innerHTML = strData3[2];
                    document.getElementById("lblCantidadOTRegistradas").innerHTML = strData6[0];
                    document.getElementById("lblCantidadOTEnProceso").innerHTML = strData6[1];
                    document.getElementById("lblCantidadOTFinalizadas").innerHTML = strData6[2];
                }
            }
        </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
