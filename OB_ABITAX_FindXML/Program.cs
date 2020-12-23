using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OB_ABITAX_FindXML
{
    class Program
    {
        static void Main(string[] args)
        {
            string nameApp = ConfigurationManager.AppSettings["NameApp"];
            string folderpath = ConfigurationManager.AppSettings["InputFolder"];
            string RequestUUIDsProcess = ConfigurationManager.AppSettings["RequestUUIDsProcess"];
            string RFCEmisorProcess = ConfigurationManager.AppSettings["RFCEmisorProcess"];
            string RFCReceptorProcess = ConfigurationManager.AppSettings["RFCReceptorProcess"];
            String obToken = string.Empty;

            Console.WriteLine("CFDI Process " + nameApp);
            Console.WriteLine("==============================");
            Console.WriteLine("Select option for Process Type ");
            Console.WriteLine("1 -> Process to OnBase all Files and subfolders contained in Input Folder ("+ folderpath + ")");
            Console.WriteLine("2 -> Process to OnBase Edifact Request UUIDs Only (" + RequestUUIDsProcess + ")");
            Console.WriteLine("3 -> Find and obtain XML Files for RFC Emisor or Receptor");
            String processType = Console.ReadLine();
            switch(processType)

            {
                case "1":
                    Console.WriteLine("Enter OnBase Session ID");
                    obToken = Console.ReadLine();
                    break;
                case "2":
                    Console.WriteLine("Enter OnBase Session ID");
                    obToken = Console.ReadLine();
                    break;
                case "3":
                    Console.WriteLine("Select option for Process Type");
                    Console.WriteLine("A -> Find by RFC Emisor (" + RFCEmisorProcess + ")");
                    Console.WriteLine("B -> Find by RFC Receptor (" + RFCReceptorProcess + ")");
                    String findType = Console.ReadLine();
                    Console.WriteLine("FileName in Console? 0 = falso 1 = true");
                    String filenameWrite = Console.ReadLine();
                    switch (findType)
                    {
                        case "A":
                            FolderList folderlistA = new FolderList();
                            folderlistA.GetFolderList(0,folderpath,1, filenameWrite);
                            Console.ReadKey();
                            break;
                        case "B":
                            FolderList folderlistB = new FolderList();
                            folderlistB.GetFolderList(0,folderpath,2, filenameWrite);
                            Console.ReadKey();
                            break;
                        default:
                            Console.WriteLine("Invalid Option");
                            Console.ReadKey();
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid Option");
                    Console.ReadKey();
                    break;
            }
        }
    }
}
