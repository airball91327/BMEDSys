$(function () {
    $("a[id='AssetFileChooseLink']").click(function () {
        var createFormUrl = $(this).attr('href');
        window.open(createFormUrl, "popup", "height=400;width=50");
        return false;
    });

    $("a[id='AssetFileCopyLink']").click(function () {
        var createFormUrl = $(this).attr('href');
        window.open(createFormUrl, "popup", "height=400;width=50");
        return false;
    });
})