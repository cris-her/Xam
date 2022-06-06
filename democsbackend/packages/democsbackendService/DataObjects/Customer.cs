using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Mobile.Server;

namespace democsbackendService.DataObjects
{
    public class Customer : EntityData
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}