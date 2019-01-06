using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinClientAzureML
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            btnSubmit.Clicked += BtnSubmit_Clicked;
        }

        

        private void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, List<Dictionary<string, string>>>() {
                        {
                            "input1",
                            new List<Dictionary<string, string>>(){new Dictionary<string, string>(){
                                            {
                                                "Class", "1"
                                            },
                                            {
                                                "sepal-length", entSepalLength.Text
                                            },
                                            {
                                                "sepal-width", entSepalWidth.Text
                                            },
                                            {
                                                "petal-length", entPetalLength.Text
                                            },
                                            {
                                                "petal-width", entPetalWidth.Text
                                            },
                                }
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };
                const string apiKey = "FYT7/RTnqbT/6i9TGNKGQQCiYNowmIK9ACpNuMMEcJzxzmo2aVcGKGQSbExJN5Ub0G416as6nNk9yUCnjb121A=="; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/2184572330564902bdafc6ff5145c22f/services/4595c4c853c74f14bbaa69c46c928a26/execute?api-version=2.0&format=swagger");
                var json = JsonConvert.SerializeObject(scoreRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync("",content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    var jsonResult = JsonConvert.DeserializeObject<MyModel>(result);

                    entScoredLabel.Text = jsonResult.Results.output1[0].ScoredLabels;
                    entScoredProb.Text = jsonResult.Results.output1[0].ScoredProbabilities;
                }
                else
                {
                    DisplayAlert("Keterangan", "Gagal akses web services", "OK");
                }
            }
        }
    }
}
