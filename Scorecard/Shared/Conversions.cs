using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scorecard.Shared
{
    class Conversions
    {
        public DateTime instToDate(String instantS)
        {
            if (String.IsNullOrWhiteSpace(instantS))
            {
                return new DateTime(1900, 1, 1);
            }

            long instant = 0;

            if (instantS.Contains("["))
            {
                string[] stringSeparators = new string[] { "[" };
                string[] dataarray = instantS.Split(stringSeparators, StringSplitOptions.None);
                instant = Convert.ToInt64(dataarray[0]);
            }
            else
            {
                instant = Convert.ToInt64(instantS);
            }
            DateTime oldenDays = new DateTime(1840, 12, 31);
            long days = (long)instant / 86400;
            long seconds = (long)instant % 86400;
            DateTime realDate = oldenDays.AddDays(days);
            realDate = realDate.AddSeconds(seconds);

            return realDate;
        }

        public sbyte convertToBitC(string input)
        {
            sbyte output = -1;
            if (String.IsNullOrEmpty(input))
            {
                output = -1;
            }
            else if (input.CompareTo("Y") == 0 || input.CompareTo("1") == 0)
            {
                output = 1;
            }
            else if (input.CompareTo("N") == 0 || input.CompareTo("0") == 0)
            {
                output = 0;
            }
            return output;
        }

        //RAS - adding an option that can return null
        public Nullable<sbyte> convertToBitWithNull(string input)
        {
            sbyte? output = null;

            if (input.CompareTo("Y") == 0 || input.CompareTo("1") == 0)
            {
                output = 1;
            }
            else if (input.CompareTo("N") == 0 || input.CompareTo("0") == 0)
            {
                output = 0;
            }
            //else
            //{
            //    output = DBNull.Value;
            //}
            return output;
        }

        public string convertToString(object input)
        {
            string output = "";
            if (input == DBNull.Value)
            {
                input = "";
            }
            try
            {
                output = (string)input;
            }
            catch (Exception e)
            {
                Debug.WriteLine("convertToString error: " + e.Message);
                Debug.WriteLine("convertToString input: " + input);
            }
            return output;
        }

        public sbyte convertToSbyte(string input)
        {
            sbyte output = 0;
            if (String.IsNullOrEmpty(input))
            {
                input = "0";
            }
            try
            {
                output = Convert.ToSByte(input);
            }
            catch (Exception e)
            {
                Debug.WriteLine("convertToSbyte error: " + e.Message);
                Debug.WriteLine("convertToSbyte input: " + input);
            }
            return output;
        }

        public short convertToShort(string input)
        {
            short output = 0;
            if (String.IsNullOrEmpty(input))
            {
                input = "0";
            }
            try
            {
                output = Convert.ToInt16(input);
            }
            catch (Exception e)
            {
                Debug.WriteLine("convertToShort error: " + e.Message);
                Debug.WriteLine("convertToShort input: " + input);
            }
            return output;
        }

        public int convertToInt(string input)
        {
            int output = 0;
            if (String.IsNullOrEmpty(input))
            {
                input = "0";
            }
            try
            {
                output = Convert.ToInt32(input);
            }
            catch (Exception e)
            {
                Debug.WriteLine("convertToInt error: " + e.Message);
                Debug.WriteLine("convertToInt input: " + input);
            }
            return output;
        }

        public long convertToLong(string input)
        {
            long output = 0;
            if (String.IsNullOrEmpty(input))
            {
                input = "0";
            }
            try
            {
                output = Convert.ToInt64(input);
            }
            catch (Exception e)
            {
                Debug.WriteLine("convertToLong error: " + e.Message);
                Debug.WriteLine("convertToLong input: " + input);
            }
            return output;
        }

        public long convertToLong(object input)
        {
            long output = 0;
            if (input == DBNull.Value)
            {
                input = "0";
            }
            try
            {
                output = Convert.ToInt64(input);
            }
            catch (Exception e)
            {
                Debug.WriteLine("convertToLong error: " + e.Message);
                Debug.WriteLine("convertToLong input: " + input);
            }
            return output;
        }

        public int convertToInt(object input)
        {
            int output = 0;
            if (input == DBNull.Value)
            {
                input = "0";
            }
            try
            {
                output = Convert.ToInt32(input);
            }
            catch (Exception e)
            {
                Debug.WriteLine("convertToInt error: " + e.Message);
                Debug.WriteLine("convertToInt input: " + input);
            }
            return output;
        }

        public double convertToDouble(string input)
        {
            double output = 0.0;
            if (String.IsNullOrEmpty(input))
            {
                input = "0";
            }
            try
            {
                output = Convert.ToDouble(input);
            }
            catch (Exception e)
            {
                Debug.WriteLine("convertToDouble error: " + e.Message);
                Debug.WriteLine("convertToDouble input: " + input);
            }
            return output;
        }

        public DateTime convertToDate(string input)
        {
            DateTime output = new DateTime();
            if (String.IsNullOrEmpty(input))
            {
                input = "1/1/1900";
            }
            try
            {
                output = Convert.ToDateTime(input);
            }
            catch (Exception)
            {
                try
                {
                    output = DateTime.ParseExact(input, "MMM dd, yyyy HHmm", new System.Globalization.CultureInfo("en-US"));
                }
                catch (Exception)
                {
                    try
                    {
                        output = DateTime.ParseExact(input, "yyyyMMdd", new System.Globalization.CultureInfo("en-US"));
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("convertToDate failure: " + e.Message);
                        Debug.WriteLine("convertToDate input: " + input);
                        output = Convert.ToDateTime("1/1/1900");
                    }
                }
            }
            return output;
        }

        public DateTime convertToTime(string input)
        {
            DateTime output = new DateTime();
            if (String.IsNullOrEmpty(input))
            {
                input = "1/1/1900";
            }
            try
            {
                output = Convert.ToDateTime(input);
            }
            catch (Exception e)
            {
                Debug.WriteLine("convertToDate failure: " + e.Message);
                Debug.WriteLine("convertToDate input: " + input);
                output = Convert.ToDateTime("1/1/1900");
            }
            return output;
        }

        public object BitCToSQLBit(sbyte value)
        {
            if (value == -1)
                return (object)DBNull.Value;
            else
            {
                return value;
            }
        }

        public sbyte SQLBoolToCBit(object value)
        {
            if (value is bool)
            {
                if ((bool)value == true)
                {
                    return 1;
                }
                else if ((bool)value == false)
                {
                    return 0;
                }
            }
            return 0;
        }

        public Nullable<sbyte> SQLBoolToCBitWithNull(object value)
        {
            if (value is bool)
            {
                if (value.ToString() == null)
                {
                    return (Nullable<sbyte>)null;
                }
                if ((bool)value == true)
                {
                    return (Nullable<sbyte>)1;
                }
                else if ((bool)value == false)
                {
                    return (Nullable<sbyte>)0;
                }
            }
            return (Nullable<sbyte>)null;
        }

        public string BitToYN(sbyte input)
        {
            if (input == 1)
            {
                return "Yes";
            }
            else if (input == 0)
            {
                return "No";
            }
            else
            {
                return "";
            }
        }

        public string BitToYN(Nullable<sbyte> input)
        {
            if (input == 1)
            {
                return "Yes";
            }
            else if (input == 0)
            {
                return "No";
            }
            else
            {
                return "";
            }
        }

        public string BoolToYN(bool input)
        {
            if (input == true)
            {
                return "Yes";
            }
            else if (input == false)
            {
                return "No";
            }
            else
            {
                return "";
            }
        }

        public string trimPlus(string input)
        {
            string output = "";
            char[] TrimChar = { ' ', '+' };
            output = input.TrimStart(TrimChar);
            return output;
        }

        public String Sanitize(String str)
        {
            string stringToReplace = "=";
            string replacementStrng = "<illegal char replaced>";

            if (str.StartsWith(stringToReplace))
            {
                str = str.Replace(stringToReplace, replacementStrng);
            }

            return str;
        }
    }
}
