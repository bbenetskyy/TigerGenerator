using System;
using System.Collections.Generic;

namespace TigerGenerator.Logic.Models
{
    public class Response
    {
        public bool Success { get; set; } = true;
        public List<Exception> Errors { get; } = new List<Exception>();
        public object ReturnValue { get; set; }
    }
}
