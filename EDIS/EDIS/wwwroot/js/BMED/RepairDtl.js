function showmsg2() {
    alert("儲存成功!");
    window.location.reload();   //刷新RepairDtl與RepairDtl2的頁面資訊
}

$(document).ready(function () {

    var originAssetNo = $("#AssetNo").val();

    $("#DealState").change(function () {
        /* 3 = 已完成，4 = 報廢，8 = 退件 */
        if ($(this).val() == 3 || $(this).val() == 8) {
            $("#DealDes").attr("required", "required");
            $("#AssetNo").removeAttr("required");
            $("#AssetNo").removeAttr("readonly");
            $(".assetNoControl").hide();
        }
        else if ($(this).val() == 4 ) {
            $("#DealDes").attr("required", "required");
            $(".assetNoControl").show();

            if ($('input:radio[name="HasAssetNo"]:checked').val() == 'Y') {
                $("#AssetNo").attr("required", "required");
                $("#AssetNo").removeAttr("readonly");
                $("#assetsEditBtn").removeAttr("disabled");
            }
            else {
                $("#AssetNo").val("");
                $("#AssetNo").attr("readonly", "readonly");
                $("#AssetNo").removeAttr("required");
                $("#assetsEditBtn").attr("disabled", "disabled");
            }            
        }
        else {
            $("#DealDes").removeAttr("required");
            $("#dealDesErrorMsg").html("");
            $("#AssetNo").removeAttr("required");
            $("#AssetNo").removeAttr("readonly");
            $(".assetNoControl").hide();
        }
    });
    $('#DealState').trigger("change");

    $("#assetsEditBtn").click(function () {
        $("#AssetNo").val(originAssetNo);
    });
});
//$(function () {
//    $(".datefield").datepicker({
//        format: "yyyy/mm/dd"
//    });
//});