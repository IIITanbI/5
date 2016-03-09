$(function () {
    var myFilter = Object.create(FILTER);
    myFilter.className = "log-filter-";
    myFilter.multiSelect = false;

    myFilter.getChilds = function (button) {
        return $(button).closest(".logPanel").find('.logs').children();
    };

    myFilter.getFilterButtons = function (button) {
        return $(button).closest(".log-fltr-btns").children();
    };

    myFilter.getDefaultButton = function (button) {
        return $(button).closest(".log-fltr-btns").find('.log-fltr-btn-inf');
    };

    myFilter.getChildStatus = function (child) {
        var $status = $($(child).find("span")[0]).text().toLowerCase();
        return $status;
    };


    $(".log-fltr-btn-exp").click(function (e) {
        var $cur = $(e.currentTarget).children("span");
        var $elem = $(this).closest(".logHeader").find(".log-fltr-btns");
        $elem.toggle(300, function onCompleteToggle() {
            if ($elem.is(":visible")) {
                $cur.attr("class", "glyphicon glyphicon-chevron-left");
                console.log("visible");
            } else {
                $cur.attr("class", "glyphicon glyphicon-chevron-right");
                console.log("none");
            }
        });
    });

	$(".img-btn-exp").click(function (e) {
        var $cur = $(e.currentTarget).children("span").last();
        var $elem = $(this).closest(".log").find(".image");
        $elem.toggle(300, function onCompleteToggle() {
            if ($elem.is(":visible")) {
                $cur.attr("class", "glyphicon glyphicon-triangle-top");
                console.log("visible");
            } else {
                $cur.attr("class", "glyphicon glyphicon-triangle-bottom");
                console.log("none");
            }
        });
    });


	$(".log-fltr-btns button").click(function (e) {
        myFilter.filterButtonClick(this);
    });
    var $btnsPanel = $(".log-fltr-btns");
    for(var i = 0; i < $btnsPanel.length; i++){
        myFilter.prepare($($btnsPanel[i]).find("button").first());
    }

    
});
