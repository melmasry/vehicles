
var ControllerName = null;
var AlternationColName = null;

function InitializeReport(controllerName, alternationColName) {
    ControllerName = controllerName;
    AlternationColName = alternationColName;

    $('#search').click(function () {
        $('#searchSpinner').addClass('fa fa-circle-o-notch fa-spin');
        Search();
    });
}

function Search() {
    var url = '/' + ControllerName + '/Search';
    var customerName = $('#CustomerName').val();
    var selectdStatus = $("#drp_status option:selected")[0].value;
    
    $('#searchResults').
        load(url,
        { CustomerName: customerName, Status: selectdStatus },
        function (data) {
            LoadSearchResult();
            $('#searchSpinner').removeClass('fa fa-circle-o-notch fa-spin');
            $("#search").prop('disabled', false);
        });
}

function LoadSearchResult() {
    $.ajax({
        url: '/' + ControllerName + '/GetSearchResult',
        dataType: 'json',
        success: function (data) {
            if (data.resources.length > 0)
                InitDatatable(data);
        }
    });
}

function InitDatatable(data) {
    var prevRow = null;
    var prevData = null;
    var prevClassName = null;
    var oddClass = "odd";
    var evenClass = "even";
    // column name by it will unifiy the coloring of the similar rows (aggregate column)
    var colName = AlternationColName;

    var table = $('#reportGrid').DataTable({
        "aaData": data.resources,
        "columns": data.columns,
        //Show jquery action buttons
        dom: 'Bfrtip',
        buttons: ['excel', 'pdf', {
            extend: 'print', autoPrint: false
        }],
        "ordering": false,
        scrollY: "600px",
        paging: true,
        scrollX: true,
        scrollCollapse: true,
        searching: true,
        bLengthChange: false,
        fixedColumns: {
            leftColumns: 3
        },
        "rowsGroup": [0,1,2],
        "rowCallback": function (row, data, index) {
            var currentData = data[colName];
            if (index === 0) {
                $(row).removeClass();
                $(row).addClass(oddClass);
                prevData = data[colName];
                prevClassName = row.className;
            }
            else {
                if (prevData === currentData) {
                    $(row).removeClass();
                    $(row).addClass(prevClassName);
                }
                else {
                    $(row).removeClass();
                    if (prevClassName === oddClass)
                        $(row).addClass(evenClass);
                    else
                        $(row).addClass(oddClass);
                }
                prevData = data[colName];
                prevClassName = row.className;
            }
        }
    });
}
