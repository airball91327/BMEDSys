$(function () {
    $('#AssetFileDialog').dialog({
        autoOpen: false,
        width: 750,
        height: 450,
        modal: true,
        title: '檔案選擇器',
        buttons: [{
            text: "確定",
            click: function () {
                
                $('#result').load(function () {
                    $('#result').off('load');
                    var s = $('#PurchaseNo').val();
                    $('#gvASSETLIST').html('')
                .load(encodeURI('../../Asset/BuyEvaluateListItem?id=' + s + '&upload=true'));
                    alert('檔案傳送成功!');
                    $('#btnQTY').click();
                });
                $('#filepost').attr('target', 'result');
                $('#filepost').submit();
            }
            
        }]

    });
    $("a[id='AssetFileChooseLink']").click(function () {
        var createFormUrl = $(this).attr('href');
        window.open(createFormUrl,"popup","height=400;width=50");
        $('#AssetFileDialog').html('')
        .load(encodeURI(createFormUrl));
        $('#AssetFileDialog').dialog('open');
        return false;
    });
});