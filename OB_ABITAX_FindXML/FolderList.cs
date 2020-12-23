using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OB_ABITAX_FindXML
{
    class FolderList
    {
        public void GetFolderList(int allFolders, String folderpath, int FindByRfc, string filenameWrite)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(folderpath, "*", SearchOption.TopDirectoryOnly);
                Console.WriteLine("The number of directories is {0}.", dirs.Length);
                foreach (string dir in dirs)
                {
                    if(allFolders == 0)
                    {
                        //Console.WriteLine("Subfolder " + dir);
                        string[] fileEntries = Directory.GetFiles(dir);
                        foreach (string fileName in fileEntries)
                        {
                            //Console.WriteLine("File " + fileName);
                            CfdiXml cfdi = new CfdiXml();
                            cfdi.ReadXML(fileName, FindByRfc, filenameWrite);
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
