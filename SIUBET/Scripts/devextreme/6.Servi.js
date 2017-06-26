$(document).ready(function () {

    function fnLimpiarFiltros() {
        txtSnip.dxTextBox("instance").option("value", "");
    }

    $("#btnLimpiar").on("click", function () {
        fnLimpiarFiltros();
    });

    $("#btnBuscar").on("click", function () {
        fnListarMovimientos(0, 6, gridPrestamos.dxDataGrid("instance"));
    });

    gridPrestamos = $("#gridPrestamos").dxDataGrid({
        columns: [

            { dataField: "Nro", caption: "#", alignment: "center", },
            { dataField: "Correlativo", caption: "NRO.MOV.", alignment: "center", width: 100, },
            { dataField: "FechaMov", caption: "Fec. Salida", alignment: "center", width: 140, },
            //{ dataField: "Plazo", caption: "Días Trans.", alignment: "center", width: 80, },
            { dataField: "ModalidadTexto", caption: "Modalidada del Servicio", alignment: "center", width: 200, },
            {
                caption: "Cargo",
                alignment: "center",
                width: 150,
                cellTemplate: function (container, options) {
                    var item = options.data;
                    var dato = '<span>' + item.NombreFileCargo + '</span>';
                    if (item.NombreFileCargo.length > 0) {
                        dato += '&nbsp;&nbsp;</span><a href="' + $urlReal + 'Uploads/' + item.NombreFileCargo + '" target="_blank"><span class="glyphicon glyphicon-file" aria-hidden="true"></span></a>';
                    }
                    dato = $(dato);
                    dato.appendTo(container);
                }
            },
            { dataField: "EntidadDestino", caption: "Ingeniero/Otros", width: 220, },
            { dataField: "Estado", width: 130, },
            { dataField: "Motivo", caption: "Motivo Anulación", width: 200, },
            { dataField: "Activo", dataType: "boolean", alignment: "center", },
            {
                caption: "...",
                //fixed: true,
                allowHiding: false,
                alignment: "center",
                width: 35,
                cellTemplate: function (container, options) {
                    var item = options.data;
                    var disabledE = true;
                    if (item.Activo == true) {
                        if (item.NombreFileCargo.length == 0) disabledE = false;
                    }
                    $('<div />').appendTo(container)
                    .dxToolbar({
                        items: [
							{
							    location: 'after',
							    widget: 'dxButton',
							    locateInMenu: 'auto',
							    disabled: disabledE,
							    options: {
							        icon: "glyphicon glyphicon-edit",
							        text: "Editar",
							        onClick: function () {
							            openPopupPrestamoEdit(item.IDMovimiento);
							        }
							    }
							},
                        ]
                    });
                }
            },

        ],
        remoteOperations: true,
        scrolling: {
            mode: "standard",
        },
        sorting: {
            mode: "none"
        },
        paging: {
            enabled: true,
            pageIndex: 0,
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            showNavigationButtons: true,
            showInfo: true,
            allowedPageSizes: [10, 20, 50, 100, 200],
            infoText: $infoTextPaginado,
        },
        //"export": {
        //    enabled: true,
        //    fileName: "Listado Movimientos",
        //    allowExportSelectedData: true
        //},
        loadPanel: {
            text: $textLoad
        },
        noDataText: $noDataText,
        selection: {
            mode: "single"
        },
        wordWrapEnabled: true,
        showColumnLines: false,
        showRowLines: false,
        rowAlternationEnabled: true,
        hoverStateEnabled: true,
        columnAutoWidth: true,
        allowColumnResizing: true,
        headerFilter: {
            visible: false
        },
    });

    fnListarMovimientos(0, 6, gridPrestamos.dxDataGrid("instance"));

    function openPopupPrestamoEdit(IDMovimiento) {
        popupPrestamoEdit = $("#popup-prestamo-edit").dxPopup({
            showTitle: true,
            title: 'Editar Servicio',
            width: 880,
            height: "auto",
            contentTemplate: function () {
                var $pageContent = $("<span />");
                return $pageContent.load($urlReal + 'Movimiento/REditar/' + IDMovimiento);
            },
            showCloseButton: true,
        });
        popupPrestamoEdit.dxPopup("instance").show();
    }

});
