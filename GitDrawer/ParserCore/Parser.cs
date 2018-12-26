using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using AngleSharp.Dom.Html;
using System.Net;
using System.Net.Http;

namespace GitDrawer.ParserCore
{
    class Parser
    {
        //url of repository commits information 
        string gitRepsUrl = "https://github.com/{CurrentUser}?tab=repositories";
        string gitRepComsUrl = "https://github.com/{CurrentRep}/commits/";

        DateTime SearchingDate;

        public event Action<string> OnError;
        public event Action<string> OnNewData;
        public event Action<int> OnCommits;

        public Parser(string username, DateTime searchingDate)
        {
            gitRepsUrl = gitRepsUrl.Replace("{CurrentUser}", username);
            SearchingDate = searchingDate;
            Parse();
        }

        private async void Parse()
        {
            int CommitsInDay = 0;

            try
            {
                var domParser = new HtmlParser();

                HtmlLoader RepsLoader = new HtmlLoader(gitRepsUrl);
                var RepsSource = await RepsLoader.GetSource();
                var gitPageDoc = await domParser.ParseAsync(RepsSource);

                var divs = gitPageDoc.QuerySelectorAll("div").Where(item => item.ClassName != null && item.ClassName.Contains("d-inline-block mb-1"));

                foreach (var div in divs)
                {
                    string repName = div.QuerySelectorAll("a").FirstOrDefault().GetAttribute("href");
                    HtmlLoader CommitsLoader = new HtmlLoader(gitRepComsUrl.Replace("{CurrentRep}", repName));
                    var CommitsSource = await CommitsLoader.GetSource();
                    var gitCommitsDoc = await domParser.ParseAsync(CommitsSource);

                    var times = gitCommitsDoc.QuerySelectorAll("relative-time");

                    foreach (var time in times)
                    {
                        string s = time.GetAttribute("datetime").Split('T')[0];
                        DateTime dt = DateTime.ParseExact(s, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                        bool isSame = (dt.DayOfYear == SearchingDate.DayOfYear); if (isSame) CommitsInDay++;
                        OnNewData?.Invoke(repName + ": " + dt.ToLongDateString() + " : " + SearchingDate.ToLongDateString() + " ? " + isSame.ToString());
                    }
                }
            }
            catch
            {
                OnError?.Invoke("Error");
            }

            OnCommits?.Invoke(CommitsInDay);
        }

        class HtmlLoader
        {
            readonly HttpClient client;
            readonly string url;

            public HtmlLoader(string url)
            {
                client = new HttpClient();
                this.url = url;
            }

            public async Task<string> GetSource()
            {
                var response = await client.GetAsync(url);
                string source = null;

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    source = await response.Content.ReadAsStringAsync();
                }

                return source;
            }
        }
    }
}
