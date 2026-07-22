$(document).ready(function () {
    $('#ASET_NAME').autocomplete({
        source: '@Url.Action("getSuggession")'
    });
});
$(function () {
    $('.viewAsset').click(function () {
        var id = $('#ASET_NAME').val();
        var route = 'AssetEntry/SingleAssetView?AssetName=' + id;
        $('#ViewEditPartialView1').load(route);
    });
    $('.addacce').click(function () {
        var id = $(this).data('assigned-id');
        var id2 = $(this).data('asgrid');
        var route = 'AddNewAcce?id=' + id + '&AsgrId=' + id2;
        $('#ViewEditPartialViewModal').load(route);
    });

    $('.viewacce').click(function () {
        var id = $(this).data('vhclid');
        var route = 'AcceList?id=' + id;
        $('#ViewEditPartialViewModal').load(route);
    });
});