$(document).ready(function () {
    $('#ASET_NAME').autocomplete({
        source: '@Url.Action("Autocomplete")'
    });
});

function FindDepts() {
    var url = '@Url.Action("getDept", "KSAAssets")';
    var ASET_DEPT = $('#ASET_DEPT');
    var id = $('#AllBEUN').val();
    $.getJSON(url, { BeuId: id }, function (response) {
        ASET_DEPT.empty();
        ASET_DEPT.append($("<option></option>").attr("value", '').text('--Select Unit--'));
        $.each(response, function (index, item) {
            ASET_DEPT.append($("<option></option>").attr("value", item.DEPT_TEXT).text(item.DEPT_NAME));
        });
    });
};
function CopyToCode() {
    var a = $('#ASET_TEXT').val();
    if (a.length === 0) {
        $('#ASET_TEXT').val($('#ASET_NAME').val());
    }
};