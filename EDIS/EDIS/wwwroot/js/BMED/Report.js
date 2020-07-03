function smgREPORT(data)
{
    console.log(data);
    if (data.success === false)
    {
        alert(data.error);
    }
}
var onFailed = function (data, jqXHR) {
    console.log(data);
    console.log(data.error);
    console.log(jqXHR);
    alert(data.error);
    alert(data.responseText);
    $.Toast.hideToast();
    $('#imgLOADING_Flow').hide();
};

$(function () {
    $("#pnlASSETNO").hide();
    if ($("#ReportClass").val() == "維修保養履歷")
        $("#pnlASSETNO").show();
    //$(".datefield").datepicker({
    //    format: "yyyy/mm/dd"
    //});
    //if ($.browser.msie || $.browser.mozilla) {
    //    $("#Sdate").datepicker({
    //        format: "yyyy/mm/dd",
    //        orientation: "bottom"
    //    });
    //    $("#Edate").datepicker({
    //        format: "yyyy/mm/dd"
    //    });
    //}
});