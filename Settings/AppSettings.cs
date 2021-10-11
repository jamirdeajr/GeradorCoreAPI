using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//Baseado em
//https://stackoverflow.com/questions/40970944/how-to-update-values-into-appsetting-json/42705862

namespace GeradorCoreAPI.Settings
{
    public class AppSettings
    {

        public string dllMapeamentoNH1 { get; set; }
        public string dllMapeamentoNH2 { get; set; }
        public string dllMapeamentoNH3 { get; set; }
        public string dllMapeamentoNH4 { get; set; }

        public string sufixoAPI { get; set; }
        public string dirRaiz { get; set; }
        public string comAutenticacao { get; set; }

        public override bool Equals(object obj)
        {
            AppSettings toCompare = obj as AppSettings;

            if (toCompare == null)
            {
                return false;
            }

            if (!(dllMapeamentoNH1 ?? "").Equals(toCompare.dllMapeamentoNH1 ?? ""))
                return false;
            if (!(dllMapeamentoNH2 ?? "").Equals(toCompare.dllMapeamentoNH2 ?? ""))
                return false;
            if (!(dllMapeamentoNH3 ?? "").Equals(toCompare.dllMapeamentoNH3 ?? ""))
                return false;
            if (!(dllMapeamentoNH4 ?? "").Equals(toCompare.dllMapeamentoNH4 ?? ""))
                return false;

            if (!(sufixoAPI ?? "").Equals(toCompare.sufixoAPI ?? ""))
                return false;

            if (!(dirRaiz ?? "").Equals(toCompare.dirRaiz ?? ""))
                return false;

            if (!(comAutenticacao ?? "").Equals(toCompare.comAutenticacao ?? ""))
                return false;

            return true;
        }

        
        public override int GetHashCode()
        {
            int hash = GetType().GetHashCode();
            hash = (hash * 397) ^ dllMapeamentoNH1.GetHashCode();
            hash = (hash * 397) ^ dllMapeamentoNH2.GetHashCode();
            hash = (hash * 397) ^ dllMapeamentoNH3.GetHashCode();
            hash = (hash * 397) ^ dllMapeamentoNH4.GetHashCode();
            hash = (hash * 397) ^ sufixoAPI.GetHashCode();
            hash = (hash * 397) ^ dirRaiz.GetHashCode();
            hash = (hash * 397) ^ comAutenticacao.GetHashCode();

            return hash;
        }

    }
}
