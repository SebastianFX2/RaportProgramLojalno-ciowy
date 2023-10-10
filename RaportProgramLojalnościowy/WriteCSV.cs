using Soneta.Business;
using Soneta.Handel;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaportProgramLojalnościowy
{
    internal class WriteCSV
    {
        public void WriteHeader(System.IO.StreamWriter plik)
        {
            plik.WriteLine("NIP;Numer klienta LS;Nr dok. Sprz.;Data sprzed.;Symbol (kod);" +
                              "Ilość;Ilość punktów za towar;Suma punktów za towar");
        }

        public void GetData(System.IO.StreamWriter plik, Context context)
        {
            View nav = context[typeof(View)] as View;
            HandelModule hm = HandelModule.GetInstance(context.Session);
            string[] wiersz = new string[8];


            foreach (DokumentHandlowy dh in nav)
            {
                if(!dh.Definicja.Symbol.Contains("FV"))
                {
                    continue;
                }

                foreach(PozycjaDokHandlowego poz in dh.Pozycje)
                {
                    //bool IsNumerKlientaLs = dh.Kontrahent.Features["Numer klient LS"] != null;

                    wiersz[0] = dh.Kontrahent.NIP;//NIP
                    wiersz[1] = dh.Kontrahent.Features["Numer klienta LS"].ToString();//Numer klienta LS
                    wiersz[2] = dh.NumerPelnyZapisany;//Nr dokumentu sprzedaży
                    wiersz[3] = dh.Data.ToString();//Data sprzedaży
                    wiersz[4] = poz.Symbol.ToString();//symbol(kod) towaru/pozycji
                    wiersz[5] = poz.Ilosc.ToString();//ilosc
                    wiersz[6] = poz.Towar.Features["Punkty lojalnościowe"].ToString();//ilosc punktów za towar
                    wiersz[7] = (poz.Ilosc.Value * Int32.Parse(poz.Towar.Features["Punkty lojalnościowe"].ToString())).ToString();

                    StringBuilder builder = new StringBuilder();

                    foreach(string wartosc in wiersz)
                    {
                        builder.Append(wartosc);
                        builder.Append(';');
                    }
                    plik.WriteLine(builder.ToString());
                }


            }
        }
    }
}
