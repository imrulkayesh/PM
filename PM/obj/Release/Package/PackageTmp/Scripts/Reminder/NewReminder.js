function getBase() {
    var Ast = $('#ASET_ID').val();
    var Itm = $('#ITEM_ID').val();
    if (Itm == '' || Ast == '') {
        return;
    }
    $.post('/JsonReaderPM/IsNotifyActive', { ast: Ast, itm: Itm },
        function (data) {
            if (data === 0) {
                $('#msg').text('');
                ItemNotify(Itm) //call another function
            } else {
                $('#mbase').hide();
                $('#dbase').hide();
                swal('Opps..', 'This notification already exist', 'warning');
            }
        });
};

function ItemNotify( Itm) {
    $.post("/JsonReaderPM/ItemNotifyBase",
           { itemId: Itm },
                   function (data) {
                       if (data === 'D') {
                           $('#dbase').show();
                           $('#C_DATE').prop('required', true);
                           $('#N_DATE').prop('required', true);
                           $('#mbase').hide();
                           $('#C_STATE').prop('required', false);
                           $('#N_STATE').prop('required', false);
                       }
                       else {
                           $('#mbase').show();
                           $('#C_STATE').prop('required', true);
                           $('#N_STATE').prop('required', true);
                           $('#dbase').hide();
                           $('#C_DATE').prop('required', false);
                           $('#N_DATE').prop('required', false);
                       }
                   });
}




function FromDept() {
    var url = '/JsonReaderPM/getDept';
    var ASET_DEPARTMENT = $('#ASET_DEPARTMENT');
    var ASET_ID = $('#ASET_ID');
    var ITEM_ID = $('#ITEM_ID');
    var id = $('#ASET_BUSINESS').val();
    if (id === "") {
        ASET_DEPARTMENT.empty();
        ASET_DEPARTMENT.append($("<option></option>").attr("value", '').text('--Select--'));
        ASET_ID.empty();
        ASET_ID.append($("<option></option>").attr("value", '').text('--Select--'));
        ITEM_ID.empty();
        ITEM_ID.append($("<option></option>").attr("value", '').text('--Select--'));
        return;
    };
    $.getJSON(url, { bus_id: id }, function (response) {
        ASET_DEPARTMENT.empty();
        ASET_DEPARTMENT.append($("<option></option>").attr("value", '').text('--Select--'));
        ASET_ID.empty();
        ASET_ID.append($("<option></option>").attr("value", '').text('--Select--'));
        ITEM_ID.empty();
        ITEM_ID.append($("<option></option>").attr("value", '').text('--Select--'));
        $.each(response, function (index, item) {
            ASET_DEPARTMENT.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}

function FromAsset() {
    var url = '/JsonReaderPM/getActiveAsset';
    var ASET_ID = $('#ASET_ID');
    var ITEM_ID = $('#ITEM_ID');
    var id = $('#ASET_GROUP').val();
    var did = $('#ASET_DEPARTMENT').val();
    if (id === "" || did === "") {
        ASET_ID.empty();
        ASET_ID.append($("<option></option>").attr("value", '').text('--Select--'));
        ITEM_ID.empty();
        ITEM_ID.append($("<option></option>").attr("value", '').text('--Select--'));
        return;
    };
    $.getJSON(url, { dept: did, group: id }, function (response) {
        ASET_ID.empty();
        ASET_ID.append($("<option></option>").attr("value", '').text('--Select--'));
        ITEM_ID.empty();
        ITEM_ID.append($("<option></option>").attr("value", '').text('--Select--'));
        $.each(response, function (index, item) {
            ASET_ID.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}


function getReminderItem() {
    var url = '/JsonReaderPM/ReminderItem';
    var ITEM_ID = $('#ITEM_ID');
    var id = $('#ASET_ID').val();
    var gid = $('#ASET_GROUP').val();
    if (id === "" && gid==="") {
        ITEM_ID.empty();
        ITEM_ID.append($("<option></option>").attr("value", '').text('--Select--'));
        return;
    };
    $.getJSON(url, {gid: gid},function (response) {
        ITEM_ID.empty();
        ITEM_ID.append($("<option></option>").attr("value", '').text('--Select--'));
        $.each(response, function (index, item) {
            ITEM_ID.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}

function clearfrom() {
    var ASET_ID = $('#ASET_ID');
    ASET_ID.empty();
    ASET_ID.append($("<option></option>").attr("value", '').text('--Select--'));
    var ITEM_ID = $('#ITEM_ID');
    ITEM_ID.empty();
    ITEM_ID.append($("<option></option>").attr("value", '').text('--Select--'));
}