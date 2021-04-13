using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErestAPI.Models.CommonModels
{
    public class CommonModel
    {
        public string PageURL { get; set; }
        public string APIURL { get; set; }
        public int UserID { get; set; }
        public string ClientIp { get; set; }
        public string Date { get; set; }
    }
}
