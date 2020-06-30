$(function () {

    $("#btnTRANS").click(function () {
        $("#imgLOADING").show();
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: '../BMED/Budget/TransToBuyEvaluate',
            dataType: "json",
            success: function (data) {
                $("#imgLOADING").hide();
                if (data.success) {
                    alert('產生完成!!');
                }
                else {
                    alert(data.error);
                }
            }
        });
    });
});