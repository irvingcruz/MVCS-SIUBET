//variables globales
$URL_BASE = window.location.href;
$stateSearch = false;
$stateSendMail = false;
$infoTextPaginado = "Página {0} de {1} ({2} registros)";
$infoTextError = "Error al consultar información";
$textLoad = "Obteniendo información...";
$noDataText = "No se encontraron registros";

var notificationTypes = {
    info: 'info',
    warning: 'warning',
    error: 'error',
    success: 'success'
}

var showNotification = function (message, notificationType, duration) {
    DevExpress.ui.notify({
        message: message,
        position: {
            my: 'top center',
            at: 'top center',
            of: window,
            offset: '0 40 0 0'
        }
        //width: $(window).width()
        //,maxwidth:1200
    }, notificationType, duration);
};

//Funciones Globales


//Eventos Globales
$(document).ready(function () {
    var Tooltips = $("#Tooltip").dxTooltip({
        //contentTemplate: function (contentElement) {
        //    contentElement.append("xD");
        // },
        position: "right",
        animation: {
            show: { type: "fade", from: 0, to: 1 },
            hide: { type: "fade", from: 0, to: 0 }
        },
    });


    $("body")
        .on("mouseover", ".tooltip-app", function (e) {
            ShowTooltip($(this), $(this).attr("data-tooltip-app"));
        })
        .on("mouseout", ".tooltip-app", function (e) { HideTooltip(); });

    function ShowTooltip(dom, TextoHtml) {
        Tooltips.dxTooltip("instance").option("target", dom);
        Tooltips.dxTooltip("instance").option("contentTemplate", TextoHtml);
        Tooltips.dxTooltip("instance").show();
    }
    function HideTooltip() {
        Tooltips.dxTooltip("instance").option("contentTemplate", "");
        Tooltips.dxTooltip("instance").hide();
    }

});
//mover a otro archivo js..
//Obtener Datos Globales
