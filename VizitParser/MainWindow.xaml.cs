using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VizitParser
{
    public partial class MainWindow : Window
    {
        List<TextBlock> blocks = new List<TextBlock>();

        public MainWindow()
        {
            InitializeComponent();
            FillListBox();            
        }

        private async Task FillListBox()
        {
            var client = new HttpClient();
            HtmlDocument doc = new HtmlDocument();
            var stream = await client.GetStreamAsync("https://www.vizit.ks.ua/novosti/");
            using (stream)
            {
                doc.Load(stream, true);
            }
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//h2[@class='entry-title']/a");
            if (nodes != null)
            {
                foreach (HtmlNode node in nodes)
                {
                    TextBlock block = new TextBlock();
                    block.FontSize = 20;
                    block.Text = node.InnerText.Replace("&#8220;", "'").Replace("&#8221;", "'");
                    block.MouseDown += (sender, e) =>
                    {
                        if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
                        {
                            System.Diagnostics.Process.Start(node.Attributes[0].Value);
                        }
                    };
                    if (node.InnerText.ToLower().Contains("вод")) block.Foreground = Brushes.Red;
                    blocks.Add(block);
                }
                list.ItemsSource = blocks;
            }
            else
            {
                MessageBox.Show("Something went wrong");
            }
        }
    }
}