function Filter(){
    
    this.defaultButton = null;
    this.filterButtons = [];
    this.parent = null;
    this.childs = [];
    
    
    this.multiSelect = true;
    this.activatedClassName = "activated";
};

Filter.prototype.isActive = function(button){
    return button.classList.contains(this.activatedClassName);
};

Filter.prototype.activateButton = function (button) {
    if (this.isActive(button)) return false;

    button.classList.add(this.activatedClassName);
    this.onButtonActivated(button);
    
    return true;
};

Filter.prototype.deactivateButton = function (button) {
    if (!this.isActive(button)) return false;

    button.classList.remove(this.activatedClassName);
    this.onButtonDeactivated(button);
    
    return true;
};

Filter.prototype.deactivateButtons = function(buttons, excludeButton){
    for(var i = 0; i < buttons.length; i++){
        if (buttons[i] !== excludeButton)
            this.deactivateButton(buttons[i]);
    }
};


Filter.prototype.init = function () {
    //a += this.filterButtons.length + this.childs.length + 2;
    //console.log(a);
    var thisObj = this;
    this.filterButtons.click(function (e) {
        thisObj.filterButtonClick(e.currentTarget);
    });
};

Filter.prototype.prepare = function(button) {
    this.defaultButton.click();
};

Filter.prototype.filterButtonClick = function (button) {
    this.deactivateButton(button) || this.activateButton(button);

    if (!this.multiSelect)  {
        if (!this.isActive(button)){
            this.activateButton(this.defaultButton);
            this.deactivateButtons(this.filterButtons, this.defaultButton);
        }
        else {
            this.deactivateButtons(this.filterButtons, button);
        }
    }
    else {
        if (button !== this.defaultButton){
            this.deactivateButton(this.defaultButton);
        }
        else {
            this.deactivateButtons(this.filterButtons, this.defaultButton);
        }
    }

    this.doFilter(button);
};

Filter.prototype.doFilter = function (button) {
    var filters = [];
    var $filterButtons = this.filterButtons;
    var thisObj = this;

    for(var i = 0; i < this.filterButtons.length; i++){
        if (this.isActive(this.filterButtons[i])){
            $.merge(filters, thisObj.getFilterFromButton(this.filterButtons[i]));
        }
    }
    if (filters.length === 0) {
        this.defaultButton.click();
        return;
    }

    var childs = this.childs;
	var $parent = this.parent;
    
    for (var i = 0; i < childs.length; i++) {
        var $child = $(childs[i]);
		var status = this.getChildStatus($child);
		
		var css = childs[i].style.display;
        if ($.inArray(status, filters) === -1) {
			if (css == "block" || css == ""){
			    if (!$parent.is(":visible"))
			        childs[i].style.display = "none";
			    else
			        $child.slideToggle();
			}
        } else {
            if (css == "none") {
			    if (!$parent.is(":visible")) 
			        childs[i].style.display = "block";
			    else
			        $child.slideToggle();
			}
        }
    }
};

Filter.prototype.getFilterFromButton = function (button) {
    var filter = [];

    var filters = button.getAttribute('filter');
    if (filters) {
       filters = filters.trim();
       filter = filters.split(new RegExp("\\s+"));
    }
       

    console.log("filter = " + filter);
    return filter;
};

Filter.prototype.onButtonActivated = function(button){};

Filter.prototype.onButtonDeactivated = function(button){};

Filter.prototype.getChildStatus = function (child) {};