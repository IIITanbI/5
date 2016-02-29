namespace QA.AutomatedMagic
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;
    using CommandsMagic;

    public static class ReflectionManager
    {
        private static List<string> _loadedAssemblies = new List<string>();
        private static Dictionary<Type, MetaType> _type_metaType = new Dictionary<Type, MetaType>();
        private static Dictionary<Type, CommandManager> _type_commandManager = new Dictionary<Type, CommandManager>();

        public static void LoadAssemblies(string pathToLibFolder = null)
        {
            var assemblies = new List<Assembly>();

            assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().ToList());

            if (pathToLibFolder == null)
                pathToLibFolder = Directory.GetCurrentDirectory();

            if (Directory.Exists(pathToLibFolder))
            {
                var assemblyFiles = Directory.GetFiles(pathToLibFolder, "*.dll").ToList();
                try
                {
                    assemblyFiles.ForEach(af => assemblies.Add(Assembly.LoadFrom(af)));
                }
                catch
                {

                }
            }

            foreach (var assembly in assemblies)
            {
                LoadAssembly(assembly);
            }

            foreach (var metaType in _type_metaType.Values)
            {
                foreach (var possibleAssiganbleType in _type_metaType.Values)
                {
                    if (metaType.TargetType.IsAssignableFrom(possibleAssiganbleType.TargetType))
                    {
                        if (!possibleAssiganbleType.TargetType.IsAbstract && !metaType.AssignableTypes.Contains(possibleAssiganbleType))
                            metaType.AssignableTypes.Add(possibleAssiganbleType);
                    }
                }
            }
        }
        public static void LoadAssembly(Assembly assembly)
        {
            var assemblyName = assembly.GetName().Name;
            if (_loadedAssemblies.Contains(assemblyName))
                return;

            var types = assembly.DefinedTypes.ToList();

            foreach (var type in types)
            {
                LoadType(type);
            }

            _loadedAssemblies.Add(assemblyName);
        }

        public static CommandManager GetCommandManagerByTypeName(string managerTypeName)
        {
            return _type_commandManager.First(tcm => tcm.Key.Name == managerTypeName).Value;
        }

        public static void LoadType(Type type)
        {
            if (typeof(IMetaObject).IsAssignableFrom(type))
            {
                var metaType = new MetaType(type);
                if (_type_metaType.ContainsKey(type))
                    throw new Exception($"Found duped metaType for type: {type}");

                _type_metaType.Add(type, metaType);
            }

            if (typeof(ICommandManager).IsAssignableFrom(type))
            {
                var commandManagerAttribute = type.GetCustomAttribute<CommandManagerAttribute>();

                if (commandManagerAttribute != null)
                {
                    var commandManager = new CommandManager(commandManagerAttribute, type);
                    if (_type_commandManager.ContainsKey(type))
                        throw new Exception($"Found duped commandManager for type: {type}");
                    _type_commandManager.Add(type, commandManager);
                }
            }
        }


        public static MetaType GetMetaType(Type type)
        {
            return _type_metaType[type];
        }
    }
}
