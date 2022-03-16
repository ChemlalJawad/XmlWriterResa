using System;
using System.Xml.Linq;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Xml;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Web;

namespace XmlWriterResa
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] FILES_XML = { "FaqTips.20220119.xml",
            "GeneralMeetings.20220119.xml",
            "News.20220119.xml",
            "Notification.20220119.xml",
            "Pages.20220119.xml",
            "Taxonomy.Categories.20220119.xml",
            "Taxonomy.FAQ-Themes.20220119.xml",
            "Taxonomy.News-Themes.20220119.xml" };
            // Lis chaque fichier XML
           /* foreach (var l in FILES_XML)
            {*/
                GetExtract("News.20220119.xml");
                //GetTranslate("");
            //}

           
        }
        public static Dictionary<string,Traduction> GetTranslate (string translatePath)
        {
            var result = new Dictionary<string, Traduction>();
            //var lines = File.ReadLines(@"C:\Users\jawad\OneDrive\Documents\VueJs Basic\translations_20220119\fresh_translations_20220119_DE_test.csv");
            
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = false,
                Comment = '#',
                AllowComments = false,
                Delimiter = ";",
            };
            using var streamReader = File.OpenText(@"C:\Users\jawad\OneDrive\Documents\VueJs Basic\translations_20220119\clean_translations_20220119_DE.csv");
            using var csvReader = new CsvReader(streamReader, csvConfig);
            while (csvReader.Read())
            {
                var key = csvReader.GetField(0);
                var french = csvReader.GetField(1);
                var deutch = csvReader.GetField(2);
                Traduction newTrad = new Traduction(key, french, deutch);

                result.Add(newTrad.key,newTrad);
            }

            return result;
        }
        public static void GetExtract(string pathFile)
        {
            string[] KEYS_TO_EXCLUDE = {"ID", "Date", "PostType", "Permalink", "url", "URL", "target", "Languages", "wp_page_template", "Status", "Slug", "Format", "Template", "Parent", "ParentSlug", "Order", "CommentStatus", "PingStatus", "PostModifiedDate", "ResaAPIoptions", "APIendpointURL", "APIdefaulttimeout", "Mollie", "MollieAPIkey", "FacebookAPIoptions", "FacebookPageID", "FacebookPageToken", "FacebookTag", "SocialNetworks", "Breadcrumb", "Afficherchemindenavigation", "header_picture", "header_mode", "header_button_tag", "header_video", "modifiers",
        "Header" };
            // , "post_translations", "_icon_slug","Icone" ,"FooterContact", "AuthorEmail", "AuthorFirstName", "AuthorLastName", "AuthorID", "AuthorUsername", "_icon_set", "_icon_color", "_answer", "_excerpté"
            var valeurChange = 0;

            var xml = XDocument.Load("c:\\Users\\jawad\\OneDrive\\Documents\\VueJs Basic\\resa.translations.20220119\\"+pathFile);
            XmlDocument doc = new XmlDocument();
            doc.Load("c:\\Users\\jawad\\OneDrive\\Documents\\VueJs Basic\\resa.translations.20220119\\" + pathFile);
            var query = from c in xml.Root.Descendants()
                        where !KEYS_TO_EXCLUDE.Contains(c.Name.ToString())
                        select c;

           /* foreach (string name in query)
            {
                if (name.Length > 0)
                {
                    foreach (KeyValuePair<string, Traduction> item in GetTranslate("test"))
                    {
                        var md5 = GetMD5(name);
                        if (md5 == item.Key)
                        {
                            valeurChange++;
                            var elementsToUpdate = xml.Descendants()
                                                      .Where(o => o.Value == name && !o.HasElements);

                            foreach (XElement element in elementsToUpdate)
                            { 
                                if (element.FirstNode.NodeType.ToString() == "CDATA")
                                {
                                    element.Value = item.Value.deutch;
                                }
                                else { element.Value = item.Value.deutch; }
                            }

                            //File.WriteAllText("c:\\Users\\jawad\\OneDrive\\Documents\\VueJs Basic\\resa.translations.20220119\\new_" + pathFile, xml.ToString());
                            xml.Save("c:\\Users\\jawad\\OneDrive\\Documents\\VueJs Basic\\resa.translations.20220119\\new_" + pathFile);
                            //Console.WriteLine("Valeur a changer : {0}\n EN : {1}", name, item.Value);

                        }
                    }
                }
            }*/
            Console.WriteLine("Valeur changé : {0}", valeurChange); 
        }

        public static string GetMD5(string encryptString)
        {
            var passByteCrypt = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(encryptString));

            return ByteArrayToString(passByteCrypt);
        }
        public static string ByteArrayToString(byte[] bytes)
        {
            var output = new StringBuilder(bytes.Length);

            foreach (var t in bytes)
            {
                output.Append(t.ToString("X2"));
            }

            return output.ToString().ToLower();
        }
    }
}

/*foreach (var l in lines)
{
    var lsplit = l.Split(";");
    if (lsplit.Length > 2)
    {
        var newkey = lsplit[0];
        var deVal = lsplit[2];   
        result[newkey] = deVal;
    }
    else
    {
        if (lsplit.Length > 1 && lsplit.Length < 3) { 
            var newkey = lsplit[0];
            var frval = lsplit[1];
            result[newkey] = frval;
        }
    }
}
if (lines.Contains("90630eadd6d588188d103364d2aa7673")) { Console.WriteLine("existe"); }*/
