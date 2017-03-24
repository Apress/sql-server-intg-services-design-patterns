/* Microsoft SQL Server Integration Services Script Component
*  Write scripts using Microsoft Visual C# 2010.
*  ScriptMain is the entry point class of the script.*/

using System;
using System.Data;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

[Microsoft.SqlServer.Dts.Pipeline.SSISScriptComponentEntryPointAttribute]
public class ScriptMain : UserComponent
{
    string pathToXmlFile;

    public override void AcquireConnections(object Transaction)
    {
        // Call the base class
        base.AcquireConnections(Transaction);

        // The File Connection Manager's AcquireConnection() method returns us the path as a string.
        pathToXmlFile = (string)Connections.CustomerFile.AcquireConnection(Transaction);
    }

    public override void PreExecute()
    {
        // Call the base class
        base.PreExecute();

        // Make sure the file path exists
        if (!File.Exists(pathToXmlFile))
        {
            string errorMessage = string.Format("Source XML file does not exist. Path: {0}", pathToXmlFile);
            bool bCancel;
            ComponentMetaData.FireError(0, ComponentMetaData.Name, errorMessage, string.Empty, 0, out bCancel);
        }
    }

public override void CreateNewOutputRows()
{
    foreach (var xdata in (
        from customer in StreamReader(pathToXmlFile, "Customer")
        select new
        {
            Key = customer.Attribute("Key").Value,
            FirstName = customer.Element("Name").Element("FirstName").Value,
            LastName = customer.Element("Name").Element("LastName").Value,
            BirthDate = customer.Element("BirthDate").Value,
            Gender = customer.Element("Gender").Value,
            YearlyIncome = customer.Element("YearlyIncome").Value,
        }
    ))
    {
        try
        {
            CustomersBuffer.AddRow();
            CustomersBuffer.Key = Convert.ToInt32(xdata.Key);
            CustomersBuffer.FirstName = xdata.FirstName;
            CustomersBuffer.LastName = xdata.LastName;
            CustomersBuffer.BirthDate = Convert.ToDateTime(xdata.BirthDate);
            CustomersBuffer.Gender = xdata.Gender;
            CustomersBuffer.YearlyIncome = Convert.ToDecimal(xdata.YearlyIncome);
        }
        catch (Exception e)
        {
            string errorMessage = string.Format("Error retrieving data. Exception message: {0}", e.Message);
            bool bCancel;
            ComponentMetaData.FireError(0, ComponentMetaData.Name, errorMessage, string.Empty, 0, out bCancel);
        }
    }
}

static IEnumerable<XElement> StreamReader(String filename, string elementName)
{
    using (XmlReader xr = XmlReader.Create(filename))
    {
        xr.MoveToContent();

        while (xr.Read()) //Reads the first element
        {
            while (xr.NodeType == XmlNodeType.Element && xr.Name == elementName)
            {
                XElement node = (XElement)XElement.ReadFrom(xr);
                yield return node;
            }
        }

        xr.Close();
    }
}

    public override void ReleaseConnections()
    {
        // Call the base class
        base.ReleaseConnections();

        // Release our connection
        Connections.CustomerFile.ReleaseConnection(pathToXmlFile);
    }
}
