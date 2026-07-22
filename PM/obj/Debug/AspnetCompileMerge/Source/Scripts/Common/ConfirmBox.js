$('#submit').click(function () {
    var isValid = $("#frmMain").validate();
    if (!isValid) {
        e.preventDefault(); //prevent the default action
    }
    $('#frmMain').submit();
});

$(document).ready(function () {
    $('.FileUp').click(function () {
        var url = $(this).data('url');
        $.get(url, function (data) {
            $('#modalContainer').html(data);
            $('#pvModal').modal('show');
        });
    });
    $('.FileDn').click(function () {
        var url = $(this).data('url');
        $.get(url, function (data) {
            $('#modalContainer').html(data);
            $('#pvModal').modal('show');
        });
    });
    $('.TrnDel').click(function () {
        var url = $(this).data('url');
        $.get(url, function (data) {
            $('#modalContainer').html(data);
            $('#pvModal').modal('show');
        });
    });
    $('.btn4').click(function () {
        var url = $(this).data('url');
        $.get(url, function (data) {
            $('#modalContainer').html(data);
            $('#pvModal').modal('show');
        });
    });
    $('#btn5').click(function () {
        var url = $(this).data('url');
        $.get(url, function (data) {
            $('#modalContainer').html(data);
            $('#pvModal').modal('show');
        });
    });
    $('#btn6').click(function () {
        var url = $(this).data('url');
        $.get(url, function (data) {
            $('#modalContainer').html(data);
            $('#pvModal').modal('show');
        });
    });
    $('#btn7').click(function () {
        var url = $(this).data('url');
        $.get(url, function (data) {
            $('#modalContainer').html(data);
            $('#pvModal').modal('show');
        });
    });
    $('#btn8').click(function () {
        var url = $(this).data('url');
        $.get(url, function (data) {
            $('#modalContainer').html(data);
            $('#pvModal').modal('show');
        });
    });
    $('#btn9').click(function () {
        var url = $(this).data('url');
        $.get(url, function (data) {
            $('#modalContainer').html(data);
            $('#pvModal').modal('show');
        });
    });
    $('#btn10').click(function () {
        var url = $(this).data('url');
        $.get(url, function (data) {
            $('#modalContainer').html(data);
            $('#pvModal').modal('show');
        });
    });
});