<Query Kind="Program" />

void Main()
{
	foreach (var xdata in (
		from customer in StreamReader(@"C:\Users\mmasson\Desktop\Mesh\Design Patterns\XML\Customers.xml", "Customer")
		   select new 
		   {
		     Key = Convert.ToInt32(customer.Attribute("Key").Value),
		   	 FirstName = customer.Element("Name").Element("FirstName").Value,
			 LastName = customer.Element("Name").Element("LastName").Value,
			 BirthDate = customer.Element("BirthDate").Value,
			 Gender = customer.Element("Gender").Value,
			 YearlyIncome = customer.Element("YearlyIncome").Value,
		   }
	))
	{
		Console.WriteLine(xdata);
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

