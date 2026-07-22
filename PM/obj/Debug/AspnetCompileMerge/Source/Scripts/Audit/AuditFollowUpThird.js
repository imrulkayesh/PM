//Save
function saveAuditFollowUpThird(data) {
    //alert(3000);
    return $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        url: "CreateThirdFollowUpReport",
        data: data,
        success: function (response) {
            if (response.success) {
                swal('Success..', response.reportpid, 'success');
            } else {
                swal('Opps..', response.reportpid, 'warning');
            }
        },
        error: function () {
            swal('Ohh..', 'Error while saving Audit Follow Up Data', 'warning');
        },
    });
}

function clearItem() {
    $('#AUDIT_RPT_ID :selected').remove();
    $("#TOTAL_FINDINGS").val('');
    $("#ACTION_FINDINGS").val('');

    $('#RISK_TYPE_ID :selected').remove();
    $('#RISK_MITIGATION_ID :selected').remove();
    $('#FOLLOWUP_REQUIREMENT_ID :selected').remove();

    $("#RISK_MITIGATION_MARKS").val('');
    $("#REPLY_PERSON").val('');
    $("#RESPONSIBLE_DEPT_HEAD").val('');
    $("#REMARKS").val('');
    $("#SIGNIFICANT_GAP").val('');
}

$("#CreateAuditThirdFollowUp").click(function (e) {
    //alert(3);
    e.preventDefault();
    debugger;

    var AUDIT_RPT_ID = $("#AUDIT_RPT_ID").val(),
        TOTAL_FINDINGS = $("#TOTAL_FINDINGS").val(),
        ACTION_FINDINGS = $("#ACTION_FINDINGS").val(),
        NO_ACTION_FINDINGS = $("#NO_ACTION_FINDINGS").val(),
        ACTION_PERCENTAGE = $("#ACTION_PERCENTAGE").val(),

        FOLLOWUP_DATE = $("#FOLLOWUP_DATE").val();
    RISK_TYPE_ID = $("#RISK_TYPE_ID").val(),
    RISK_MITIGATION_ID = $("#RISK_MITIGATION_ID").val(),
    RISK_MITIGATION_MARKS = $("#RISK_MITIGATION_MARKS").val(),
    REPLY_PERSON = $("#REPLY_PERSON").val();

    RESPONSIBLE_DEPT_HEAD = $("#RESPONSIBLE_DEPT_HEAD").val(),
    REMARKS = $("#REMARKS").val(),
    SIGNIFICANT_GAP = $("#SIGNIFICANT_GAP").val(),
    FOLLOWUP_REQUIREMENT_ID = $("#FOLLOWUP_REQUIREMENT_ID").val();

    if (AUDIT_RPT_ID == "") {
        swal('Ohh..', 'Please select the Report Name.', 'warning');
        return;
    };

    if (FOLLOWUP_REQUIREMENT_ID == "") {
        swal('Ohh..', 'Please select the Next Followup Requirement.', 'warning');
        return;
    };


    var data = JSON.stringify({
        AUDIT_RPT_ID, TOTAL_FINDINGS, ACTION_FINDINGS, NO_ACTION_FINDINGS, ACTION_PERCENTAGE, FOLLOWUP_DATE, RISK_TYPE_ID, RISK_MITIGATION_ID, RISK_MITIGATION_MARKS, REPLY_PERSON, RESPONSIBLE_DEPT_HEAD, REMARKS, SIGNIFICANT_GAP, FOLLOWUP_REQUIREMENT_ID
    });

    $.when(saveAuditFollowUpThird(data)).then(function (response) {
        console.log(response);
    }).fail(function (err) {
        console.log(err);
    });
    clearItem();
});