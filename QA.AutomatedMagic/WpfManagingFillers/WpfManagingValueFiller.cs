namespace QA.AutomatedMagic.WpfManagingFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Editors;
    using MetaMagic;
    using WpfValueFillers;

    public class WpfManagingValueFiller : IManagingValueFiller
    {
        protected List<BaseWpfValueFiller> _valueFillers = new List<BaseWpfValueFiller>
        {
            new ByteFiller(),
            new IntFiller(),
            new DoubleFiller(),
            new StringFiller(),
            new EnumFiller(),
            new DateTimeFiller(),
            new TimeSpanFiller(),
            new BoolFiller(),
            new XElementFiller()
        };

        public object FillCreateControls(object container, MetaTypeValueMember valueMember)
        {
            throw new NotImplementedException();
        }

        public object FillCreateControls(object container, Type type, string name)
        {
            throw new NotImplementedException();
        }

        public object FillEditControls(object container, object obj, string name)
        {
            throw new NotImplementedException();
        }

        public object FillEditControls(object container, object parentObj, MetaTypeValueMember valueMember)
        {
            var containerStackPanel = container as StackPanel;
            if (containerStackPanel == null)
                throw new ManagingFillerException();

            var rootGroupBox = new GroupBox();
            containerStackPanel.Children.Add(rootGroupBox);
            var headerWrapPanel = new WrapPanel();
            rootGroupBox.Header = headerWrapPanel;
            var nameLabel = new Label { Content = $"{valueMember.Info.Name} : {valueMember.MemberType.Name}" };
            headerWrapPanel.Children.Add(nameLabel);
            var editBtn = new Button { Content = "Edit" };
            headerWrapPanel.Children.Add(editBtn);
            var saveBtn = new Button { Content = "Save" };
            headerWrapPanel.Children.Add(saveBtn);
            var cancelBtn = new Button { Content = "Cancel" };
            headerWrapPanel.Children.Add(cancelBtn);

            var rootStackPanel = new StackPanel();
            rootGroupBox.Content = rootStackPanel;

            var filler = _valueFillers.First(f => f.IsMatch(valueMember.MemberType));
            filler.FillInfoControls(rootStackPanel, valueMember.GetValue(parentObj), valueMember.MemberType);

            var valueEditor = new ValueEditor(editBtn, saveBtn, cancelBtn, filler, rootStackPanel);
            return null;
        }

        public void FillInfoControls(object container, object obj, string name)
        {
            var containerStackPanel = container as StackPanel;
            if (containerStackPanel == null)
                throw new ManagingFillerException();

            var objType = obj.GetType();

            var rootGroupBox = new GroupBox { Header = $"{name} : {objType.Name}" };
            containerStackPanel.Children.Add(rootGroupBox);

            var rootStackPanel = new StackPanel();
            rootGroupBox.Content = rootStackPanel;

            var filler = _valueFillers.First(f => f.IsMatch(objType));
            filler.FillInfoControls(rootStackPanel, obj, objType);
        }
        public void FillInfoControls(object container, object parentObj, MetaTypeValueMember valueMember)
        {
            var containerStackPanel = container as StackPanel;
            if (containerStackPanel == null)
                throw new ManagingFillerException();

            var rootGroupBox = new GroupBox { Header = $"{valueMember.Info.Name} : {valueMember.MemberType.Name}" };
            containerStackPanel.Children.Add(rootGroupBox);

            var rootStackPanel = new StackPanel();
            rootGroupBox.Content = rootStackPanel;

            var filler = _valueFillers.First(f => f.IsMatch(valueMember.MemberType));
            filler.FillInfoControls(rootStackPanel, valueMember.GetValue(parentObj), valueMember.MemberType);
        }
    }
}
