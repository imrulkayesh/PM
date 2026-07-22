
function clearItem() {

    $("#DEMOTION").val('');
    $("#DEVELOPMENT").val('');
    $("#JOB_SACK_OUT").val('');
    $("#TRANSFER").val('');
    $("#WARNING_LETTER").val('');
    $("#PENALTY").val('');

    $("#SAVING_AMOUNT").val('');
    $("#TASK_CATEGORY_TYPE_ID").val('');
    //$("#TASK_CATEGORY_ID").prop('selectedIndex', 0);
    $("#TASK_CATEGORY_ID").val('');
    $("#AUDIT_SAVING_TYPE_ID").val('');
    $("#AUDIT_SAVING_NAME_ID").val('');
    $('#DIRECT_SAVING_TYPE_ID :selected').remove();
    $("#DIRECT_SAVING_AMOUNT").val('');
    $('#INDIRECT_SAVING_TYPE_ID :selected').remove();
    $("#INDIRECT_SAVING_AMOUNT").val('');
    $("#AUDIT_RPT_NAME").val('');
}

//Save for local server
function saveAuditReport(data) {
    //alert(999);
    return $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        url: "/Audit/UpdateWithoutAuditReport",
        data: data,
        success: function (response) {
            if (response.success) {
                swal('Success..', response.reportid, 'success');
            } else {
                swal('Opps..', response.reportid, 'warning');
            }
        },
        error: function () {
            swal('Ohh..', 'Error while saving Audit Report', 'warning');
        }
    });
}

//Save for online server
//function saveAuditReport(data) {
//    //alter(999999);
//    return $.ajax({
//        contentType: 'application/json; charset=utf-8',
//        dataType: 'json',
//        type: 'POST',
//        url: "/PM2/Audit/UpdateAuditReport",
//        data: data,
//        success: function (response) {
//            if (response.success) {
//                swal('Success..', response.reportpid, 'success');
//            } else {
//                swal('Opps..', response.reportpid, 'warning');
//            }
//        },
//        error: function () {
//            swal('Ohh..', 'Error while saving Audit Report', 'warning');
//        }
//    });
//}


$("#CreateAuditReport").click(function (e) {
    //alert(456);
    e.preventDefault();

    var TASK_ID = $("#TASK_ID").val(),

       AUDIT_RPT_NAME = $("#AUDIT_RPT_NAME").val(),
       WORK_SCOPE = $("#WORK_SCOPE").val(),
       WORK_OPPORTUNITY = $("#WORK_OPPORTUNITY").val(),
       ASSIST_BY = $("#ASSIST_BY").val(),
       RPT_SENT_DATE = $("#RPT_SENT_DATE").val(),
       AUDIT_RPT_ID = $("#AUDIT_RPT_ID").val(),

       DIRECT_SAVING_TYPE_ID = $("#DIRECT_AUDIT_SAVING_NAME").val(),
       DIRECT_SAVING_AMOUNT = $("#DIRECT_SAVING_AMOUNT").val(),
       INDIRECT_SAVING_TYPE_ID = $("#INDIRECT_AUDIT_SAVING_NAME").val(),
       INDIRECT_SAVING_AMOUNT = $("#INDIRECT_SAVING_AMOUNT").val(),

       MANAGEMENT_ACTION_ID = $("#MANAGEMENT_ACTION_ID").val(),
       HOLD_PAYMENT = $("#HOLD_PAYMENT").val(),
       SUPPLIER_BLACKLIST = $("#SUPPLIER_BLACKLIST").val()

        DEMOTION = $("#DEMOTION").val(),
        DEVELOPMENT = $("#DEVELOPMENT").val(),
        JOB_SACK_OUT = $("#JOB_SACK_OUT").val(),
        TRANSFER = $("#TRANSFER").val(),
        WARNING_LETTER = $("#WARNING_LETTER").val(),
        PENALTY = $("#PENALTY").val(),
        REMARK = $("#REMARK").val(),

        TASK_CATEGORY_TYPE_ID = $("#TASK_CATEGORY_TYPE_ID").val(),
        TASK_CATEGORY_ID = $("#TASK_CATEGORY_ID").val();

    if (DIRECT_SAVING_TYPE_ID == "") {
        swal('Ohh..', 'Select a Direct Saving data', 'warning');
        return;
    };


    var data = JSON.stringify({
        TASK_ID, AUDIT_RPT_NAME, WORK_SCOPE, WORK_OPPORTUNITY, ASSIST_BY, RPT_SENT_DATE, AUDIT_RPT_ID, DIRECT_SAVING_TYPE_ID, DIRECT_SAVING_AMOUNT, INDIRECT_SAVING_TYPE_ID, INDIRECT_SAVING_AMOUNT, MANAGEMENT_ACTION_ID, HOLD_PAYMENT, SUPPLIER_BLACKLIST, DEMOTION, DEVELOPMENT, JOB_SACK_OUT, TRANSFER, WARNING_LETTER, PENALTY, TASK_CATEGORY_TYPE_ID, TASK_CATEGORY_ID, REMARK,
    });

    $.when(saveAuditReport(data)).then(function (response) {
        console.log(response);
    }).fail(function (err) {
        console.log(err);
    });

    //clearItem();

});