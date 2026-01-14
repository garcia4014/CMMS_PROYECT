<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmGnrlLogin.aspx.vb" Inherits="pryCMMS.frmGnrlLogin" %>
<%@ Register assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

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
    <title>Sistema CMMS - Movitécnica</title>
    <link href="Imagenes/favicon.ico" rel="shortcut icon" type="image/ico" />
    <link href="Content/Contenido.css" rel="stylesheet" type="text/css"/>

    <!-- begin meta -->
	<meta charset="utf-8">
	<meta name="description" content="Movitécnica, empresa peruana.">
	<meta name="keywords" content="Movitecnica, Movitecnica SAC, Movimiento de equipos, Gruas">
	<meta name="author" content="Movitécnica S.A.C.">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
	<!-- end meta -->

    <script type="text/javascript" language="javascript">
        //Función para deshabilitar el ENTER y ejecutar el botón por defecto.
        function stopRKey(evt) {
            var evt = (evt) ? evt : ((event) ? event : null);
            var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
            if ((evt.keyCode == 13) && ((node.type == "text") || (node.type == "radio") || (node.type == "checkbox"))) { return false; }
        }
        document.onkeypress = stopRKey;

        //Función para utilizar para al presionar el ENTER se cambie de foco.        
        function CambiaFoco(cajadestino) {
            var key = window.event.keyCode;
            if (key == 13)
                document.getElementById(cajadestino).focus();
        }
    </script>
    <!-- ===============================================-->
    <!--    Favicons-->
    <!-- ===============================================-->
    <link rel="apple-touch-icon" sizes="180x180" href="assets/img/favicons/apple-touch-icon.png">
    <link rel="manifest" href="assets/img/favicons/manifest.json">
    <meta name="msapplication-TileImage" content="assets/img/favicons/mstile-150x150.png">
    <meta name="theme-color" content="#ffffff">
    <script src="assets/js/config.js"></script>
    <script src="vendors/overlayscrollbars/OverlayScrollbars.min.js"></script>

    <!-- ===============================================-->
    <!--    Stylesheets-->
    <!-- ===============================================-->
    <link rel="preconnect" href="https://fonts.gstatic.com">
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,500,600,700%7cPoppins:300,400,500,600,700,800,900&amp;display=swap" rel="stylesheet">
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

        function reemplaza_imagen(imagen) {
            imagen.onerror = "";
            imagen.src = "Imagenes/Usr/Sin_Imagen.jpg";
            return true;
        }
    </script>

    <!--INICIO: JMUG: 30/01/2022 - Plugins para generar las notificaciones - css -->
        <link href="assets/notifications/notification.css" rel="stylesheet" />
    <!--FINAL: JMUG: 30/01/2022 - Plugins para generar las notificaciones - css -->
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
            </ajaxToolkit:ToolkitScriptManager>
            <asp:HiddenField ID="hfdIdPais" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                <asp:Panel ID="pnlCabecera" runat="server" DefaultButton="btnAcceder" CssClass="container">
                    <div class="row flex-center min-vh-100 py-6">
                    <div class="col-sm-12 col-md-10 col-lg-8 col-xl-7 col-xxl-5">
                        <div class="card">
                            <asp:Image ID="imgFondo" runat="server" 
                                CssClass="card-img-top img-fluid h-25" ImageUrl="~/Imagenes/login.jpg" />
                            <div id="centrador" class="flex-active text-center image">
                                <asp:Image ID="imgUsuario" runat="server" Height="100px" Width="100px" BorderColor="#D5D8DD" BorderStyle="Ridge" onerror="reemplaza_imagen(this);" 
                                    CssClass="img-fluid imagenusuario rounded-circle img-thumbnail" />
                            </div>
                            <div class="card-header py-2">
                                <h3 class="text-center h6 text-primary">Acceso del Cliente - CMMS <asp:Label ID="lblAmbiente" runat="server"></asp:Label> </h3>
                            </div>
                            <div class="card-block">
                                <div class="container-fluid">
                                    <div class="form-group">
                                        <div class="row mt-2">
                                            <label class="col-md-3 text-black-50 col-form-label col-form-label-sm">Empresa:</label>
                                            <div class="col-md-9">
                                                <div class="input-group">
                                                    <asp:DropDownList ID="cboEmpresa" runat="server" TabIndex="1" CssClass="form-select form-select-sm js-choice"
                                                        AppendDataBoundItems="True"
                                                        style="text-transform :uppercase" AutoPostBack="True">
                                                        <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvEmpresa" runat="server" 
                                                        ControlToValidate="cboEmpresa" EnableClientScript="False" Display="Static" InitialValue="SELECCIONE DATO"
                                                        ErrorMessage="Ingrese la empresa" 
                                                        ForeColor="Red">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row pt-1">
                                            <label class="col-md-3 text-black-50 col-form-label col-form-label-sm">Local:</label>
                                            <div class="col-md-9">
                                                <div class="input-group">
                                                    <asp:DropDownList ID="cboLocal" runat="server" TabIndex="3" CssClass="form-select form-select-sm js-choice"
                                                        AppendDataBoundItems="True" 
                                                        style="text-transform :uppercase" AutoPostBack="True">
                                                    <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvLocal" runat="server" 
                                                            ControlToValidate="cboLocal" EnableClientScript="False" Display="Static" InitialValue="SELECCIONE DATO"
                                                            ErrorMessage="Ingrese el local" 
                                                            ForeColor="Red">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row pt-1" id="divPuntoVenta" runat="server" visible="false">
                                            <label class="col-md-3 text-black-50 col-form-label col-form-label-sm">Punto Venta:</label>
                                            <div class="col-md-9">
                                                <div class="input-group">
                                                    <asp:DropDownList ID="cboPuntoVenta" runat="server" TabIndex="2" CssClass="form-select form-select-sm js-choice"
                                                        AppendDataBoundItems="True"
                                                        style="text-transform :uppercase" >
                                                        <asp:ListItem>SELECCIONE DATO</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvPuntoVenta" runat="server" 
                                                            ControlToValidate="cboPuntoVenta" EnableClientScript="False" Display="Static" InitialValue="SELECCIONE DATO"
                                                            ErrorMessage="Ingrese el punto de venta" 
                                                            ForeColor="Red">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row pt-1">
                                            <label class="col-md-3 text-black-50 col-form-label col-form-label-sm">Usuario:</label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtUsuario" runat="server" TabIndex="4" CssClass="form-control form-control-sm"
                                                        style="text-transform :uppercase" AutoPostBack="True"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" 
                                                        ControlToValidate="txtUsuario" EnableClientScript="False" 
                                                        ErrorMessage="Ingrese el usuario"  
                                                        ForeColor="Red">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <label class="col-md-3 text-black-50 col-form-label col-form-label-sm">Contraseña:</label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtContraseña" runat="server" TabIndex="5" CssClass="form-control form-control-sm"
                                                        style="text-transform :uppercase" TextMode="Password"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvContraseña" runat="server" 
                                                            ControlToValidate="txtContraseña" EnableClientScript="False" 
                                                            ErrorMessage="Ingrese la contraseña" 
                                                            ForeColor="Red">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row pt-1 no-gutters" id="divTipoCambio" runat="server" visible="false">
                                            <label class="col-sm-3 text-black-50 col-form-label col-form-label-sm">T.C. Compra:</label>
                                            <div class="col-sm-3">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtTCCompra" runat="server" TabIndex="6" CssClass="form-control form-control-sm"
                                                            style="text-transform :uppercase" Enabled="False"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvTCCompra" runat="server" 
                                                            ControlToValidate="txtTCCompra" EnableClientScript="False" 
                                                            ErrorMessage="Ingrese el tipo de cambio compra"  
                                                            ForeColor="Red">(*)</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <label class="col-sm-3 text-black-50 col-form-label col-form-label-sm">T.C. Vta:</label>
                                            <div class="col-sm-3">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtTCVenta" runat="server" TabIndex="7" CssClass="form-control form-control-sm"
                                                            style="text-transform :uppercase" Enabled="False"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvTCVenta" runat="server" 
                                                            ControlToValidate="txtTCVenta" EnableClientScript="False" 
                                                            ErrorMessage="Ingrese el tipo de cambio Venta"  
                                                            ForeColor="Red">(*)</asp:RequiredFieldValidator>
                                                    <asp:ImageButton ID="imgbtnCargarTipoCambio" runat="server" 
                                                            ImageUrl="~/Imagenes/25x25/Refrescar.png" 
                                                            ToolTip="Cargar el tipo de cambio de la Sunat" Width="25px" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group my-2 col-12 col-form-label col-form-label-sm">
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" role="alert" CssClass="alert alert-warning alert-dismissible fade show" />
                                        </div>
                                        <div class="col-md-12 mb-2">
                                            <div class="row">
                                                <div class="text-center">
                                                    <asp:Label ID="lblPrueba" runat="server"></asp:Label>
                                                    <asp:Button ID="btnAcceder" runat="server" Text="Acceder" CssClass="btn btn-primary" TabIndex="20"
                                                        ToolTip="Ingresar al Sistema"> </asp:Button>
                                                    <asp:LinkButton ID="btnMenuPrincipal" runat="server" Text="<span class='fab fa-google-play'></span> Video" CssClass="btn btn-primary" TabIndex="20"
                                                        ToolTip="Retornar al Menú Principal"/>
                                                </div>
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
        </div>
        <div>
            <!-- ===============================================-->
            <!--    JavaScripts-->
            <!-- ===============================================-->
            <script src="vendors/popper/popper.min.js"></script>
            <script src="vendors/bootstrap/bootstrap.min.js"></script>
            <script src="vendors/anchorjs/anchor.min.js"></script>
            <script src="vendors/is/is.min.js"></script>
            <script src="vendors/fontawesome/all.min.js"></script>
            <script src="vendors/lodash/lodash.min.js"></script>
            <script src="https://polyfill.io/v3/polyfill.min.js?features=window.scroll"></script>
            <script src="vendors/list.js/list.min.js"></script>
            <script src="assets/js/theme.js"></script>

            <%--INICIO: Los siguientes scripts se utilizan para las notificaciones y alertas por segundos--%>
            <script src="assets/notifications/notifications.js"></script>
            <script src="assets/js/jquery.min.js"></script>
            <script src="assets/notifications/notify.min.js"></script>
            <script src="assets/notifications/notify-metro.js"></script>
            <script src="assets/notifications/notifications.js"></script>
            <%--FINAL: Los siguientes scripts se utilizan para las notificaciones y alertas por segundos--%>
        </div>
    </form>
</body>
</html>
