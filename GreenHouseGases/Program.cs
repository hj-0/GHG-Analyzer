using System;
using System.Xml;
using System.Xml.XPath;

/*
 * Program: Green House Gases in Canada .NET Core Application		
 * Purpose: uses an XML file for data storage and generates parameterized reports on console based on user selections		
 * Coder: Haris	
 * Date: June 28th, 2021			
 */

namespace GreenHouseGases
{
    public class Program
    {
        private const string XML_FILE = "ghg-canada.xml";
        public static int defaultStartYear = 2015;
        public static int defaultEndYear = 2019;
        public static void Main(string[] args)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(XML_FILE);

                XPathNavigator nav = doc.CreateNavigator();

                string menuChoice;
                do
                {
                    menuChoice = MainMenuMethods.processMenu();
                    switch (menuChoice)
                    {
                        case "Y":
                            MainMenuMethods.adjustRangeYears();
                            break;
                        case "R":
                            MainMenuMethods.selectRegion(nav);
                            break;
                        case "S":
                            MainMenuMethods.selectSpecificGHG(nav);
                            break;
                        case "X":
                            break;
                    }
                }
                while (!menuChoice.Equals("X"));
            }
            catch(XmlException err)
            {
                Console.WriteLine("\nXML ERROR: " + err.Message);
            }
            catch(XPathException err)
            {
                Console.WriteLine("\nXPath ERROR: " + err.Message);
            }
            catch (Exception err)
            {
                Console.WriteLine("\nERROR: " + err.Message);
            }
        }
    }
}
