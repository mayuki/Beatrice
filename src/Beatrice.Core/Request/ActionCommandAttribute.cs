using System;

namespace Beatrice.Request
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ActionCommandAttribute : System.Attribute
    {
        public string Name { get; }
        public ActionCommandAttribute(string name)
        {
            Name = name;
        }
    }
}
