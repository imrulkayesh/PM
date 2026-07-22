function getIItem() {
    var aUrl = 'GetDeptUser';
    var INSPH_ASET = $("#INSPH_ASET").val();
    var INSPD_ITEM = $("#INSPD_ITEM").val();
    if (INSPD_ITEM == "" ) {
        swal('Ohh..', 'Select a valid Item', 'warning');
        return;
    };
    $.getJSON(aUrl, { AsetId: INSPH_ASET }, function (response) {
        $('#AsetLoc').val(response.prim);
        $('#WorkLoc').val(response.seco);
    });
    $("#INSPD_ITEM").prop("disabled", true);
    LastRate(INSPH_ASET);
}
function LastRate(INSPH_ASET) {
    var url = 'getLastRate';
    //   var INSPH_ASET = $('#INSPH_ASET :selected').val();
    var INSPD_ITEM = $('#INSPD_ITEM :selected').val();
    if (INSPD_ITEM == '') {
        return;
    }
    $.getJSON(url, { asetId: INSPH_ASET, itemId: INSPD_ITEM }, function (response) {
        $('#INSPD_RATE').val(response.LastRate);
        $('#OldRec').val(response.Infor);
    });
}

//Add Multiple Order.
$("#addItem").click(function (e) {

    e.preventDefault();

    var INSPH_ASET = $.trim($("#INSPH_ASET :selected").val()),
                Aset_Name = $("#INSPH_ASET :selected").text(),
    INSPH_IUSER = $.trim($("#INSPH_IUSER").val()),
    MILEAGE = $.trim($("#MILEAGE").val()),
    itemId = $.trim($("#INSPD_ITEM :selected").val()),
     itemRate = $.trim($("#INSPD_IRATE").val()),
     itemNote = $.trim($("#INSPH_NOTE").val())

    if (INSPH_ASET == "" || INSPH_IUSER == "" || MILEAGE == "" || parseInt(MILEAGE) <= 0 || itemId == "" || itemRate == "" || itemNote == "") {
        swal('Ohh..', 'Select a valid asset data', 'warning');
        return;
    };


    var InspectionDetailBody = $("#InspectionDetail tbody");

    var InspectionItem = '<tr><td>' + INSPH_ASET + '</td><td>' + Aset_Name + '</td><td>' + MILEAGE + '</td><td>' + itemRate + '</td><td>' + itemNote + '</td><td><a data-itemId="0" href="#" class="deleteItem btn btn-danger btn-sm"><i class="fa fa-trash" /></a></td></tr>';
    InspectionDetailBody.append(InspectionItem);
    $("#INSPH_IUSER").prop("disabled", true);
    clearItem();
});
//After Add A New Order In The List, Clear Clean The Form For Add More Order.
function clearItem() {
    $('#INSPH_ASET :selected').remove();
    $("#MILEAGE").val('');
    $("#INSPD_IRATE").val('');
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
function saveInspection(data) {
    return $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        url: "../Inspection/SaveBundleInspection",
        data: data,
        success: function (response) {
            if (response.success) {
                location.reload();
                swal('Success..', response.inspid, 'success');
            } else {
                swal('Opps..', response.inspid, 'warning');
            }
        },
        error: function () {
            swal('Ohh..', 'Error while saving Bundle Inspection', 'warning');
        }
    });
}
//Collect Multiple Order List For Pass To Controller
$("#CreateJob").click(function (e) {
    e.preventDefault();
    var ItemArr = [];
    ItemArr.length = 0;
    debugger;
    $.each($("#InspectionDetail tbody tr"), function () {
        ItemArr.push({
            INSPH_ASET: $(this).find('td:eq(0)').html(),
            MILEAGE: $(this).find('td:eq(2)').html(),
            INSPD_IRATE: $(this).find('td:eq(3)').html(),
            INSPH_NOTE: $(this).find('td:eq(4)').html()
        });
    });

    var INSPD_ITEM = $("#INSPD_ITEM").val(),
        INSPH_IUSER = $("#INSPH_IUSER").val()

    if (INSPD_ITEM == "" || INSPH_IUSER == "") {
        swal('Ohh..', 'Select a valid asset data', 'warning');
        return;
    };
    if (ItemArr.length <= 1) {
        swal('Ohh..', 'Select at least two Asset', 'warning');
        return;
    }
    var data = JSON.stringify({
        INSPD_ITEM, INSPH_IUSER,
        NewBundleInspectionD: ItemArr
    });

    $.when(saveInspection(data)).then(function (response) {
        console.log(response);
    }).fail(function (err) {
        console.log(err);
    });
});