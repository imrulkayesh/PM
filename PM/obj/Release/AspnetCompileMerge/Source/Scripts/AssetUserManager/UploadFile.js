var formdata = new FormData(); //FormData object
$(document).ready(function () {
    $("#fileInput").on("change", function () {
        var fileInput = document.getElementById('fileInput');
        //Iterating through each files selected in fileInput
        for (i = 0; i < fileInput.files.length; i++) {

            var sfilename = fileInput.files[i].name;
            let srandomid = Math.random().toString(36).substring(7);

            formdata.append(sfilename, fileInput.files[i]);

            var markup = "<tr id='" + srandomid + "'><td>" + sfilename + "</td><td><a href='#' onclick='DeleteFile(\"" + srandomid + "\",\"" + sfilename +
                "\")'><span class='glyphicon glyphicon-remove red'></span></a></td></tr>"; // Binding the file name
            $("#FilesList tbody").append(markup);

        }
        chkatchtbl();
        $('#fileInput').val('');
    });
    $("#btnupload").click(function () {
        formdata.delete('uploadername');//Clear
        formdata.append('uploadername', $('#txtTRNS_TEXT').val());
        //if (Object.keys(FormData).length === 0) {
        //    swal("No file selected", "please select at least 1 file.!", "error");
        //    return;
        //}
        $.ajax({
            url: '/AssetUserManager/UploadFile',
            type: "POST",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            data: formdata,
            async: false,
            success: function (result) {
                location.reload();
                if (result != "") {
                    swal("Wow...", +result.fc + " File(s) uploaded", "success");
                }
            },
            error: function (err) {
                swal("Opps..", err.statusText, "error");
            }
        });
    });
});
function DeleteFile(Fileid, FileName) {
    formdata.delete(FileName)
    $("#" + Fileid).remove();
    chkatchtbl();
}
function chkatchtbl() {
    if ($('#FilesList tr').length > 1) {
        $("#FilesList").css("visibility", "visible");
    } else {
        $("#FilesList").css("visibility", "hidden");
    }
}