﻿using LumenWorks.Framework.IO.Csv.Events;
using LumenWorks.Framework.IO.Csv.Exceptions;
using System;
using System.Globalization;

namespace LumenWorks.Framework.IO.Csv
{
    /// <summary>
    /// Metadata about a CSV column.
    /// </summary>
    public class Column
    {
        private Type type;
        private string typeName;

        /// <summary>
        /// Creates a new instance of the <see cref="Column" /> class.
        /// </summary>
        public Column()
        {
            Type = typeof(string);
            Culture = CultureInfo.CurrentCulture;
            NumberStyles = NumberStyles.Any;
            DateTimeStyles = DateTimeStyles.None;
            DateParseExact = null;
        }

        public event EventHandler<ConvertErrorEventArgs> ConvertError;

        protected virtual void OnConvertError(ConvertErrorEventArgs e)
        {
            var handler = ConvertError;

            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Get or set the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set the type.
        /// </summary>
        public Type Type
        {
            get { return type; }
            set
            {
                type = value;
                typeName = value.Name;
            }
        }

        /// <summary>
        /// Get or set the default value of the column.
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Get or set the override value of the column.
        /// </summary>
        public string OverrideValue { get; set; }

        public CultureInfo Culture { get; set; }

        public NumberStyles NumberStyles { get; set; }
        
        public DateTimeStyles DateTimeStyles { get; set; }
        
        public string DateParseExact { get; set; }

        /// <summary>
        /// Converts the value into the column type.
        /// </summary>
        /// <param name="value">Value to use</param>
        /// <returns>Converted value.</returns>
        public object Convert(string value)
        {
            var converted = TryConvert(value, out object x);

            if (converted)
            {
               return x;
            }
            else 
            {
               HandleConversionError(value);
               return null;
            }
        }

        private void HandleConversionError(string value)
        {
            OnConvertError(
               new ConvertErrorEventArgs(
                  new MalformedFieldException(this, value)));
        }

        /// <summary>
        /// Converts the value into the column type.
        /// </summary>
        /// <param name="value">Value to use</param>
        /// <param name="result">Object to hold the converted value.</param>
        /// <returns>true if the conversion was successful, otherwise false.</returns>
        public bool TryConvert(string value, out object result)
        {
            bool converted;

            switch (typeName)
            {
                case "String":
                    result = value;
                    converted = true;
                    break;

                case "Guid":
                    try
                    {
                        result = new Guid(value);
                        converted = true;
                    }
                    catch
                    {
                        result = Guid.Empty;
                        converted = false;
                    }
                    break;

                case "Byte[]":
                    {
                        try
                        {
                            result = System.Convert.FromBase64String(value);
                            converted = true;
                        }
                        catch
                        {
                            result = new byte[0];
                            converted = false;
                        }
                    }
                    break;

                case "Boolean":
                    {
                        int x;
                        converted = int.TryParse(value, NumberStyles, Culture, out x);
                        if (converted)
                        {
                            result = x != 0;
                        }
                        else
                        {
                            bool y;
                            converted = bool.TryParse(value, out y);
                            result = y;
                        }
                    }
                    break;

                case "Int32":
                    {
                        int x;
                        converted = int.TryParse(value, NumberStyles, Culture, out x);
                        result = x;
                    }
                    break;

                case "Int64":
                    {
                        long x;
                        converted = long.TryParse(value, NumberStyles, Culture, out x);
                        result = x;
                    }
                    break;

                case "Single":
                    {
                        float x;
                        converted = float.TryParse(value, NumberStyles, Culture, out x);
                        result = x;
                    }
                    break;

                case "Double":
                    {
                        double x;
                        converted = double.TryParse(value, NumberStyles, Culture, out x);
                        result = x;
                    }
                    break;

                case "Decimal":
                    {
                        decimal x;
                        converted = decimal.TryParse(value, NumberStyles, Culture, out x);
                        result = x;
                    }
                    break;

                case "DateTime":
                    {
                        DateTime x;
                        if (!string.IsNullOrEmpty(DateParseExact))
                        {
                            converted = DateTime.TryParseExact(value, DateParseExact, Culture, DateTimeStyles, out x);
                        }
                        else
                        {
                            converted = DateTime.TryParse(value, Culture, DateTimeStyles, out x);
                        }
                        result = x;
                    }
                    break;

                default:
                    converted = false;
                    result = value;
                    break;
            }

            return converted;
        }
    }
}
