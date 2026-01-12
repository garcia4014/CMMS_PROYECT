<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmMensaje.aspx.vb" Inherits="pryCMMS.frmMensaje" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <!-- ===============================================-->
    <!--    Document Title-->
    <!-- ===============================================-->
    <title>Movitécnica - CMMS</title>

    <!-- ===============================================-->
    <!--    Stylesheets-->
    <!-- ===============================================-->
    <link href="vendors/overlayscrollbars/OverlayScrollbars.min.css" rel="stylesheet">
    <link href="assets/css/theme-rtl.min.css" rel="stylesheet">
    <link href="assets/css/theme.min.css" rel="stylesheet">
    <link href="assets/css/user-rtl.min.css" rel="stylesheet">
    <link href="assets/css/user.min.css" rel="stylesheet">

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
            background-color: #ffffff;
            position: fixed;
            z-index: 100;
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


    <script language="JavaScript">
    <!--
        var Counter = 3; /* seconds to wait */
        var AFTER_URL = "frmGnrlLogin.aspx"; /* where to send the user when the time expires */
        var TIME = 1 * 1000; /* 1 second */
        var TIMER = null;

        function stop() {
            if (TIMER) {
                clearInterval(TIMER);
                TIMER = null;
            }
        }

        function start() {
            stop();
            TIMER = setInterval("click()", TIME);
        }

        function initThis() {
            start();
        }

        function click() {
            Counter--;
            if (!Counter) {
                document.location = AFTER_URL;
            }
        }
    -->
    </script>
    <link href="Imagenes/favicon.ico" rel="shortcut icon" type="image/ico" />
    <link href="Styles/Contenido.css" rel="stylesheet" type="text/css"/>
    <%--Inicio: Para Cargar Bootstrap--%>
    <link rel="stylesheet" href="Styles/cssbootstrap/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">
    <%--Fin: Para Cargar BootStrap--%>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMensajes" runat="server" CssClass="container">
                <div class="row flex-center min-vh-100 py-6">
                    <div class="col-12">
                        <div class="card">
                            <div class="card-header py-2">
                                <h3 class="text-center h1 text-900">Mensaje del Sistema <asp:Label ID="lblAmbiente" runat="server"></asp:Label> </h3>
                            </div>
                            <div class="card-block">
                                <div class="container-fluid">
                                    <div class="row mt-2 text-center">
                                        <asp:Label ID="lblMensaje" runat="server" CssClass="text-wrap text-danger" Text="Mensaje" Font-Size="Large"></asp:Label>
                                    </div>
                                    <div class="row mt-2">
                                        <div class="col-lg-4 col-md-6 text-center">
                                            <img alt="Pie de Página" src="Imagenes/pie_mensaje.jpg" />
                                        </div>
                                        <div class="col-lg-8 col-md-6">
                                            <div class="row pt-3">
                                                <div class="col-lg-2 col-sm-3">
                                                    <img src="Imagenes/pie_urlweb.png" />
                                                </div>
                                                <div class="col-lg-10 col-sm-9">
                                                    <span>https://movitecnica.com.pe/</span>
                                                </div>
                                                <div class="col-lg-2 col-sm-3">
                                                    <img src="Imagenes/pie_whatsapp.png" />
                                                </div>
                                                <div class="col-lg-10 col-sm-9">
                                                    <span>(+51)94053-6803</span>
                                                </div>
                                                <div class="col-lg-2 col-sm-3">
                                                    <img src="Imagenes/pie_correo.png" />
                                                </div>
                                                <div class="col-lg-10 col-sm-9">
                                                    <span>info@movitecnica.com.pe</span>
                                                </div>
                                                <div class="col-lg-2 col-sm-3">
                                                    <img src="Imagenes/pie_ubicacion.png" />
                                                </div>
                                                <div class="col-lg-10 col-sm-9">
                                                    <span>Calle Pacto Andino 224, Chorrillos</span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mt-4">
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
    </form>
</body>
</html>
