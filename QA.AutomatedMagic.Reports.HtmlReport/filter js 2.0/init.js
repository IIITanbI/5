
var a = 3;
$(function () {
   $(".log-slideup").click(function (e) {
       
       var $cur = $(e.currentTarget);
       var $parent = null;

        
       $parent = $cur.closest(".step");
       if ($parent.length == 0)
           $parent = $cur.closest(".test");
        
       if ($parent.length == 0)
           return;
        $parent.find(".btnlog").first().click();

       //var position = $parent.position();
       //console.log( "left: " + position.left + ", top: " + position.top );
       
       //window.scrollTo(position.left, position.top);
    });   
    
     $(".test-slideup").click(function (e) {
       
       var $cur = $(e.currentTarget);
       var $parent = null;

       if ($cur.closest(".test").length > 0){
			$parent = $cur.closest(".test");
            $parent.find(".btnexp").first().click();
       }
       else return;
       
       
       
       //var position = $parent.position();
       //console.log( "left: " + position.left + ", top: " + position.top );
       
       //window.scrollTo(position.left, position.top);
    });   
    
    $(".step-slideup").click(function (e) {
       
       var $cur = $(e.currentTarget);
       var $parent = null;


       if ($cur.closest(".step").length > 0){
           $parent = $cur.closest(".step");
       }
       else if ($cur.closest(".test").length > 0){
		   $parent = $cur.closest(".test");
       }
       else return;
       
	   $parent.find(".btnstep").first().click();
       
       
       //var position = $parent.position();
      // console.log( "left: " + position.left + ", top: " + position.top );
       
       //window.scrollTo(position.left, position.top);
    });   
    
	$(".parentstep-slideup").click(function (e) {
       
       var $cur = $(e.currentTarget);
       var $parent = null;


       if ($cur.closest(".step").length > 0){
           $parent = $cur.closest(".step");
       }
       else if ($cur.closest(".test").length > 0){
		   $parent = $cur.closest(".test");
       }
       else return;
       
	   $parent.find(".btnparentstep").first().click();
    });   
});
