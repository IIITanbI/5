namespace QA.AutomatedMagic.MagicServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Configuration;
    using System.Xml.Linq;
    using MetaMagic;
    using Models;

    public static class SolutionManager
    {
        private static string _storagePath;
        public static MagicStorage Storage;

        public static void Init()
        {
            _storagePath = ConfigurationManager.AppSettings["MagicStorage.Path"];

            AutomatedMagicManager.LoadAssemblies();
            AutomatedMagicManager.LoadAssemblies(Environment.CurrentDirectory);

            var storageDoc = XDocument.Load(_storagePath);
            Storage = MetaType.Parse<MagicStorage>(storageDoc.Element("MagicStorage"));

            Storage.MagicSolutions.ForEach(s => s.Init());
        }

        public static void Add(MagicSolution magicSolution)
        {
            Storage.MagicSolutions.Add(magicSolution);
            MetaType.SerializeObject(Storage).Save(_storagePath);
        }
    }
}