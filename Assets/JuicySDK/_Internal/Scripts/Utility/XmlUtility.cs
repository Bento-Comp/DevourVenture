using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace JuicyInternal
{
    public class XmlUtility
    {
        public static bool TryGetXmlDocument(string path, out XmlDocument document)
        {
            document = new XmlDocument();
            document.PreserveWhitespace = true;

            try
            {
                document.Load(path);
            }

            catch
            {
                document = null;
                return false;
            }

            return true;
        }

        public static bool LoadXmlFromResources(string resourcesName, out XmlDocument document)
        {
            document = null;
            TextAsset textAsset = (TextAsset)Resources.Load(resourcesName);
            if (textAsset == null)
                return false;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(textAsset.text);
            document = xmldoc;
            return true;
        }

        /// <summary>
        /// Get the first node with name node name in the manifest
        /// </summary>
        public static XmlNode GetNode(XmlDocument document, string nodeName)
        {
            //See XPath doc : https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-4.0/ms256086(v=vs.100)?redirectedfrom=MSDN
            return document.SelectSingleNode("//" + nodeName);
        }

        public static XmlNode GetNode(XmlNode parentNode, string nodeName)
        {
            return parentNode.SelectSingleNode(nodeName);
        }

        /// <summary>
        /// Get the first node in parent node with name nodeName which contains an attribute named attributeName with a value of attributeValue
        /// </summary>
        public static XmlNode GetNode(XmlNode parentNode, string nodeName, string attributeName, string attributeValue)
        {
            if (parentNode == null)
                return null;

            foreach (XmlNode node in parentNode)
            {
                if (node.Name != nodeName)
                    continue;

                if (node.Attributes == null)
                    continue;

                foreach (XmlAttribute attribute in node.Attributes)
                {
                    if (attribute.Name == attributeName)
                    {
                        if (attribute.Value == attributeValue)
                        {
                            return node;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Add new node named nodeName as child of parentNode
        /// </summary>
        public static XmlNode AddNode(XmlDocument document, XmlNode parentNode, string nodeName)
        {
            XmlNode node = document.CreateElement(nodeName);
            parentNode.AppendChild(node);
            return node;
        }

        /// <summary>
        /// Set Attribute to node
        /// </summary>
        public static void SetNodeAttribute(XmlDocument document, XmlNode node, string prefix, string localName, string namespaceURI, string value)
        {
            XmlAttribute attribute = null;

            if (node.Attributes != null)
            {
                foreach (XmlAttribute attr in node.Attributes)
                {
                    if (attr.Name == prefix + ":" + localName)
                        attribute = attr;
                }
            }

            if (attribute == null)
            {
                attribute = document.CreateAttribute(prefix, localName, namespaceURI);
                attribute.Value = value;
                node.Attributes.Append(attribute);
            }

            attribute.Value = value;
        }
    }
}