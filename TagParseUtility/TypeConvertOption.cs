using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagParseUtility.NodeParse;

namespace TagParseUtility
{
    public static class TypeConvertOption
    {
        private static readonly Dictionary<int, Func<string, string>> _typeConvertByDriverId = new();
        private static readonly Dictionary<int, Delegate> _exportConvertByDriverId = new();

        public static Func<string, string>? GetTypeConvertByDriverId(int driverId)
        {
            _typeConvertByDriverId.TryGetValue(driverId, out var convertFunc);
            return convertFunc;
        }

        public static Func<IVisitor<T>, ExportCsvContent>? GetExportConvertByDriverId<T>(int driverId)
        {
            if (_exportConvertByDriverId.TryGetValue(driverId, out var convertFunc))
            {
                return convertFunc as Func<IVisitor<T>, ExportCsvContent>;
            }
            return null;
        }
    }
}
