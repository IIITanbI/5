﻿namespace QA.AutomatedMagic.WpfManagingFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;
    using System.Windows.Controls;

    public class WpfManagingCollectionFiller : IManagingCollectionFiller
    {
        public object FillCreateControlls(object container, object parentObj, MetaTypeCollectionMember collectionMember)
        {
            throw new NotImplementedException();
        }

        public object FillEditControls(object container, object parentObj, MetaTypeCollectionMember collectionMember)
        {
            throw new NotImplementedException();
        }

        public void FillInfoControlls(object container, object parentObj, MetaTypeCollectionMember collectionMember)
        {
            var containerStackPanel = container as StackPanel;
            if (containerStackPanel == null)
                throw new ManagingFillerException();

            var collectionObj = collectionMember.GetValue(parentObj) ?? new List<object>();

            var objs = collectionMember.CollectionWrapper.GetChildren(collectionObj);

            var rootGroupBox = new GroupBox { Header = $"{collectionMember.Info.Name} : {collectionMember.CollectionWrapper.GetCollectionType()} of {(collectionMember.ChildrenMetaType?.Value.TargetType ?? collectionMember.ChildrenType).Name}" };
            containerStackPanel.Children.Add(rootGroupBox);

            var rootExpander = new Expander { Header = $"Count: {objs.Count}" };
            rootGroupBox.Content = rootExpander;

            var rootStackPanel = new StackPanel();
            rootExpander.Content = rootStackPanel;

            var counter = 1;
            if (collectionMember.ChildrenMetaType == null)
            {
                var managingValueFiller = collectionMember.ParentType.ManagingFiller.GetManagingValueFiller();

                foreach (var obj in objs)
                {
                    managingValueFiller.FillInfoControls(rootStackPanel, obj, $"Item: {counter++}");
                }
            }
            else
            {
                var managingObjectFiller = collectionMember.ChildrenMetaType.Value.ManagingFiller.GetManagingObjectFiller();
                foreach (var obj in objs)
                {
                    managingObjectFiller.FillInfoControls(rootStackPanel, obj, collectionMember.ChildrenMetaType.Value, $"Item: {counter++}", collectionMember.IsAssignableTypesAllowed);
                }
            }
        }
    }
}