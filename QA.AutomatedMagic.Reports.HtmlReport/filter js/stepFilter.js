$(function () {
    var myFilter = Object.create(FILTER);
    myFilter.className = "step-filter-";
	
    myFilter.getChilds = function (button) {
        return $(button).closest('.steps').children(".step");
    };
    myFilter.getFilterButtons = function (button) {
		return $(button).closest(".step-fltr-btns").children();
    };
    myFilter.getDefaultButton = function (button) {
		return $(button).closest(".step-fltr-btns").children().first();
    };
    myFilter.getChildStatus = function (child) {
        var $needClass = "status";
        var $panelHeading = $(child).find('.panel-heading')[0];
        var $className = $($panelHeading).children('p[class*=' + $needClass + ']').attr('class');
        var $status = $className.substring($className.indexOf($needClass) + $needClass.length).toLowerCase();

        return $status;
    };

    //myFilter.prepare($(".step-fltr-btns button").first());
    var $btnsPanel = $(".step-fltr-btns");
    for(var i = 0; i < $btnsPanel.length; i++){
        myFilter.prepare($($btnsPanel[i]).find("button").first());
    }



    $(".step-fltr-btns button").click(function (e) {
        myFilter.filterButtonClick(this);
    });
});
