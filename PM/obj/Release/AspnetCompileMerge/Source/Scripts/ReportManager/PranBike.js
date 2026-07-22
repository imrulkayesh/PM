function FromDept() {
    var url = '/JsonReaderPM/getDept';
    var ASET_DEPARTMENT = $('#ASET_DEPARTMENT');
    var ASET_ID = $('#ASET_ID');
    var id = $('#ASET_BUSINESS').val();
    if (id === "") {
        ASET_DEPARTMENT.empty();
        ASET_DEPARTMENT.append($("<option></option>").attr("value", '').text('--Select All--'));
        ASET_ID.empty();
        ASET_ID.append($("<option></option>").attr("value", '').text('--Select All--'));
        return;
    };
    $.getJSON(url, { bus_id: id }, function (response) {
        ASET_DEPARTMENT.empty();
        ASET_DEPARTMENT.append($("<option></option>").attr("value", '').text('--Select All--'));
        ASET_ID.empty();
        ASET_ID.append($("<option></option>").attr("value", '').text('--Select All--'));
        $.each(response, function (index, item) {
            ASET_DEPARTMENT.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}


function ClearAsset() {
    var ASET_ID = $('#ASET_ID');
    ASET_ID.empty();
    ASET_ID.append($("<option></option>").attr("value", '').text('--Select All--'));
}

function FromAsset() {
    var url = '/JsonReaderPM/getActiveAsset';
    var ASET_ID = $('#ASET_ID');
    var id = $('#ASET_GROUP').val();
    var did = $('#ASET_DEPARTMENT').val();
    if (id === "" || did === "") {
        ASET_ID.empty();
        ASET_ID.append($("<option></option>").attr("value", '').text('--Select All--'));
        return;
    };
    $.getJSON(url, { dept: did, group: id }, function (response) {
        ASET_ID.empty();
        ASET_ID.append($("<option></option>").attr("value", '').text('--Select All--'));
        $.each(response, function (index, item) {
            ASET_ID.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}