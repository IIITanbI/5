namespace QA.AutomatedMagic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using XmlSourceResolver;

    public class BaseMetaObjectXmlSourceResolver : ISourceResolver
    {
        private ICollectionSourceResolver _collectionSourceResolver;
        private IObjectSourceResolver _objectSourceResolver;
        private IValueSourceResolver _valueSourceResolver;

        public BaseMetaObjectXmlSourceResolver()
        {
            _collectionSourceResolver = new XmlCollectionSourceResolver();
            _objectSourceResolver = new XmlObjectSourceResolver();
            _valueSourceResolver = new XmlValueSourceResolver();
        }

        public ICollectionSourceResolver GetCollectionSourceReolver()
        {
            return _collectionSourceResolver;
        }

        public string GetSourceNodeName(object obj)
        {
            return (obj as XElement).Name.LocalName;
        }

        public IObjectSourceResolver GetObjectSourceResolver()
        {
            return _objectSourceResolver;
        }

        public IValueSourceResolver GetValueSourceResolver()
        {
            return _valueSourceResolver;
        }
    }
}
