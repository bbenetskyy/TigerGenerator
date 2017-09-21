using System;
using System.Collections.Generic;

namespace TigerGenerator.Logic.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public List<Exception> Errors { get; set; }
    }
}
