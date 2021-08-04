function Report() {
    
    this.init = function () {
        this.initDataTable();
    };

    this.initDataTable = function () {
        reportDataTable = $('#tblReport')
            .DataTable({
                bServerSide: true,
                bProcessing: true,
                sAjaxSource: "/Reports/GetData",
                bSearchable: true,
                responsive: false,
                autoWidth: false,
                processing: true,
                serverSide: true,
                data: "aaData",
                deferRender: true,
                order: [[1, 'desc']],
                language: {
                    emptyTable: "No record found.",
                    processing: //'<i class="fa fa-spinner fa-spin fa-3x fa-fw" style="color:#2a2b2b;></i><span class="sr-only">Loading..n.</span> ',
                        '<i class="fa fa-spinner fa-spin fa-3x fa-fw" style="color:#2a2b2b;"></i><span class="sr-only">Loading...</span> '
                },
                columns: [
                    {
                        data: "machId",
                        className: "text-center",
                    },
                    {
                        data: "date",
                        className: "text-center",
                    },
                    {
                        data: "arrivalTime",
                        className: "text-center", 
                    },
                    {
                        data: "problemCode",
                        className: "text-center",
                        defaultContent: "-",
                    },
                    {
                        data: "description",
                    },
                    {
                        data: "flmSlm",
                        className: "text-center",
                    }
                ],
                columnDefs: [
                    {
                        targets: "_all",
                        defaultContent: "-",
                        searchable: true
                    },
                    {
                        targets: 1,
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    {
                        targets: 2,
                        render: function (data) {
                            var newTime = moment(data,"HH:mm").format("HH:mm");
                            if (data === null) {
                                return "-";
                            }
                            return newTime;
                        }
                    },
                ],
                
                createdRow: (row, data, dataIndex) => {
                    if (data["machId"] === "5871" || data["machId"] === "5803" || data["machId"] === "5812" || data["machId"] === "588A" || data["machId"] === "5830"
                        || data["machId"] === "58CC" || data["machId"] === "58C4" || data["machId"] === "58C5" || data["machId"] === "5845" || data["machId"] === "585E"
                        || data["machId"] === "585F" || data["machId"] === "588B" || data["machId"] === "588C" || data["machId"] === "587F" || data["machId"] === "58B1"
                        || data["machId"] === "58B0" || data["machId"] === "5829" || data["machId"] === "5837"|| data["machId"] === "58DB" || data["machId"] === "5869"
                        || data["machId"] === "5811" || data["machId"] === "5826" || data["machId"] === "58CF" || data["machId"] === "5836" || data["machId"] === "587C"
                        || data["machId"] === "5839" || data["machId"] === "5848" || data["machId"] === "587E" || data["machId"] === "586C" || data["machId"] === "5846"
                        || data["machId"] === "5816" || data["machId"] === "5817" || data["machId"] === "586B" || data["machId"] === "58B2" || data["machId"] === "58D3"
                        || data["machId"] === "58D4" || data["machId"] === "5844" || data["machId"] === "586A" || data["machId"] === "4C33" || data["machId"] === "58C8" 
                        || data["machId"] === "58A5" || data["machId"] === "5804" || data["machId"] === "58B3" || data["machId"] === "5814" || data["machId"] === "5815" 
                        || data["machId"] === "5806" || data["machId"] === "5899" || data["machId"] === "5890" || data["machId"] === "58DF" || data["machId"] === "58D6"
                        || data["machId"] === "58E5" || data["machId"] === "5818" || data["machId"] === "5822" || data["machId"] === "585C" || data["machId"] === "585D"
                        || data["machId"] === "58D8" || data["machId"] === "5841" || data["machId"] === "5843" || data["machId"] === "5820" || data["machId"] === "5824"
                        || data["machId"] === "5832" || data["machId"] === "5849") {
                            $(row).addClass('vip-color');
                    }
                    else
                    {
                        if (data["flmSlm"] === "FLM" || data["flmSlm"] === "Branch") {
                            $(row).addClass('custom-color');
                        }
                        if (data["flmSlm"] === "SLM")
                        {
                            $(row).addClass('no-color');
                        }
                    }
                },
                
                search: {
                    caseInsensitive: false
                },
            });
    };
}