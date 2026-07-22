//<div id="dialog-confirm" title="Confirm Adjustment"><p>This change cannot be undone!</p></div>
//btn Class .btnOK
//Form id #frmMain
$(function () {
    debugger;
    $("#dialog-confirm").dialog({        
        autoOpen: false,
        model: true,
        width: 300,
        resizable: false,
        height: 200
    });
    $(".btnOK").click(function (e) {
        e.preventDefault();
        $("#dialog-confirm").dialog({
            buttons: {
                "Confirm": function () {
                    var isValid = $("#frmMain").validate();
                    if (!isValid) {
                        e.preventDefault(); //prevent the default action
                    }
                    $("#frmMain").submit();
                },
                "Cancel": function () {
                    $(this).dialog("close");
                }
            }
        });
        $("#dialog-confirm").dialog("open");
    });
});