using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;





/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{


    public class Submission
    {
        public static string xmlURL = "https://raw.githubusercontent.com/ThomasFilby/cse445_a4/refs/heads/main/NationalParks.xml";
        public static string xmlErrorURL = "https://raw.githubusercontent.com/ThomasFilby/cse445_a4/refs/heads/main/NationalParksErrors.xml";
        public static string xsdURL = "https://raw.githubusercontent.com/ThomasFilby/cse445_a4/refs/heads/main/NationalParks.xsd";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }
     
        // Q2.1
        //validates the XML at xmlURL against the xsd at xsdUrl, returns errors or no errors.
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            string errors = ""; //set errors
            if ( xmlUrl == null){ // check if xml is null
                return "xml empty";
            }else{
                XmlReaderSettings settings = new XmlReaderSettings(); //create new reader settings
                settings.ValidationType = ValidationType.Schema; //assign settings to type Schem

                settings.ValidationEventHandler += (sender, args) => //subscription to track total errors
                    { 
                    errors += args.Message + " Line Number: " + args.Exception.LineNumber +  //compile  formatted error results
                    " Position: " + args.Exception.LinePosition + "\n";
                    };

                try {
                    settings.Schemas.Add(null, xsdUrl);//add xsd
                    XmlReader reader = XmlReader.Create(xmlUrl, settings);//create new reader
                    while (reader.Read()){}//let it read   
                    }
                   
                    catch(XmlException Error) //xml errors
                {
                    return errors + Error.Message + " Line Number: " + Error.LineNumber + " Position: " + Error.LinePosition;
                }
                catch (Exception Error) //for anything else
                {
                    return "Error: " + Error.Message;
                }

            if (errors == "")
                {
                return "No errors are found";    
                }
            }
            return errors;
        }
            //return "No errors are found" if XML is valid. Otherwise, return the desired exception message.
        
 
        //converts a XML files at xmlUrl and converts it to JSON
        //root attributes and comments are removed for the sake of output
        public static string Xml2Json(string xmlUrl)
        {
            string text = DownloadContent(xmlUrl);//get content from helper
            XmlDocument document = new XmlDocument();//assign xml document
            document.LoadXml(text);//load text into doc
            document.DocumentElement.Attributes.RemoveAll();//remove attributes from root tag
            XmlNodeList comments = document.SelectNodes("//comment()");//xml comments interfering with JSON conversion, so comments are ignored
            for ( int i = comments.Count -1; i >= 0; i--)
            {
                comments[i].ParentNode.RemoveChild(comments[i]);
            }
            string jsonText = JsonConvert.SerializeXmlNode(document, Newtonsoft.Json.Formatting.Indented);//returned as formatted JSON
            return jsonText;
            // The returned jsonText needs to be de-serializable by Newtonsoft.Json package. (JsonConvert.DeserializeXmlNode(jsonText))
        }

        // Helper method to download content from URL
        private static string DownloadContent(string url)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }

}
