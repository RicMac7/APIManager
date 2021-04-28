using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace APIManager.APIsWorten
{
    class WortenPostNewVariableProducts
    {
        public static void Wortenpostnewproducts()
        {
            try
            {
                //string output = JsonConvert.SerializeObject(produto);
                //Rootobject deserializedProduct = JsonConvert.DeserializeObject<Rootobject>(output);           

                Console.WriteLine("construção completa");
                Console.Write(Environment.NewLine);

                //string path = @"C:\Users\ricardo.machado\Documents\filesjson\apiwoocommerce.json";
                //Directory.CreateDirectory(path);
                //File.WriteAllText(path, output);
                Console.WriteLine("finalizada a criacao do ficheiro json");
                System.Threading.Thread.Sleep(1000);
                Console.Write(Environment.NewLine);

                /*string xml = @"C:\Users\ricardo.machado\Documents\filesjson\";
                Directory.CreateDirectory(xml);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                string json = JsonConvert.SerializeXmlNode(doc);*/

                Console.WriteLine("conectar a Worten para inserir produtos");
                System.Threading.Thread.Sleep(1000);
                Console.Write(Environment.NewLine);
                try
                {
                    using (var wb = new WebClient())
                    {
                        var apiKey = "78334f-78ui-1367-zs45-67jmhy879";
                        //wb.Encoding = Encoding.UTF8;
                        wb.Headers.Add("Authorization", apiKey);
                        wb.Headers.Add("Content-Type:application/xml");
                        var response = wb.UploadFile($"https://marketplace.worten.pt/api/products/imports", @"C:\Users\ricardo\Documents\wortenapidocs\MOB0701.xml");
                    }
                    System.Threading.Thread.Sleep(2000);
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {                       
                        string path2 = @"C:\apilogs\logwebserviceerror.txt";
                        Directory.CreateDirectory(path2);
                        string response = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        Debug.WriteLine(response);
                        Console.WriteLine(ex.ToString());
                        System.Threading.Thread.Sleep(3000);
                        using StreamWriter file = new(path2, true);
                        file.Write(response);
                        file.WriteLine(Environment.NewLine + DateTime.Now);
                        file.WriteLine(Environment.NewLine + "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                    }
                }
                Console.WriteLine("nao tem mais produtos para enviar, fechar coneccoes e programa");
                System.Threading.Thread.Sleep(1000);
                Console.Write(Environment.NewLine);

            }
                catch (Exception ex2)
                {
                    string path3 = @"C:\apilogs\logerrors.txt";
                    Directory.CreateDirectory(path3);
                    Console.WriteLine(ex2.ToString());
                    System.Threading.Thread.Sleep(3000);
                    using StreamWriter file = new(path3, true);
                    file.Write(ex2);
                    file.WriteLine(Environment.NewLine + DateTime.Now);
                    file.WriteLine(Environment.NewLine + "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                }
                finally
                {
                    //conn.Close();
                }
        }
    }
}
