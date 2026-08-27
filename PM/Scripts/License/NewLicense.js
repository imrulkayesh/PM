//for local use
function FindItem() {
    //alert(123);
    var url = '/JsonReaderPM/getItem';
    var ITEM_ID = $('#ITEM_ID');
    var id = $('#LICENSE_NAME_ID').val();
    if (id === "") {
        ITEM_ID.empty();
        ITEM_ID.append($("<option></option>").attr("value", '').text('--Select Item--'));
        return;
    };
    $.getJSON(url, { License_Name_id: id }, function (response) {
        ITEM_ID.empty();
        ITEM_ID.append($("<option></option>").attr("value", '').text('--Select Item--'));
        $.each(response, function (index, item) {
            ITEM_ID.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}

//for server use
//function FindItem() {
//    //alert(123);
//    var url = '/PM2/JsonReaderPM/getItem';
//    var ITEM_ID = $('#ITEM_ID');
//    var id = $('#LICENSE_NAME_ID').val();
//    if (id === "") {
//        ITEM_ID.empty();
//        ITEM_ID.append($("<option></option>").attr("value", '').text('--Select Item--'));
//        return;
//    };
//    $.getJSON(url, { License_Name_id: id }, function (response) {
//        ITEM_ID.empty();
//        ITEM_ID.append($("<option></option>").attr("value", '').text('--Select Item--'));
//        $.each(response, function (index, item) {
//            ITEM_ID.append($("<option></option>").attr("value", item.Value).text(item.Text));
//        });
//    });
//}


