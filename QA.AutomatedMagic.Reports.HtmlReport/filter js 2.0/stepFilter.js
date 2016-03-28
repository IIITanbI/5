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
		
        //$parent.children('.parentStepContainer').toggle(500);
		$parent.children('.stepContainer').toggle(500);
	});

	$(".btnparentstep").click(function (e) {
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
		
        $parent.children('.stepContainer').find('.parentStepContainer').first().toggle(500);
		//$parent.children('.stepContainer').toggle(500);
	});
	
  
	 
	var $btnsPanel = $(".step-fltr-btns");
	var arr = [];
	arr.length = $btnsPanel.length;
	for (var i = 0; i < $btnsPanel.length; i++) {
        var $cur = $($btnsPanel[i]);
        var ff = new Filter();
        
        ff.filterButtons = $cur.find("input");
        ff.defaultButton = ff.filterButtons[0];


        ff.parent = $cur.closest(".step");
        if (ff.parent.length == 0)
            ff.parent = $cur.closest(".test");
         
		var $ch = $cur.closest(".parentSteps");
		if ($ch.length == 0)
			$ch = $cur.closest(".steps");
		
        ff.childs = $ch.children(".step");


		ff.onButtonActivated = activated;
        ff.onButtonDeactivated = deactivated;
        ff.getChildStatus = getChildStatus;

        ff.init();

        $cur.attr("for", i);
        arr[i] = ff;
    }
    
    function activated(button){
        $(button).attr("checked"," true");
    };
    
    function deactivated(button){
        $(button).removeAttr("checked");
    };
    
    function getChildStatus(child) {
        var $needClass = "status";
        var $panelHeading = $(child).find('.panel-heading')[0];
        var $className = $($panelHeading).find('*[class*=' + $needClass + ']').attr('class');
        var $status = $className.substring($className.indexOf($needClass) + $needClass.length).toLowerCase();

        return $status;
    };


	$(".step-fltr-btn-exp").click(function (e) {
        var $cur = $(e.currentTarget).children("span");
        var $elem = $(this).closest(".stepHeader").find(".step-fltr-btns");
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
   
});
