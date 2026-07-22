function FindCompany() {
    //alert(123);
    var url = '../JsonReaderPM/getCompany';
    var COMPANY_ID = $('#COMPANY_ID');
    var id = $('#COMPANY_GROUP_ID').val();
    //alert(id);
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

function FindCategory() {
    var url = '../JsonReaderPM/getTaskCategory';
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

//function FindSavingType() {
//    alert("112");
//    var url = '../JsonReaderPM/getSavingName';
//    var AUDIT_SAVING_NAME_ID = $('#AUDIT_SAVING_NAME_ID');
//    var id = $('#AUDIT_SAVING_TYPE_ID').val();
//    if (id === "") {
//        AUDIT_SAVING_NAME_ID.empty();
//        AUDIT_SAVING_NAME_ID.append($("<option></option>").attr("value", '').text('--Select Saving Type--'));
//        return;
//    };
//    $.getJSON(url, { saving_type_id: id }, function (response) {
//        AUDIT_SAVING_NAME_ID.empty();
//        AUDIT_SAVING_NAME_ID.append($("<option></option>").attr("value", '').text('--Select Saving Type--'));
//        $.each(response, function (index, item) {
//            AUDIT_SAVING_NAME_ID.append($("<option></option>").attr("value", item.Value).text(item.Text));
//        });
//    });
//}

