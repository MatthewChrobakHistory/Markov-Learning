using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EclipseChatDownloader
{
    public partial class Browser : Form
    {
        public Browser()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var downloader = new EclipseChatStorer();
            
            for (int i = 0; i < 650; i++) {
                downloader.StoreLog(i);
            }
        }
    }
}
