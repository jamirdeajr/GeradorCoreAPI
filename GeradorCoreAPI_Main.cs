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


        #region Gera Versão
        /// <summary>
        /// Gera Versão
        /// </summary>
        public bool GeraVersao()
        {

            log.Info($"{this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}...");

            StreamWriter sw;

            try
            {
                sw = new StreamWriter(File.Open($"{diretorioApi}{Path.DirectorySeparatorChar}Versao_REN.cs", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error($"Exception {this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}", ex);
                return false;
            }

            StreamReader sr = File.OpenText($"Templates{Path.DirectorySeparatorChar}Versao_cs.txt");

            string aaaammdd = DateTime.Now.ToString("yyyyMMdd");

            string s = sr.ReadToEnd().Replace("[nomeAPI]", nomeApi).Replace("[siglaAPI]", siglaApi).Replace("[aaaammdd]",aaaammdd);

            sw.Write(s);
            sw.Close();
            return true;
        }
        #endregion

        #region Gera Startup
        /// <summary>
        /// Gera Startup
        /// </summary>
        public bool GeraStartup()
        {

            log.Info($"{this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}...");

            StreamWriter sw;

            try
            {
                sw = new StreamWriter(File.Open($"{diretorioApi}{Path.DirectorySeparatorChar}Startup.cs", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error($"Exception {this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}", ex);
                return false;
            }

            StreamReader sr = File.OpenText($"Templates{Path.DirectorySeparatorChar}Startup_cs.txt");

            string aaaammdd = DateTime.Now.ToString("yyyyMMdd");

            string s = sr.ReadToEnd().Replace("[nomeAPI]", nomeApi).Replace("[siglaAPI]", siglaApi).Replace("[aaaammdd]", aaaammdd);

            sw.Write(s);
            sw.Close();
            return true;
        }
        #endregion

        #region Gera Program
        /// <summary>
        /// Gera Program
        /// </summary>
        public bool GeraProgram()
        {

            log.Info($"{this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}...");

            StreamWriter sw;

            try
            {
                sw = new StreamWriter(File.Open($"{diretorioApi}{Path.DirectorySeparatorChar}Program.cs", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error($"Exception {this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}", ex);
                return false;
            }

            StreamReader sr = File.OpenText($"Templates{Path.DirectorySeparatorChar}Program_cs.txt");

            string aaaammdd = DateTime.Now.ToString("yyyyMMdd");

            string s = sr.ReadToEnd().Replace("[nomeAPI]", nomeApi).Replace("[siglaAPI]", siglaApi).Replace("[aaaammdd]", aaaammdd);

            sw.Write(s);
            sw.Close();
            return true;
        }
        #endregion

        #region Gera Configs
        /// <summary>
        /// Gera Versão
        /// </summary>
        public bool GeraConfigs()
        {

            log.Info($"{this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}...");

            StreamWriter sw;

            #region log4Net
            try
            {
                sw = new StreamWriter(File.Open($"{diretorioApi}{Path.DirectorySeparatorChar}log4net_REN.config", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error($"Exception {this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}", ex);
                return false;
            }

            StreamReader sr = File.OpenText($"Templates{Path.DirectorySeparatorChar}log4net_config.txt");

            string aaaammdd = DateTime.Now.ToString("yyyyMMdd");

            string s = sr.ReadToEnd().Replace("[nomeAPI]", nomeApi).Replace("[siglaAPI]", siglaApi).Replace("[aaaammdd]", aaaammdd);

            sw.Write(s);
            sw.Close();
            #endregion

            #region appsettings
            try
            {
                sw = new StreamWriter(File.Open($"{diretorioApi}{Path.DirectorySeparatorChar}appsettings_REN.json", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error($"Exception {this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}", ex);
                return false;
            }

            sr = File.OpenText($"Templates{Path.DirectorySeparatorChar}appsettings_json.txt");

            s = sr.ReadToEnd().Replace("[nomeAPI]", nomeApi).Replace("[siglaAPI]", siglaApi).Replace("[aaaammdd]", aaaammdd);

            sw.Write(s);
            sw.Close();
            #endregion

            #region launchSettings
            try
            {
                string workdir = Path.Combine(diretorioApi, "Properties");
                if (!Directory.Exists(workdir))
                    Directory.CreateDirectory(workdir);

                sw = new StreamWriter(File.Open($"{workdir}{Path.DirectorySeparatorChar}launchSettings_REN.json", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error($"Exception {this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}", ex);
                return false;
            }

            sr = File.OpenText($"Templates{Path.DirectorySeparatorChar}launchSettings_json.txt");

            s = sr.ReadToEnd().Replace("[nomeAPI]", nomeApi).Replace("[siglaAPI]", siglaApi).Replace("[aaaammdd]", aaaammdd);

            sw.Write(s);
            sw.Close();
            #endregion

            #region csproj
            try
            {
                sw = new StreamWriter(File.Open($"{diretorioApi}{Path.DirectorySeparatorChar}{nomeApi}_REN.csproj", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error($"Exception {this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}", ex);
                return false;
            }

            sr = File.OpenText($"Templates{Path.DirectorySeparatorChar}nomeAPI_csproj.txt");

            s = sr.ReadToEnd().Replace("[nomeAPI]", nomeApi).Replace("[siglaAPI]", siglaApi).Replace("[aaaammdd]", aaaammdd);

            //"<ProjectReference Include = "..\..\..\..\..\Nucleo_Hibernate_[siglaAPI]\Nucleo_Hibernate_[siglaAPI].csproj"/>
            //Substituir [ReferenceHibernate]
            StringBuilder referenceHibernate = new StringBuilder();

            foreach(string nucleo_hibernate in nucleos_hibernate)
            {
                referenceHibernate.Append($"<ProjectReference Include = \"..\\{nucleo_hibernate}\\{nucleo_hibernate}.csproj\"/>\r\n");
            }

            s = s.Replace("[ReferenceHibernate]", referenceHibernate.ToString());


            sw.Write(s);
            sw.Close();
            #endregion

            #region hibernate.cfg
            try
            {
                sw = new StreamWriter(File.Open($"{diretorioApi}{Path.DirectorySeparatorChar}hibernate.cfg_REN.xml", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error($"Exception {this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}", ex);
                return false;
            }

            sr = File.OpenText($"Templates{Path.DirectorySeparatorChar}hibernate.cfg_xml.txt");

            s = sr.ReadToEnd().Replace("[nomeAPI]", nomeApi).Replace("[siglaAPI]", siglaApi).Replace("[aaaammdd]", aaaammdd);

            sw.Write(s);
            sw.Close();
            #endregion

            return true;
        }
        #endregion


    }
}


