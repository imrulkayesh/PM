$(document).ready(function () {
    $('#ASET_NAME').autocomplete({
        source: '@Url.Action("getSuggession")'
    });
});
$(function () {
    $('.viewAsset').click(function () {
        var id = $('#ASET_NAME').val();
        var route = '@Url.Action("SingleAssetView", "AssetEntry")?AssetName=' + id;
        $('#ViewEditPartialView1').load(route);
    });
    $('.addacce').click(function () {
        var id = $(this).data('assigned-id');
        var id2 = $(this).data('asgrid');
        var route = '@Url.Action("AddNewAcce", "KSAAssets")?id=' + id + '&AsgrId=' + id2;
        $('#ViewEditPartialViewModal').load(route);
    });

    $('.viewacce').click(function () {
        var id = $(this).data('vhclid');
        var route = '@Url.Action("AcceList", "KSAAssets")?id=' + id;
        $('#ViewEditPartialViewModal').load(route);
    });
});