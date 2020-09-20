using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;

namespace RebarClass
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Получаем доступ к документу
            Document doc = commandData.Application.ActiveUIDocument.Document;

			List<RebarBarType> barTypesList = new FilteredElementCollector(doc)
				.WhereElementIsElementType()
				.OfClass(typeof(RebarBarType))
				.Cast<RebarBarType>()
				.ToList();

			int classesCount = 0;

			using (Transaction t = new Transaction(doc))
			{
				t.Start("Заполнение класса арматуры");
				foreach (RebarBarType barType in barTypesList)
				{
					Parameter classParam = barType.LookupParameter("Арм.КлассЧисло");
					if (classParam == null)
					{
						TaskDialog.Show("Ошибка", "У элемента нет параметра с id:" + barType.Id);
						return Result.Cancelled;
					}

					string typeName = barType.Name;
					if (typeName.Contains("Вр")) continue;
					string[] typeNameArray = typeName.Split(' ');
					string typeClassText = typeNameArray[1];
					string classNumberText = typeClassText.Substring(1);

					double barClass = 0;
					double.TryParse(classNumberText, out barClass);
					classParam.Set(barClass);
					classesCount++;
				}
				t.Commit();
			}

			TaskDialog.Show("Отчёт", "Обработано классов арматуры: " + classesCount.ToString());

			return Result.Succeeded;
        }
    }
}
