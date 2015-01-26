using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;

namespace NetChina.Common
{
    public class operateXml
    {
        /// <summary>
        /// XML转为DataSet
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <returns></returns>
        public static DataSet ConvertXMLFileToDataSet(string xmlFile)
        {
            DataSet ds = new DataSet();
            if (xmlFile == "")
            {
                return ds;
            }
            if (!File.Exists(xmlFile))
            {
                return ds;
            }
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                XmlDocument xmld = new XmlDocument();
                xmld.Load(xmlFile);

                //DataSet xmlDS = new DataSet();
                stream = new StringReader(xmld.InnerXml);
                //从stream装载到XmlTextReader
                reader = new XmlTextReader(stream);
                ds.ReadXml(reader);
                //xmlDS.ReadXml(xmlFile);

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return ds;
        }
    }
}
