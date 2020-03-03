//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Text.RegularExpressions;
//using System.Threading;
//using System.Web;

//namespace KBCC.Helper
//{
//    public class HtmlHelper
//    {
//        private double GetIdValue(string url, int index)
//        {
//            string code = "";
//            Regex r1 = new Regex("\\d+");
//            Match m1 = r1.Match(url);
//            while (m1.Success)
//            {
//                code += m1.Value + ';';
//                m1 = m1.NextMatch();
//            }
//            return double.Parse(code.Split(';')[index - 1]);
//        }
//        public bool IsNumber(string pText)
//        {
//            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
//            return regex.IsMatch(pText);
//        }
//        public string GetInfo(string url, int pattern)
//        {
//            if (IsExistUrl(url))
//            {
//                if (GetTitle(GetWebContent(url)) != "none")
//                {
//                    //dt.Rows.Add(i, getAuthor(txturl.Text), GetTitle(GetWebContent(txturl.Text.Replace(id.ToString(), i.ToString()))), LayNoiDung(GetWebContent(url), pattern), "trang nhất");
//                }
//            }

//            Thread.Sleep(3000);
//            return "";
//        }

//        public bool IsvalidUrl(string url)
//        {
//            Uri uriResult;
//            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
//                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
//            return result;
//        }
//        public bool IsExistUrl(string url)
//        {
//            if (IsvalidUrl(url) == true)
//            {
//                WebRequest webRequest = WebRequest.Create(url);
//                WebResponse webResponse;
//                try
//                {
//                    webResponse = webRequest.GetResponse();
//                }
//                catch //If exception thrown then couldn't get response from address
//                {
//                    return false;
//                }
//                return true;
//            }
//            return false;
//        }
//        public string GetWebContent(string strLink)
//        {
//            string strContent = "";
//            try
//            {
//                WebRequest objWebRequest = WebRequest.Create(strLink);
//                objWebRequest.Credentials = CredentialCache.DefaultCredentials;
//                WebResponse objWebResponse = objWebRequest.GetResponse();
//                Stream receiveStream = objWebResponse.GetResponseStream();
//                StreamReader readStream = new StreamReader(receiveStream, System.Text.Encoding.UTF8);
//                strContent = readStream.ReadToEnd();
//                objWebResponse.Close();
//                readStream.Close();
//            }
//            catch (Exception ex)
//            {
//                return ex.Message;
//            }
//            return strContent;
//        }

//        public string GetTitle(string Content)
//        {
//            string pattern = "<title>[^>]*";
//            Regex Title = new Regex(pattern);
//            Match m = Title.Match(Content);
//            if (m.Success)
//            {
//                return m.Value.Substring(7, m.Value.Length - 14);
//            }
//            return "none";
//        }
//        public string GetAuthor(string url)
//        {
//            return url.Split('/')[2].ToString();
//        }

        
//        public string GetContent(string Content, string patternPart)
//        {
//            //truyền pattern vào, có website div id = content,có web là topic,..

//            string pattern = @"<" + patternPart + ">[^~]+";
//            Regex title = new Regex(pattern);
//            int num = pattern.Length - 5;
//            Match m = title.Match(Content);
//            if (m.Success)
//            {
//                return m.Value.Substring(num, m.Value.Length - num);
//            }
//            return num.ToString() + pattern;
//        }
//    }
//}