$(document).ready(function () {

    function fnLimpiarFiltros() {
        txtSnip.dxTextBox("instance").option("value", "");
    }

    $("#btnLimpiar").on("click", function () {
        fnLimpiarFiltros();
    });

    $("#btnBuscar").on("click", function () {
        fnListarPrestamos(0);
    });

    gridPrestamos = $("#gridPrestamos").dxDataGrid({
        columns: [

            { dataField: "Nro", caption: "#", alignment: "center", },
            { dataField: "Correlativo", caption: "NRO.MOV.", alignment: "center", width: 100, },
            { dataField: "FechaMov", caption: "Fec. Salida", alignment: "center", width: 140, },
            { dataField: "Plazo", caption: "Días Trans.", alignment: "center", width: 80, },
            { dataField: "FechaRetornoEstimada", caption: "Fec. Retorno Estimada", alignment: "center", width: 135, },
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
            { dataField: "EntidadDestino", caption: "Ingeniero/Otros", width: 200, },
            //{ dataField: "FechaRecepcion", caption: "Fec. Recepción", alignment: "center", width: 100, },
            {
                caption: "Fec. Retorno Real",
                //alignment: "center",
                width: 140,
                cellTemplate: function (container, options) {
                    var item = options.data;
                    var dato = '<span>' + item.FechaFinal + '</span>';
                    /*if (item.NombreFileFinal.length > 0) {
                        dato += '&nbsp;&nbsp;</span><a href="'+$urlReal+'Uploads/P/' + item.NombreFileFinal + '" target="_blank"><span class="glyphicon glyphicon-file" aria-hidden="true"></span></a>';
                    }*/
                    dato = $(dato);
                    dato.appendTo(container);

                }
            },
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
                    var disabledR = true;
                    var disabledF = true;
                    if (item.Activo == true) {
                        if (item.FechaFinal.length <= 0 && item.NombreFileCargo.length > 0) disabledR = false;
                        if (item.FechaFinal.length <= 0 && item.NombreFileCargo.length == 0) disabledF = false;                        
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
                                    text: "Retornar",
                                    onClick: function () {
                                        openPopupRetornaRecepciona(item);
                                    }
                                }

                            },
							{
							    location: 'after',
							    widget: 'dxButton',
							    locateInMenu: 'auto',
							    disabled: disabledF,
							    options: {
							        icon: "glyphicon glyphicon-file",
							        text: "Anexar cargo",
							        onClick: function () {
							            openPopupRetornaRecepciona(item);
							        }
							    }
							},

                            //{
                            //    location: 'after',
                            //    widget: 'dxButton',
                            //    locateInMenu: 'auto',
                            //    disabled: disabledR,
                            //    options: {
                            //        icon: "glyphicon glyphicon-remove-circle",
                            //        text: "Anular",
                            //        onClick: function () {
                            //            openPopupAnular(item);
                            //        }
                            //    }
                            //},
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
        "export": {
            enabled: true,
            fileName: "Listado Movimientos",
            allowExportSelectedData: true
        },
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

    fnListarMovimientos(0, 4, gridPrestamos.dxDataGrid("instance"));

});
