using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VMKeyboard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Version ver = new Version(Application.ProductVersion);
            this.Text = "VMKeyboard " + ver.Major + "." + ver.Minor;
        }

        private void Button_Type_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Button Type Clicked");
            this.TypeContentAsync(this.Text_Main.Text);
            //this.TypeContentAsync(this.Text_Main.Text);
        }

        private async Task TypeContentAsync(string content)
        {
            await Task.Delay((int)this.Text_Timeout.Value * 1000);

            KeyboardEmulator.TypeText(content);
            ////SendKeys.Send(content);
            //string badChars = "{}[]()+^%~";
            //// Simulate typing the content
            //foreach (char c in content)
            //{
            //    string value = c.ToString();
            //    if (badChars.Contains(c))
            //    {
            //        value = "{" + c + "}";
            //    }
            //    SendKeys.Send(value);
            //}
        }
    }
}
