$(function () {
	$(".btnexp").click(function (e) {
		//$(this).closest(".test").children('.tests').toggle(500);
        
	    var $parent = $(this).closest(".test");

	    if ($(this).attr("magic") != "true") {
	        var ind = $parent.find('.test-fltr-btns').first().attr("for");
	        arr[ind].prepare();

	        $(this).attr("magic", true);
	    }

	    $parent.children('.testContainer').toggle(500);
	});
    $(".test-fltr-btns .checkbox[defaultexpander='true'] label").click(function(e){
        var elems = $(e.currentTarget).closest(".test-fltr-btns").children(".checkbox[defaultexpander!='true']");
        
        elems.slideToggle();
    });

	var $btnsPanel = $(".test-fltr-btns");
	var arr = [];
	arr.length = $btnsPanel.length;
     for(var i = 0; i < $btnsPanel.length; i++){
         var $cur = $($btnsPanel[i]);
         var ff = new Filter();
         
         ff.filterButtons = $cur.find("input");
         ff.defaultButton = ff.filterButtons[0];
         
         
         ff.parent = $cur.closest(".test");
         ff.childs = ff.parent.find('.tests').first().children();
         
         ff.onButtonActivated = activated;
         ff.onButtonDeactivated = deactivated;
         ff.getChildStatus = getChildStatus;
         
         ff.init();
         
         $btnsPanel[i].setAttribute("for", i);

         arr[i] = ff;
     }
    
    function activated(button){
        $(button).attr("checked"," true");
    };
    
    function deactivated(button){
        $(button).removeAttr("checked");
    };
    
    function getChildStatus (child) {
        var $needClass = "status";
        var $panelHeading = $(child).find('.panel-heading')[0];
        var $className = $($panelHeading).find('*[class*=' + $needClass + ']').attr('class');
        var $status = $className.substring($className.indexOf($needClass) + $needClass.length).toLowerCase();

        return $status;
    };
});

