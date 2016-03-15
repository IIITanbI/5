$(function () {
	$(".btnexp").click(function (e) {
		$(this).closest(".test").children('.tests').toggle(500);
		myFilter.prepare($(this).closest(".test").find('.test-fltr-btns').find("button").first());
	});
	
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
        var $className = $($panelHeading).find('*[class*=' + $needClass + ']').attr('class');
        var $status = $className.substring($className.indexOf($needClass) + $needClass.length).toLowerCase();

        return $status;
    };
	
	
	
	$(".test-fltr-btns button").click(function (e) {
        myFilter.filterButtonClick(this);
    });

    // var $btnsPanel = $(".test-fltr-btns");
    // for(var i = 0; i < $btnsPanel.length; i++){
        // myFilter.prepare($($btnsPanel[i]).find("button").first());
    // }
});

