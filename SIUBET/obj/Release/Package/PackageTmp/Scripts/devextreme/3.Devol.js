$(document).ready(function () {

    function fnLimpiarFiltros() {
        txtSnip.dxTextBox("instance").option("value", "");
    }

    $("#btnLimpiar").on("click", function () {
        fnLimpiarFiltros();
    });

    $("#btnBuscar").on("click", function () {
        fnListarMovimientos(0, 2, gridDevoluciones.dxDataGrid("instance"));
    });

    gridDevoluciones = $("#gridDevoluciones").dxDataGrid({
        columns: [

            { dataField: "Nro", caption: "#", alignment: "center", },
            { dataField: "Correlativo", caption: "NRO.MOV.", alignment: "center", width: 100, },
            //{ dataField: "TipoMov", },
            //{ dataField: "FechaEmision", caption: "F.Emisión", alignment: "center", width: 150, },
            {
                caption: "Fec. Emisión",
                //alignment: "center",
                width: 140,
                cellTemplate: function (container, options) {
                    var item = options.data;
                    var dato = '<span>' + item.FechaEmision + '</span>';
                    if (item.NombreFileEmision.length > 0) {
                        dato += '&nbsp;&nbsp;</span><a href="'+$urlReal+'Uploads/' + item.NombreFileEmision + '" target="_blank"><span class="glyphicon glyphicon-file" aria-hidden="true"></span></a>';
                    }
                    dato = $(dato);
                    dato.appendTo(container);
                    
                }
            },
            { dataField: "FechaMov", caption: "Fec. Salida", alignment: "center", width: 140, },
            { dataField: "Responsable", width: 150, },
            { dataField: "TipoMov", caption: "T.Mov.", width: 120, },
            {
                caption: "N° Cargo",
                alignment: "center",
                width: 100,
                cellTemplate: function (container, options) {
                    var item = options.data;
                    var dato = '<span>' + item.NumeroCargo + '</span>';
                    if (item.NombreFileFinal.length == 0) {
                        dato += '&nbsp;&nbsp;</span><a href="' + $urlReal + 'Uploads/' + item.NombreFileCargo + '" target="_blank"><span class="glyphicon glyphicon-file" aria-hidden="true"></span></a>';
                    }
                    dato = $(dato);                    
                    dato.appendTo(container);
                }
            },
            { dataField: "UndEjec_CAC", caption: "Unidad Ejecutora / CAC", width: 200, },
            //{ dataField: "FechaRecepcion", caption: "Fec. Recepción", alignment: "center", width: 100, },
            {
                caption: "Fec. Recepción",
                //alignment: "center",
                width: 140,
                cellTemplate: function (container, options) {
                    var item = options.data;
                    var dato = '<span>' + item.FechaFinal + '</span>';
                    if (item.NombreFileFinal.length > 0) {
                        dato += '&nbsp;&nbsp;</span><a href="'+$urlReal+'Uploads/' + item.NombreFileFinal + '" target="_blank"><span class="glyphicon glyphicon-file" aria-hidden="true"></span></a>';
                    }
                    dato = $(dato);
                    dato.appendTo(container);
                }
            },
            { dataField: "Estado", width: 100, },
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
                    var disabledR = true;
                    var disabledA = true;
                    if (item.Activo == true) {
                        if (item.FechaFinal.length <= 0) {
                            disabledR = false;
                            disabledA = false;
                        }
                    }
                    $('<div />').appendTo(container)
                    .dxToolbar({
                        items: [
                            {
                                location: 'after',
                                widget: 'dxButton',
                                locateInMenu: 'auto',
                                disabled: disabledR,
                                options: {
                                    icon: "glyphicon glyphicon-ok-circle",
                                    text: "Recepcionar",
                                    onClick: function () {
                                        openPopupRetornaRecepciona(item);
                                    }
                                }

                            },
                            {
                                location: 'after',
                                widget: 'dxButton',
                                locateInMenu: 'auto',
                                disabled: disabledA,
                                options: {
                                    icon: "glyphicon glyphicon-remove-circle",
                                    text: "Anular",
                                    onClick: function () {
                                        openPopupAnular(item);
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
    fnListarMovimientos(0, 2, gridDevoluciones.dxDataGrid("instance"));

});
