function getDeptList() {
    var url = '/JsonReaderPM/getDept';
    var OWNR_DEPT = $('#OWNR_DEPT');
    var id = $('#OWNR_BUSINESS').val();
    if (id === "") {
        OWNR_DEPT.empty();
        OWNR_DEPT.append($("<option></option>").attr("value", '').text('--Select--'));
        return;
    };
    $.getJSON(url, { bus_id: id }, function (response) {
        OWNR_DEPT.empty();
        OWNR_DEPT.append($("<option></option>").attr("value", '').text('--Select--'));
        $.each(response, function (index, item) {
            OWNR_DEPT.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}