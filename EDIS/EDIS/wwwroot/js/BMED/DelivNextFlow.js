function flowmsg(data) {
    $("#btnGO").attr("disabled", false);
    if (!data.success) {
        $("#btnSEND").attr("disabled", false);
        alert(data.error);
    }
    else {
        alert("送出成功!");
        window.opener.location = "javascript:BMEDDelivReSubmit();";
        window.close();
    }
}

function presend() {
    document.getElementById('btnSEND').disabled = true;
}

$.fn.addItems = function (data) {

    return this.each(function () {
        var list = this;
        $.each(data, function (val, text) {
            var option = new Option(text.text, text.value);
            list.add(option);
        });
    });
};
$.fn.frmsend = function () {
    var s = $('#WartyDataForm').serialize();
    $.ajax({
        url: '../../Delivery/EditData',
        type: "POST",
        async: true,
        data: s,
        error: function () {
            return '驗收明細儲存錯誤!';
        }
    });
    return '';
};
$(function () {
    $('#imgLOADING').hide();
    var c = $('span[id="cls_now"]').text();
    if (c === "得標廠商") {
        $('#pnlASSET').html('');
        $('#pnlASSETVIEW').html('');
        $('#pnlMEDMGR').html('');
        $('#pnlMEDENG').html('');
        $('#pnlTODO').html('');
    }
    else if (c === "設備主管") {
        $('#pnlASSETVIEW').html('');
        $('#pnlASSET').html('');
        $('#pnlVENDOR').html('');
        $('#pnlMEDENG').html('');
    }
    else if (c === "設備工程師") {
        $('#pnlASSET').html('');
        $('#pnlASSETVIEW').html('');
        $('#pnlVENDOR').html('');
        $('#pnlMEDMGR').html('');
        $('#pnlTODO').html('');
    }
    else if (c === "設備經辦") {
        $('#pnlASSETVIEW').html('');
        $('#pnlVENDOR').html('');
        $('#pnlMEDMGR').html('');
        $('#pnlMEDENG').html('');
    }
    else {
        $('#pnlASSET').html('');
        $('#pnlVENDOR').html('');
        $('#pnlMEDMGR').html('');
        $('#pnlMEDENG').html('');
        $('#pnlTODO').html('');
    }

    $('#SelectCls').change(function () {
        var select = $('#SelectEng');
        $('option', select).remove();
        if ($(this).val() === "維修工程師") {
            $('#SelectVendor').removeProp("disabled");
        }
        if ($(this).val() === "結案" || $(this).val() === "廢除") {
            var appenddata;
            appenddata += "<option value = '0' selected=true></option>";
            select.html(appenddata);
        }
        else {
            $('#imgLOADING_flow').show();
            $('#SelectVendor').val('');
            $('#SelectVendor').prop("disabled", true);
            $.ajax({
                url: '../../DelivFlow/GetNextEmp',
                type: "POST",
                async: true,
                dataType: "json",
                data: "cls=" + $(this).val() + "&docid=" + $('#DocId').val(),
                success: function (data) {
                    var select = $('#SelectEng');
                    $('option', select).remove();
                    select.addItems(data);
                    $('#imgLOADING_flow').hide();
                }
            });
        }
    });
    $('#btnSEND').click(function () {
        if ($('#SelectCls').val() === '結案') {
            var s = $('#WartyDataForm').serialize();
            $.ajax({
                url: '../../Delivery/EditData',
                type: "POST",
                async: true,
                data: s,
                success: function () {
                    $("#fmFLOW").submit();
                    //$.ajax({
                    //    url: '../../DelivFlow/EndFlow',
                    //    type: "POST",
                    //    async: true,
                    //    dataType: "json",
                    //    data: "id=" + $('#Docid').val() + "&op=" + $('#Opinions').val(),
                    //    success: function (data) {
                    //        location.replace('../../Home/Index');
                    //    }
                    //});
                },
                error: function () {
                    alert('驗收明細儲存錯誤!');
                    return false;
                }
            });
            return false;
        }
        else {
            var c = $('span[id="cls_now"]').text();
            if (c === "得標廠商" || c === "設備工程師") {
                var msg = "";
                $(".fstatus").html(function (index, value) {
                    msg += value;
                });
                if (msg.trim() !== '') {
                    alert(msg);
                    return false;
                }
            }
            if (c === "設備主管" || c === "設備經辦" || c === "設備工程師") {
                s = $('#WartyDataForm').serialize();
                $.ajax({
                    url: '../../Delivery/EditData',
                    type: "POST",
                    async: true,
                    data: s,
                    success: function () {
                        $("#fmFLOW").submit();
                    },
                    error: function () {
                        alert('驗收明細儲存錯誤!');
                        return false;
                    }
                });
                return false;
            }
        }
    });
});
