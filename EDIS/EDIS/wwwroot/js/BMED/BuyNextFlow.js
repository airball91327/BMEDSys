function flowmsg(data) {
    $("#btnGO").attr("disabled", false);
    if (!data.success) {
        $("#btnSEND").attr("disabled", false);
        alert(data.error);
    }
    else {
        alert("送出成功!");
        window.opener.location = "javascript:BuyEvaReSubmit();";
        window.close();
    }
}

var onFailed = function (data) {
    alert(data.responseText);
    $.Toast.hideToast();
};

$.fn.addItems = function (data) {

    return this.each(function () {
        var list = this;
        $.each(data, function (val, text) {

            var option = new Option(text.text, text.value);
            list.add(option);
        });
    });

};

$(function () {
    $('#SelectCls').change(function () {
        var select = $('#SelectEng');
        $('option', select).remove();
        if ($(this).val() === "設備工程師") {
            $('#SelectVendor').removeProp("disabled");
        }
        else {
            $('#SelectVendor').val('');
            $('#SelectVendor').prop("disabled", true);
        }
        $.ajax({
            url: '../../BuyFlow/GetNextEmp',
            type: "POST",
            async: true,
            dataType: "json",
            data: "cls=" + $(this).val() + "&docid=" + $('#DocId').val(),
            beforeSend: function () {
                $('#imgLOADING_flow').show();
            },
            success: function (data) {
                var select = $('#SelectEng');
                $('option', select).remove();
                select.addItems(data);
                $('#imgLOADING_flow').hide();
            }
        });
    });

    $('#IsAgree').change(function () {
        var c = $(this).is(':checked');
        var d = $('#DocId').val();
        $.ajax({
            url: '../../BuyEvaluate/UpdAgreeDate',
            type: "POST",
            data: { docid: d, isagree: c },
            success: function (data) {
                if (data.success) {
                    alert('更新完成');
                }
                else {
                    alert(data.error);
                }
            }
        });
    });

    $('#btnSTANDARD').click(function () {
        var d = $('#DocId').val();
        var s = $('#Standard').val();
        $.ajax({
            url: '../../BuyEvaluate/UpdStandard',
            type: "POST",
            data: { docid: d, standard: s },
            success: function (data) {
                alert(data);
            },
            error: function (data) {
                return data.reponseText;
            }
        });
    });

    $('#btnSEND').click(function (e) {
        var c = $('span[id="cls_now"]').text();
        if ($('#SelectCls').val() == '結案') {
            $.ajax({
                url: '../../BuyFlow/EndFlow',
                type: "POST",
                async: true,
                dataType: "json",
                data: "id=" + $('#DocId').val() + "&op=" + $('#Opinions').val(),
                success: function (data) {
                    $("#fmFLOW").submit();
                    //location.replace('../../Members/Index');
                }
            });
        }
        else if (c == "設備主管") {
            if ($("#IsAgree").is(':checked') == true) {

            }
            else {
                if (confirm("規格尚未確定，是否仍要送出?")) {

                }
                else {
                    return false;
                }
            }

        }
    });
});
