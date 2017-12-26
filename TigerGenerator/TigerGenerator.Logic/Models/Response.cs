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

    public class Response<T>
    {
        public bool Success => Errors.Count == 0;
        public List<Exception> Errors { get; } = new List<Exception>();
        public T ReturnValue { get; set; }
    }
}
