var FILTER = {
    className: "",
	defaultButton: {},
	multiSelect: true,
    activatedClassName: "activated"
};

FILTER.prepare = function(button) {
    var $filterButtons = this.getFilterButtons(button);
    var $totalButton = this.getDefaultButton(button);

    this.deactivateButtons($filterButtons, $totalButton);
    this.activateButton($totalButton);
};

FILTER.filterButtonClick = function (button) {
    this.deactivateButton(button) || this.activateButton(button);

    var $filterButtons = this.getFilterButtons(button);
    var $totalButton = this.getDefaultButton(button);

    if (!this.multiSelect)  {
        if (!this.isActive(button)){
            this.activateButton($totalButton);
            this.deactivateButtons($filterButtons, $totalButton);
        }
        else {
            this.deactivateButtons($filterButtons, button);
        }
    }
    else {
        if (!$(button).is($totalButton)){
            this.deactivateButton($totalButton);
        }
        else {
            this.deactivateButtons($filterButtons, $totalButton);
        }
    }

    this.doFilter(button);
};

FILTER.activateButton = function (button) {
    if (this.isActive(button)) return false;

    var $button = $(button);
    $button.addClass(this.activatedClassName);
    this.onButtonActivated(button);
    return true;
};

FILTER.deactivateButton = function (button) {
    if (!this.isActive(button)) return false;

    var $button = $(button);
    $button.removeClass(this.activatedClassName);
    this.onButtonDeactivated(button);
    return true;
};

FILTER.deactivateButtons = function(buttons, excludeButton){
    var thisObj = this;
    $.each(buttons, function(i, value){
        if (!$(value).is(excludeButton))
            thisObj.deactivateButton(value);
    });
};

FILTER.onButtonActivated = function(button){
    return null;
};

FILTER.onButtonDeactivated = function(button){
    return null;
};


FILTER.getFilterFromButton = function (button) {
    var filter = [];

    var $filters = $(button).attr('filter');
    if ($filters != null)
        $filters = $filters.trim();

    filter = $filters.split(new RegExp("\\s+"));

	//for (var i = 0; i < matches.length; i++){
	//	filter.push(matches[i].substr(this.className.length));
	//}

    console.log("filter = " + filter);
    return filter;
};

FILTER.getChilds = function (button) { return $() };
FILTER.getFilterButtons = function (button) {return $() };
FILTER.getDefaultButton = function (button) {return $() };
FILTER.getChildStatus = function (child) { return $()};


FILTER.isActive = function(button){
    return($(button).hasClass(this.activatedClassName));
};

FILTER.doFilter = function (button) {
    var $filters = [];
    var $filterButtons = this.getFilterButtons(button);
	var $totalButton = this.getDefaultButton(button);
    var thisObj = this;

    $filterButtons.each(function (index, item) {
        if (thisObj.isActive(item)) {
            $.merge($filters, thisObj.getFilterFromButton(item));
        }
    });
    if ($filters.length === 0) {
        $totalButton.click();
        return;
    }

    var $childs = this.getChilds(button);
    for (var i = 0; i < $childs.length; i++) {
        var $child = $($childs[i]);
		var $status = this.getChildStatus($child);
		
		var css = $child.css("display");
        if ($.inArray($status, $filters) === -1) {
			if (css == "block"){
				$child.slideToggle();
			}
        } else {
			if (css == "none"){
				$child.slideToggle();
			}
        }
    }
};










