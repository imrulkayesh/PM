$('.ActionMenu').click(function () {
    var Me = $(this);
    $.ajax({
        type: "POST",
        url: $(this).data('url'),
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                debugger;
                Me.hide();
                swal('Success..', response.responseText, 'success');
            } else {
                swal('Ohhh..', response.responseText, 'warning');
            }
        },
        error: function (response) {
            swal('Ohhh..', 'Error in Server', 'error');
        }
    });
});