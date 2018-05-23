using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Extensions
{
    public static class ClassExtension
    {
        /// <summary>
        /// https://stackoverflow.com/questions/457676/check-if-a-class-is-derived-from-a-generic-class
        /// use to check the class is inhreic from generic class
        /// </summary>
        /// <param name="generic"></param>
        /// <param name="toCheck"></param>
        /// <returns></returns>
        public static bool IsSubclassOfRawGeneric(this Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
