using System.Collections.Generic;
using Unity.UIWidgets.foundation;

namespace Unity.UIWidgets.DevTools
{
    public class EnumUtils<T> {
        public EnumUtils(List<T> enumValues) {
            foreach (var val in enumValues) {
                var enumDescription = DiagnosticUtils.describeEnum(val);
                _lookupTable[enumDescription] = val;
                _reverseLookupTable[val] = enumDescription;
            }
        }

        public readonly Dictionary<string, T> _lookupTable = new Dictionary<string, T>(){};
        public readonly Dictionary<T, string> _reverseLookupTable = new Dictionary<T, string>(){};

        public T enumEntry(string enumName) => _lookupTable[enumName];

        string name(T enumEntry) => _reverseLookupTable[enumEntry];
    }
}