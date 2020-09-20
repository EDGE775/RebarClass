using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Structure;

namespace RebarClass
{
	[Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

	class CommandCopyIntersectGrids : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
			Document doc = commandData.Application.ActiveUIDocument.Document;

			ElementId selFiId = null;

			Selection sel = commandData.Application.ActiveUIDocument.Selection;
			if (sel.GetElementIds().Count != 1)
			{
				Reference r = sel.PickObject(ObjectType.Element, new SelectionFilter(), "Выберите семейство!");
				selFiId = r.ElementId;
			}
			else
			{
				selFiId = sel.GetElementIds().First();
			}

			FamilyInstance selFi = doc.GetElement(selFiId) as FamilyInstance;
			if (selFi == null)
			{
				TaskDialog.Show("Ошибка", "Было выбрано не семейство!");
				return Result.Failed;
			}
			FamilySymbol mySymbol = selFi.Symbol;

			//Получаем доступ к текущему виду и затем получаем уровень данного вида

			View curView = doc.ActiveView;
			Level lv = curView.GenLevel;
			if (lv == null) throw new Exception("Не удалось получить уровень по его Id");

			//Получаем все оси

			List<Grid> grids = new FilteredElementCollector(doc, doc.ActiveView.Id)
				.OfClass(typeof(Grid))
				.Cast<Grid>()
				.ToList();

			//Сделаем список точек и проверку на пересечения
			List<XYZ> points = new List<XYZ>();
			foreach (Grid g1 in grids)
			{
				Curve curve1 = g1.Curve;
				foreach (Grid g2 in grids)
				{
					if (g2.Id == g1.Id) continue;
					Curve curve2 = g2.Curve;

					IntersectionResultArray resArray;
					SetComparisonResult scr = curve2.Intersect(curve1, out resArray);
					if (resArray == null) continue;
					if (resArray.Size < 1) continue;

					foreach (IntersectionResult res in resArray)
					{
						XYZ intersectPoint = res.XYZPoint;
						if (!ContainsPoint(points, intersectPoint))
						{
							points.Add(intersectPoint);
						}
					}
				}
			}

			using (Transaction t = new Transaction(doc))
			{
				t.Start("Размещение семейств");

				foreach (XYZ p in points)
				{
					FamilyInstance fi = doc.Create.NewFamilyInstance(p, mySymbol, lv, StructuralType.NonStructural);

				}

				t.Commit();
			}
			return Result.Succeeded;
		}

		private bool ContainsPoint(List<XYZ> pointsList, XYZ point)
		{
			foreach (XYZ curpoint in pointsList)
			{
				if (curpoint.IsAlmostEqualTo(point))
				{
					return true;
				}
			}
			return false;
		}

	}
}
