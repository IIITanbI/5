$(function () {
    $(".btnstep").click(function (e) {
        var $parent = $(this).closest(".step");
        if ($parent.length == 0) {
            $parent = $(this).closest(".test");
            if ($parent.length == 0)
                return;
        }

        if ($(this).attr("magic") != "true") {
            var ind = $parent.find('.step-fltr-btns').first().attr("for");
            arr[ind].prepare();

            $(this).attr("magic", true);
        }
		
        $parent.children('.stepContainer').toggle(500);
	});
	
//    var myFilter = Object.create(FILTER);
//    myFilter.className = "step-filter-";
//	
//    myFilter.getChilds = function (button) {
//        return $(button).closest('.steps').children(".step");
//    };
//    myFilter.getFilterButtons = function (button) {
//		return $(button).closest(".step-fltr-btns").children();
//    };
//    myFilter.getDefaultButton = function (button) {
//		return $(button).closest(".step-fltr-btns").children().first();
//    };
//    myFilter.getChildStatus = function (child) {
//        var $needClass = "status";
//        var $panelHeading = $(child).find('.panel-heading')[0];
//        var $className = $($panelHeading).find('*[class*=' + $needClass + ']').attr('class');
//        var $status = $className.substring($className.indexOf($needClass) + $needClass.length).toLowerCase();
//
//        return $status;
//    };
//	myFilter.getParent = function (button){
//		if ($(button).closest(".step").length > 0) {
//			return $(button).closest(".step").children('.steps');
//		}
//		else if ($(button).closest(".test").length > 0){
//			return $(button).closest(".test").children('.steps');
//		}
//	};
//	
//	$(".step-fltr-btns button").click(function (e) {
//        myFilter.filterButtonClick(this);
//    });

	// var $btnsPanel = $(".step-fltr-btns");
    // for(var i = 0; i < $btnsPanel.length; i++){
        // myFilter.prepare($($btnsPanel[i]).find("button").first());
    // }
    
	 
	var $btnsPanel = $(".step-fltr-btns");
	var arr = [];
	arr.length = $btnsPanel.length;
	for (var i = 0; i < $btnsPanel.length; i++) {
        var $cur = $($btnsPanel[i]);
        var ff = new Filter();
        
        ff.filterButtons = $cur.find("button");
        ff.defaultButton = ff.filterButtons[0];


        ff.parent = $cur.closest(".step");
        if (ff.parent.length == 0)
            ff.parent = $cur.closest(".test");
         
        ff.childs = $cur.closest(".steps").children(".step");

        ff.getChildStatus = getChildStatus;

        ff.init();

        $cur.attr("for", i);
        arr[i] = ff;
    }
    
    
    
    function getChildStatus(child) {
        var $needClass = "status";
        var $panelHeading = $(child).find('.panel-heading')[0];
        var $className = $($panelHeading).find('*[class*=' + $needClass + ']').attr('class');
        var $status = $className.substring($className.indexOf($needClass) + $needClass.length).toLowerCase();

        return $status;
    };

   
});
