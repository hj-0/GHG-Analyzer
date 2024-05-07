using System;
using System.Xml.XPath;

namespace GreenHouseGases
{
    public class MainMenuMethods
    {
        public static string processMenu()
        {
            string input;
            bool valid = false;

            do
            {
                Console.WriteLine("\nGreenhouse Gas Emissions in Canada");
                Console.WriteLine("==================================\n");
                Console.WriteLine("'Y' to adjust the range of years");
                Console.WriteLine("'R' to select a region");
                Console.WriteLine("'S' to select a specific GHG source");
                Console.WriteLine("'X' to exit the program");

                Console.Write("Your Selection: ");
                input = Console.ReadLine().ToUpper();

                switch (input)
                {
                    case "Y":
                    case "R":
                    case "S":
                        valid = true;
                        break;
                    case "X":
                        Console.Write("\nPress any key to continue");
                        Console.ReadKey();
                        Console.WriteLine("\n\nAll Done!");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("\nERROR: Please enter a valid choice from the menu and try again.\n");
                        break;
                }
            }
            while (!valid);

            return input;
        }

        public static void adjustRangeYears()
        {
            int startYear, endYear;
            string startInput, endInput;

            do
            {
                Console.Write("\nStarting year (1990 to 2019): ");
                startInput = Console.ReadLine();

                if (int.TryParse(startInput, out startYear))
                {
                    if (startYear >= 1990 && startYear <= 2019)
                    {
                        Program.defaultStartYear = startYear;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\nERROR: Starting year must be an integer between 1990 and 2019");
                    }
                }
                else
                {
                    Console.WriteLine("\nERROR: Invalid Numeric Input.");
                }
            }
            while (true);

            do
            {
                Console.Write("\nEnding year (1990 to 2019): ");
                endInput = Console.ReadLine();

                if (int.TryParse(endInput, out endYear))
                {
                    if(endYear >= 1990 && endYear <= 2019)
                    {
                        if(endYear - Program.defaultStartYear <= 5 && endYear - Program.defaultStartYear >= 0)
                        {
                            Program.defaultEndYear = endYear;
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"\nERROR: Ending year must be an integer between {startYear} and {startYear + 4}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nERROR: Starting year must be an integer between 1990 and 2019");
                    }
                }
                else
                {
                    Console.WriteLine("\nERROR: Invalid Numeric Input.");
                }
            }
            while (true);

            Console.Write("\nPress any key to continue.");
            Console.ReadKey();
            Console.Clear();
        }

        public static void selectRegion(XPathNavigator nav)
        {
            int counter = 1, regionSelection;
            string queryTextRegion = "//region/@name", regionInput;
            XPathNodeIterator regionNameIt = nav.Select(queryTextRegion);

            Console.WriteLine("\nSelect a region by number as shown below...");

            while (regionNameIt.MoveNext())
            {
                Console.WriteLine($"{counter, 3}. {regionNameIt.Current.Value}");
                counter++;
            }

            do
            {
                Console.Write("\nEnter a region #: ");
                regionInput = Console.ReadLine();

                if (!int.TryParse(regionInput, out regionSelection))
                {
                    Console.WriteLine("\nERROR: Invalid Numeric Input.");
                }
                else if(regionSelection < 1 || regionSelection > 15)
                {
                    Console.WriteLine("\nERROR: Please enter a valid region # from the list above and try again");
                }
                else
                {
                    break;
                }
            }
            while (true);

            string queryTextProvince = string.Format("//region[{0}]/@name", regionSelection);
            XPathNodeIterator provinceNameIt = nav.Select(queryTextProvince);

            while (provinceNameIt.MoveNext())
            {
                string title = $"\nEmissions in {provinceNameIt.Current.Value} (Megatonnes)";
                Console.WriteLine(title);
                for(int i = 1; i < title.Length; i++)
                {
                    Console.Write("-");
                }
                Console.WriteLine();
            }
            Console.Write($"\n{"Source",54}");

            for(int i = Program.defaultStartYear; i <= Program.defaultEndYear; i++)
            {
                Console.Write($"{i,9}");
            }
            Console.WriteLine();

            string queryTextDesc = string.Format("//region[{0}]/source/@description", regionSelection);
            string queryTextEmissionsByYear;
            XPathNodeIterator descIt = nav.Select(queryTextDesc);
            XPathNodeIterator emissionsIt;
            double roundedValue;
            string dash = "-", value;


            Console.WriteLine();
            while (descIt.MoveNext())
            {
                Console.Write($"{descIt.Current.Value, 54}");

                queryTextEmissionsByYear = string.Format("//region[{0}]", regionSelection) + "/source[@description='" + descIt.Current.Value + "']" + 
                    "/emissions[@year >=" + Program.defaultStartYear + " and" + " @year <= " + Program.defaultEndYear + "]";
                emissionsIt = nav.Select(queryTextEmissionsByYear);

                for (int i = Program.defaultStartYear; i <= Program.defaultEndYear; i++)
                {
                    if (emissionsIt.MoveNext())
                    {
                        roundedValue = Double.Parse(emissionsIt.Current.Value);
                        roundedValue = Math.Round(roundedValue, 3);
                        value = string.Format("{0:0.000}", roundedValue);

                        Console.Write($"{value,9}");
                    }
                    else
                    {
                        Console.Write($"{dash,9}");
                    }
                }
                Console.WriteLine();
            }
            Console.Write("\nPress any key to continue.");
            Console.ReadKey();
            Console.Clear();
        }

        public static void selectSpecificGHG(XPathNavigator nav)
        {
            int counter = 1, sourceSelection;
            string queryTextSourceNames = "//region[1]/source/@description", sourceInput;
            XPathNodeIterator sourceNamesIt = nav.Select(queryTextSourceNames);

            Console.WriteLine("\nSelect a source by number as shown below...");

            while (sourceNamesIt.MoveNext())
            {
                Console.WriteLine($"{counter,3}. {sourceNamesIt.Current.Value}");
                counter++;
            }
            do
            {
                Console.Write("\nEnter a source #: ");
                sourceInput = Console.ReadLine();

                if (!int.TryParse(sourceInput, out sourceSelection))
                {
                    Console.WriteLine("\nERROR: Invalid Numeric Input.");
                }
                else if(sourceSelection < 1 || sourceSelection > 8)
                {
                    Console.WriteLine("\nERROR: Please enter a valid source # from the list above and try again");
                }
                else
                {
                    break;
                }
            }
            while (true);

            string queryTextSource = string.Format("//region[1]/source[{0}]/@description", sourceSelection);
            XPathNodeIterator sourceIt = nav.Select(queryTextSource);

            while (sourceIt.MoveNext())
            {
                string title = $"\nEmissions from {sourceIt.Current.Value} (Megatonnes)";
                Console.WriteLine(title);
                for (int i = 1; i < title.Length; i++)
                {
                    Console.Write("-");
                }
                Console.WriteLine();
            }
            Console.Write($"\n{"Region",54}");

            for (int i = Program.defaultStartYear; i <= Program.defaultEndYear; i++)
            {
                Console.Write($"{i,9}");
            }
            Console.WriteLine();

            string queryTextAllRegions = "//region/@name";
            string queryTextEmissionsByYear;
            XPathNodeIterator allRegionsIt = nav.Select(queryTextAllRegions);
            XPathNodeIterator emissionsIt;
            double roundedValue;
            string dash = "-", value;


            Console.WriteLine();
            while (allRegionsIt.MoveNext())
            {
                Console.Write($"{allRegionsIt.Current.Value,54}");

                queryTextEmissionsByYear = string.Format("//region[@name ='{0}']/source[@description ='{1}']",allRegionsIt.Current.Value, sourceIt.Current.Value) +
                    "/emissions[@year >=" + Program.defaultStartYear + " and" + " @year <= " + Program.defaultEndYear + "]";
                emissionsIt = nav.Select(queryTextEmissionsByYear);

                for (int i = Program.defaultStartYear; i <= Program.defaultEndYear; i++)
                {
                    if (emissionsIt.MoveNext())
                    {
                        roundedValue = Double.Parse(emissionsIt.Current.Value);
                        roundedValue = Math.Round(roundedValue, 3);
                        value = string.Format("{0:0.000}", roundedValue);

                        Console.Write($"{value,9}");
                    }
                    else
                    {
                           Console.Write($"{dash,9}");
                    }
                }
                Console.WriteLine();
            }
            Console.Write("\nPress any key to continue.");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
