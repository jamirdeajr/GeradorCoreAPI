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

        #region Gera Helpers
        /// <summary>
        /// Gera Helpers
        /// </summary>

        public bool GeraHelpers()
        {
        
            log.Info($"{this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}...");

            StreamWriter sw;
            StreamReader sr;
            string s;

            #region AppSettings
            try
            {
                string workdir = Path.Combine(diretorioApi, "Helpers");
                if (!Directory.Exists(workdir))
                    Directory.CreateDirectory(workdir);

                sw = new StreamWriter(File.Open($"{workdir}{Path.DirectorySeparatorChar}AppSettings_REN.cs", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error($"Exception {this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}", ex);
                return false;
            }

            sr = File.OpenText($"Templates{Path.DirectorySeparatorChar}AppSettings_cs.txt");

            s = sr.ReadToEnd().Replace("[nomeAPI]",nomeApi).Replace("[siglaAPI]", siglaApi);

            sw.Write(s);
           
            sw.Close();
            #endregion

            #region IWritableOptions
            try
            {
                string workdir = Path.Combine(diretorioApi, "Helpers");
                if (!Directory.Exists(workdir))
                    Directory.CreateDirectory(workdir);

                sw = new StreamWriter(File.Open($"{workdir}{Path.DirectorySeparatorChar}IWritableOptions.cs", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error($"Exception {this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}", ex);
                return false;
            }

            sr = File.OpenText($"Templates{Path.DirectorySeparatorChar}IWritableOptions_cs.txt");

            s = sr.ReadToEnd().Replace("[nomeAPI]", nomeApi).Replace("[siglaAPI]", siglaApi);

            sw.Write(s);

            sw.Close();
            #endregion

            #region WritableOptions
            try
            {
                string workdir = Path.Combine(diretorioApi, "Helpers");
                if (!Directory.Exists(workdir))
                    Directory.CreateDirectory(workdir);

                sw = new StreamWriter(File.Open($"{workdir}{Path.DirectorySeparatorChar}WritableOptions.cs", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error($"Exception {this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}", ex);
                return false;
            }

            sr = File.OpenText($"Templates{Path.DirectorySeparatorChar}WritableOptions_cs.txt");

            s = sr.ReadToEnd().Replace("[nomeAPI]", nomeApi).Replace("[siglaAPI]", siglaApi);

            sw.Write(s);

            sw.Close();
            #endregion

            #region ServiceCollectionExtensions
            try
            {
                string workdir = Path.Combine(diretorioApi, "Helpers");
                if (!Directory.Exists(workdir))
                    Directory.CreateDirectory(workdir);

                sw = new StreamWriter(File.Open($"{workdir}{Path.DirectorySeparatorChar}ServiceCollectionExtensions.cs", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error($"Exception {this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}", ex);
                return false;
            }

            sr = File.OpenText($"Templates{Path.DirectorySeparatorChar}ServiceCollectionExtensions_cs.txt");

            s = sr.ReadToEnd().Replace("[nomeAPI]", nomeApi).Replace("[siglaAPI]", siglaApi);

            sw.Write(s);

            sw.Close();
            #endregion

            return true;
        }
        
        #endregion

    }
}
