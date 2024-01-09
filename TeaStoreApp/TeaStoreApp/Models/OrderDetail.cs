using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaStoreApp.Models
{
    public class OrderDetail
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("qty")]
        public int Qty { get; set; }

        [JsonProperty("subTotal")]
        public int SubTotal { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("productImage")]
        public string ProductImage { get; set; }
        public string FullImageUrl => AppSettings.ApiUrl + ProductImage;

        [JsonProperty("productPrice")]
        public int ProductPrice { get; set; }
    }
}
