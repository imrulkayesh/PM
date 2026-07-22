function getInspection() {
    var url = '/JsonReaderPM/InspectionHead';
    var id = $('#JOBH_ASET').val();
    var INSPH_ID = $('#INSPH_ID');
    if (id === "") {
        INSPH_ID.empty();
        INSPH_ID.append($("<option></option>").attr("value", '').text('--Select--'));
        return;
    };
    $.getJSON(url, { id: id }, function (response) {
        INSPH_ID.empty();
        INSPH_ID.append($("<option></option>").attr("value", '').text('--Select--'));
        $.each(response, function (index, item) {
            INSPH_ID.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}

function getInspectionItems() {
    getRestItems();

    var id = $('#INSPH_ID').val();
    var route = 'InspectionItems/' + id;
    $('#DetailItem').load(route);
}

function getRestItems() {
    var url = '/JsonReaderPM/InspectionItemNotIn'
    var Asset = $('#JOBH_ASET').val();
    var Inspection = $('#INSPH_ID').val();
    var JOBD_ITEM = $('#JOBD_ITEM');
    if (Asset === "" || Inspection === "") {
        JOBD_ITEM.empty();
        JOBD_ITEM.append($("<option></option>").attr("value", '').text('--Select--'));
        return;
    };
    $.getJSON(url, { aid: Asset, iid: Inspection }, function (response) {
        JOBD_ITEM.empty();
        JOBD_ITEM.append($("<option></option>").attr("value", '').text('--Select--'));
        $.each(response, function (index, item) {
            JOBD_ITEM.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}




//Add Multiple Order.
$("#addItem").click(function (e) {

    e.preventDefault();


    var JOBH_ASET = $.trim($("#JOBH_ASET").val()),
    INSPH_ID = $.trim($("#INSPH_ID").val()),
    JOBH_DATE = $.trim($("#JOBH_DATE").val()),
    JOBH_VENDOR = $.trim($("#JOBH_VENDOR").val()),
    JOBH_NOTE = $.trim($("#JOBH_NOTE").val())

    if (JOBH_ASET == "" || INSPH_ID == "" || JOBH_DATE == "" || JOBH_VENDOR == "" || JOBH_NOTE == "") {
        swal('Ohh..', 'Select a valid asset data', 'warning');
        return;
    };

    var itemId = $.trim($("#JOBD_ITEM :selected").val()),
        itemName = $("#JOBD_ITEM :selected").text(),
        itemQty = $.trim($("#JOBD_JQTY").val()),
        itemRate = $.trim($("#JOBD_JRATE").val()),
        itemNote = $.trim($("#JOBD_NOTE").val()),
        JobDetailBody = $("#JobDetailBody tbody");

    if (itemId == "" || itemQty == "" || itemRate == "" || parseInt(itemQty) <= 0 || parseFloat(itemRate) < 0 || itemNote == "") {
        swal('Ohh..', 'Select a valid item', 'warning');
        return;
    };

    var JobDetailBodytd = '<tr><td>' + itemId + '</td><td>' + itemName + '</td><td>0</td><td>0</td><td>0</td><td><input type="number" value="' + itemQty + '" class = "form-control" /> </td><td><input type="number" value="' + itemRate + '" class = "form-control" /></td><td><input type="text" value="' + itemNote + '" class = "form-control"/></td><td><a data-itemId="0" href="#" class="deleteItem btn btn-danger btn-sm"><i class="fa fa-trash" /></a></td></tr>';
    JobDetailBody.append(JobDetailBodytd);
    $("#JOBH_ASET").prop("disabled", true);
    $("#INSPH_ID").prop("disabled", true);
    $("#JOBH_VENDOR").prop("disabled", true);
    clearItem();
});
//After Add A New Order In The List, Clear Clean The Form For Add More Order.
function clearItem() {
    $('#JOBD_ITEM :selected').remove();
    $("#JOBD_JQTY").val('');
    $("#JOBD_JRATE").val('');
    $("#JOBD_NOTE").val('');
}
//Clear List if user want
$(document).on('click', 'a.deleteItem', function (e) {
    e.preventDefault();
    var $self = $(this);
    if ($(this).attr('data-itemId') == "0") {
        $(this).parents('tr').css("background-color", "#ff6347").fadeOut(800, function () {
            $(this).remove();
        });
    }
});


//Save
function saveJobCard(data) {
    return $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        url: "NewJobCard",
        data: data,
        success: function (response) {
            if (response.success) {
                swal('Success..', response.jobid, 'success');
            } else {
                swal('Opps..', response.jobid, 'warning');
            }
        },
        error: function () {
            swal('Ohh..', 'Error while saving Job Card', 'warning');
        }
    });
}


//Collect Multiple Order List For Pass To Controller
$("#CreateJob").click(function (e) {
    e.preventDefault();
    var ItemArr = [];
    ItemArr.length = 0;
    var Skp = "";
    $.each($("#JobDetailBody tbody tr"), function () {
        var JI = $.trim($(this).find('td:eq(0)').html());
        var IQ = $.trim($(this).find('td:eq(2)').html());
        var IR = $.trim($(this).find('td:eq(3)').html());
        var JQ = $.trim($(this).find('td:eq(5) input').val());
        var JR = $.trim($(this).find('td:eq(6) input').val());
        var JN = $.trim($(this).find('td:eq(7) input').val());
        if (JI == "" || JQ == "" || JR == "" || parseInt(JQ) <= 0 || parseFloat(JR) < 0 || JN == "") {
            Skp = JI;
        }
        ItemArr.push({
            JOBD_ITEM: JI,
            JOBD_IQTY: IQ,
            JOBD_IRATE:IR,
            JOBD_JQTY: JQ,
            JOBD_JRATE: JR,
            JOBD_NOTE: JN
        });
    });
    if (Skp !== "") {
        swal('Opps..', Skp + ' :Item data is not completed', 'info');
        return;
    }
    var JOBH_ASET = $("#JOBH_ASET").val(),
        INSPH_ID = $("#INSPH_ID").val(),
        JOBH_DATE = $("#JOBH_DATE").val(),
        JOBH_VENDOR = $("#JOBH_VENDOR").val(),
        JOBH_NOTE = $("#JOBH_NOTE").val(),
        JOBH_UPAY = $("#JOBH_UPAY").val(),
        JOBH_IPAY = $("#JOBH_IPAY").val();

    if (JOBH_ASET == "" || INSPH_ID == "" || JOBH_DATE == "" || JOBH_NOTE == "" || parseInt(JOBH_UPAY) < 0 || parseInt(JOBH_IPAY) < 0) {
        swal('Ohh..', 'Select a valid asset data', 'warning');
        return;
    };
    if (ItemArr.length <= 0) {
        swal('Ohh..', 'Select at least one item', 'warning');
        return;
    }

    var data = JSON.stringify({
        JOBH_ASET, INSPH_ID, JOBH_DATE, JOBH_VENDOR, JOBH_NOTE, JOBH_UPAY, JOBH_IPAY,
        JobCardDetails: ItemArr
    });

    $.when(saveJobCard(data)).then(function (response) {
        console.log(response);
    }).fail(function (err) {
        console.log(err);
    });
});




function LastRate() {
    var url = '/Inspection/getLastRate';
    var INSPH_ASET = $('#INSPH_ASET :selected').val();
    var INSPD_ITEM = $('#INSPD_ITEM :selected').val();
    if (INSPD_ITEM == '') {
        return;
    }
    $.getJSON(url, { asetId: INSPH_ASET, itemId: INSPD_ITEM }, function (response) {
        $('#INSPD_RATE').val(response.LastRate);
        $('#OldRec').val(response.Infor);
    });

}


