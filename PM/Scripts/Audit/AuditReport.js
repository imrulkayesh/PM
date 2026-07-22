function FindSavingType() {
    //alert("112");
    var url = '../JsonReaderPM/getSavingName';
    var AUDIT_SAVING_NAME_ID = $('#AUDIT_SAVING_NAME_ID');
    var id = $('#AUDIT_SAVING_TYPE_ID').val();
    //alert(id);
    if (id === "") {
        AUDIT_SAVING_NAME_ID.empty();
        AUDIT_SAVING_NAME_ID.append($("<option></option>").attr("value", '').text('--Select Saving Type--'));
        return;
    };
    $.getJSON(url, { saving_type_id: id }, function (response) {
        AUDIT_SAVING_NAME_ID.empty();
        AUDIT_SAVING_NAME_ID.append($("<option></option>").attr("value", '').text('--Select Saving Type--'));
        $.each(response, function (index, item) {
            AUDIT_SAVING_NAME_ID.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}

//Assist By Multiple

//$("#addAssistBy").click(function (e) {
//    alert(999);

//    e.preventDefault();
//    debugger;

//    if (Assist_BY_ID == "") {
//        swal('Ohh..', 'Please select Assist By', 'warning');
//        return;
//    };

//    var assistById = $.trim($("#Assist_BY_ID :selected").val()),
//        assistByName = $("#Assist_BY_ID :selected").text(),

//        AuditAssistByDetailBody = $("#AuditAssistByDetail tbody");

//    if (assistById == "") {
//        swal('Ohh..', 'Select a valid item', 'warning');
//        return;
//    };

//    var ReportAssistByItem = '<tr><td>' + assistById + '</td><td>' + assistByName + '</td><td>' + actionRemark + '</td><td><a data-itemId="0" href="#" class="deleteItem btn btn-danger btn-sm"><i class="fa fa-trash" /></a></td></tr>';
//    AuditAssistByDetailBody.append(ReportAssistByItem);
//    clearItem();
//});


