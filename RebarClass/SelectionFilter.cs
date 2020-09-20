using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace RebarClass
{
	public class SelectionFilter : ISelectionFilter
	{


		public bool AllowElement(Element elem)
		{
			if (elem is FamilyInstance)
			{
				return true;
			}
			return false;
		}

		public bool AllowReference(Reference reference, XYZ position)
		{
			return false;
		}
	}

}
