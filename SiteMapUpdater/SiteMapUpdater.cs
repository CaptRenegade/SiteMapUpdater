using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceProcess;
using System.IO;
using System.Xml;
using System.Net;
using System.Timers;

namespace SiteMapUpdater
{
    public partial class SiteMapUpdater : ServiceBase
    {

        static string path = Environment.CurrentDirectory + "\\settings.config";
        string sPath = "";
        string s2Path = "";
        string tmp = "";
        string NewsPath = "";

        AESEncryption AES = new AESEncryption();

        public SiteMapUpdater()
        {
            InitializeComponent();
        }
        
        Timer timer1 = new Timer();

        protected override void OnStart(string[] args)
        {
            timer1.Interval = 24 * 3600000; //every 1 day
            timer1.Elapsed += new ElapsedEventHandler(daily_update);
            timer1.Enabled = true;
        }

        protected override void OnStop()
        {
            timer1.Enabled = false;
        }

        private void daily_update(object sender, EventArgs e)
        {
            if (File.Exists(path))
            {               
                foreach (string line in File.ReadAllLines(path))
                {
                    sPath = AES.Decrypt(line) + "\\sitemap.xml";
                    s2Path = AES.Decrypt(line) + "\\sitemap2.xml";
                    tmp = AES.Decrypt(line) + "\\tmpfile.tmp";
                    NewsPath = AES.Decrypt(line) + "\\sitemap.xml";

                    if (File.Exists(sPath))
                    {
                        WriteSiteMapXML();
                    }
                   
                }
            }          
        }

        //*********START DAILY UPDATE****************
        public void WriteSiteMapXML()
        {
            if (!File.Exists(s2Path))
            {
                StreamWriter sw = File.CreateText(s2Path);
                {
                    for (int i = 0; i <= GetSiteMapXMLStream().Length - 1; i++)
                    {
                        sw.WriteLine(GetSiteMapXMLStream().GetValue(i).ToString());
                    }

                    DataSet ds = GetSiteMapXML();


                    for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                    {
                        ds.Tables[0].Rows[i][1] = DateTime.Now.ToString("s") + "+00:00";
                    }


                    sw.WriteLine("");
                    sw.WriteLine("");
                    sw.Close();


                    FileStream fappend = File.Open(tmp, FileMode.Append);
                    {
                        foreach (DataTable dtt in ds.Tables)
                        {
                            dtt.WriteXml(fappend);
                        }
                        fappend.Close();

                    }


                    FileStream XMLAppend = File.Open(s2Path, FileMode.Append);
                    {
                        StreamWriter append = new StreamWriter(XMLAppend);
                        {
                            int i = 0;
                            foreach (string line in GetSiteMapTMPStream(tmp))
                            {
                                if (i != 0 && i != GetSiteMapTMPStream(tmp).Length)
                                {
                                    append.WriteLine(line);
                                }
                                i++;
                            }
                            append.Close();
                        }

                    }

                }


            }

            File.Delete(tmp);
            File.Delete(sPath);
            File.Copy(s2Path, NewsPath);
            File.Delete(s2Path);
        }
        //*********END DAILY UPDATE****************


        //*********REFERENCED FUNCTIONS DEFINITIONS****************
        public Array GetSiteMapXMLStream()
        {
            List<string> Lines = new List<string>();
            StreamReader sr = File.OpenText(sPath);
            {
                string line;
                line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    Lines.Add(line);
                    line = sr.ReadLine();
                }
                sr.Close();
            }

            return Lines.ToArray();
        }

        public Array GetSiteMapTMPStream(string tmpfile)
        {
            List<string> Lines = new List<string>();
            StreamReader sr = File.OpenText(tmpfile);
            {
                string line;
                line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    Lines.Add(line);
                    line = sr.ReadLine();
                }
                sr.Close();
            }

            return Lines.ToArray();

        }

        public DataSet GetSiteMapXML()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            XmlReader XML = XmlReader.Create(sPath, new XmlReaderSettings());
            DataSet ds = new DataSet();
            ds.ReadXml(XML);
            XML.Close();

            return ds;
        }



    }
}
