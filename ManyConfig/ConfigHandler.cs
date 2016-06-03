using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace ManyConfig
{
    public class ConfigHandler
    {
        private static readonly Dictionary<string, object> Dictionary = new Dictionary<string, object>();
        
        public static ManyConsoleAttribute GetManyConsoleAttribute(PropertyInfo propertyInfo)
        {
            var customAttributes = propertyInfo.GetCustomAttributes<ManyConsoleAttribute>().ToArray();

            var customAttribute = new ManyConsoleAttribute();
            if (customAttributes.Length > 0)
            {
                customAttribute = customAttributes[0];
            }
            if (string.IsNullOrEmpty(customAttribute.Key))
            {
                customAttribute.Key = propertyInfo.Name;
            }

            return customAttribute;
        }

        public static void SkipIfEmpty(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            Dictionary[key] = value;
        }

        public static T Get<T>() where T : new()
        {
            var fullName = typeof(T).FullName;

            var foo = new T();

            var propertyInfos = foo.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var consoleAttribute = GetManyConsoleAttribute(propertyInfo);

                if (string.IsNullOrEmpty(consoleAttribute.DefaultValue) == false)
                {
                    propertyInfo.SetValue(foo, Convert.ChangeType(consoleAttribute.DefaultValue, propertyInfo.PropertyType));
                }
                var valueFromAppConfig = TryGetFromAppConfig(consoleAttribute.Key);
                if (valueFromAppConfig != null)
                {
                    propertyInfo.SetValue(foo, Convert.ChangeType(valueFromAppConfig, propertyInfo.PropertyType));
                }
            }

            foreach (var key in Dictionary.Keys)
            {
                var splittedString = key.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
                if (splittedString.Length > 1 && splittedString[0].ToLower() == fullName.ToLower())
                {
                    var prop = typeof(T).GetProperty(splittedString[1]);
                    prop.SetValue(foo, Convert.ChangeType(Dictionary[key], prop.PropertyType));

                    prop.GetValue(foo);
                }
            }

            foreach (var propertyInfo in propertyInfos)
            {
                var value = propertyInfo.GetValue(foo);
                if (value == null)
                {
                    throw new Exception(string.Format("Null configuration value {0}, name: {1}. You should add to [ManyConsole] attribute " +
                        "DefaultValue or add to app.config value with key {2} and Key to attribute", propertyInfo.Name, fullName, propertyInfo.Name.ToLower()));
                }

                Console.WriteLine("{0}::{1} = {2}", fullName, propertyInfo.Name, value);
            }

            Console.WriteLine();

            return foo;
        }

        public static string TryGetFromAppConfig(string key)
        {
            string result = null;

            if (ConfigurationManager.ConnectionStrings[key] != null)
            {
                result = ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }

            if (ConfigurationManager.AppSettings[key] != null)
            {
                result = ConfigurationManager.AppSettings[key];
            }

            return result;
        }
    }
}