using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace GeradorCoreAPI
{
    public partial class GeradorCoreAPI
    {

        #region Gera Models
        /// <summary>
        /// Gera Models
        /// </summary>


        public bool GeraModels()
        {

            log.Info($"{this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}...");

            StreamWriter sw;

            try
            {
                string workdir = Path.Combine(diretorioApi, "Model");
                if (!Directory.Exists(workdir))
                    Directory.CreateDirectory(workdir);

                sw = new StreamWriter(File.Open($"{workdir}{Path.DirectorySeparatorChar}{nomeApi}Model.cs", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error($"Exception {this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}", ex);
                return false;
            }

            StreamReader sr = File.OpenText($"Templates{Path.DirectorySeparatorChar}nomeAPI_Model_cs.txt");

            string s = sr.ReadToEnd().Replace("[nomeAPI]", nomeApi).Replace("[siglaAPI]", siglaApi);

            sw.Write(s);

            sw.Close();

            return true;
        }

        #endregion

    }
}


