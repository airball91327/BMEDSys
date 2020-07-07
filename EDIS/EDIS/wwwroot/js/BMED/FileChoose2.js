$(function () {
    var f = $('#pnlFILES');
   
    $('#AttainFileDialog').dialog({
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
                    var s = $('#FileDocId').val();
                    var t = $('#DocType').val();
                    $.ajax({
                        url: '../../AttainFile/List',
                        data: { docid: s, doctyp: t },
                        method: "POST",
                        dataType: "html", 
                        complete: function (data) {
                            f.html(data.responseText);
                            alert('上載檔案成功!');
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            f.html(XMLHttpRequest.readyState + XMLHttpRequest.status + XMLHttpRequest.responseText);
                            alert(XMLHttpRequest.readyState + XMLHttpRequest.status + XMLHttpRequest.responseText);
                        }
                    });
                   
                });
                $('#filepost').attr('target', 'result');
                $('#filepost').submit();

            }
            //'Cancel': function () {
            //    var s = $('#Docid').val();
            //    var t = $('#DocTyp').val();
            //    $('#FileList').html('')
            //.load(encodeURI('../../AttainFile/List?id=' + s + '&typ=' + t));
            //    $(this).dialog('close');
            //}
        }]

    });

    $('#FileChooseLink').click(function () {
        var createFormUrl = $(this).attr('href');
        $('#AttainFileDialog')
            .load(encodeURI(createFormUrl));
        $('#AttainFileDialog').dialog('open');
        return false;
    });
    $('#FileChooseLink1').click(function () {
        var createFormUrl = $(this).attr('href');
        $('#AttainFileDialog').html('')
        .load(encodeURI(createFormUrl));
        $('#AttainFileDialog').dialog('open');
        return false;
    });
    $('#FileChooseLink2').click(function () {
        var createFormUrl = $(this).attr('href');
        $('#AttainFileDialog').html('')
        .load(encodeURI(createFormUrl));
        $('#AttainFileDialog').dialog('open');
        return false;
    });
});