using Soneta.Business;
using Soneta.Handel;
using Soneta.Types;
using System;
using System.IO;
using System.Text;

[assembly: Worker(typeof(RaportProgramLojalnościowy.Main), typeof(DokHandlowe))]

namespace RaportProgramLojalnościowy
{
    public class Main
    {
        [Context]
        public Context context { get; set; }

        [Action(
            "Export Faktur LS",
            Priority = 1000,
            Icon = ActionIcon.Save,
            Mode = ActionMode.SingleSession,
            Target = ActionTarget.Menu | ActionTarget.ToolbarWithText)]
        public NamedStream SaveDataWorker()
        {
            string filename = context.Login.Database.Name + "_" + Date.Today + ".csv";
            var nowyPlik = new StreamWriter(new MemoryStream(), Encoding.UTF8);

            WriteCSV writeCsv = new WriteCSV();
            writeCsv.WriteHeader(nowyPlik);
            writeCsv.GetData(nowyPlik, context);
            nowyPlik.Flush();

            return new NamedStream(filename, ((MemoryStream)nowyPlik.BaseStream).ToArray());
        }

        public static bool IsVisibleSaveDataWorker(Context cx)
        {
            UILocation uILocation = cx[typeof(UILocation)] as UILocation;
            return uILocation.FolderNormalizedPath == "Handel/Sprzedaz/FakturySprzedazy";
        }
    }
}