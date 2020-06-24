$(function () {
    $('#AttainFileDialog').dialog({
        autoOpen: false,
        width: 750,
        height: 450,
        modal: true,
        title: '檔案選擇器',
        buttons: [{
            text: "確定",
            click: function () {
                if ($("#Title").val() == '') {
                    alert('摘要不可空白!!');
                    return false;
                }
                $('#result').load(function () {
                    $('#result').off('load');
                    var s = $('#Docid').val();
                    var t = $('#DocTyp').val();
                    var u = $('#Rtp').val();
                    $('#FileList'+s).html('')
                .load(encodeURI('../AttainFile/VenderList?id=' + s + '&typ=' + t + '&uniteno=' + u));
                    alert('檔案傳送成功!');
                });
                $('#filepost').attr('target', 'result');
                $('#filepost').submit();

            }
            
        }]

    });
    $("a[id='FileChooseLink']").click(function () {
        var createFormUrl = $(this).attr('href');
        $('#AttainFileDialog').html('')
        .load(encodeURI(createFormUrl));
        $('#AttainFileDialog').dialog('open');
        return false;
    });
});