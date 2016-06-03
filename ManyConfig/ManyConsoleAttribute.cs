using System;

namespace ManyConfig
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ManyConfigAttribute : Attribute
    {
        /// <summary>
        /// Ключ 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Описание для ManyConsole
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Обязательный параметр
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// Значение по умолчанию
        /// </summary>
        public string DefaultValue { get; set; }
    }
}