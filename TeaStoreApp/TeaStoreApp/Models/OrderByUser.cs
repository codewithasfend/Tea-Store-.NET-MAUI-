using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaStoreApp.Models
{
    public class OrderByUser
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("orderTotal")]
        public int OrderTotal { get; set; }

        [JsonProperty("orderPlaced")]
        public DateTime OrderPlaced { get; set; }
    }
}
