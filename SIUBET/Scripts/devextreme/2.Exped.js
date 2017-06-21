$(document).ready(function () {

    var txtNumeroHT = $("#txtNumeroHT").dxTextBox({
		placeholder: "Escribir código...",
		showClearButton: true,
	});

    var estados = [
        { "ID": 0, "Name": "-- TODOS --" },
        { "ID": 1, "Name": "TRANSFERIDOS" },
        { "ID": 2, "Name": "DEVUELTOS" },
        //{ "ID": 3, "Name": "EN CUSTODIA" },
        { "ID": 4, "Name": "EN PRESTAMO" },
        { "ID": 5, "Name": "DERIVADOS" },      
	];

    var etapas = [
        { "ID": 0, "Name": "-- TODOS --" },
        { "ID": 1, "Name": "ADMISIBILIDAD" },
        { "ID": 2, "Name": "LEGIBILIDAD" },
        { "ID": 3, "Name": "CALIDAD" },
    ];
	var cboEstado = $("#cboEstado").dxSelectBox({
	    dataSource: new DevExpress.data.ArrayStore({
	        data: estados,
	        key: "ID"
	    }),
	    displayExpr: "Name",
	    valueExpr: "ID",
	    value: estados[0].ID,
	});
	var cboEtapa = $("#cboEtapa").dxSelectBox({
	    dataSource: new DevExpress.data.ArrayStore({
	        data: etapas,
	        key: "ID"
	    }),
	    displayExpr: "Name",
	    valueExpr: "ID",
	    value: etapas[0].ID,
	});

	function fnLimpiarFiltros() {
		txtSnip.dxTextBox("instance").option("value", "");
		txtNumeroHT.dxTextBox("instance").option("value", "");
		//txtProyecto.dxTextBox("instance").option("value", "");
	}

	$("#btnLimpiar").on("click", function () {
		fnLimpiarFiltros();
	});

	$("#btnBuscar").on("click", function () {
		fnListarExpedientes(0);
	});

	_selectedItems = "";

    function fnOpenPopupEtapa(){
        if (_selectedItems == "") {
            showNotification("Debe elegir uno o mas registros.", notificationTypes.warning, 2000);
            return;
        }

        popupEtapa = $("#popup-etapa").dxPopup({
            showTitle: true,
            title: "Actualizar Etapa de los Documentos",
            width: 400,
            height: "auto",
            contentTemplate: function () {
                var $pageContent = $("<span />");
                return $pageContent.load($urlReal + 'Expedientes/Etapa')
            },
            showCloseButton: true,
        });
        popupEtapa.dxPopup("instance").show();
    }

	function fnOpenPopupDD(IDTipoMov) {
	    //console.log(IDTipoMov);
        //[2]Devolución, [5]Derivación
	    if (_selectedItems == "") {
	        showNotification("Debe elegir uno o mas registros.", notificationTypes.warning, 2000);
	        return;
	    }

	    popupDD = $("#popup-DD").dxPopup({
	        showTitle: true,
	        title: IDTipoMov == 2 ? 'Crear Devolución' : 'Crear Derivación',
	        width: 880,
	        height: "auto",
	        contentTemplate: function () {
	            var $pageContent = $("<span />");
	            return $pageContent.load($urlReal + 'Movimiento/DDCrear/' + IDTipoMov);
	        },
	        showCloseButton: true,
	    });
	    popupDD.dxPopup("instance").show();	    
	}

	function fnOpenPopupPrestamo() {
	    //console.log(_selectedItems);
	    if (_selectedItems == "") {
	        showNotification("Debe elegir uno o mas registros.", notificationTypes.warning, 2000);
	        return;
	    }

	    popupPrestamo = $("#popup-prestamo").dxPopup({
	        showTitle: true,
	        title: 'Crear Préstamo',
	        width: 880,
	        height: "auto",
	        contentTemplate: function () {
	            var $pageContent = $("<span />");
	            return $pageContent.load($urlReal + 'Movimiento/PreCrear/'+_selectedItems);
	        },
	        showCloseButton: true,
	    });
	    popupPrestamo.dxPopup("instance").show();
	}

	function fnOpenPopupTransferencia() {
	    //console.log(_selectedItems);
	    if (_selectedItems == "") {
	        showNotification("Debe elegir uno o mas registros.", notificationTypes.warning, 2000);
	        return;
	    }

	    popupTransferencia = $("#popup-transferencia").dxPopup({
	        showTitle: true,
	        title: 'Crear Transferencia',
	        width: 880,
	        height: "auto",
	        contentTemplate: function () {
	            var $pageContent = $("<span />");
	            return $pageContent.load($urlReal + 'Movimiento/TCrear/' + _selectedItems);
	        },
	        showCloseButton: true,
	    });
	    popupTransferencia.dxPopup("instance").show();
	}

	gridExpedientes = $("#gridExpedientes").dxDataGrid({
		columns: [

			{ dataField: "Nro", caption: "#" },
			//{ dataField: "IDVersion", caption: "ID", },
			//{ dataField: "IDExpTecnico", caption: "IDExpTec", alignment: "center", },
			{ dataField: "Snip", alignment: "center", },
			{ dataField: "NombreProyecto", caption: "Proyecto", width: 300, },
            { dataField: "NumeroHT", caption: "Documento HT", width: 150, },
			{ dataField: "NVersion", caption: "Documento Ingreso", width: 150, },
            { dataField: "Etapa", caption: "Etapa", alignment: "center", width: 100, },
            { dataField: "Estado", caption: "Estado Actual", alignment: "center", width: 250, },
            { dataField: "UbiTopografica", caption: "U.Topográfica (E:C:B) | (PQ:PO)", width: 200, },
			//{ dataField: "Activo", dataType: "boolean", alignment: "center", },
			{
			    caption: "...",
			    fixed: true,
			    allowHiding: false,
			    alignment: "center",
			    width: 35,
			    cellTemplate: function (container, options) {
			        var item = options.data;
			        $('<div />').appendTo(container)
					.dxToolbar({
					    items: [
							{
							    location: 'after',
							    widget: 'dxButton',
							    locateInMenu: 'auto',
							    //disabled: item.Vincular == false,
							    options: {
							        icon: "glyphicon glyphicon-edit",
							        text: "Actualizar Etapa",
							        onClick: function () {
							            fnOpenPopupEtapa();
							        }
							    }
							},
							{
							    location: 'after',
							    widget: 'dxButton',
							    locateInMenu: 'auto',
							    options: {
							        icon: "glyphicon glyphicon-retweet",
							        text: "Préstamo",
							        onClick: function () {
							            fnOpenPopupPrestamo();
							        }
							    }

							},
							{
							    location: 'after',
							    widget: 'dxButton',
							    locateInMenu: 'auto',
							    options: {
							        icon: "glyphicon glyphicon-repeat",
							        text: "Devolución",
							        onClick: function () {
							            fnOpenPopupDD(2);
							        }
							    }
							},
							{
							    location: 'after',
							    widget: 'dxButton',
							    locateInMenu: 'auto',
							    options: {
							        icon: "glyphicon glyphicon-repeat",
							        text: "Derivación",
							        onClick: function () {
							            fnOpenPopupDD(5);
							        }
							    }
							},
							{
							    location: 'after',
							    widget: 'dxButton',
							    locateInMenu: 'auto',
							    options: {
							        icon: "glyphicon glyphicon-share",
							        text: "Transferir",
							        onClick: function () {
							            fnOpenPopupTransferencia();
							        }
							    }
							},
							{
							    location: 'after',
							    widget: 'dxButton',
							    locateInMenu: 'auto',
							    //disabled: item.Vincular == false,
							    options: {
							        icon: "glyphicon glyphicon-th-list",
							        text: "Ver historial",
							        onClick: function () {
							            fnOpenPopupHistorial(item.IDVersion);
							        }
							    }
							},                            
					    ]
					});
			    }
			},
		],
        onRowPrepared: function (e) {
            if (e.rowType == "data") {
                //console.log(e.data)
                if (e.data.IDTipoMov == 1) {//Transferencias
                    //Amarillo
                    e.rowElement.css('border-left', '5px solid rgba(221, 166, 2,0.9)');
                    e.rowElement.css('background', 'rgba(243, 207, 85, 0.5)');
                    e.rowElement.css('color', 'black');
                }
                else if (e.data.IDTipoMov == 2) {//Devoluciones
                    //Beish
                    e.rowElement.css('border-left', '5px solid rgba(222, 140, 85, 0.9)');
                    e.rowElement.css('background', 'rgba(207, 176, 149, 0.5)');
                    e.rowElement.css('color', 'black');
                }
                else if (e.data.IDTipoMov == 4) {//Préstamos
                    //Celeste
                    e.rowElement.css('border-left', '5px solid rgba(22, 126, 230, 0.9)');
                    e.rowElement.css('background', 'rgba(149, 222, 227, 0.5)');
                    e.rowElement.css('color', 'black');
                }
                else if (e.data.IDTipoMov == 5) {//Derivaciones
                    //Verde
                    e.rowElement.css('border-left', '5px solid rgba(5, 164, 98, 0.9)');
                    e.rowElement.css('background', 'rgba(136, 176, 75, 0.5)');
                    e.rowElement.css('color', 'black');
                }
            }
        },
		onSelectionChanged: function (selectedItems) {
		    var data = selectedItems.selectedRowsData;
		    if (data.length > 0) {
		        _selectedItems = $.map(data, function (value) {
		            return value.IDVersion;
		        }).join("|");                
		    }
		    else {
		        _selectedItems = "";
		    }
		    console.log("vars:" + _selectedItems);
		    $("#et_selected").val(_selectedItems);
		},
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
			fileName: "Listado Expedientes",
			allowExportSelectedData: true
		},
		loadPanel: {
			text: $textLoad
		},
		noDataText: $noDataText,
		selection: {
		    mode: "multiple"
		},
		wordWrapEnabled: true,
		showColumnLines: false,
		showRowLines: false,
		//rowAlternationEnabled: true,
		hoverStateEnabled: true,
		columnAutoWidth: true,
		allowColumnResizing: true,
	});

	function fnListarExpedientes(pageIndex) {
		var listadoExpedientes = new DevExpress.data.CustomStore({
			load: function (loadOptions) {
				var deferred = $.Deferred(),
					args = {};
				args.snip = txtSnip.dxTextBox("instance").option("value");
				if (args.snip == "") args.snip = 0;
				args.numeroHT = txtNumeroHT.dxTextBox("instance").option("value");
				args.estado = cboEstado.dxSelectBox("instance").option("value");
				args.etapa = cboEtapa.dxSelectBox("instance").option("value");
				args.pageNumber = pageIndex == 1 ? pageIndex : gridExpedientes.dxDataGrid("instance")._options.paging.pageIndex + 1;
				args.pageSize = loadOptions.take || 10;

				$.ajax({
				    url: $urlReal + "Expedientes/ListadoExpedientes",
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
						deferred.resolve(result.items, { totalCount: result.totalRowsFilter });
					},
					error: function () {
						deferred.reject($infoTextError);
					},
					timeout: 500000
				});
				return deferred.promise();
			},

		});

		gridExpedientes.dxDataGrid("instance").option({
			dataSource: {
				store: listadoExpedientes
			},
		});
	}	

	function fnOpenPopupHistorial(IDExpedienteVersion) {

	    popupHistorial = $("#popup-historial").dxPopup({
	        showTitle: true,
	        title: 'Historial del Expediente',
	        width: 880,
	        height: "auto",
	        showCloseButton: true,
	        //onShown: function () {
	        //    $("#NoteScroll").dxScrollView();
	        //}
	    });
	    popupHistorial.dxPopup("instance").show();

	    gridHistorial = $("#gridHistorial").dxDataGrid({
	        columns: [
                { dataField: "Nro", alignment: "center", caption: "#" },
                { dataField: "FechaMov", caption: "Fecha", width: 100, },
                { dataField: "TipoMov", caption: "T.Movimiento", alignment: "center", },
                { dataField: "Responsable", width: 200, },
                { dataField: "NumeroCargo", caption: "N° Cargo", width: 150, },
                { dataField: "EntidadDestino", caption: "U.Ejecutora/Ingernieros", width: 250, },
                { dataField: "Observaciones", width: 250, },
                { dataField: "Estado", width: 100, },
                { dataField: "FechaFinal", caption: "F.Final", width: 140, },
                { dataField: "Numero", width: 100, },
                { dataField: "Folios", },
                { dataField: "CDs", },
                { dataField: "Planos", },
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
	    });
	    
	    var listadoHistorial = new DevExpress.data.CustomStore({
	        load: function (loadOptions) {
	            var deferred = $.Deferred(),
                    args = {};

	            args.IDExpedienteVersion = IDExpedienteVersion;
	            args.pageNumber = gridHistorial.dxDataGrid("instance")._options.paging.pageIndex + 1;
	            args.pageSize = loadOptions.take || 10;
	            console.log(args);
	            $.ajax({
	                url: $urlReal + "Expedientes/ListadoExpedientesHistorial",
	                data: JSON.stringify(args),
	                type: 'POST',
	                dataType: "json",
	                contentType: "application/json; charset=utf-8",
	                success: function (result) {
	                    console.log(result)
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

	    gridHistorial.dxDataGrid("instance").option({
	        dataSource: {
	            store: listadoHistorial
	        },
	    });
	    
	}

	fnListarExpedientes(0);

});
