function getAssetData() {
    var url = '/AssetTransfer/getAssetData';
    var TRNS_FUSER = $('#TRNS_FUSER');
    var TRNS_FDEPT = $('#TRNS_FDEPT');
    var TRNS_FDEPO = $('#TRNS_FDEPO');
    var AcceNameNew = $('#AcceNameNew');
    var id = $('#TRNS_ASET').val();
    $.getJSON(url, { id: id }, function (response) {
        TRNS_FUSER.empty();
        TRNS_FDEPT.empty();
        TRNS_FDEPO.empty();
        AcceNameNew.empty();
        $.each(response.DeptList, function (index, item) {
            TRNS_FDEPT.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
        $.each(response.DepotList, function (index, item) {
            TRNS_FDEPO.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
        $.each(response.NewAcceList, function (index, item) {
            AcceNameNew.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
        $.each(response.FromUserList, function (index, item) {
            TRNS_FUSER.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
    //get Acce list
    var route = '/AssetTransfer/ShowAccessories/?id=' + id;
    $('#ViewEditPartialView').load(route);
};
function getToDeptList() {
    var url = '/AssetTransfer/getToDeptList';
    var TRNS_TDEPT = $('#TRNS_TDEPT');
    var id = $('#TRNS_TCOMP').val();
    $.getJSON(url, { id: id }, function (response) {
        TRNS_TDEPT.empty();
        TRNS_TDEPT.append($("<option></option>").attr("value", '').text('--Select Dept--'));
        $.each(response.retunDeptList, function (index, item) {
            TRNS_TDEPT.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
};
$("#addToList").click(function (e) {
    e.preventDefault();

    if ($.trim($("#AcceNameNew").val()) == "" || $.trim($("#AcceQtyNew").val()) == "" || $.trim($("#AccePriceNew").val()) == "" || $.trim($("#TRNS_FUSER :selected").val()) == "" || $.trim($("#TRNS_ASET :selected").val()) == "" || $.trim($("#TRNS_TUSER :selected").val()) == "") return;

    var acceItem = $("#AcceNameNew :selected").val(),
        acceItemName = $("#AcceNameNew :selected").text(),
        TRNS_ASET = $("#TRNS_ASET :selected").text(),
        TRNS_FUSER = $("#TRNS_FUSER :selected").text(),
        TRNS_TUSER = $("#TRNS_TUSER :selected").text(),
        quantity = $("#AcceQtyNew").val(),
        prices = $("#AccePriceNew").val(),
        notes = $("#AcceNoteNew").val(),
        idate = new Date();
    issuedate = idate.getDate() + "/" + (idate.getMonth() + 1) + "/" + idate.getFullYear();
    detailsTableBody = $("#AcceListTable tbody");

    var AcceItems = '<tr><td><input type="checkbox" id=' + acceItem + '" checked/></td><td><input type="text" class="form-control"  disabled = "disabled" value="' + acceItem + '" /></td><td>' + acceItemName + '</td><td>' + TRNS_ASET + '</td><td>' + TRNS_FUSER + '</td><td>' + TRNS_TUSER + '</td><td>' + 0 + '</td><td><input type="text" class="form-control" value="' + quantity + '" /></td><td>' + (parseFloat(prices) * parseInt(quantity)) + '</td><td>' + issuedate + '</td><td><input type="text" class="form-control" value="' + notes + '" /></td><td><a data-itemId="0" href="#" class="deleteItem btn btn-flat btn-danger">Del</a></td></tr>';
    detailsTableBody.append(AcceItems);
    clearItem();

    $("#AcceNameNew :selected").remove();
});

// After Add A New Order In The List, Clear Clean The Form For Add More Order.
function clearItem() {
    $("#AcceQtyNew").val('');
    $("#AccePriceNew").val('');
    $("#AcceNoteNew").val('');
}
// After Add A New Order In The List, If You Want, You Can Remove It.
$(document).on('click', 'a.deleteItem', function (e) {
    e.preventDefault();
    var $self = $(this);
    if ($(this).attr('data-itemId') == "0") {
        $(this).parents('tr').css("background-color", "#ff6347").fadeOut(800, function () {
            $(this).remove();
        });
    }
});

//After Click Save Button Pass All Data View To Controller For Save Database
function saveOrder(data) {
    return $.ajax({
        contentType: 'application/json; charset=utf-8',// Not to set any content header
        dataType: 'json',
        type: 'POST',
        url: "/AssetTransfer/SaveTransfer",
        data: data,
        success: function (result) {
            $("#myElem").show();
            setTimeout(function () { $("#myElem").hide(); }, 5000);
            //alert(result);
            location.reload();
        },
        error: function (err) {
            alert(err.statusText);
        }
    });
};


//Collect Multiple Order List For Pass To Controller
$("#saveOrder").click(function (e) {
    e.preventDefault();

    $("#AssetTransfer").validate();

    if ($.trim($("#TRNS_ASET :selected").val()) == "" || $.trim($("#TRNS_DATE").val()) == "" || $.trim($("#TRNS_NOTE").val()) == "" || $.trim($("#TRNS_FUSER :selected").val()) == "" || $.trim($("#TRNS_TUSER :selected").val()) == "" || $.trim($("#TRNS_FDEPT :selected").val()) == "" || $.trim($("#TRNS_TDEPT :selected").val()) == "" || $.trim($("#TRNS_FDEPO :selected").val()) == "" || $.trim($("#TRNS_TDEPO :selected").val()) == "") return "Enter all data";


    //Acce item List Assgin to array
    var accArr = [];
    accArr.length = 0;
    $.each($("#AcceListTable tbody tr"), function () {
        if ($(this).find('td:eq(0) input').is(":checked")) { //push only selected items
            accArr.push({
                ACCE_ASAC: $(this).find('td:eq(1) input').val(),
                ACCE_QTY: $(this).find('td:eq(7) input').val(),
                ACCE_PRICE: $(this).find('td:eq(8)').html(),
                ACCE_NOTE: $(this).find('td:eq(10) input').val()
            });
        }
    });
    //Assign header values
    var data = JSON.stringify({
        TRNS_ASET: $("#TRNS_ASET :selected").val(),
        TRNS_DATE: $("#TRNS_DATE").val(),
        TRNS_NOTE: $("#TRNS_NOTE").val(),
        // TRNS_POST: $("#TRNS_POST").val(),
        TRNS_FUSER: $("#TRNS_FUSER :selected").val(),
        TRNS_FDEPT: $("#TRNS_FDEPT :selected").val(),
        TRNS_FDEPO: $("#TRNS_FDEPO :selected").val(),
        TRNS_TUSER: $("#TRNS_TUSER :selected").val(),
        TRNS_TDEPT: $("#TRNS_TDEPT :selected").val(),
        TRNS_TDEPO: $("#TRNS_TDEPO :selected").val(),
        acceitemlist: accArr //send items array
    });
    //Show Messages
    $.when(saveOrder(data)).then(function (response) {
        console.log(response);
    }).fail(function (err) {
        console.log(err);
    });
});

$("#AssetTransfer").removeAttr("novalidate");