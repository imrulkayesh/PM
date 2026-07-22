function getJobCard() {
    var url = '/JsonReaderPM/JobHead';
    var id = $('#BILH_ASET').val();
    var JOBH_ID = $('#JOBH_ID');
    ClearDataFiled();
    if (id === "") {
        JOBH_ID.empty();
        JOBH_ID.append($("<option></option>").attr("value", '').text('--Select--'));
        return;
    };
    $.getJSON(url, { id: id }, function (response) {
        JOBH_ID.empty();
        JOBH_ID.append($("<option></option>").attr("value", '').text('--Select--'));
        $.each(response, function (index, item) {
            JOBH_ID.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
    loadTable();
}

function getJD() {
    debugger;
    var url = 'getJD';
    var JOBH_ID = $('#JOBH_ID').val();
    var UserPay = $('#UserPay');
    var InsurancePay = $('#InsurancePay');
    var CompanyPay = $('#CompanyPay');
    var LG = $('#LG');
    $.getJSON(url, { id: JOBH_ID }, function (response) {
        $('#UserPay').val(response.UPAY);
        $('#InsurancePay').val(response.IPAY);
        $('#CompanyPay').val(response.CPAY);
        $('#LG').val(response.TPAY);
    });
    if (JOBH_ID === "") {
        return;
    };
    var Durl = 'VData';
    $.getJSON(Durl, { id: JOBH_ID }, function (response) {
        $('#jobdetail').val(response.VDATA);
    });
}
function ClearDataFiled() {
    $('#UserPay').val('');
    $('#InsurancePay').val('');
    $('#CompanyPay').val('');
    $('#LG').val('');
}


var dataTable;
function loadTable() {
    var id = $('#BILH_ASET').val();
    debugger;
    var dataTable = $('#Old_Data').DataTable({
        "ajax": {
            "url": "BillStory/" + id,
            "type": "GET",
            "datatype": "json"
        },
        "bDestroy": true,
        "columns": [
                       { "mData": "BILH_ID" },
                       {
                           "mData": "BILH_DATE", "render": function (mData) {
                               if (mData === null) return "";
                               return moment(mData).format('DD/MM/YYYY');
                           }
                       },
                       { "mData": "BILH_NOTE" },
                       { "mData": "BILH_REF" },
                       { "mData": "JOBH_ID" },
                       { "mData": "BILH_RECEIVER" },
                       { "mData": "BILH_PAID" },
                       { "mData": "BILH_IPAY" },
                       { "mData": "BILH_UPAY" },
                       { "mData": "BILH_CPAY" },
        ],
        "language": { "emptyTable": "no data found" }, "width": "100%"
    });


};