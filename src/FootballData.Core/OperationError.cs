using System;
using System.Collections.Generic;
using System.Text;

namespace FootballData.Core
{
    public class OperationError
    {
        public OperationError(string code, string description = null)
        {
            Code = code;
            Description = description;
        }

        public string Code { get; }
        public string Description { get; }
    }
}
