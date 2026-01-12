<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/PM_Cmms.Master" CodeBehind="Web2.aspx.vb" Inherits="pryCMMS.Web2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="assets/css/theme-rtl.min.css" rel="stylesheet" id="style-rtl">
    <link href="assets/css/theme.min.css" rel="stylesheet" id="style-default">
    <link href="assets/css/user-rtl.min.css" rel="stylesheet" id="user-style-rtl">
    <link href="assets/css/user.min.css" rel="stylesheet" id="user-style-default">


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

            #BarraHTML ul {
                list-style-type: none;
            }

            #BarraHTML li {
                display: inline;
                text-align: center;
                margin: 0 0 0 0;
            }

                #BarraHTML li a {
                    padding: 2px 7px 2px 7px;
                    text-decoration: none;
                }

                    #BarraHTML li a:hover {
                        background-color: #333333;
                        color: #ffffff;
                    }

        #texto {
            padding: 60px 0 0 0;
        }

        .kanban-items-container.scrollbar {
            max-height: 500px; /* Ajusta según la altura deseada */
            overflow-y: auto;
        }
    </style>


    <script>
        // Función para guardar la posición del scroll
        function guardarScroll() {
            sessionStorage.setItem("scrollPos", JSON.stringify({
                y: window.scrollY,
                x: window.scrollX
            }));
        }

        // Función para restaurar la posición del scroll
        function restaurarScroll() {
            const coords = JSON.parse(sessionStorage.getItem("scrollPos"));
            if (coords) {
                window.scrollTo(coords.x, coords.y);
            }
        }

        // Restaurar el scroll al cargar la página
        window.onload = function () {
            restaurarScroll();
        };
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cuerpo" runat="server">

    <div>
        <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" OnClientClick="guardarScroll();" OnClick="btnActualizar_Click" />
        <asp:GridView ID="yourGridView" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="Column1" HeaderText="Column 1" />
                <asp:BoundField DataField="Column2" HeaderText="Column 2" />
                <asp:BoundField DataField="Column3" HeaderText="Column 3" />
            </Columns>
        </asp:GridView>
    </div>

    <link href="cssbootstrap/bootstrap-switch.css" rel="stylesheet" />
    <script src="jsbootstrap/bootstrap-switch.js"></script>

    <script src="vendors/anchorjs/anchor.min.js"></script>
    <script src="vendors/glightbox/glightbox.min.js"> </script>
    <script src="vendors/lodash/lodash.min.js"></script>
    <script src="assets/js/theme.js"></script>
</asp:Content>
