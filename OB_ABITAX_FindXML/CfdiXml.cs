using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace OB_ABITAX_FindXML
{
    class CfdiXml
    {
        public void ReadXML(string fullPathFile, int FindByRfc, string filenameWrite)
        {
            bool fullpathWrite = (filenameWrite == "1") ? true : false;
            try
            {
                // Load the document and set the root element.  
                XmlDocument doc = new XmlDocument();
                doc.Load(fullPathFile);

                // namespace manager added here
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmgr.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                XmlElement cfdi = doc.DocumentElement;
                //gets general fields from cfdi:Comprobante
                string strxmlVersion = string.Empty;
                if (cfdi.HasAttribute("Version"))
                {
                    XmlAttribute xmlVersion = cfdi.GetAttributeNode("Version");
                    strxmlVersion = xmlVersion.InnerXml;
                }
                if (cfdi.HasAttribute("version"))
                {
                    XmlAttribute xmlVersion = cfdi.GetAttributeNode("version");
                    strxmlVersion = xmlVersion.InnerXml;
                }
                //Console.WriteLine("strxmlVersion " + strxmlVersion);
                //Console.ReadKey();

                string strxmlFecha = string.Empty;
                string strxmlTComp = string.Empty;
                string strXmlUuid = string.Empty;
                string strXmlEmisorRfc = string.Empty;
                string strXmlReceptorRfc = string.Empty;

                //gets general fields from cfdi:Complemento & Timbre Fiscal
                XmlElement cfdiComp = (XmlElement)cfdi.SelectSingleNode("cfdi:Complemento", nsmgr);
                XmlElement cfdiCompTimbre = (XmlElement)cfdiComp.SelectSingleNode("tfd:TimbreFiscalDigital", nsmgr);
                XmlAttribute xmlUuid = cfdiCompTimbre.GetAttributeNode("UUID");
                strXmlUuid = xmlUuid.InnerXml;

                if (strxmlVersion == "3.3")
                {
                    XmlAttribute xmlFecha = cfdi.GetAttributeNode("Fecha");
                    strxmlFecha = xmlFecha.InnerXml;
                    XmlAttribute xmlTComp = cfdi.GetAttributeNode("TipoDeComprobante");
                    strxmlTComp = xmlTComp.InnerXml;
                    //gets general fields from cfdi:Emisor
                    XmlElement cfdiEmisor = (XmlElement)cfdi.SelectSingleNode("cfdi:Emisor", nsmgr);
                    XmlAttribute xmlEmisorRfc = cfdiEmisor.GetAttributeNode("Rfc");
                    strXmlEmisorRfc = xmlEmisorRfc.InnerXml;
                    //gets general fields from cfdi:Emisor
                    XmlElement cfdiReceptor = (XmlElement)cfdi.SelectSingleNode("cfdi:Receptor", nsmgr);
                    XmlAttribute xmlReceptorRfc = cfdiReceptor.GetAttributeNode("Rfc");
                    strXmlReceptorRfc = xmlReceptorRfc.InnerXml;
                }
                if (strxmlVersion == "3.2")
                {
                    XmlAttribute xmlFecha = cfdi.GetAttributeNode("fecha");
                    strxmlFecha = xmlFecha.InnerXml;
                    XmlAttribute xmlTComp = cfdi.GetAttributeNode("tipoDeComprobante");
                    strxmlTComp = xmlTComp.InnerXml;
                    //gets general fields from cfdi:Emisor
                    XmlElement cfdiEmisor = (XmlElement)cfdi.SelectSingleNode("cfdi:Emisor", nsmgr);
                    XmlAttribute xmlEmisorRfc = cfdiEmisor.GetAttributeNode("rfc");
                    strXmlEmisorRfc = xmlEmisorRfc.InnerXml;
                    //gets general fields from cfdi:Emisor
                    XmlElement cfdiReceptor = (XmlElement)cfdi.SelectSingleNode("cfdi:Receptor", nsmgr);
                    XmlAttribute xmlReceptorRfc = cfdiReceptor.GetAttributeNode("rfc");
                    strXmlReceptorRfc = xmlReceptorRfc.InnerXml;
                }
                
                FindRfc findRfc = new FindRfc();
                string searchRfc = (FindByRfc == 1) ? strXmlEmisorRfc : strXmlReceptorRfc; //Define RFC a buscar 1=Emisor y 2=Receptor

                string txtWrite = string.Empty;
                if (fullpathWrite)
                    txtWrite = DateTime.Now.ToLongTimeString() + "  " + fullPathFile + " ";
                else
                    txtWrite = DateTime.Now.ToLongTimeString() + "  ";

                if (findRfc.FindRfcinList(FindByRfc, searchRfc))
                {
                    //Console.WriteLine("strXmlEmisorRfc " + strXmlEmisorRfc + " Existe");
                    if (ExportXML(fullPathFile, strXmlEmisorRfc, strXmlUuid, strxmlFecha, strxmlTComp))
                    {
                        Console.WriteLine(txtWrite + strXmlUuid + "  " + strXmlEmisorRfc + " Existe");
                    }
                }
                else
                {
                    Console.WriteLine(txtWrite + strXmlUuid);
                }
                //Console.WriteLine("strXmlUuid " + strXmlUuid);
                //Console.ReadKey();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }

        public bool ExportXML(string pathOrigen, string strXmlEmisorRfc, string strXmlUuid, string strxmlFecha, string strxmlTComp)
        {
            bool result = false;
            try
            {
                string OutputFolder = ConfigurationManager.AppSettings["OutputFolder"];
                string year = strxmlFecha.Substring(0, 4);
                string date = strxmlFecha.Substring(0, 10);
                string yearFolderPath = String.Format("{0}{1}", OutputFolder, year);
                string rfcFolderPath = String.Format("{0}{1}\\{2}", OutputFolder, year, strXmlEmisorRfc);
                string fullPathDest = String.Format("{0}{1}\\{2}\\{3}_{4}_{5}.xml", OutputFolder, year, strXmlEmisorRfc, date, strxmlTComp, strXmlUuid);
                //Console.Write(fullPathDest);
                if (!Directory.Exists(yearFolderPath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(yearFolderPath);
                }

                if (!Directory.Exists(rfcFolderPath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(rfcFolderPath);
                }

                if(!File.Exists(fullPathDest))
                {
                    File.Copy(pathOrigen, fullPathDest);
                }
                if (File.Exists(fullPathDest))
                {
                    result = true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
            return result;
        }
    }
}
