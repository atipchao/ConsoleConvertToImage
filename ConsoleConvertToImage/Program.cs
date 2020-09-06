using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleConvertToImage
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = @"  
                <!DOCTYPE html>  
                    <html>  
                        <head>  
                            <style>  
                                p.uspsBarCode {font-family: USPSIMBCompact; font-size:14pt}
                          </style>  
                         </head>  
                    <body>  
                      <p class='uspsBarCode'>
                       FFTTDAADTTADTFDDFDDTFAFATDTDDFDAFDADDADDAFAAAFTTFTFDTFAAADADDDFDF 
                      </p>
                     </body>  
                    </html> ";
            StartBrowser(source);
            //Console.ReadLine();
            return ;
        }
        private static void StartBrowser(string source)
        {
            var th = new Thread(() =>
            {
                var webBrowser = new WebBrowser();
                webBrowser.ScrollBarsEnabled = false;
                webBrowser.IsWebBrowserContextMenuEnabled = true;
                webBrowser.AllowNavigation = true;

                webBrowser.DocumentCompleted += webBrowser_DocumentCompleted;
                webBrowser.DocumentText = source;

                Application.Run();
            });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }
        static void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var webBrowser = (WebBrowser)sender;
            using (Bitmap bitmap =
                new Bitmap(
                    webBrowser.Width = 290,
                    webBrowser.Height = 45))
            {
                webBrowser
                    .DrawToBitmap(
                    bitmap,
                    new System.Drawing
                        .Rectangle(0, 0, 290, 45));
                bitmap.Save(@"IMB.jpg",
                    System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            // Kill the thread - we done with the barcode
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
