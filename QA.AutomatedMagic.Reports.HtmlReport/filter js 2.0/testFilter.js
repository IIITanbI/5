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
	
//    var myFilter = new Filter();
//    
//
//    myFilter.getChilds = function (button) {
//        return $(button).closest(".test").find('.testTD').first().children('.tests').children();
//        return $(button).closest(".test").children('.tests').children();
//    };
//    myFilter.getFilterButtons = function (button) {
//        return $(button).closest(".test-fltr-btns").find("input");
//    };
//    myFilter.getDefaultButton = function (button) {
//        return $(button).closest(".test-fltr-btns").find("input").first();
//    };
//    myFilter.getChildStatus = function (child) {
//        var $needClass = "status";
//        var $panelHeading = $(child).find('.panel-heading')[0];
//        var $className = $($panelHeading).find('*[class*=' + $needClass + ']').attr('class');
//        var $status = $className.substring($className.indexOf($needClass) + $needClass.length).toLowerCase();
//
//        return $status;
//    };
//	
//	myFilter.getParent = function (button){
//		return $(button).closest(".test");
//	};
	
//    myFilter.onButtonActivated = function(button){
//        $(button).attr("checked"," true");
//    };
//    myFilter.onButtonDeactivated = function(button){
//        $(button).removeAttr("checked");
//    };
    
//	$(".test-fltr-btns input").click(function (e) {
//        myFilter.filterButtonClick(this);
//    });

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

         $cur.attr("for", i);
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

