    $urlProtocol = window.location.protocol;
    $urlDomain = window.location.host;
    $localhost = $urlDomain.substr(0, 10);
    $urlReal = $urlProtocol + '//';

    if ($localhost == "localhost:") { $urlReal += $urlDomain + '/'; }
    else { $urlReal += $urlDomain + '/SIUBET/'; }

$(document).ready(function () {



    //alert($urlReal);
    $("#logo").attr("src", $urlReal + "img/img-LogoMVCS.png");
	txtSnip = $("#txtSnip").dxTextBox({
		placeholder: "Escribir Snip...",
		showClearButton: true,
	});



});

function openPopupAnular(options) {
    console.log(options);
    IDTipoMov = options.IDTipoMov;
    var _title = "";
    if (IDTipoMov == 2) _title = "Anular Devolución";
    if (IDTipoMov == 4) _title = "Anular Préstamo";
    if (IDTipoMov == 5) _title = "Anular Derivación";    
    var popupAnular = $("#popup-Anular").dxPopup({
        showTitle: true,
        title: _title,
        width: 400,
        height: "auto",
    });
    popupAnular.dxPopup("instance").show()

    var motivoAnulacion = $("#motivoAnulacion").dxTextArea({
        placeholder: "Escribe el motivo de anulación...",
        height: 60
    });
    motivoAnulacion.dxTextArea("instance").option("value", "");

    $("#btnAnular").dxButton({
        text: "Anular",
        icon: "trash",
        height: 30,
        width: 100,
        type: "danger",
        onClick: function (params) {

            vMotivo = motivoAnulacion.dxTextArea("instance").option("value");

            if (vMotivo.trim().length <= 0) {
                showNotification("Ingresar el motivo de anulación", notificationTypes.warning, 2000);
                return;
            }
            var _message = "<h6>¿Seguro que desea anular ";
            if (IDTipoMov == 2) _message += "la devolución";
            if (IDTipoMov == 4) _message += "el préstamo";
            if (IDTipoMov == 5) _message += "la derivación";
            _message += "?<h6/>";
            var result = DevExpress.ui.dialog.custom({
                title: "Confirmar",
                message: _message,
                toolbarItems: [
                    { text: "Si", onClick: function () { return true; } },
                    { text: "No", onClick: function () { return false; } }
                ]
            });

            result.show().done(function (dialogResult) {
                if (dialogResult) {

                    var args = {};
                    args.IDMovimiento = options.IDMovimiento;
                    args.vMotivo = vMotivo;
                    args.IDTipoMov = IDTipoMov;

                    $.ajax({
                        url: $urlReal + "Movimiento/AnularMovimiento",
                        data: JSON.stringify(args),
                        type: 'POST',
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (result) {
                            if (result.success) {
                                showNotification(result.message, notificationTypes.success, 2000);
                                popupAnular.dxPopup("instance").hide();
                                if (IDTipoMov == 4) {
                                    gridPrestamos.dxDataGrid("instance").refresh();
                                }
                                else {
                                    gridDevoluciones.dxDataGrid("instance").refresh();
                                }
                                
                            } else {
                                showNotification(result.message, notificationTypes.warning, 2000);
                            }
                        },
                        error: function () {
                            showNotification("Error inesperado al anular movimiento", notificationTypes.error, 2000);
                        },
                        timeout: 500000
                    });

                }
            });

        }
    });

    $("#btnCancelar").dxButton({
        text: "Cancelar",
        icon: "close",
        height: 30,
        width: 100,
        type: "info",
        onClick: function (params) {
            popupAnular.dxPopup("instance").hide();
        }
    });
}

function fnListarMovimientos(pageIndex, IDTipo, gridMovs) {
    var listado = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var deferred = $.Deferred(),
                args = {};
            args.Snip = txtSnip.dxTextBox("instance").option("value");
            if (args.Snip == "") args.Snip = 0;
            args.IDTipoMov = IDTipo; //[2]Devoluciones, [4]Préstamos, [1]Transferencias
            args.pageNumber = pageIndex == 1 ? pageIndex : gridMovs._options.paging.pageIndex + 1;
            args.pageSize = loadOptions.take || 10;

            $.ajax({
                url: $urlReal + "Movimiento/ListadoMovimientos",
                data: JSON.stringify(args),
                type: 'POST',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    //console.log(result)
                    $stateSearch = false;
                    $(".totalRowsFilter").text(result.totalRowsFilter)
                    $(".totalRows").text(result.totalRows)
                    showNotification(result.message, notificationTypes.success, 2000);
                    deferred.resolve(result.items2, { totalCount: result.totalRowsFilter });
                },
                error: function () {
                    deferred.reject($infoTextError);
                },
                timeout: 500000
            });
            return deferred.promise();
        },

    });

    gridMovs.option({
        dataSource: {
            store: listado
        },
    });
}

function openPopupRetornaRecepciona(options) {
    var movimiento = options;
    _url = movimiento.IDTipoMov != 4 ? "DDRecepcionar/" : "PreRetornar/";
    if (movimiento.IDTipoMov == 2) _title = "Recepcionar Devolución";
    if (movimiento.IDTipoMov == 4) _title = "Retornar Préstamo";
    if (movimiento.IDTipoMov == 5) _title = "Recepcionar Derivación";
    _width = movimiento.IDTipoMov == 4 ? 500 : 400;
    RetornaRecepciona = $("#popup-RetornaRecepciona").dxPopup({
        showTitle: true,
        title: _title,
        width: _width,
        height: "auto",
        contentTemplate: function () {
            var $pageContent = $("<span />");
            return $pageContent.load($urlReal + 'Movimiento/' + _url + movimiento.IDMovimiento);
        },
        showCloseButton: true,
    });

    RetornaRecepciona.dxPopup("instance").show();
}

function fnLimpiarDivs(){
    $("#popup-DD").empty();
    $("#popup-prestamo").empty();
    $("#popup-transferencia").empty();
}



