$(function () {
    var myFilter = Object.create(FILTER);
    myFilter.className = "filter-";

    myFilter.getChilds = function (button) {
        return $(button).closest(".test").children('.tests').children();
    };
    myFilter.getFilterButtons = function (button) {
        return $(button).closest(".test-fltr-btns").find("button");
    };
    myFilter.getDefaultButton = function (button) {
        return $(button).closest(".test-fltr-btns").find("button").first();
    };
    myFilter.getChildStatus = function (child) {
        var $needClass = "status";
        var $panelHeading = $(child).find('.panel-heading')[0];
        var $className = $($panelHeading).children('p[class*=' + $needClass + ']').attr('class');
        var $status = $className.substring($className.indexOf($needClass) + $needClass.length).toLowerCase();

        return $status;
    };

    //myFilter.prepare($(".test-fltr-btns button").first());
    var $btnsPanel = $(".test-fltr-btns");
    for(var i = 0; i < $btnsPanel.length; i++){
        myFilter.prepare($($btnsPanel[i]).find("button").first());
    }

    $(".test-fltr-btns button").click(function (e) {
        myFilter.filterButtonClick(this);
    });



});

