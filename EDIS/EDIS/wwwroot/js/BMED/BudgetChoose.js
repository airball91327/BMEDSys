$(function () {
    $('#BudgetChooseDialog').dialog({
        autoOpen: false,
        width: 750,
        height: 450,
        modal: true,
        title: '預算案選擇器',
        buttons: {
            '確定': function () {
                var $obj = $('input:radio[name="rblSELECT"]:checked').parents('tr').children();
                $("#BudgetId").val($obj.get(0).innerText);
                $("#PlantCnam").val($obj.get(1).innerText);
                $("#AccDpt").val($obj.get(2).innerText);
                $("#AccDptNam").val($obj.get(3).innerText);
                $("#Place").val($obj.get(3).innerText);
                $("#Amt").val($obj.get(4).innerText);
                $("#Price").val($obj.get(5).innerText);
                $("#TotalPrice").val($obj.get(6).innerText);
                $("#EngId option").each(function () {
                    if ($(this).text() == $.trim($obj.get(7).innerText))
                    {
                        $(this).attr("selected","selected");
                    }
                });
                $(this).dialog('close');
            },
            'Cancel': function () {
                $(this).dialog('close');
            }
        }
    });

    $('#BudgetChooseLink').click(function () {
        var createFormUrl = $(this).attr('href');
        $('#BudgetChooseDialog').html('')
            .load(encodeURI(createFormUrl));
        $('#BudgetChooseDialog').dialog("option", "width", 900);
        $('#BudgetChooseDialog').dialog('open');
        return false;
    });
});