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

//Add Multiple Order.
$("#addItem").click(function (e) {
    
    e.preventDefault();

    var INSPH_ASET = $.trim($("#INSPH_ASET").val()),
    INSPH_DATE = $.trim($("#INSPH_DATE").val()),
    INSPH_IUSER = $.trim($("#INSPH_IUSER").val()),
    MILEAGE = $.trim($("#MILEAGE").val()),
    INSPH_TYPE = $.trim($("#INSPH_TYPE").val()),
    A_DEPTF = $.trim($("#A_DEPTF").val()),
    A_AREAF = $.trim($("#A_AREAF").val()),
    A_USERF = $.trim($("#A_USERF").val());

    if (INSPH_ASET == "" || INSPH_DATE == "" || INSPH_IUSER == "" || MILEAGE == "" || parseInt(MILEAGE) <= 0 || INSPH_TYPE == "" || A_DEPTF == "" || A_AREAF == "" || A_USERF == "") {
        swal('Ohh..', 'Select a valid asset data', 'warning');
        return;
    };

    var itemId = $.trim($("#INSPD_ITEM :selected").val()),
        itemName = $("#INSPD_ITEM :selected").text(),
        itemQty = $.trim($("#INSPD_IQTY").val()),
        itemRate = $.trim($("#INSPD_IRATE").val()),
        itemNote = $.trim($("#INSPD_NOTE").val()),
        InspectionDetailBody = $("#InspectionDetail tbody");

    if (itemId == "" || itemQty == "" || itemRate == "" || parseInt(itemQty) <= 0 || parseFloat(itemRate) < 0 || itemNote=="") {
        swal('Ohh..', 'Select a valid item', 'warning');
        return;
    };

    var InspectionItem = '<tr><td>' + itemId + '</td><td>' + itemName + '</td><td><input type="text" value="' + itemQty + '" class = "form-control" type="number"/></td><td><input type="text" value="' + itemRate + '" class = "form-control" type="number"/></td><td><input type="text" value="' + itemNote + '" class = "form-control" type="number"/></td><td><a data-itemId="0" href="#" class="deleteItem btn btn-danger btn-sm"><i class="fa fa-trash" /></a></td></tr>';
    InspectionDetailBody.append(InspectionItem);
    $("#INSPH_ASET").prop("disabled", true);
    $("#INSPH_IUSER").prop("disabled", true);
    $("#MILEAGE").prop("disabled", true);
    $("#INSPH_TYPE").prop("disabled", true);
    clearItem();
});
//After Add A New Order In The List, Clear Clean The Form For Add More Order.
function clearItem() {
    $('#INSPD_ITEM :selected').remove();
    $("#INSPD_IQTY").val('');
    $("#INSPD_IRATE").val('');
    $("#INSPD_NOTE").val('');
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
        url: "/Inspection/UpdateInspection",
        data: data,
        success: function (response) {
            if (response.success) {
                swal('Success..', response.inspid, 'success');
            } else {
                swal('Opps..', response.inspid, 'warning');
            }
        },
        error: function (request, error) {
            swal('Ohh..', error + ": Error while saving Inspection", 'warning');
        }
    });
}
//Collect Multiple Order List For Pass To Controller
$("#CreateJob").click(function (e) {
    e.preventDefault();
    var ItemArr = [];
    ItemArr.length = 0;
    var Skp="";
    $.each($("#InspectionDetail tbody tr"), function () {
        var II =$(this).find('td:eq(0)').html();
        var IQ=$(this).find('td:eq(2) input').val();
        var IR =  $(this).find('td:eq(3) input').val();
        var IN = $(this).find('td:eq(4) input').val()
        if (II == "" || IQ == "" || IR == ""  || parseInt(IQ) <= 0 || parseFloat(IR) < 0 || IN == "") {
            Skp = II;
        }
        ItemArr.push({
            INSPD_ITEM:II,
            INSPD_IQTY: IQ,
            INSPD_IRATE:IR,
            INSPD_NOTE: IN
        });
    });
    if (Skp !=="") {
        swal('Opps..', Skp + ' :Item data is not completed', 'info');
        return;
    }
    var INSPH_ASET = $("#INSPH_ASET").val(),
        INSPH_DATE = $("#INSPH_DATE").val(),
        INSPH_IUSER = $("#INSPH_IUSER").val(),
        MILEAGE = $("#MILEAGE").val(),
        INSPH_TYPE = $("#INSPH_TYPE").val(),
        INSPH_NOTE = $("#INSPH_NOTE").val(),
        A_DEPTF = $("#A_DEPTF").val(),
        A_AREAF = $("#A_AREAF").val(),
        A_USERF = $("#A_USERF").val(),
        INSPH_ID = $("#INSPH_ID").val();

    if (INSPH_ASET == "" || INSPH_DATE == "" || INSPH_IUSER == "" || MILEAGE == "" || parseInt(MILEAGE) <= 0 || INSPH_TYPE == "" || A_DEPTF == "" || A_AREAF == "" || A_USERF == "" || INSPH_ID == "") {
        swal('Ohh..', 'Select a valid asset data', 'warning');
        return;
    };
    if (ItemArr.length <= 0) {
        swal('Ohh..', 'Select at least one item', 'warning');
        return;
    }

    var data = JSON.stringify({
        INSPH_ASET, INSPH_DATE, INSPH_IUSER, MILEAGE, INSPH_TYPE, INSPH_NOTE, A_DEPTF, A_AREAF, A_USERF, INSPH_ID,
        InspectionD: ItemArr
    });

    $.when(saveInspection(data)).then(function (response) {
        console.log(response);
    }).fail(function (err) {
        console.log(err);
    });
});