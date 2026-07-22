// After Add A New Asset In The List, If You Want, You Can Remove It.
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
function saveAsset(data) {
    return $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        url: "../AssetManager/SaveFridgeExcel",
        data: data,
        beforeSend: function () {
            $('#overlay').show();    /*showing  a div with spinning image */
        },
        success: function (result) {
            swal('Information..', result, 'info');
            //  location.reload();
        },
        error: function () {
            swal('Ohh..', 'Error! Data Not Upload', 'error');
            location.reload();
            //location.href = "/Insurance/InputResult";
        },
        complete: function () {
            $('#overlay').hide();
        }
    });
}
//Collect Multiple Asset List For Pass To Controller
$("#saveDatabase").click(function (e) {
    e.preventDefault();

    var assetArr = [];
    assetArr.length = 0;

    $.each($("#table1 tbody tr"), function () {
        assetArr.push({
            ASET_CODE_NAME: $(this).find('td:eq(1)').html(),
            ASET_DEPARTMENT: $(this).find('td:eq(2)').html(),
            ASET_AREA: $(this).find('td:eq(3)').html(),
            ASET_BRAND: $(this).find('td:eq(4)').html(),
            ASET_GROUP: $(this).find('td:eq(5)').html(),
            ASET_MODEL: $(this).find('td:eq(6)').html(),
            ASET_PO_ENGINE: $(this).find('td:eq(7)').html(),
            ASET_GRN_CHASIS: $(this).find('td:eq(8)').html(),
            ASET_CAPACITY: $(this).find('td:eq(9)').html(),
            ASET_PRICE: $(this).find('td:eq(10)').html(),
            ASET_REG_DATE: $(this).find('td:eq(11):last').html(),
            ASET_MFG_YEAR: $(this).find('td:eq(12)').html(),
            ASET_NOTE: $(this).find('td:eq(13)').html()
        });
    });
    if (assetArr.length <= 0) {
        swal('Ohh..', 'Select at least one item', 'warning');
        return;
    }
    var data = JSON.stringify({
        ExlData: assetArr
    });

    $.when(saveAsset(data)).then(function (response) {
        console.log(response);
    }).fail(function (err) {
        console.log(err);
    });
});