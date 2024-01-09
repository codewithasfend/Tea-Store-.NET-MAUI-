using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaStoreApp.Models
{
    public class Order
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("orderTotal")]
        public int OrderTotal { get; set; }

        [JsonProperty("userId")]
        public int UserId { get; set; }
    }
}
