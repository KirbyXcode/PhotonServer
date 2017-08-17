using System;
using System.Collections.Generic;

namespace yukiServer.Table
{
    class User
    {
        public virtual int Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
    }
}
