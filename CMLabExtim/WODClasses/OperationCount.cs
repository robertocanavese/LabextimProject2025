using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace CMLabExtim.WODClasses
{
    [Serializable]
    [XmlRoot(ElementName = "ORDER_NUMBER")]
    public class ORDER_NUMBER
    {
        [XmlAttribute(AttributeName = "DataSize")]
        public string DataSize { get; set; }
        [XmlAttribute(AttributeName = "DataType")]
        public string DataType { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "SUM")]
    public class SUM
    {
        [XmlAttribute(AttributeName = "DataSize")]
        public string DataSize { get; set; }
        [XmlAttribute(AttributeName = "DataType")]
        public string DataType { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "Fields")]
    public class Fields
    {
        [XmlElement(ElementName = "ORDER_NUMBER")]
        public ORDER_NUMBER ORDER_NUMBER { get; set; }
        [XmlElement(ElementName = "SUM")]
        public SUM SUM { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "Record")]
    public class Record
    {
        [XmlElement(ElementName = "ORDER_NUMBER")]
        public string ORDER_NUMBER { get; set; }
        [XmlElement(ElementName = "SUM")]
        public string SUM { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "Query")]
    public class Query_OperationCount
    {
        [XmlElement(ElementName = "SQL")]
        public string SQL { get; set; }
        [XmlElement(ElementName = "Fields")]
        public Fields Fields { get; set; }
        [XmlElement(ElementName = "Record")]
        public Record Record { get; set; }
    }
}
