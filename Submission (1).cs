using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text.Json.Serialization;



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
        public static string xmlErrorURL = "https://raw.githubusercontent.com/ThomasFilby/cse445_a4/refs/heads/main/NatioanlParksErrors.xml";
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
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            string errors = ""; //set errors
            if ( xmlUrl == null){ // check if xml is null
                return "xml empty";
            }else{
                XmlReaderSettings settings = new XmlReaderSettings(); //create new reader settings
                settings.ValidationType = ValidationType.Schema; //assign settings to type Schem

                try {
                    settings.Schemas.Add(null, xsdUrl);//add xsd
                    XmlReader reader = XmlReader.Create(xmlUrl, settings);//create new reader
                    while (reader.Read()){}//let it read   
                    }
                    catch(XmlSchemaValidationException Error)// broke xsd rules
                    {
                        return Error.Message + " Line Number: " + Error.LineNumber + " Position: " + Error.LinePosition;//return content and position
                    }
                    catch(XmlException Error) //xml errors
                {
                    return Error.Message + " Line Number: " + Error.LineNumber + " Position: " + Error.LinePosition;
                }
                catch (Exception Error) //for anything else
                {
                    return "Error: " + Error.Message;
                }

            
                return "No errors are found";
            
            }
        }
            //return "No Error" if XML is valid. Otherwise, return the desired exception message.
        
 
//2.2
        public static string Xml2Json(string xmlUrl)
        {
            string text = DownloadContent(xmlUrl);//get content from helper
            XmlDocument document = new XmlDocument();//assign xml document
            document.LoadXml(text);//load text into doc
            document.DocumentElement.Attributes.RemoveAll();//remove attributes from root tag
            return JsonConvert.SerializeXmlNode(document, Newtonsoft.Json.Formatting.Indented);//returned as formatted JSON
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
