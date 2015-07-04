using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public static class IntrospectionUtils
    {
        public static IEnumerable<Type> GetAllCompatibleTypes<T>(
            bool allowAbstract = false,
            bool allowInterface = false)
        {
            var type = typeof(T);
            var types = 
                AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));

            if (!allowAbstract)
            {
                types = types.Where(p => !p.IsAbstract);
            }
            if (!allowInterface)
            {
                types = types.Where(p => !p.IsInterface);
            }
            return types;
        }
    }
}
