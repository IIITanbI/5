$(function () {
    $('.btnlog').click(function (e) {
        var $parent = $(this).closest(".step");
        if ($parent.length == 0) {
            $parent = $(this).closest(".test");
            if ($parent.length == 0)
                return;
        }

        if ($(this).attr("magic") != "true") {
            var ind = $parent.find('.log-fltr-btns').first().attr("for");
            arr[ind].prepare();

            $(this).attr("magic", true);
        }

        $parent.find('.logPanel').first().slideToggle();
	});
	

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

    
	var $btnsPanel = $(".log-fltr-btns");
	var arr = [];
	arr.length = $btnsPanel.length;
    for(var i = 0; i < $btnsPanel.length; i++){
         var $cur = $($btnsPanel[i]);
         var ff = new Filter();

         ff.multiSelect = false;
        
         ff.filterButtons = $cur.find("button");
         ff.defaultButton = ff.filterButtons[3];

         ff.parent = $cur.closest(".step");
         if (ff.parent.length == 0)
             ff.parent = $cur.closest(".test");
        
         ff.childs = ff.parent.find('.logs').first().children();

         ff.getChildStatus = getChildStatus;
            
         ff.init();

         $btnsPanel[i].setAttribute("for", i);
         arr[i] = ff;
    }
    function getChildStatus(child) {
        var $status = $($(child).find("span")[0]).text().toLowerCase();
        return $status;
    };
});
