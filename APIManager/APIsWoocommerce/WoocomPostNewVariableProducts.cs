using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

namespace APIManager.APIsWoocommerce
{
    class WoocomPostNewVariableProducts
    {
        public static void Woocommercepostnewproducts()
        {
            string sku1 = "";
            Console.WriteLine("conectar a base dados...");
            SqlConnection conn = new("Data Source=192.168.1.15;Initial Catalog=TESTE_Ricardo;Integrated Security=SSPI; MultipleActiveResultSets=true");

            bool breakFlag = true;
            try
            {
                while (breakFlag)
                {
                    int temp;
                    string sent;
                    string querysent = @"SELECT TOP 1
                    count(*) FROM [TESTE_Ricardo].[dbo].[woocommerceprod]
                    where sent=0";

                    SqlCommand cmd0 = new(querysent, conn);
                    conn.Open();
                    temp = Convert.ToInt32(cmd0.ExecuteScalar().ToString());
                    sent = temp.ToString();
                    conn.Close();
                    cmd0.Dispose();

                    if (sent == "0")
                    {
                        breakFlag = false;
                        break;
                        //return;
                    }

                    var dateTimeNow = DateTime.Now;
                    var date1 = dateTimeNow.ToShortDateString();
                    //string date1 = "01/04/2021";
                    Console.WriteLine(date1);
                    string query = @"SELECT TOP 1
                    [name]
                    ,[type]    
                    ,[description]
	                ,[short_description]
	                ,[sku]
	                ,price
	                ,stock
	                ,[manage_stock]	  
	                FROM [TESTE_Ricardo].[dbo].[woocommerceprod]
                    where sent=0";

                    //where orderdate > '" + date1 + "' and sent=0"

                    SqlCommand cmd = new(query, conn);
                    conn.Open();
                    Console.WriteLine("Conecção a base dados ok!");
                    Console.Write(Environment.NewLine);
                    Console.WriteLine("executar comando para construcao do json");
                    System.Threading.Thread.Sleep(2000);
                    Console.Write(Environment.NewLine);
                    var dataReader = cmd.ExecuteReader();
                    dataReader.Read();
                    string rootName = dataReader["name"].ToString();
                    string rootType = dataReader["type"].ToString();
                    string rootDescription = dataReader["description"].ToString();
                    string rootShort_description = dataReader["short_description"].ToString();
                    sku1 = dataReader["sku"].ToString();
                    string rootSku = sku1;
                    string rootRegular_price = dataReader["price"].ToString();
                    string stock_quantity1 = dataReader["stock"].ToString();
                    int rootStock_quantity = Convert.ToInt32(stock_quantity1);
                    string rootManage_stock = dataReader["manage_stock"].ToString();
                    conn.Close();
                    cmd.Dispose();
                    dataReader.Close();

                    query = @"SELECT idcat FROM
                    [TESTE_Ricardo].[dbo].woocommercecat
		            where [TESTE_Ricardo].[dbo].woocommercecat.sku = '" + sku1 + "'";
                    conn.Open();
                    List<int> dataListcat = new();
                    DataTable cat = new();
                    new SqlDataAdapter(query, conn).Fill(cat);
                    dataListcat = cat.Rows.OfType<DataRow>().Select(dr => dr.Field<int>("idcat")).ToList();
                    List<int> CategoryId = dataListcat;
                    conn.Close();
                    //dataListcat.ForEach(Console.WriteLine);

                    query = @"SELECT src FROM
                    [TESTE_Ricardo].[dbo].woocommerceimg
		            where [TESTE_Ricardo].[dbo].woocommerceimg.sku = '" + sku1 + "'";
                    conn.Open();
                    List<string> dataListsrc = new();
                    DataTable src = new();
                    new SqlDataAdapter(query, conn).Fill(src);
                    dataListsrc = src.Rows.OfType<DataRow>().Select(dr => dr.Field<string>("src")).ToList();
                    List<string> imageSrc = dataListsrc;
                    conn.Close();

                    query = @"SELECT options FROM
                    [TESTE_Ricardo].[dbo].woocommerceatt
		            where [TESTE_Ricardo].[dbo].woocommerceatt.sku = '" + sku1 + @"' and 
                    [TESTE_Ricardo].[dbo].woocommerceatt.idatt = 1";
                    conn.Open();
                    DataTable opt1 = new();
                    new SqlDataAdapter(query, conn).Fill(opt1);
                    List<string> dataListopt1 = new();
                    dataListopt1 = opt1.Rows.OfType<DataRow>().Select(dr => dr.Field<string>("options")).ToList();
                    List<string> attributeOptions1 = dataListopt1;
                    conn.Close();
                    //dataListopt1.ForEach(Console.WriteLine);

                    query = @"SELECT options FROM
                    [TESTE_Ricardo].[dbo].woocommerceatt
		            where [TESTE_Ricardo].[dbo].woocommerceatt.sku = '" + sku1 + @"' and 
                    [TESTE_Ricardo].[dbo].woocommerceatt.idatt = 2";
                    conn.Open();
                    DataTable opt2 = new();
                    new SqlDataAdapter(query, conn).Fill(opt2);
                    List<string> dataListopt2 = new();
                    dataListopt2 = opt2.Rows.OfType<DataRow>().Select(dr => dr.Field<string>("options")).ToList();
                    List<string> attributeOptions2 = dataListopt2;
                    conn.Close();
                    //dataListopt2.ForEach(Console.WriteLine);

                    //json serialize
                    JObject produto =
                        new(
                            new JProperty(
                                "name", rootName
                                ),
                              new JProperty(
                             "type", rootType
                             ),
                              new JProperty(
                             "description", rootDescription
                             ),
                              new JProperty(
                             "Short_description", rootShort_description
                             ),
                              new JProperty(
                             "sku", rootSku
                             ),
                              new JProperty(
                             "regular_price", rootRegular_price
                             ),
                              new JProperty(
                             "stock_quantity", rootStock_quantity
                             ),
                              new JProperty(
                             "manage_stock", rootManage_stock
                             ),
                              new JProperty("categories",
                        new JArray(
                            from c in CategoryId
                            group c by c
                            into g
                            orderby g.Key descending
                            select new JObject(
                            new JProperty(
                                "id", g.Key
                                )))),
                             new JProperty("images",
                        new JArray(
                            from a in imageSrc
                            group a by a
                            into b
                            orderby b.Key descending
                            select new JObject(
                                new JProperty(
                                    "src", b.Key
                                    )))),
                            new JProperty("attributes",
                            new JArray(
                             new JObject(
                                new JProperty(
                                    "id", 1
                                    ),
                                 new JProperty(
                                    "visible", "true"
                                    ),
                                  new JProperty(
                                    "variation", "true"
                                    ),
                                   new JProperty(
                                    "options", attributeOptions1
                                    )),
                                   new JObject(
                                   new JProperty(
                                    "id", 2
                                    ),
                                 new JProperty(
                                    "visible", "true"
                                    ),
                                  new JProperty(
                                    "variation", "true"
                                    ),
                                   new JProperty(
                                    "options", attributeOptions2
                                    )
                                ))));

                    string output = JsonConvert.SerializeObject(produto);
                    //Rootobject deserializedProduct = JsonConvert.DeserializeObject<Rootobject>(output);

                    Console.WriteLine("construção completa");
                    Console.Write(Environment.NewLine);

                    string path = @"C:\Users\ricardo\Documents\filesjson\apiwoocommerce.json";
                    File.WriteAllText(path, output);
                    Console.WriteLine("finalizada a criacao do ficheiro json");
                    System.Threading.Thread.Sleep(1000);
                    Console.Write(Environment.NewLine);

                    Console.WriteLine("conectar ao Woocommerce para inserir produtos");
                    System.Threading.Thread.Sleep(1000);
                    Console.Write(Environment.NewLine);
                    try
                    {
                        using (var wb = new WebClient())
                        {
                            //wb.Encoding = Encoding.UTF8;
                            wb.Headers.Add("Content-Type:application/json");
                            var response = wb.UploadString($"https://teste.api.de/wp-json/wc/v3/products?consumer_key=ck_co86547cojhi3popoi3uy3uy&consumer_secret=cs_okef566oko5o65ojg56ot332o", output);
                        }
                        System.Threading.Thread.Sleep(2000);
                    }
                    catch (WebException ex)
                    {
                        if (ex.Response != null)
                        {
                            string path2 = @"C:\Users\ricardo\Documents\filesjson\logwebserviceerror.txt";
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
                    SqlCommand cmd99 = new("Update [TESTE_Ricardo].[dbo].[woocommerceprod] set [sent]=@a1 where sku='" + sku1 + "'", conn);
                    conn.Open();
                    cmd99.Parameters.Add("a1", SqlDbType.NVarChar, 50).Value = "1";
                    cmd99.ExecuteNonQuery();
                    conn.Close();
                    cmd99.Dispose();

                }
                Console.WriteLine("nao tem mais produtos para enviar, fechar coneccoes e programa");
                System.Threading.Thread.Sleep(1000);
                Console.Write(Environment.NewLine);

            }
            catch (Exception ex2)
            {
                string path3 = @"C:\Users\ricardo\Documents\filesjson\logerrors.txt";
                Console.WriteLine(ex2.ToString());
                System.Threading.Thread.Sleep(3000);
                using StreamWriter file = new(path3, true);
                file.Write(ex2);
                file.WriteLine(Environment.NewLine + DateTime.Now);
                file.WriteLine(Environment.NewLine + "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            }
            finally
            {
                conn.Close();
            }
        }
    }
}

