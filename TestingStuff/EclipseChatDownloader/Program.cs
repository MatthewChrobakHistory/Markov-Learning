using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EclipseChatDownloader
{
    public class Program
    {
        public static Browser frmBrowser;

        [STAThread]
        public static void Main(string[] args)
        {
            frmBrowser = new Browser();
            frmBrowser.Show();

            while (Program.frmBrowser.Visible) {
                Application.DoEvents();
            }
        }
    }

    public class EclipseChatStorer : EclipseChatDownloader
    {
        public void StoreLog(int number)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + $"/logs/{number}/";
            Directory.CreateDirectory(path);

            var entries = this.GetChatOnPage(number);

            foreach (var entry in entries) {
                string line = $"{entry.Username}:{entry.Shout}\r\n";
                File.AppendAllText($"{path}total.txt", line);
                File.AppendAllText($"{path}{entry.Username}.txt", line);
            }
        }
    }


    public class BotClient
    {

        public BotClient()
        {
            Program.frmBrowser.Show();
        }

        public string GetString(string url)
        {
            Program.frmBrowser.innerBrowser.Navigate(url);
            
            while(Program.frmBrowser.innerBrowser.ReadyState != WebBrowserReadyState.Complete) {
                Application.DoEvents();
            }            

            return Program.frmBrowser.innerBrowser.DocumentText;
        }
    }

    public class EclipseChatDownloader : BotClient
    {
        private static Regex usernameFinder = new Regex("<div class=\"user\">.*?<\\/div>");
        private static Regex shoutFinder = new Regex("<div class=\"text\">.*?<\\/div>");

        public List<ShoutboxEntry> GetChatOnPage(int pageNumber)
        {
            var results = new List<ShoutboxEntry>();
            var source = this.GetString("https://www.eclipseorigins.com/index.php?action=shoutbox_archive&page=" + pageNumber);

            var usernames = usernameFinder.Matches(source);
            var shouts = shoutFinder.Matches(source);

            for (int i = 0; i < Math.Min(usernames.Count, shouts.Count); i++) {
                results.Add(new ShoutboxEntry(usernames[i].Value, shouts[i].Value));
            }

            return results;
        }
    }

    public class ShoutboxEntry
    {
        public string Username { private set; get; }
        public string Shout { private set; get; }

        private static Regex ShoutPurger = new Regex("<span.*?<\\/span>");
        private static Regex UsernamePurger = new Regex("<strong>.*?<\\/strong><\\/span>");

        public ShoutboxEntry(string usernameDivCode, string shoutDivCode)
        {
            this.AssignAndPurgeShout(shoutDivCode);
            this.AssignAndPurgeUsername(usernameDivCode);
        }

        private void AssignAndPurgeUsername(string usernameDivCode)
        {
            string match = usernameDivCode; // UsernamePurger.Match(usernameDivCode).Value;
            while (match.Contains(">") && match.Contains("<")) {
                int start = match.IndexOf("<");
                int end = match.IndexOf(">");
                match = match.Remove(start, end - start + 1);
            }
            this.Username = match;
        }

        private void AssignAndPurgeShout(string shoutDivCode)
        {
            string match = shoutDivCode; //ShoutPurger.Match(shoutDivCode).Value;
            while (match.Contains(">") && match.Contains("<")) {
                int start = match.IndexOf("<");
                int end = match.IndexOf(">");
                match = match.Remove(start, end - start + 1);
            }
            this.Shout = match;
        }
    }
}
