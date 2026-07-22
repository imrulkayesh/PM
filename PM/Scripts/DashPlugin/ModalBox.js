$('#submit').click(function () {
    var isValid = $("#frmMain").validate();
    if (!isValid) {
        e.preventDefault(); //prevent the default action
    }
    $('#frmMain').submit();
});

$(document).ready(function () {

    $('.btn4').click(function () {
        var url = $(this).data('url');
        $.get(url, function (data) {
            $('#modalContainer').html(data);
            $('#pvModal').modal('show');
        });
    });
});