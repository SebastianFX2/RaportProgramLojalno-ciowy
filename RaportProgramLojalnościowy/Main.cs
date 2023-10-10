using Soneta.Business;
using Soneta.Business.UI;
using Soneta.Handel;
using Soneta.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CsvHelper;
using Soneta.Core.DbTuples;
using System.Runtime.InteropServices.ComTypes;
using Soneta.Langs;

[assembly: Worker(typeof(RaportProgramLojalnościowy.Main), typeof(DokHandlowe))]

namespace RaportProgramLojalnościowy
{
    public class Main
    {
        public DokHandlowe dokHandlowe { get; set; }
        [Context]
        public Context context {  get; set; }


        [Action(
            "Export do CSV",
            Priority = 1000,
            Icon = ActionIcon.ExcelPrint,
            Mode = ActionMode.Progress,
            Target = ActionTarget.Menu | ActionTarget.ToolbarWithText)]
        public NamedStream SaveDataWorker()
        {
 
            string filename = context.Login.Database.Name + "_" + Date.Today+".csv";
            var nowyPlik = new StreamWriter(new MemoryStream(), Encoding.UTF8);

            WriteCSV writeCsv = new WriteCSV();
            writeCsv.WriteHeader(nowyPlik);
            writeCsv.GetData(nowyPlik,context);
            nowyPlik.Flush();  

           return new NamedStream(filename, ((System.IO.MemoryStream)nowyPlik.BaseStream).ToArray());

        }


        public static bool IsVisibleFun(Context cx)
        {
            UILocation uILocation = cx[typeof(UILocation)] as UILocation;
            return uILocation.FolderNormalizedPath == "Handel/Sprzedaz/FakturySprzedazy";
        }
    }
}
