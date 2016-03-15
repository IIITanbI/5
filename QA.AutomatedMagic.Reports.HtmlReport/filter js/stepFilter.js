$(function () {
	$(".btnstep").click(function (e) {
		if ($(this).closest(".step").length > 0) {
			myFilter.prepare($(this).closest(".step").find('.step-fltr-btns').find("button").first());
			$(this).closest(".step").children('.steps').toggle(500);
		}
		else if ($(this).closest(".test").length > 0){
			myFilter.prepare($(this).closest(".test").find('.step-fltr-btns').find("button").first());
			$(this).closest(".test").children('.steps').toggle(500);
		}
	});
	
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
        var $className = $($panelHeading).find('*[class*=' + $needClass + ']').attr('class');
        var $status = $className.substring($className.indexOf($needClass) + $needClass.length).toLowerCase();

        return $status;
    };
	myFilter.getParent = function (button){
		if ($(this).closest(".step").length > 0) {
			return $(this).closest(".step").children('.steps');
		}
		else if ($(this).closest(".test").length > 0){
			return $(this).closest(".test").children('.steps');
		}
	};
	
	$(".step-fltr-btns button").click(function (e) {
        myFilter.filterButtonClick(this);
    });

	// var $btnsPanel = $(".step-fltr-btns");
    // for(var i = 0; i < $btnsPanel.length; i++){
        // myFilter.prepare($($btnsPanel[i]).find("button").first());
    // }

   
});
