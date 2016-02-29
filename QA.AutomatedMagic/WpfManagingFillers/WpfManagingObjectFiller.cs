namespace QA.AutomatedMagic.WpfManagingFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;
    using System.Windows.Controls;

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

        public object FillEditControls(object container, object parentObj, MetaTypeObjectMember objectMember)
        {
            var containerStackPanel = container as StackPanel;
            if (containerStackPanel == null)
                throw new ManagingFillerException();

            var rootGroupBox = new GroupBox();
            var headerWrapPanel = new WrapPanel();
            var nameLabel = new Label { Content = $"{objectMember.Info.Name} : {objectMember.MemberType.Name}" };
            headerWrapPanel.Children.Add(nameLabel);
            rootGroupBox.Header = headerWrapPanel;

            containerStackPanel.Children.Add(rootGroupBox);

            var obj = objectMember.GetValue(parentObj);
            if (obj == null)
            {
                var createButton = new Button { Content = "Create" };
                headerWrapPanel.Children.Add(createButton);

                rootGroupBox.Content = new Label { Content = "Value is not specified" };
                return null;
            }

            var editButton = new Button { Content = "Edit" };
            headerWrapPanel.Children.Add(editButton);

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
                    valueMember.ManagingValueFiller.FillEditControls(rootStackPanel, obj, valueMember);
                }

                var collectionMember = metaTypeMember as MetaTypeCollectionMember;
                if (collectionMember != null)
                {
                    collectionMember.ManagingCollectionFiller.FillEditControls(rootStackPanel, obj, collectionMember);
                }

                var objectMember1 = metaTypeMember as MetaTypeObjectMember;
                if (objectMember1 != null)
                {
                    objectMember1.MemberMetaType.Value.ManagingFiller.GetManagingObjectFiller().FillEditControls(rootStackPanel, obj, objectMember1);
                }
            }

            return obj;
        }

        public object FillEditControls(object container, object obj, MetaType metaType, string name, bool isAssignableTypesAllowed)
        {
            var containerStackPanel = container as StackPanel;
            if (containerStackPanel == null)
                throw new ManagingFillerException();

            var rootGroupBox = new GroupBox { Header = $"{name} : {metaType.Info.Name}" };
            containerStackPanel.Children.Add(rootGroupBox);


            if (isAssignableTypesAllowed)
                metaType = ReflectionManager.GetMetaType(metaType.TargetType);

            var rootExpander = new Expander { Header = metaType.Info.Name };
            rootGroupBox.Content = rootExpander;

            var rootStackPanel = new StackPanel();
            rootExpander.Content = rootStackPanel;

            if (obj == null)
            {
                rootStackPanel.Children.Add(new Label { Content = "Value is not specified" });
                return null;
            }


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
                    collectionMember.ManagingCollectionFiller.FillInfoControlls(rootStackPanel, obj, collectionMember);
                }

                var objectMember = metaTypeMember as MetaTypeObjectMember;
                if (objectMember != null)
                {
                    objectMember.MemberMetaType.Value.ManagingFiller.GetManagingObjectFiller().FillEditControls(rootStackPanel, obj, objectMember);
                }
            }

            return null;
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
                    collectionMember.ManagingCollectionFiller.FillInfoControlls(rootStackPanel, obj, collectionMember);
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


            if (isAssignableTypesAllowed)
                metaType = ReflectionManager.GetMetaType(metaType.TargetType);

            var rootExpander = new Expander { Header = metaType.Info.Name };
            rootGroupBox.Content = rootExpander;

            var rootStackPanel = new StackPanel();
            rootExpander.Content = rootStackPanel;

            if (obj == null)
            {
                rootStackPanel.Children.Add(new Label { Content = "Value is not specified" });
                return;
            }


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
                    collectionMember.ManagingCollectionFiller.FillInfoControlls(rootStackPanel, obj, collectionMember);
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
