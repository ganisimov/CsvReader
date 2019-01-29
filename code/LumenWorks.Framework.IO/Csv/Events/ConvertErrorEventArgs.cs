using LumenWorks.Framework.IO.Csv.Exceptions;
using System;

namespace LumenWorks.Framework.IO.Csv.Events
{
   public class ConvertErrorEventArgs : EventArgs
   {
      private MalformedFieldException _error;

      public ConvertErrorEventArgs(MalformedFieldException error)
      {
         _error = error;
      }

      public MalformedFieldException Error 
      { 
         get { return _error; }
      }
   }
}
