using System;
using System.Collections.Generic;

namespace TigerGenerator.Logic.Models
{
    public class Response
    {
        public bool Success => Errors.Count == 0;
        public List<Exception> Errors { get; } = new List<Exception>();
        public object ReturnValue { get; set; }
    }
}
