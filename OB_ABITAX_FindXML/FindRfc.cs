using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OB_ABITAX_FindXML
{
    class FindRfc
    {
        public bool FindRfcinList(int FindByRfc, string SearchRfc)
        {
            string RFCEmisorProcess = ConfigurationManager.AppSettings["RFCEmisorProcess"];
            string RFCReceptorProcess = ConfigurationManager.AppSettings["RFCReceptorProcess"];
            var rfcFile = (FindByRfc == 1) ? File.ReadAllLines(RFCEmisorProcess) : File.ReadAllLines(RFCEmisorProcess);
            var rfcFileList = new List<string>(rfcFile);
            //foreach(var rfc in rfcFileList)
            //{
            //    Console.WriteLine("RFC " + rfc);
            //}
            return rfcFileList.Exists(e => e.Equals(SearchRfc));
        }
    }
}
