using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CMLabExtim.WODClasses
{
    [Serializable]
    [XmlRoot(ElementName = "Var")]
    public class Var
    {
        [XmlElement(ElementName = "Value")]
        public string Value { get; set; }
        [XmlAttribute(AttributeName = "Prefix")]
        public string Prefix { get; set; }
        [XmlAttribute(AttributeName = "Suffix")]
        public string Suffix { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "DHL")]
    public class DHL
    {
        [XmlElement(ElementName = "Var")]
        public List<Var> Var { get; set; }
    }

}
