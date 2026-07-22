function FindDepts() {
    var url = '/JsonReaderPM/getDept';
    var ASET_DEPARTMENT = $('#ASET_DEPARTMENT');
    var id = $('#ASET_BUSINESS').val();
    if (id === "") {
        ASET_DEPARTMENT.empty();
        ASET_DEPARTMENT.append($("<option></option>").attr("value", '').text('--select--'));
        return;
    };
    $.getJSON(url, { bus_id: id }, function (response) {
        ASET_DEPARTMENT.empty();
        ASET_DEPARTMENT.append($("<option></option>").attr("value", '').text('--select--'));
        $.each(response, function (index, item) {
            ASET_DEPARTMENT.append($("<option></option>").attr("value", item.Value).text(item.Text));
        });
    });
}