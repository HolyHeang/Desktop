using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Library;

namespace CamemisOffLine.Component
{
    /// <summary>
    /// Interaction logic for StaffAttdanceApprovedPopup.xaml
    /// </summary>
    public partial class StaffAttdanceApprovedPopup : Window
    {
        public StaffAttdanceApprovedPopup(string requestId)
        {
            InitializeComponent();
            this.requestId = requestId;
        }
        string requestId = "";
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var accessUrl = Properties.Settings.Default.acessUrl;
            var token = Properties.Settings.Default.Token;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (HttpResponseMessage res = client.PostAsJsonAsync(accessUrl + "/action-staff-attendance-permission/" + requestId, new StaffPermission {approved_comment=txtComment.Text }).Result)
                {
                    using (HttpContent content = res.Content)
                    {
                        string datas = await content.ReadAsStringAsync();
                        //var obj = JObject.Parse(datas).ToObject<ApprovedLearningResult>();
                    }
                }
            }
            this.Close();
        }
    }
}
