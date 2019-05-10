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

    SetEngsDropDown();

    $('#AssetNo').change(function () {
        /* Get engineers. */
        $.ajax({
            url: '../BMED/Repair/GetAssetEngId',
            type: "POST",
            dataType: "json",
            data: {
                AssetNo: $('#AssetNo').val()
            },
            async: false,
            success: function (data) {
                $('#EngId').val(data.engId);
                $('#EngName').val(data.fullName);
                //var select = $('#EngId');
                //$('option', select).remove();
                //select.addItems(data);
                //console.log(data + ";" + select.val()); // ForDebug
            }
        });
    });

    /* While user change DptId, search the DptName. */
    $("#DptId").change(function () {
        var DptId = $(this).val();
        $.ajax({
            url: '../BMED/Repair/GetDptName',
            type: "POST",
            dataType: "json",
            data: { dptId: DptId },
            success: function (data) {
                //console.log(data);
                if (data == "") {
                    $("#DptNameErrorMsg").html("查無部門!");
                }
                else {
                    $("#DptNameErrorMsg").html("");
                }
                $("#DptName").val(data);
            }
        });       
    });
    /* While user change AccDpt, search the AccDptName. */
    $("#AccDpt").change(function () {
        var AccDptId = $(this).val();
        $.ajax({
            url: '../Repair/GetDptName',
            type: "POST",
            dataType: "json",
            data: { dptId: AccDptId },
            success: function (data) {
                //console.log(data);
                if (data == "") {
                    $("#AccDptNameErrorMsg").html("查無部門!");
                }
                else {
                    $("#AccDptNameErrorMsg").html("");
                }
                $("#AccDptName").val(data);  
            }
        });    
    });

    /* If user select "增設", show Mgr dropdown for user to select. */
    $("#DptMgr").hide();    //Default setting.
    $("input[type=radio][name=RepType]").change(function () {
        if (this.value == '增設') {
            $("#DptMgr").show();
        }
        else {
            $("#DptMgr").hide();
        }
    });

    /* Get managers by query string. */
    $("#MgrQryBtn").click(function () {
        var queryStr = $("#DptMgrQry").val();
        $.ajax({
            url: '../BMED/Repair/QueryUsers',
            type: "GET",
            data: { QueryStr: queryStr },
            success: function (data) {
                var select = $('#DptMgrId');
                $('option', select).remove();
                select.addItems(data);
            }
        });
    });
    $("#CheckerQryBtn").click(function () {
        var queryStr = $("#CheckerQry").val();
        $.ajax({
            url: '../BMED/Repair/QueryUsers',
            type: "GET",
            data: { QueryStr: queryStr },
            success: function (data) {
                var select = $('#CheckerId');
                $('option', select).remove();
                select.addItems(data);
            }
        });
    });
    $('#modalFILES').on('hidden.bs.modal', function () {
        var docid = $("#DocId").val();
        $.ajax({
            url: '../AttainFile/List3',
            type: "POST",
            data: { docid: docid, doctyp: "1" },
            success: function (data) {
                $("#pnlFILES").html(data);
            }
        });
    });
});

function onSuccess() {
    $.Toast.hideToast();
    alert("已送出");

    var DocId = $("#DocId").val();
    var repType = $('input:radio[name="RepType"]:checked').val();
    /* If repair type is 送修, print before submit. */
    //console.log(repType);
    if (repType == "送修") {
        window.printRepairDoc(DocId);
    }

    location.href = '../Home/Index';
}

function getAssetName() {
    var AssetNo = $("#AssetNo").val();
    $.ajax({
        url: '../Repair/GetAssetName',
        type: "POST",
        dataType: "json",
        data: { assetNo: AssetNo },
        success: function (data) {
            //console.log(data); // debug
            if (data == "") {
                $("#AssetNameErrorMsg").html("查無資料!");
            }
            else {
                $("#AssetNameErrorMsg").html("");
            }
            $("#AssetName").val(data.cname);
            $("#assetAccDate").val(data.accDate);
        }
    });  
}


function printRepairDoc(DocId) {
    
    var printContent = "";
    /* Get print page. */
    $.ajax({
        url: '../Repair/PrintRepairDoc',
        type: "GET",
        async: false,
        data: { docId: DocId, printType : 1 },
        success: function (data) {
            printContent = data;
        }
    });
    $.ajaxSettings.async = true; // Set this ajax async back to true.
    /* New a window to print. */
    var printPage = window.open();
    printPage.document.open();
    printPage.document.write("<BODY onload='window.print();window.close()'>");
    printPage.document.write(printContent);
    printPage.document.close();
}

/* Get and check the user department's location, if has many location, show #divLocations. */
function GetDptLocation(DptId) { 
    $.ajax({
        url: '../Repair/GetDptLoc',
        type: "POST",
        dataType: "json",
        data: { dptId: DptId },
        success: function (data) {
            //console.log(data); //Debug
            if (data == "查無地點") {
                $("#divLocations").hide();
                alert("查無部門資料!");
            }
            else if (data == "多個地點") {
                $("#divLocations").show();
            }
            else {          
                $("#Building").val(data.buildingId);
                $('#Building').trigger("change");
                $("#Floor").val(data.floorId);
                $('#Floor').trigger("change");
                $("#Area").val(data.placeId);
                $('#Area').trigger("change");
       
                $("#divLocations").hide();
            }
        }
    });
}

function SetEngsDropDown() {
    $.ajax({
        url: '../Repair/GetAllEngs',
        type: "GET",
        dataType: "json",
        data: { },
        success: function (data) {
            //console.log(data); // For debug.
            var select = $('#PrimaryEngId');
            var i = 0;
            var defaultOption = 0;
            var displayTrigger = 0;
            select.empty();
            $.each(data, function (index, item) {  // item is now an object containing properties 
                if (i === defaultOption) {
                    select.append($('<option selected="selected"></option>').text("無").val(0));
                }
                else {
                    if (item.dptId != displayTrigger) {
                        switch (item.dptId) {
                            case '8411':
                                select.append($('<optgroup label="工務一課"></optgroup>'));
                                break;
                            case '8412':
                                select.append($('<optgroup label="工務二課"></optgroup>'));
                                break;
                            case '8413':
                                select.append($('<optgroup label="工務三課-中華院區工務組"></optgroup>'));
                                break;
                            case '8414':
                                select.append($('<optgroup label="工務三課-教研工務組"></optgroup>'));
                                break;
                            case '8430':
                                select.append($('<optgroup label="營建部"></optgroup>'));
                                break;
                        }
                        displayTrigger = item.dptId;
                    }
                    select.append($('<option></option>').text(item.fullName).val(item.id));
                }
                i++;
            });
        }
    });
}