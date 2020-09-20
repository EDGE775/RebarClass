using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using System.Windows.Media.Imaging;

namespace RebarClass
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    public class App : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }


        public Result OnStartup(UIControlledApplication application)
        {
            //Находим путь к файлу:
            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //создаём вкладку:
            application.CreateRibbonTab("Revit API");
            //Создаём панель:
            RibbonPanel panel = application.CreateRibbonPanel("Revit API", "Моя первая панель");

            PushButtonData pbd =
                new PushButtonData("WriteRebarclass", "Заполнить\nкласс", assemblyPath, "RebarClass.Command");
            //Добавляем кнопку 1:
            pbd.LargeImage = PngImageSource("RebarClass.Resources.equipment.png");
            pbd.Image = PngImageSource("RebarClass.Resources.equipment.png");
            panel.AddItem(pbd);

            PushButtonData pbd1 =
                new PushButtonData("DeleteRebarclass", "Удалить\nкласс", assemblyPath, "RebarClass.Command1");
            //Добавляем кнопку 2:
            panel.AddItem(pbd1);

            PushButtonData pbdCopyByGrids =
            new PushButtonData("CopyByGrids", "По пересечениям\nосей", assemblyPath, "RebarClass.CommandCopyIntersectGrids");
            //Добавляем кнопку 3:
            panel.AddItem(pbdCopyByGrids);

            PushButtonData pbdArrayCopy =
            new PushButtonData("ArrayCopy", "Прямоугольный\nмассив", assemblyPath, "RebarClass.CommandArrayCopy");
            //Добавляем кнопку 4:
            panel.AddItem(pbdArrayCopy);

            return Result.Succeeded;
        }

        private System.Windows.Media.ImageSource PngImageSource(string embeddedPathname)
        {
            System.IO.Stream st = this.GetType().Assembly.GetManifestResourceStream(embeddedPathname);

            PngBitmapDecoder decoder = new PngBitmapDecoder(st, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

            return decoder.Frames[0];
        }

    }
}
