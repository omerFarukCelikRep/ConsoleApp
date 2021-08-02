using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Exceptions
{
    public class CommanLineMustEndWithEException : Exception
    {
        public CommanLineMustEndWithEException() : base("Command line must ends with ':E' ")
        {

        }
    }
}
