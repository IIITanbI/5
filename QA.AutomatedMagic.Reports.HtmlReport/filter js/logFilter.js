$(function () {
	$('.btnlog').click(function (e) {
        
        
        
		if ($(this).closest(".step").length > 0) {
            if ($(this).attr("magic") != "true") 
			     myFilter.prepare($(this).closest(".step").find('.log-fltr-btns').find("button").first());
			$(this).closest(".step").find('.logPanel').first().slideToggle();
		}
		else if ($(this).closest(".test").length > 0){
            if ($(this).attr("magic") != "true") 
			     myFilter.prepare($(this).closest(".test").find('.log-fltr-btns').find("button").first());
			$(this).closest(".test").find('.logPanel').first().slideToggle();
		}
        $(this).attr("magic", true);
	});
	
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
	
	myFilter.getParent = function (button){
		return $(button).closest(".logPanel");
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

	$(".img-btn-exp .glyphicon").click(function (e) {
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
	
    // var $btnsPanel = $(".log-fltr-btns");
    // for(var i = 0; i < $btnsPanel.length; i++){
        // myFilter.prepare($($btnsPanel[i]).find("button").first());
    // }

});
