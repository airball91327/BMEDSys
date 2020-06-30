$(function () {
    $('#AjaxLoading').hide();
    $('#VendorChooseLink').click(function () {
        $('#AjaxLoading').show();
        var createFormUrl = $(this).attr('href');
        var v = $('#txtVENDOR').val();
        if (v == '')
        {
            $('#AjaxLoading').hide();
            alert('關鍵字或統一編號不可空白!!');
            return false;
        }
        if (v != null) {
            if ($('input:radio[name="rbVENDOR"]:checked').val() == "0")
                createFormUrl = createFormUrl + "?KeyWord=" + v;
            else
                createFormUrl = createFormUrl + "?UniteNo=" + v;
        }
        $('#Vendors').html('')
        .load(encodeURI(createFormUrl), function () {
            $('#SelectVendor').change(function () {
                var txt2 = $(this).val();
                $("#VendorNo").val(txt2);
                $("#VendorNo").trigger("change");
                $("#VendorId").val(txt2);
                $("#UniteNo").val(txt2);
                txt2 = $(this).find(":selected").text();
                $("#VendorNam").val(txt2);
            });
        });
        return false;
    });
    $('#VendorNo').change(function () {
        $.fn.addItems = function (data) {

            return this.each(function () {
                var list = this;
                list.add(new Option("請選擇", ""));
                $.each(data, function (val, text) {

                    var option = new Option(text.Text, text.Value);
                    list.add(option);
                });
            });
        };
        var select = $('#SelDelivPson');
        $('option', select).remove();
        $.ajax({
            url: '../../Vendor/IsInVendor',
            type: "POST",
            async: true,
            dataType: "text",
            data: "uniteno=" + $(this).val(), 
            success: function (data) {
                if (data != "") {
                    var s = "<a href='" + encodeURI("../../Vendor/New?uniteno=" + $("#VendorNo").val() + "&vname=" + $("#VendorNam").val()) +
                        "' target='blank'>建立廠商</a>";
                    $('#msg').html(data + s);
                    $('#btnSEND').prop("disabled", true);
                }
                else {
                    $('#msg').html('');
                    $('#btnSEND').removeProp("disabled");
                }
            }
        });
        $.ajax({
            url: '../../Vendor/GetMembers',
            type: "POST",
            async: true,
            dataType: "json",
            data: "uniteno=" + $(this).val(),
            success: function (data) {
                select.addItems(data);
            }
        });
    });
});