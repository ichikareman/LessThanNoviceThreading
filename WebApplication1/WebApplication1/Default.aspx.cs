using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Description;
using WebApplication1.MockGeneratorServices;

namespace WebApplication1
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //var mock = new MockGeneratorServices.MockGeneratorServicesClient().GetPopularSubCat();
            //TextBox1.Text = mock.TotalMarketCount.ToString();

            var mock = new MockGeneratorServicesClient();
            TextBox1.Text = mock.GetPopularSubCats().TotalMarketCount.ToString();
            
            var popsubcat = mock.GetPopularSubCats();
            var stream1 = new MemoryStream();
            var ser = new DataContractJsonSerializer(typeof(WebApplication1.MockGeneratorServices.psc));
            ser.WriteObject(stream1,popsubcat);
            stream1.Position = 0;
            var sr = new StreamReader(stream1);
            Label1.Text = sr.ReadToEnd().Replace("}","}<br />");
        }
    }
}