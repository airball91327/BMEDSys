$(function () {
    var f = $('#pnlFILES');
    var typ = $('#doctype').val();
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
                    var msg = $('#result').html();
                    //alert(msg);
                    //
                    var s = $('#DocId').val();
                    var t = $('#DocType').val();
                    $.ajax({
                        url: '../AttainFile/List',
                        data: { docid: s, doctyp: t },
                        method: "POST",
                        dataType: "html", 
                        complete: function (data) {
                            f.html(data.responseText);
                            alert('上載檔案成功!');
                        }
                    });

                });
                
                $('#filepost').attr('target', 'result');
                $('#filepost').submit();

            }
        }]

    });

    $('#AttainFileDialog').on("dialogclose", function (event, ui) {
        if (typ === "0") {
            location.reload();
        }
    });

    $('#FileChooseLink').click(function () {
        var createFormUrl = $(this).attr('href');
        $('#AttainFileDialog').html('')
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