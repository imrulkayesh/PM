$(document).on("click", ".UpdateR", function () {
    $('#REMIND_ID').val($(this).data('id'));
    $('#REMIND_DEL').val($(this).data('id'));
    $('#REMIND_COM').val($(this).data('id'));
});