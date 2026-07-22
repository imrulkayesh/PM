function FindDepts() {
    var url = '/JsonReaderPM/getDept';
    var ASET_DEPARTMENT = $('#ASET_DEPARTMENT');
    var id = $('#ASET_BUSINESS').val();
    if (id === "") {
        ASET_DEPARTMENT.empty();
        ASET_DEPARTMENT.append($("<option></option>").attr("value", '').text('--Select Dept--'));
        return;
    };
    $.getJSON(url, { bus_id: id }, function (response) {
        ASET_DEPARTMENT.empty();
        ASET_DEPARTMENT.append($("<option></option>").attr("value", '').text('--Select Dept--'));
        $.each(response, function (index, item) {
            ASET_DEPARTMENT.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}


function FindVehicleByDept() {
    var url = '/JsonReaderPM/VehilceListByDept';
    var ASET_ID = $('#ASET_ID');
    var id = $('#ASET_DEPARTMENT').val();
    if (id === "") {
        ASET_ID.empty();
        ASET_ID.append($("<option></option>").attr("value", '').text('--Select Asset--'));
        return;
    };
    $.getJSON(url, { DeptId: id }, function (response) {
        ASET_ID.empty();
        ASET_ID.append($("<option></option>").attr("value", '').text('--Select Asset--'));
        $.each(response, function (index, item) {
            ASET_ID.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}



function FindAcceByAcce() {
    var url = '/JsonReaderPM/AccesoriesByAsset';
    var ACCE_ID = $('#ACCE_ID');
    var id = $('#ASET_ID').val();
    if (id === "") {
        ACCE_ID.empty();
        ACCE_ID.append($("<option></option>").attr("value", '').text('--Select Acce--'));
        return;
    };

    //$('#Asset').val($('#ASET_ID :selected').text());

    var id = $('#ASET_ID').val();
    $.getJSON(url, { AsetId: id }, function (response) {
        ACCE_ID.empty();
        ACCE_ID.append($("<option></option>").attr("value", '').text('--Select Acce--'));
        $.each(response, function (index, item) {
            ACCE_ID.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });

    //get Acce list
    var route = '/AssetManager/ShowAccessoreis/?AssetId=' + id;
    $('#ShowPartial').load(route);   

}

function FindAcceByAsset() {
    var url = '/JsonReaderPM/AccAssociateByAsset';
    var ACCE_ID = $('#ACCE_ID');
    var id = $('#ASET_ID').val();
    if (id === "") {
        ACCE_ID.empty();
        ACCE_ID.append($("<option></option>").attr("value", '').text('--Select Acce--'));
        return;
    };

    var id = $('#ASET_ID').val();
    $.getJSON(url, { AsetId: id }, function (response) {
        ACCE_ID.empty();
        ACCE_ID.append($("<option></option>").attr("value", '').text('--Select Acce--'));
        $.each(response, function (index, item) {
            ACCE_ID.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });

    //get Acce list
    var route = '/AssetManager/ShowAccessoreis/?AssetId=' + id;
    $('#ShowPartial').load(route);

}

$(document).on("click", ".UpdateR", function () {
    $('#ACCE_IDA').val($(this).data('acce'));
    $('#ASET_IDA').val($(this).data('id'));
    $('#ACCE_IDD').val($(this).data('acce'));
    $('#ASET_IDD').val($(this).data('id'));
});