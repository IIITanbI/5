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

    public static class AutomatedMagicManager
    {
        private static List<string> _loadedAssemblyNames = new List<string>();
        private static Dictionary<Type, MetaType> _type_metaType = new Dictionary<Type, MetaType>();
        private static Dictionary<Type, CommandManager> _type_commandManager = new Dictionary<Type, CommandManager>();

        public static List<MetaType> LoadedMetaTypes { get; private set; } = new List<MetaType>();
        public static List<CommandManager> LoadedCommandManagers { get; private set; } = new List<CommandManager>();

        public static void LoadAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetCustomAttribute<AutomatedMagicAssemblyAttribute>() != null).ToList();

            assemblies.ForEach(a => _loadedAssemblyNames.Add(a.FullName));

            LoadAssemblies(assemblies);
        }
        public static void LoadAssemblies(string pathToLibFolder, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var assemblies = new List<Assembly>();

            if (Directory.Exists(pathToLibFolder))
            {
                var assemblyFiles = Directory.GetFiles(pathToLibFolder, "*.dll", searchOption).ToList();
                assemblyFiles.AddRange(Directory.GetFiles(pathToLibFolder, "*.exe", searchOption));

                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(CurrentDomain_ReflectionOnlyAssemblyResolve);

                foreach (var assemblyFile in assemblyFiles)
                {
                    try
                    {
                        Console.WriteLine($"TRY LOAD ASSEMBLY: {assemblyFile}");
                        var reflectionOnlyAssembly = Assembly.ReflectionOnlyLoadFrom(assemblyFile);
                        var customAttributes = reflectionOnlyAssembly.GetCustomAttributesData().ToList();

                        if (customAttributes.Any(ca => ca.AttributeType.FullName == typeof(AutomatedMagicAssemblyAttribute).FullName))
                        {
                            if (!_loadedAssemblyNames.Contains(reflectionOnlyAssembly.FullName))
                            {
                                assemblies.Add(Assembly.LoadFrom(assemblyFile));
                                _loadedAssemblyNames.Add(reflectionOnlyAssembly.FullName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }

            LoadAssemblies(assemblies);
        }

        static Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Assembly.ReflectionOnlyLoad(args.Name);
        }

        private static void LoadAssemblies(List<Assembly> assemblies)
        {
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
        private static void LoadAssembly(Assembly assembly)
        {
            var types = assembly.DefinedTypes.ToList();

            foreach (var type in types)
            {
                LoadType(type);
            }
        }
        private static void LoadType(Type type)
        {
            if (typeof(IMetaObject).IsAssignableFrom(type))
            {
                var metaType = new MetaType(type);
                if (_type_metaType.ContainsKey(type))
                    throw new Exception($"Found duped metaType for type: {type}");

                _type_metaType.Add(type, metaType);
                LoadedMetaTypes.Add(metaType);
            }

            if (typeof(BaseCommandManager).IsAssignableFrom(type))
            {
                var commandManagerAttribute = type.GetCustomAttribute<CommandManagerAttribute>();

                if (commandManagerAttribute != null)
                {
                    var commandManager = new CommandManager(commandManagerAttribute, type);
                    if (_type_commandManager.ContainsKey(type))
                        throw new Exception($"Found duped commandManager for type: {type}");
                    _type_commandManager.Add(type, commandManager);
                    LoadedCommandManagers.Add(commandManager);
                }
            }
        }

        public static CommandManager GetCommandManagerByTypeName(string managerTypeName)
        {
            return _type_commandManager.FirstOrDefault(tcm => tcm.Key.Name == managerTypeName).Value;
        }
        public static T CreateCommandManager<T>(BaseMetaObject config = null)
            where T : BaseCommandManager
        {
            var type = typeof(T);
            var commandmanager = _type_commandManager.First(tcm => tcm.Key.Name == type.Name).Value;
            return (T)commandmanager.CreateInstance(config);
        }

        public static MetaType GetMetaType(Type type)
        {
            if (!_type_metaType.ContainsKey(type))
                return null;
            return _type_metaType[type];
        }

        public static MetaType GetMetaType(string typeName)
        {
            var key = _type_metaType.Keys.FirstOrDefault(k => k.Name == typeName);
            if (key == null) return null;
            return _type_metaType[key];
        }

        public static string GetReflectionInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Loaded assemblies count: {_loadedAssemblyNames.Count}");
            sb.AppendLine($"Loaded MetaTypes count: {LoadedMetaTypes.Count}");
            sb.AppendLine($"Loaded CommandManagers count: {LoadedCommandManagers.Count}");

            sb.AppendLine($"Loaded assemblies:");
            var counter = 1;
            _loadedAssemblyNames.ForEach(lan => sb.AppendLine($"#{counter++} : {lan}"));

            sb.AppendLine($"Loaded MetaTypes:");
            counter = 1;
            LoadedMetaTypes.ForEach(lmt => sb.AppendLine($"#{counter++} : {lmt}"));

            sb.AppendLine($"Loaded CommandManagers:");
            counter = 1;
            LoadedCommandManagers.ForEach(lcm => sb.AppendLine($"#{counter++} : {lcm}"));

            return sb.ToString();
        }
    }
}
