function FromDept() {
    var url = '/JsonReaderPM/getDept';
    var F_ASET_DEPARTMENT = $('#F_ASET_DEPARTMENT');
    var id = $('#F_ASET_BUSINESS').val();
    if (id === "") {
        F_ASET_DEPARTMENT.empty();
        F_ASET_DEPARTMENT.append($("<option></option>").attr("value", '').text('--Select from Dept--'));
        return;
    };
    $.getJSON(url, { bus_id: id }, function (response) {
        F_ASET_DEPARTMENT.empty();
        F_ASET_DEPARTMENT.append($("<option></option>").attr("value", '').text('--Select from Dept--'));
        $.each(response, function (index, item) {
            F_ASET_DEPARTMENT.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}
function FromAsset() {
    var url = '/JsonReaderPM/getFreeAsset';
    var F_ASET_ID = $('#F_ASET_ID');
    var id = $('#F_ASET_GROUP').val();
    var did = $('#F_ASET_DEPARTMENT').val();
    if (id === "" || did === "") {
        F_ASET_ID.empty();
        F_ASET_ID.append($("<option></option>").attr("value", '').text('--Select Asset--'));
        return;
    };
    $.getJSON(url, { dept: did, group: id }, function (response) {
        F_ASET_ID.empty();
        F_ASET_ID.append($("<option></option>").attr("value", '').text('--Select Asset--'));
        $.each(response, function (index, item) {
            F_ASET_ID.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}


function FromUDept() {
    var url = '/JsonReaderPM/getDept';
    var U_ASET_DEPARTMENT = $('#U_ASET_DEPARTMENT');
    var id = $('#U_ASET_BUSINESS').val();
    if (id === "") {
        U_ASET_DEPARTMENT.empty();
        U_ASET_DEPARTMENT.append($("<option></option>").attr("value", '').text('--Select from Dept--'));
        return;
    };
    $.getJSON(url, { bus_id: id }, function (response) {
        U_ASET_DEPARTMENT.empty();
        U_ASET_DEPARTMENT.append($("<option></option>").attr("value", '').text('--Select from Dept--'));
        $.each(response, function (index, item) {
            U_ASET_DEPARTMENT.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}

function FromUser() {
    var url = '/JsonReaderPM/getUserList';
    var U_ASET_USER = $('#U_ASET_USER');
    var id = $('#U_ASET_AREA').val();
    var did = $('#U_ASET_DEPARTMENT').val();
    if (id === "" || did === "") {
        U_ASET_USER.empty();
        U_ASET_USER.append($("<option></option>").attr("value", '').text('--Select Asset--'));
        return;
    };
    $.getJSON(url, { dept: did, area: id }, function (response) {
        U_ASET_USER.empty();
        U_ASET_USER.append($("<option></option>").attr("value", '').text('--Select Asset--'));
        $.each(response, function (index, item) {
            U_ASET_USER.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}

function ToDept() {
    var url = '/JsonReaderPM/getDept';
    var T_ASET_DEPARTMENT = $('#T_ASET_DEPARTMENT');
    var id = $('#T_ASET_BUSINESS').val();
    if (id === "") {
        T_ASET_DEPARTMENT.empty();
        T_ASET_DEPARTMENT.append($("<option></option>").attr("value", '').text('--Select Dept--'));
        return;
    };
    $.getJSON(url, { bus_id: id }, function (response) {
        T_ASET_DEPARTMENT.empty();
        T_ASET_DEPARTMENT.append($("<option></option>").attr("value", '').text('--Select Dept--'));
        $.each(response, function (index, item) {
            T_ASET_DEPARTMENT.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}

function FindAcceByAset() {
    var id = $('#F_ASET_ID').val();
    var route = '/AssetUserManager/ShowAccessoreis/?id=' + id;
    $('#LoadPartialView').load(route);
}

function ClearAsset() {
    var F_ASET_ID = $('#F_ASET_ID');
    F_ASET_ID.empty();
    F_ASET_ID.append($("<option></option>").attr("value", '').text('--Select Asset--'));
}
function FindUserAsset() {
    var url = 'getUserCurrentData';
    var UserId = $('#U_ASET_USER').val();
    if (UserId == '') {
        return;
    }
    $.getJSON(url, { id: UserId }, function (response) {
        $('#UserInfoDATA').val(response.InfoData);
    });
}

$("#vasset").on("click", function (e) {
    e.preventDefault();
    var url = 'getAssetOrUserInfo';
    var AssetUser = $('#assetstaff').val();
    var whoId = this.id;
    if (AssetUser == '') {
        return;
    }
    $.getJSON(url, { id: AssetUser, selct: whoId }, function (response) {
        swal('Asset info..', response.InfoData, 'info');
       // $('#assetstaffdata').val(response.InfoData);
    });
});

$("#vstaff").on("click", function (e) {
    e.preventDefault();
    var url = 'getAssetOrUserInfo';
    var AssetUser = $('#assetstaff').val();
    var whoId = this.id;
    if (AssetUser == '') {
        return;
    }
    $.getJSON(url, { id: AssetUser, selct: whoId }, function (response) {
        swal('User info..', response.InfoData, 'info');
        //    $('#assetstaffdata').val(response.InfoData);
    });
});