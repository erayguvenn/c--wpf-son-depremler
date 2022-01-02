using CsQuery.Engine.PseudoClassSelectors;
using DocumentFormat.OpenXml.Bibliography;
using LanguageExt.TypeClasses;
using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;




namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string yazdirilacak;
        /// <summary>
        ///  earthquakesLocal adında bir list oluşturup bu list yardımıyla earthquakes den gelen verileri datagrid ekliyor.
        ///  pinLocation adında bir lit oluşturup bu list ki enlem ve boylam verisine göre pushpin ekliyor.
        /// </summary>
        public MainWindow()
        {

            GetApiData();
            InitializeComponent();
            List<Earthquake> earthquakesLocal = new List<Earthquake>();
            List<Location> pinLocation = new List<Location>();

            for (int i = 0; i < earthquakes.Count; i++)
            {
                earthquakesLocal.Add(earthquakes[i]);
                EarthquakeDataGrid.Items.Add(earthquakesLocal[i]);
                pinLocation.Add(new Location(earthquakes[i].Enlem, earthquakes[i].Boylam));
                Pushpin pin = new Pushpin();
                pin.Location = pinLocation[i];

                myMap.Children.Add(pin);
            }
            ExampleAsync(earthquakesLocal);


        }




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


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }


       
        /// <summary>
        /// deprem adında bir list oluşturup foreach ile bunu gezerek list içindeki bütün elemanları bir text dosyasına yazdırıyor.
        /// </summary>
        /// <param name="deprem"></param>
        /// <returns></returns>
        public static async Task ExampleAsync(List<Earthquake> deprem)
        {
            String yazdir = "";
            foreach (var item in deprem)
            {
                yazdir += item.Tarih + "  " + item.Saat + " " +item.Buyukluk + " " + item.Yer + " " + item.Sehir + ".......";
            }
            await File.WriteAllTextAsync("deprem_deneme.txt", yazdir);
        }
    }

}
