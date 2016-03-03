namespace QA.AutomatedMagic.WpfManagingFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;
    using System.Windows.Controls;
    using Creators;
    using Editors;
    public class WpfManagingObjectFiller : IManagingObjectFiller
    {
        public object FillCreateControls(object container, MetaTypeObjectMember objectMember)
        {
            throw new NotImplementedException();
        }

        public object FillCreateControls(object container, MetaType metaType, string name, bool isAssignableTypesAllowed)
        {
            throw new NotImplementedException();
        }

        public void FillEditControls(object container, object parentObj, MetaTypeObjectMember objectMember)
        {
            var containerStackPanel = container as StackPanel;
            if (containerStackPanel == null)
                throw new ManagingFillerException();

            var rootGroupBox = new GroupBox();
            containerStackPanel.Children.Add(rootGroupBox);

            var headerWrapPanel = new WrapPanel();
            rootGroupBox.Header = headerWrapPanel;
            headerWrapPanel.Children.Add(new Label { Content = $"{objectMember.Info.Name} : {objectMember.MemberType.Name}" });

            var obj = objectMember.GetValue(parentObj);
            if (obj == null)
            {
                rootGroupBox.Content = new Label { Content = "Value is not specified" };
                var creator = new ObjectCreator(headerWrapPanel, objectMember, parentObj);
                return;
            }

            var metaType = objectMember.MemberMetaType.Value;
            if (objectMember.IsAssignableTypesAllowed)
                metaType = ReflectionManager.GetMetaType(obj.GetType());

            var rootExpander = new Expander { Header = metaType.Info.Name };
            rootGroupBox.Content = rootExpander;

            var rootStackPanel = new StackPanel();
            rootExpander.Content = rootStackPanel;

            foreach (var metaTypeMember in metaType.Members)
            {
                var valueMember = metaTypeMember as MetaTypeValueMember;
                if (valueMember != null)
                {
                    valueMember.ManagingValueFiller.FillInfoControls(rootStackPanel, obj, valueMember);
                }

                var collectionMember = metaTypeMember as MetaTypeCollectionMember;
                if (collectionMember != null)
                {
                    collectionMember.ManagingCollectionFiller.FillInfoControls(rootStackPanel, obj, collectionMember);
                }

                var objectMember1 = metaTypeMember as MetaTypeObjectMember;
                if (objectMember1 != null)
                {
                    objectMember1.MemberMetaType.Value.ManagingFiller.GetManagingObjectFiller().FillInfoControls(rootStackPanel, obj, objectMember1);
                }
            }

            var editor = new ObjectEditor(headerWrapPanel, objectMember, parentObj);
        }

        public Func<object> FillEditControls(object container, object obj, MetaType metaType, string name, bool isAssignableTypesAllowed)
        {
            var containerStackPanel = container as StackPanel;
            if (containerStackPanel == null)
                throw new ManagingFillerException();

            var rootGroupBox = new GroupBox();
            containerStackPanel.Children.Add(rootGroupBox);

            var headerWrapPanel = new WrapPanel();
            rootGroupBox.Header = headerWrapPanel;
            headerWrapPanel.Children.Add(new Label { Content = $"{name} : {metaType.Info.Name}" });

            if (isAssignableTypesAllowed)
                metaType = ReflectionManager.GetMetaType(obj.GetType());

            var rootExpander = new Expander { Header = metaType.Info.Name };
            rootGroupBox.Content = rootExpander;

            var rootStackPanel = new StackPanel();
            rootExpander.Content = rootStackPanel;

            foreach (var metaTypeMember in metaType.Members)
            {
                var valueMember = metaTypeMember as MetaTypeValueMember;
                if (valueMember != null)
                {
                    valueMember.ManagingValueFiller.FillInfoControls(rootStackPanel, obj, valueMember);
                }

                var collectionMember = metaTypeMember as MetaTypeCollectionMember;
                if (collectionMember != null)
                {
                    collectionMember.ManagingCollectionFiller.FillInfoControls(rootStackPanel, obj, collectionMember);
                }

                var objectMember1 = metaTypeMember as MetaTypeObjectMember;
                if (objectMember1 != null)
                {
                    objectMember1.MemberMetaType.Value.ManagingFiller.GetManagingObjectFiller().FillInfoControls(rootStackPanel, obj, objectMember1);
                }
            }

            var editor = new ObjectEditor(headerWrapPanel, metaType, obj, name);
            return editor.GetGet();
        }

        public void FillInfoControls(object container, object parentObj, MetaTypeObjectMember objectMember)
        {
            var containerStackPanel = container as StackPanel;
            if (containerStackPanel == null)
                throw new ManagingFillerException();

            var rootGroupBox = new GroupBox { Header = $"{objectMember.Info.Name} : {objectMember.MemberType.Name}" };
            containerStackPanel.Children.Add(rootGroupBox);

            var obj = objectMember.GetValue(parentObj);
            if (obj == null)
            {
                rootGroupBox.Content = new Label { Content = "Value is not specified" };
                return;
            }

            var metaType = objectMember.MemberMetaType.Value;
            if (objectMember.IsAssignableTypesAllowed)
                metaType = ReflectionManager.GetMetaType(obj.GetType());

            var rootExpander = new Expander { Header = metaType.Info.Name };
            rootGroupBox.Content = rootExpander;

            var rootStackPanel = new StackPanel();
            rootExpander.Content = rootStackPanel;

            foreach (var metaTypeMember in metaType.Members)
            {
                var valueMember = metaTypeMember as MetaTypeValueMember;
                if (valueMember != null)
                {
                    valueMember.ManagingValueFiller.FillInfoControls(rootStackPanel, obj, valueMember);
                }

                var collectionMember = metaTypeMember as MetaTypeCollectionMember;
                if (collectionMember != null)
                {
                    collectionMember.ManagingCollectionFiller.FillInfoControls(rootStackPanel, obj, collectionMember);
                }

                var objectMember1 = metaTypeMember as MetaTypeObjectMember;
                if (objectMember1 != null)
                {
                    objectMember1.MemberMetaType.Value.ManagingFiller.GetManagingObjectFiller().FillInfoControls(rootStackPanel, obj, objectMember1);
                }
            }
        }
        public void FillInfoControls(object container, object obj, MetaType metaType, string name, bool isAssignableTypesAllowed)
        {
            var containerStackPanel = container as StackPanel;
            if (containerStackPanel == null)
                throw new ManagingFillerException();

            var rootGroupBox = new GroupBox { Header = $"{name} : {metaType.Info.Name}" };
            containerStackPanel.Children.Add(rootGroupBox);

            if (obj == null)
            {
                rootGroupBox.Content = new Label { Content = "Value is not specified" };
                return;
            }

            if (isAssignableTypesAllowed)
                metaType = ReflectionManager.GetMetaType(metaType.TargetType);

            var rootExpander = new Expander { Header = metaType.Info.Name };
            rootGroupBox.Content = rootExpander;

            var rootStackPanel = new StackPanel();
            rootExpander.Content = rootStackPanel;

            foreach (var metaTypeMember in metaType.Members)
            {
                var valueMember = metaTypeMember as MetaTypeValueMember;
                if (valueMember != null)
                {
                    var value = valueMember.GetValue(obj);
                    valueMember.ManagingValueFiller.FillInfoControls(rootStackPanel, value, valueMember.Info.Name);
                }

                var collectionMember = metaTypeMember as MetaTypeCollectionMember;
                if (collectionMember != null)
                {
                    collectionMember.ManagingCollectionFiller.FillInfoControls(rootStackPanel, obj, collectionMember);
                }

                var objectMember = metaTypeMember as MetaTypeObjectMember;
                if (objectMember != null)
                {
                    objectMember.MemberMetaType.Value.ManagingFiller.GetManagingObjectFiller().FillInfoControls(rootStackPanel, obj, objectMember);
                }
            }
        }
    }
}
