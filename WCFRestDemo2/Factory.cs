using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WCFRestDemo2
{
    /// <summary>
    /// Instead of getting a 3rd party factory from Nuget, just roll a custom one to speed up compilation.
    /// This one relies on there being no name collisions.  The weakness with this one is that the types are registered in config file.
    /// This means that a proxy object must be used for mocking in tests.  Unity Enterprise library and various 3rd party factories 
    /// allow programmatic registration, and would be better for production.  e.g.  Fluent, AutoFac
    /// </summary>
    public class Factory
    {
        public static I MakeInstance<I>(params string[] services) where I : class
        {
            Type baseInterface = typeof(I);
            string name = baseInterface.Name;
            Type toCreate = GetSubclassType(name);
            object toReturn = CreateSubClassInstance(services, toCreate);
            return (I)toReturn;
        }

        /// <summary>
        /// Recursive injection.
        /// </summary>
        /// <param name="services">Services to inject.</param>
        /// <param name="toCreate">The type to create an instance of.</param>
        /// <returns></returns>
        private static object CreateSubClassInstance(string[] services, Type toCreate)
        {
            object toReturn = null;
            if (services.Length == 0)
            {
                toReturn = Activator.CreateInstance(toCreate);
            }
            else
            {
                IList<object> arguments = new List<object>();
                foreach (string service in services)
                {
                    Type type = GetSubclassType(service);
                    var constructors = type.GetConstructors();
                    if (constructors.Length > 1) throw new ArgumentException("Factory only supports one constructor. Dependency has more than one", "services");
                    var parameters = constructors[0].GetParameters();
                    var dependencies = parameters.Select(p => p.ParameterType.Name).ToArray();
                    arguments.Add(CreateSubClassInstance(dependencies, GetSubclassType(service)));
                }
                toReturn = Activator.CreateInstance(toCreate, arguments.ToArray());
            }

            return toReturn;
        }

        private static Type GetSubclassType(string name)
        {
            string concrete = ConfigurationManager.AppSettings[name];
            Assembly assemblyToLoadFrom = Assembly.GetExecutingAssembly();
            Type toCreate = assemblyToLoadFrom.GetType("WCFRestDemo2." + concrete);
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            if (toCreate == null)
            {
                foreach (Assembly assembly in assemblies)
                {
                    var candidate = assembly.GetTypes().Where(t => t.Name == concrete).FirstOrDefault();
                    if (candidate != null)
                    {
                        toCreate = candidate;
                        break;
                    }
                }
            }

            //Ultimately assembly could be loaded in order to find a missing type, but out of scope for this challenge
            //if (toCreate == null)
            //{
            //    DirectoryInfo folder = new DirectoryInfo(assemblyToLoadFrom.Location);
            //    FileInfo[] files = folder.GetFiles("*.dll");
            //    foreach (FileInfo file in files)
            //    {
            //        //load assemblies   
            //    }
            //}

            return toCreate;
        }
    }
}