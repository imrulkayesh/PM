//for local use
function FindCompany() {
    var url = '/JsonReaderPM/getCompany';
    var COMPANY_ID = $('#COMPANY_ID');
    var id = $('#COMPANY_GROUP_ID').val();
    if (id === "") {
        COMPANY_ID.empty();
        COMPANY_ID.append($("<option></option>").attr("value", '').text('--Select Com--'));
        return;
    };
    $.getJSON(url, { group_id: id }, function (response) {
        COMPANY_ID.empty();
        COMPANY_ID.append($("<option></option>").attr("value", '').text('--Select Com--'));
        $.each(response, function (index, item) {
            COMPANY_ID.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}

//for server use
//function FindCompany() {
//    var url = '/PM2/JsonReaderPM/getCompany';
//    var COMPANY_ID = $('#COMPANY_ID');
//    var id = $('#COMPANY_GROUP_ID').val();
//    if (id === "") {
//        COMPANY_ID.empty();
//        COMPANY_ID.append($("<option></option>").attr("value", '').text('--Select Com--'));
//        return;
//    };
//    $.getJSON(url, { group_id: id }, function (response) {
//        COMPANY_ID.empty();
//        COMPANY_ID.append($("<option></option>").attr("value", '').text('--Select Com--'));
//        $.each(response, function (index, item) {
//            COMPANY_ID.append($("<option></option>").attr("value", item.Value).text(item.Text));
//        });
//    });
//}

function FindCategory() {
    var url = '/JsonReaderPM/getTaskCategory';
    var TASK_CATEGORY_ID = $('#TASK_CATEGORY_ID');
    var id = $('#TASK_CATEGORY_TYPE_ID').val();
    if (id === "") {
        TASK_CATEGORY_ID.empty();
        TASK_CATEGORY_ID.append($("<option></option>").attr("value", '').text('--Select Category--'));
        return;
    };
    $.getJSON(url, { category_type_id: id }, function (response) {
        TASK_CATEGORY_ID.empty();
        TASK_CATEGORY_ID.append($("<option></option>").attr("value", '').text('--Select Category--'));
        $.each(response, function (index, item) {
            TASK_CATEGORY_ID.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}

