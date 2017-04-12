using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresURL_3._0
{
    class TKeyFormat : IFormatProvider, ICustomFormatter
    {
        private const int KEY_LENGTH = 11;

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }

        public string Format(string fmt, object arg, IFormatProvider formatProvider)
        {

            // Provide default formatting if arg is not an Int64.
            if (arg.GetType() != typeof(long))
                try
                {
                    return HandleOtherFormats(fmt, arg);
                }
                catch (FormatException e)
                {
                    throw new FormatException(String.Format("The format of '{0}' is invalid.", fmt), e);
                }

            //// Provide default formatting for unsupported format strings.
            //string ufmt = fmt.ToUpper(CultureInfo.InvariantCulture);
            //if (!(ufmt == "H" || ufmt == "I"))
            //    try
            //    {
            //        return HandleOtherFormats(fmt, arg);
            //    }
            //    catch (FormatException e)
            //    {
            //        throw new FormatException(String.Format("The format of '{0}' is invalid.", fmt), e);
            //    }

            // Convert argument to a string.
            string result = arg.ToString();

            // If Key is less than 11 characters, pad with leading zeroes.
            if (result.Length < KEY_LENGTH)
                result = result.PadLeft(KEY_LENGTH, '0');
            // If Key is more than 11 characters, truncate to 11 characters.
            if (result.Length > KEY_LENGTH)
                result = result.Substring(0, KEY_LENGTH);


            return result;

            //if (ufmt == "I")                    // Integer-only format. 
            //    return result;
            //// Add hyphens for H format specifier.
            //else                                         // Hyphenated format.
            //    return result.Substring(0, 5) + "-" + result.Substring(5, 3) + "-" + result.Substring(8);
        }

        private string HandleOtherFormats(string format, object arg)
        {
            //if (arg is IFormattable)
            //    return ((IFormattable)arg).ToString(format, CultureInfo.CurrentCulture);
            if (arg != null)
                return arg.ToString();
            else
                return String.Empty;
        }

    }

}
