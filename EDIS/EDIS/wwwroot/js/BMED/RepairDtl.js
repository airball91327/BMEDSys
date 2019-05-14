function showmsg2() {
    alert("儲存成功!");
    window.location.reload();   //刷新RepairDtl與RepairDtl2的頁面資訊
}

$(document).ready(function () {

    $("#DealState").change(function () {
        /* 3 = 已完成，4 = 報廢，8 = 退件 */
        if ($(this).val() == 3 || $(this).val() == 8) {
            $("#DealDes").attr("required", "required");
        }
        else if ($(this).val() == 4 ) {
            $("#DealDes").attr("required", "required");         
        }
        else {
            $("#DealDes").removeAttr("required");
            $("#dealDesErrorMsg").html("");
        }
    });
    $('#DealState').trigger("change");

});
//$(function () {
//    $(".datefield").datepicker({
//        format: "yyyy/mm/dd"
//    });
//});