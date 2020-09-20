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

    class CommandArrayCopy : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            FormArrayCopy form1 = new FormArrayCopy();
            form1.ShowDialog();

            if (form1.DialogResult != System.Windows.Forms.DialogResult.OK)
            {
                return Result.Cancelled;
            }

            Document doc = commandData.Application.ActiveUIDocument.Document;
            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            ElementId selId = sel.GetElementIds().First();

            int countX = form1.CountX;
            double stepX = form1.StepX / 304.8;

            int countY = form1.CountY;
            double stepY = form1.StepY / 304.8;

            List<ElementId> idsToCopyY = new List<ElementId>();
            //Вписываем пока толкьо первый самый элемент:
            idsToCopyY.Add(selId);

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Прямоугольный массив");

                for (int i = 1; i < countX; i++)
                {
                    XYZ vector = new XYZ(stepX * i, 0, 0);
                    ElementId newId = ElementTransformUtils.CopyElement(doc, selId, vector).First();
                    idsToCopyY.Add(newId);
                }

                for (int j = 1; j < countY; j++)
                {
                    XYZ vectorY = new XYZ(0, stepY * j, 0);
                    ElementTransformUtils.CopyElements(doc, idsToCopyY, vectorY);
                }

                t.Commit();
            }
            return Result.Succeeded;
        }
    }
}
