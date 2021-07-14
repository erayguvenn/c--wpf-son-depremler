using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;

namespace WpfApp2
{
    internal class ConnectManager
    {
        
        public List<Earthquake> earthquakes;
        /// <summary>
        /// Bu metotta API'dan gelen verileri çekilip ve earthquakes adlı Earthquake listesine kaydediliyor.
        /// </summary>
        internal void GetApiData()
        {
            /* WebRequest class'ındaki Create metodununun yardımıyla API'dan bilgiyi çekiliyor ve eğer boş geliyorsa boş değer döndürür.
             */
            var webRequest = WebRequest.Create("https://turkiyedepremapi.herokuapp.com/api") as HttpWebRequest;
            if (webRequest == null)
            {
                return;
            }
            /* Yukarıda çektiğimiz bilginin response bilgisine ulaşıp sonra o response'un stream bilgileri çekilir. Sonra bu
             * bilgiler StreamReader aracılığı ile bir var değişkenine atanır. Sonra bu bilgiler sonuna kadar okunur. Son olarak
             * bu gelen json formatındaki bilgiler Deserialize edilerek earthquakes listesine kaydedilir.
             */
            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var contributorsAsJson = sr.ReadToEnd();
                    earthquakes = JsonConvert.DeserializeObject<List<Earthquake>>(contributorsAsJson);
                }
            }
        }
    }
}



