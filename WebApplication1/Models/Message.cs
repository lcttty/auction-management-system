using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace WebApplication1.Models
{
    public class Message
    {
        public int Code { get; set; }
        public string Mess { get; set; }

        /*public static implicit operator HttpContent(Message v)
        {
            throw new NotImplementedException();
        }*/
    }
}