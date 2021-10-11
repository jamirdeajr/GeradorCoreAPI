using GeradorCoreAPI.Settings;
using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace GeradorCoreAPI
{
    public partial class GeradorCoreAPI
    {
        /// <summary>
        /// log4net
        /// </summary>
        protected static readonly ILog log = LogManager.GetLogger(typeof(GeradorCoreAPI));
        protected static readonly ILog logAtv = LogManager.GetLogger(Assembly.GetCallingAssembly(), "LogAtividade");

        private readonly IWritableOptions<AppSettings> _options;

        public AppSettings _settings { get; set; }

        public IConfiguration Configuration { get; }

        private Assembly dll;

        private IList<string> nHibernateDlls;

        private FieldInfo[] meusCamposGeral = null;
        private Type type = null;

        //private string titulo = null;
        //private string tituloPlural = null;

        private string nomeColuna;
        private string nomeClasse;
        private string nomeClassePlural;
        private string nomeApi;
        private string siglaApi;
        private string diretorioApi;
        private IList<string> nucleos_hibernate;

        
        private string nameNH;
        private string idTpNH;
        private string idNameNH;
        private string generatorClassNH;

        private string tpNH;
        private string lenNH;
        private string decNH;
        private string precisionNH;
        private string columnNH;
        private IList<string> columnNHs;
        private string classNH;

        //private int colunasMax;
        //private int linhasMax;

        public bool insertNH;
        private IList<string> uniquekeysNH;
        private IList<string> compositekeysNH;
        //private IList<string> compositekeysNH2;
        private bool compositeNH = false;
        private bool manytooneNH = false;
        private bool onetomanyNH = false;
        private bool onetooneNH = false;
        private bool bagNH = false;
        //private IList<string> listaFiltros;

        private IList<string> listaObjetos = null;

        private bool comAutenticacao;

        //private IList<string> colunasIncluidas;
        //private IList<string> colunasIncluidasOrd;
        //private IList<string> colunasIncluidasDsp;

        //private IList<string> colunasIncluidasPrincipal;
        //private IList<string> colunasIncluidasOrdPrincipal;
        //private IList<string> colunasIncluidasDspPrincipal;

        public GeradorCoreAPI()
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            var services = new ServiceCollection();

            services.ConfigureWritable<AppSettings>(Configuration.GetSection("AppSettings"));

            //ConfigureServices(services);

            ServiceProvider provider = services.BuildServiceProvider();

            _options = provider.GetService<IWritableOptions<AppSettings>>();

            _settings = _options.Get("");

        }

        public void SetParametros(string[] nHibernateDlls, string siglaApi, string dirRaiz = null, bool comAutenticacao = true)
        {
            this.comAutenticacao = comAutenticacao;

            this.nHibernateDlls = new List<string>();
            nucleos_hibernate = new List<string>();

            foreach (string nHibernateDll in nHibernateDlls)
            {
                if (string.IsNullOrEmpty(nHibernateDll))
                    continue;
                this.nHibernateDlls.Add(nHibernateDll);

                //Nucleo_Hibernate_Genesis.dll"

                int idx = nHibernateDll.LastIndexOf(Path.DirectorySeparatorChar);

                string nucleo_hibernate = nHibernateDll.Substring(idx + 1);
                int idxExt = nucleo_hibernate.LastIndexOf(".");

                nucleos_hibernate.Add(nucleo_hibernate.Substring(0, idxExt));
            }

            nomeApi = "Api" + siglaApi;
            this.siglaApi = siglaApi;

            if (string.IsNullOrEmpty(dirRaiz))
            {
                dirRaiz = Directory.GetCurrentDirectory();
            }

            diretorioApi = Path.Combine(dirRaiz, nomeApi);

            if (!Directory.Exists(diretorioApi))
                Directory.CreateDirectory(diretorioApi);

        }

        static string Tabs(int tabs)
        {
            if (tabs >= 0)
                return new string('\t', tabs);

            return string.Empty;
        }

        #region UpdateSettings
        /// <summary>
        /// UpdateSettings - atualiza appsettings.json
        /// </summary>
        public bool UpdateSettings()
        {
            try
            {
                AppSettings _settingsCompare = _options.Get("");

                //Só atualiza se necessário

                //if (!_settingsCompare.Equals(_settings))
                {

                    _options.Update(opt =>
                    {
                        if (!string.IsNullOrEmpty(_settings.comAutenticacao))
                            opt.comAutenticacao = _settings.comAutenticacao;

                        if (!string.IsNullOrEmpty(_settings.dirRaiz))
                            opt.dirRaiz = _settings.dirRaiz;

                        if (!string.IsNullOrEmpty(_settings.sufixoAPI))
                            opt.sufixoAPI = _settings.sufixoAPI;

                        if (!string.IsNullOrEmpty(_settings.dllMapeamentoNH1))
                            opt.dllMapeamentoNH1 = _settings.dllMapeamentoNH1;

                        if (!string.IsNullOrEmpty(_settings.dllMapeamentoNH2))
                            opt.dllMapeamentoNH2 = _settings.dllMapeamentoNH2;

                        if (!string.IsNullOrEmpty(_settings.dllMapeamentoNH3))
                            opt.dllMapeamentoNH3 = _settings.dllMapeamentoNH3;

                        if (!string.IsNullOrEmpty(_settings.dllMapeamentoNH4))
                            opt.dllMapeamentoNH4 = _settings.dllMapeamentoNH4;

                    });
                }

                return true;
            }
            catch (Exception e)
            {
                log.Error("UpdateSettings()", e);
            }
            return false;
        }
        #endregion

        /// <summary>
        /// Deprecated! virou DLL
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            #region Configure Log4Net
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4net.config"));

            var repo = log4net.LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
            #endregion

            //GeradorCoreAPI ger = new GeradorCoreAPI(@"C:\GitRepository\spedqla\Nucleo_Hibernate_Ubi\bin\Debug\netcoreapp3.1\Nucleo_Hibernate_Ubi.dll","Ubi");

            GeradorCoreAPI ger = new GeradorCoreAPI();


            ger.SetParametros(
                new string[] { 
                    //@"C:\GitRepository\spedqla\Nucleo_Hibernate_Genesis\bin\Debug\netcoreapp3.1\Nucleo_Hibernate_Genesis.dll",
                    //@"C:\GitRepository\spedqla\Nucleo_Hibernate_Ubi\bin\Debug\netcoreapp3.1\Nucleo_Hibernate_Ubi.dll",
                    /*@"C:\GitRepository\spedqla\Nucleo_Hibernate_AS400\bin\Debug\Nucleo_Hibernate_AS400.dll",
                    @"C:\GitRepository\spedqla\Nucleo_Hibernate_PDV\bin\Debug\Nucleo_Hibernate_PDV.dll",
                    @"C:\GitRepository\spedqla\Nucleo_Hibernate_WMS\bin\Debug\Nucleo_Hibernate_WMS.dll",
                    @"C:\GitRepository\spedqla\Nucleo_Hibernate_Inventario\bin\Debug\Nucleo_Hibernate_Inventario.dll"
                    @"C:\GitRepository\spedqla\GHMNHibernate\bin\Debug\netcoreapp3.1\GHMNHibernate.dll",*/
                    @"C:\GitRepository\Nucleo_Hibernate_Filmes\bin\Debug\Nucleo_Hibernate_Filmes.dll",
                }  ,
             //"MaxiPRO",  //<--- Sufixo API   //api
             //"GEN",  //<--- Sufixo API   //api        
             "MOV",  //<--- Sufixo API   //api        
             @"C:\GitRepository\", //DirRaiz
             false // vazio/false gera com aut.
            );

            if (ger == null)
            {
                log.Error("Não foi possível instanciar GeradorCoreAPI.");
                Environment.Exit(1);
            }

            log.Info("GeradorCoreAPI instanciada com sucesso.");


            ger.GeraClassesResult();
            ger.GeraClassesNegocio();
            ger.GeraClassesController();

            ger.GeraVersao();
            ger.GeraStartup();
            ger.GeraProgram();

            ger.GeraConfigs();

            ger.GeraHelpers();
            ger.GeraServices();
            ger.GeraModels();
        }


    }
}
