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
        url: "../Insurance/SaveUpdateInsurance",
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
            INSU_ASET: $(this).find('td:eq(1)').html(),
            INSU_POLICY: $(this).find('td:eq(2)').html(),
            INSU_ASET_VALUE: $(this).find('td:eq(3)').html(),
            INSU_PREMIUM: $(this).find('td:eq(4)').html(),
            INSU_NOTE: $(this).find('td:eq(5)').html(),
            INSU_REG_START: $(this).find('td:eq(6):last').html(),
            INSU_REG_END: $(this).find('td:eq(7):last').html(),
            INSU_VAT_AMOUNT: $(this).find('td:eq(8):last').html(),
            INSU_DATE: $(this).find('td:eq(9):last').html()
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
