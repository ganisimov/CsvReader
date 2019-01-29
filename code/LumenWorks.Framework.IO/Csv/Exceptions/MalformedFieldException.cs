using System;

namespace LumenWorks.Framework.IO.Csv.Exceptions
{
   public class MalformedFieldException : Exception
   {
      private string _rawData;

      private Column _column;

      public MalformedFieldException(Column column, string rawData)
         : base($"Failed to convert colum '{column.Name}' to type '{column.Type.Name}'. Column's raw data: '{rawData}'")
      {
         _column = column;
         _rawData = rawData;
      }

      public string RawData
      {
         get { return _rawData; }
      }

      public Column Column
      {
         get { return _column; }
      }
   }
}
