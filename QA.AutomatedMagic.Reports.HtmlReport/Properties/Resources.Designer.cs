﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QA.AutomatedMagic.Reports.HtmlReport.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("QA.AutomatedMagic.Reports.HtmlReport.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .container {
        ///    min-width: 90%;
        ///}
        ///.environment {
        ///    margin-top: 3%;
        ///}
        ///
        ///.table.overall {
        ///    margin: 0px;
        ///}
        ///
        ///
        ///.checkboxes.overall input{
        ///	margin-left: 0px;
        ///}
        ///.checkboxes.overall label{
        ///	width: 100px
        ///}
        ///
        ///.checkboxes.step-fltr-btns{
        ///	margin-left: 10px;
        ///	//display: block;
        ///}
        ///
        ///.checkboxes.step-fltr-btns input{
        ///	margin-left: 0px;
        ///}
        ///.checkboxes.step-fltr-btns label{
        ///	width: 100px
        ///}
        ///
        ///
        ///
        ///.test-fltr-btns button {
        ///    width: 70px;
        ///}
        ///
        ///.test-fltr-btns button.activated {
        ///    border-wi [rest of string was truncated]&quot;;.
        /// </summary>
        public static string css {
            get {
                return ResourceManager.GetString("css", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to function Filter(){
        ///    
        ///    this.defaultButton = null;
        ///    this.filterButtons = [];
        ///    this.parent = null;
        ///    this.childs = [];
        ///    
        ///    
        ///    this.multiSelect = true;
        ///    this.activatedClassName = &quot;activated&quot;;
        ///};
        ///
        ///Filter.prototype.isActive = function(button){
        ///    return button.classList.contains(this.activatedClassName);
        ///};
        ///
        ///Filter.prototype.activateButton = function (button) {
        ///    if (this.isActive(button)) return false;
        ///
        ///    button.classList.add(this.activatedClassName);
        ///    this.onB [rest of string was truncated]&quot;;.
        /// </summary>
        public static string filter {
            get {
                return ResourceManager.GetString("filter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///var a = 3;
        ///$(function () {
        ///   $(&quot;.log-slideup&quot;).click(function (e) {
        ///       
        ///       var $cur = $(e.currentTarget);
        ///       var $parent = null;
        ///
        ///        
        ///       $parent = $cur.closest(&quot;.step&quot;);
        ///       if ($parent.length == 0)
        ///           $parent = $cur.closest(&quot;.test&quot;);
        ///        
        ///       if ($parent.length == 0)
        ///           return;
        ///        $parent.find(&quot;.btnlog&quot;).first().click();
        ///
        ///       //var position = $parent.position();
        ///       //console.log( &quot;left: &quot; + position.left + &quot;, top: &quot; + position.t [rest of string was truncated]&quot;;.
        /// </summary>
        public static string init {
            get {
                return ResourceManager.GetString("init", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to $(function () {
        ///    $(&apos;.btnlog&apos;).click(function (e) {
        ///        var $parent = $(this).closest(&quot;.step&quot;);
        ///        if ($parent.length == 0) {
        ///            $parent = $(this).closest(&quot;.test&quot;);
        ///            if ($parent.length == 0)
        ///                return;
        ///        }
        ///
        ///        if ($(this).attr(&quot;magic&quot;) != &quot;true&quot;) {
        ///            var ind = $parent.find(&apos;.log-fltr-btns&apos;).first().attr(&quot;for&quot;);
        ///            arr[ind].prepare();
        ///
        ///            $(this).attr(&quot;magic&quot;, true);
        ///        }
        ///
        ///        $parent.find(&apos;.logPanel&apos;) [rest of string was truncated]&quot;;.
        /// </summary>
        public static string logFilter {
            get {
                return ResourceManager.GetString("logFilter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to $(function () {
        ///    $(&quot;.btnstep&quot;).click(function (e) {
        ///        var $parent = $(this).closest(&quot;.step&quot;);
        ///        if ($parent.length == 0) {
        ///            $parent = $(this).closest(&quot;.test&quot;);
        ///            if ($parent.length == 0)
        ///                return;
        ///        }
        ///
        ///        if ($(this).attr(&quot;magic&quot;) != &quot;true&quot;) {
        ///            var ind = $parent.find(&apos;.step-fltr-btns&apos;).first().attr(&quot;for&quot;);
        ///            arr[ind].prepare();
        ///
        ///            $(this).attr(&quot;magic&quot;, true);
        ///        }
        ///		
        ///        //$parent.children(&apos;. [rest of string was truncated]&quot;;.
        /// </summary>
        public static string stepFilter {
            get {
                return ResourceManager.GetString("stepFilter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to $(function () {
        ///	$(&quot;.btnexp&quot;).click(function (e) {
        ///		//$(this).closest(&quot;.test&quot;).children(&apos;.tests&apos;).toggle(500);
        ///        
        ///	    var $parent = $(this).closest(&quot;.test&quot;);
        ///
        ///	    if ($(this).attr(&quot;magic&quot;) != &quot;true&quot;) {
        ///	        var ind = $parent.find(&apos;.test-fltr-btns&apos;).first().attr(&quot;for&quot;);
        ///	        arr[ind].prepare();
        ///
        ///	        $(this).attr(&quot;magic&quot;, true);
        ///	    }
        ///
        ///	    $parent.children(&apos;.testContainer&apos;).toggle(500);
        ///	});
        ///	
        /////    var myFilter = new Filter();
        /////    
        /////
        /////    myFilter.getChilds = fu [rest of string was truncated]&quot;;.
        /// </summary>
        public static string testFilter {
            get {
                return ResourceManager.GetString("testFilter", resourceCulture);
            }
        }
    }
}
