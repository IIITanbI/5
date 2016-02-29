namespace QA.AutomatedMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WpfManagingFillers;

    public class BaseMetaObjectWpfFiller : IManagingFiller
    {
        private IManagingCollectionFiller _managingCollectionFiller = new WpfManagingCollectionFiller();
        private IManagingObjectFiller _managingObjectFiller = new WpfManagingObjectFiller();
        private IManagingValueFiller _managingValueFiller = new WpfManagingValueFiller();

        public IManagingCollectionFiller GetManagingCollectionFiller()
        {
            return _managingCollectionFiller;
        }

        public IManagingObjectFiller GetManagingObjectFiller()
        {
            return _managingObjectFiller;
        }

        public IManagingValueFiller GetManagingValueFiller()
        {
            return _managingValueFiller;
        }
    }
}
