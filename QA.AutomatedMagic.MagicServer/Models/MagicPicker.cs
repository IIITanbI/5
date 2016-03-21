namespace QA.AutomatedMagic.MagicServer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    
    public class MagicPicker
    {
        public string SolutionName { get; set; }
        public List<MagicPickerItem> Items { get; set; } = new List<MagicPickerItem>();
    }
}