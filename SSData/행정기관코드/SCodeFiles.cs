using System;
using System.Collections.Generic;
using System.Text;

namespace SSData
{
    public class SCodeFiles
    {
        public List<DO_CODE> DO_ITEMS { get; set; }
        public List<SGG_CODE> SGG_ITEMS { get; set; }
        public string CRE_YMD { get; set; }
        public void Save(string _filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(_filename, System.IO.FileMode.Create);
            try
            {
                System.Xml.Serialization.XmlSerializer sz = new System.Xml.Serialization.XmlSerializer(typeof(SCodeFiles));
                sz.Serialize(fs, this);
                fs.Close();

            }
            catch (Exception e)
            {
                fs.Close();
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.IO.File.Delete(_filename);//
            }
        }
        public SCodeFiles Load(string _directory)
        {
            string _filename = System.IO.Path.Combine(_directory, "SCode.DAT");

            if (!System.IO.File.Exists(_filename)) return null;
            System.IO.FileStream fs = new System.IO.FileStream(_filename, System.IO.FileMode.Open);
            try
            {
                System.Xml.Serialization.XmlSerializer sz = new System.Xml.Serialization.XmlSerializer(typeof(SCodeFiles));
                SCodeFiles result = (SCodeFiles)sz.Deserialize(fs);
                fs.Close();

                return result;
            }
            catch (Exception e)
            {
                fs.Close();
                System.Diagnostics.Debug.WriteLine(e.Message);
                return new SCodeFiles();
            }
        }
    }
}
 
