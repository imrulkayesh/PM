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

//$(document).ready(function () {
//    $('#ASSIST_BY_LIST').select2({
//        width: '100%',
//        placeholder: 'Select Auditor(s)',
//        allowClear: true
//    });
//});

//$(document).ready(function () {
//    $('#ASSIST_BY_LIST').select2({
//        width: '100%',
//        placeholder: 'Select Auditor(s)',
//        allowClear: true,
//        minimumResultsForSearch: 0   // forces search box to always show, even with few options
//    });
//});

// WITH THIS:
$(document).ready(function () {
    $('#ASSIST_BY_LIST').select2({
        width: '100%',
        placeholder: 'Select Auditor(s)',
        allowClear: true,
        ajax: {
            url: '/Audit/SearchAuditors',
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return { term: params.term };
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        return { id: item.AUDITOR_STAFF_ID, text: item.AUDITOR_NAME };
                    })
                };
            },
            cache: true
        },
        minimumInputLength: 1
    });
});


