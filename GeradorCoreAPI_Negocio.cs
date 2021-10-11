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

        #region Gera Classes Negocio
        /// <summary>
        /// Gera Classe Negocio
        /// </summary>

        public bool GeraClassesNegocio()
        {
            log.Info($"GeraClasseNegocio...");

            StreamWriter sw;

            #region INegocio__BO
            try
            {
                string workdir = Path.Combine(diretorioApi, "Negocio");
                if (!Directory.Exists(workdir))
                    Directory.CreateDirectory(workdir);

                sw = new StreamWriter(File.Open($"{workdir}{Path.DirectorySeparatorChar}INegocio{nomeApi}BO.cs", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error("Exception ao gerar ClasseNegocio:", ex);
                return false;
            }

            int t = 0;

            #region P1
            sw.WriteLine($"{Tabs(t)}using {nomeApi}.Model;");

            foreach (string nucleo_hibernate in nucleos_hibernate)
            {
                sw.WriteLine($"{Tabs(t)}using {nucleo_hibernate};");
            }
            sw.WriteLine($"{Tabs(t)}using NPOI.HSSF.UserModel;");
            sw.WriteLine($"{Tabs(t)}using System;");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}namespace {nomeApi}.Negocio");
            sw.WriteLine($"{Tabs(t++)}{{");

            sw.WriteLine($"{Tabs(t)}/// <summary>");
            sw.WriteLine($"{Tabs(t)}/// Interface negocio {siglaApi}");
            sw.WriteLine($"{Tabs(t)}/// *Criada pelo GeradorCoreAPI, não editar! Inclua em outro arquivo class partial para evitar sobreposição, sugerido final _Custom ");
            sw.WriteLine($"{Tabs(t)}/// </summary>");
            sw.WriteLine($"{Tabs(t)}public partial interface INegocio{siglaApi}BO: IDisposable");
            sw.WriteLine($"{Tabs(t++)}{{");
            #endregion

            #region Gera Valida BD
            sw.WriteLine($"{Tabs(t)}GenericResult ValidaBD();");
            sw.WriteLine($"{Tabs(t)}");
            #endregion

            #region Métodos Genericos
            //sw.WriteLine($"{Tabs(t)}GenericResult ValidaBD();");
            //sw.WriteLine($"{Tabs(t)}");

            sw.WriteLine($"{Tabs(t)}GenericResult SearchGenerico(string entidade, string fields = null, string where = null, string orderby = null, int inicio = 0, int numeroRegistros = 0);");
            sw.WriteLine($"{Tabs(t)}GenericResult QueryGenericoSQL(string querysql, string orderby = null);");
            sw.WriteLine($"{Tabs(t)}GenericResult GetGenerico(string entidade, decimal id, string fields);");
            sw.WriteLine($"{Tabs(t)}HSSFWorkbook ExcelGenerico(string entidade, string fields = null, string where = null, string orderby = null);");
            //sw.WriteLine($"{Tabs(t)}GenericResult PostGenerico(string entidade, object obj);");
            //sw.WriteLine($"{Tabs(t)}GenericResult PutGenerico(string entidade, object obj);");
            //sw.WriteLine($"{Tabs(t)}GenericResult DeleteGenerico(string entidade, decimal id);");

            #endregion

            #region Interfaces Negocio por Entidade
            bool ok = true;
            foreach (string nHibernateDll in nHibernateDlls)
            {
                dll = Assembly.LoadFrom(nHibernateDll);

                IList<string> entityNames = GetEntityNames();

                foreach (string className in entityNames)
                {
                    if (!GeraInterfaceNegocioPorEntidade(className, sw))
                        ok = false;
                }
            }
            #endregion

            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine("");

            sw.Close();

            #endregion

            #region Negocio_BO
            try
            {
                string workdir = Path.Combine(diretorioApi, "Negocio");
                if (!Directory.Exists(workdir))
                    Directory.CreateDirectory(workdir);

                sw = new StreamWriter(File.Open($"{workdir}{Path.DirectorySeparatorChar}Negocio{nomeApi}BO.cs", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error("Exception ao gerar ClasseNegocio:", ex);
                return false;
            }

            t = 0;

            sw.WriteLine($"{Tabs(t)}using {nomeApi}.Helpers;");
            sw.WriteLine($"{Tabs(t)}using {nomeApi}.Model;");
            sw.WriteLine($"{Tabs(t)}using log4net;");

            sw.WriteLine($"{Tabs(t)}using NHibernate;");
            sw.WriteLine($"{Tabs(t)}using Nucleo_Geral_Core;");

            foreach (string nucleo_hibernate in nucleos_hibernate)
            {
                sw.WriteLine($"{Tabs(t)}using {nucleo_hibernate};");
                //sw.WriteLine($"{Tabs(t)}using Nucleo_Hibernate_{siglaApi};");
            }

            sw.WriteLine($"{Tabs(t)}using System;");
            sw.WriteLine($"{Tabs(t)}using System.Collections.Generic;");
            sw.WriteLine($"{Tabs(t)}using System.Text;");
            sw.WriteLine($"{Tabs(t)}using System.Threading.Tasks;");
            sw.WriteLine($"{Tabs(t)}using System.Reflection;");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}namespace {nomeApi}.Negocio");
            sw.WriteLine($"{Tabs(t++)}{{");

            sw.WriteLine($"{Tabs(t)}/// <summary>");
            sw.WriteLine($"{Tabs(t)}/// Classe negocio {siglaApi}");
            sw.WriteLine($"{Tabs(t)}/// *Criada pelo GeradorCoreAPI, não editar! Inclua em outro arquivo class partial para evitar sobreposição");
            sw.WriteLine($"{Tabs(t)}/// </summary>");
            sw.WriteLine($"{Tabs(t)}public partial class Negocio{siglaApi}BO : Negocio{siglaApi}BO_Base, INegocio{siglaApi}BO");
            sw.WriteLine($"{Tabs(t++)}{{");

            #region Gera Atributos Diversos
            /* Já no BASE
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}#region Atributos diversos");
            sw.WriteLine($"{Tabs(t)}private static readonly ILog log = LogManager.GetLogger(typeof(Negocio{siglaApi}BO));");
            sw.WriteLine($"{Tabs(t)}private static readonly ILog logAtv = LogManager.GetLogger(Assembly.GetCallingAssembly(), \"LogAtividade\");");
            sw.WriteLine($"{Tabs(t)}private static readonly ILog logResumo = LogManager.GetLogger(Assembly.GetCallingAssembly(), \"LogResumo\");");

            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}public StringBuilder ultimaMensagem {{ get; set; }}");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}private StringBuilder alerta= null;");

            sw.WriteLine($"{Tabs(t)}private readonly IWritableOptions<AppSettings> _options;");
            sw.WriteLine($"{Tabs(t)}private AppSettings _settings;");

            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}#endregion");
            sw.WriteLine($"{Tabs(t)}");
            */
            #endregion

            #region Gera Session Hibernate
            /* Já no BASE
            sw.WriteLine($"{Tabs(t)}#region _session Hibernate");
            sw.WriteLine($"{Tabs(t)}protected ISession _session = null;");
            sw.WriteLine($"{Tabs(t)}");

            sw.WriteLine($"{Tabs(t)}protected ISession Session");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}get");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}try");
            sw.WriteLine($"{Tabs(t++)}{{"); 
            sw.WriteLine($"{Tabs(t)}if (_session == null)");
            sw.WriteLine($"{Tabs(t++)}{{"); 
            sw.WriteLine($"{Tabs(t)}_session = NHibernateHelper.OpenSession();");
            sw.WriteLine($"{Tabs(--t)}}}"); 
            sw.WriteLine($"{Tabs(--t)}}}"); 
            sw.WriteLine($"{Tabs(t)}catch (Exception e)");
            sw.WriteLine($"{Tabs(t)}{{");t++;
            sw.WriteLine($"{Tabs(t)}log.Error(\"Não foi possível abrir sessão NHibernate: \",e);");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}return _session;");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}set");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}_session = value;");
            
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(--t)}}}"); 
            sw.WriteLine($"{Tabs(t)}#endregion");
            sw.WriteLine($"{Tabs(t)}");
            #endregion

            #region Gera Dispose
            sw.WriteLine($"{Tabs(t)}#region Dispose");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}public void Dispose()");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}Dispose(true);");
            sw.WriteLine($"{Tabs(t)}GC.SuppressFinalize(this);");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}");

            sw.WriteLine($"{Tabs(t)}protected virtual void Dispose(bool disposing)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}if (_session != null && _session.IsOpen)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}log.Info(\"Fechando sessão hibernate.\");");
            sw.WriteLine($"{Tabs(t)}_session.Dispose();");
            sw.WriteLine($"{Tabs(t)}_session = null;");
            sw.WriteLine($"{Tabs(--t)}}}");

            //sw.WriteLine($"{Tabs(t)}if (disposing)");
            //sw.WriteLine($"{Tabs(t++)}{{");
            //sw.WriteLine($"{Tabs(t)} GC.SuppressFinalize(this);");
            //sw.WriteLine($"{Tabs(--t)}}}");

            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}#endregion");
            sw.WriteLine($"{Tabs(t)}");
            */
            #endregion

            #region Gera objetos Repositorio
            sw.WriteLine($"{Tabs(t)}#region Objetos de Repositório");
            sw.WriteLine($"{Tabs(t)}");
            //LOOP objetos nh

            string objName;
            string titName;

            foreach (string nHibernateDll in nHibernateDlls)
            {
                dll = Assembly.LoadFrom(nHibernateDll);

                IList<string> entityNames = GetEntityNames();
                foreach (string className in entityNames)
                {

                    objName = className.Substring(0, 1).ToLower() + className.Substring(1);
                    titName = className.Substring(0, 1).ToUpper() + className.Substring(1);

                    sw.WriteLine($"{Tabs(t)}#region {className}");
                    sw.WriteLine($"{Tabs(t)}IRepository<{className}> {objName}RepObj = null;");
                    sw.WriteLine($"{Tabs(t)}");
                    sw.WriteLine($"{Tabs(t)}/// <summary>");
                    sw.WriteLine($"{Tabs(t)}/// {titName}Rep");
                    sw.WriteLine($"{Tabs(t)}/// </summary>");
                    sw.WriteLine($"{Tabs(t)}");
                    sw.WriteLine($"{Tabs(t)}protected IRepository<{className}> {titName}Rep");
                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t)}get");
                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t++)}if ({objName}RepObj == null)");
                    sw.WriteLine($"{Tabs(t)}{objName}RepObj = new GenericRepository<{className}>(Session,true);");
                    sw.WriteLine($"{Tabs(--t)}");
                    sw.WriteLine($"{Tabs(t)}return {objName}RepObj;");
                    sw.WriteLine($"{Tabs(--t)}}}");
                    sw.WriteLine($"{Tabs(t)}set");
                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t)}{objName}RepObj = value;");
                    sw.WriteLine($"{Tabs(--t)}}}");
                    sw.WriteLine($"{Tabs(--t)}}}");
                    sw.WriteLine($"{Tabs(t)}#endregion");
                    sw.WriteLine($"{Tabs(t)}");
                }
            }

            sw.WriteLine($"{Tabs(t)}#endregion");
            sw.WriteLine($"{Tabs(t)}");

            #endregion

            #region Gera Construtor 

            sw.WriteLine($"{Tabs(t)}#region Construtor");
            sw.WriteLine($"{Tabs(t)}/// <summary>");
            sw.WriteLine($"{Tabs(t)}/// Construtor");
            sw.WriteLine($"{Tabs(t)}/// </summary>");
            sw.WriteLine($"{Tabs(t)}");

            sw.WriteLine($"{Tabs(t)}public Negocio{siglaApi}BO(IWritableOptions<AppSettings> options) : base(options)");

            sw.WriteLine($"{Tabs(t++)}{{");
            //sw.WriteLine($"{Tabs(t)}ultimaMensagem = new StringBuilder();");

            //sw.WriteLine($"{Tabs(t)}_options = options;");
            //sw.WriteLine($"{Tabs(t)}//Carrega Settings");
            //sw.WriteLine($"{Tabs(t)}_settings = _options?.Get(\"\");");


            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}#endregion");
            sw.WriteLine($"{Tabs(t)}");

            #endregion

            #region Gera ValidaUsuario - Ja no BASE
            /*
            sw.WriteLine($"{Tabs(t)}#region ValidaUsuario");

            sw.WriteLine($"{Tabs(t)}/// <summary>");
            sw.WriteLine($"{Tabs(t)}/// Valida Usuario API - Importante Reescrever!");
            sw.WriteLine($"{Tabs(t)}/// </summary>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"usuario\"></param>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"senhamd5\"></param>");
            sw.WriteLine($"{Tabs(t)}/// <returns></returns>");
            sw.WriteLine($"{Tabs(t)}public virtual Services.UsuarioAPI ValidaUsuario(string usuario, string senhamd5)");
            sw.WriteLine($"{Tabs(t++)}{{");

            sw.WriteLine($"{Tabs(t)}//return new Services.UsuarioAPI {{ Id = 1,Username = \"teste\" }};");
            sw.WriteLine($"{Tabs(t)}return ValidaUsuarioCustom( usuario, senhamd5);");

            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}#endregion ValidaUsuario");
            */
            #endregion

            #region Gera Log - Ja no BASE
            /*
            sw.WriteLine($"{Tabs(t)}#region Log");
            sw.WriteLine($"{Tabs(t)}private void RegistraLog(string textoLog)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}string msg = string.Empty;");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}msg = $\"{{textoLog}}.\";");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t++)}if (alerta== null)");
            sw.WriteLine($"{Tabs(t--)}alerta= new StringBuilder();");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}alerta.Append($\"{{msg}}\\n\");");
            sw.WriteLine($"{Tabs(t)}log.Info(msg);");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}#endregion");
            */
            #endregion

            #region Gera Valida BD - Ja no BASE
            /*
            sw.WriteLine($"{Tabs(t)}#region Valida BD");
            sw.WriteLine($"{Tabs(t)}///<summary>");
            sw.WriteLine($"{Tabs(t)}///Valida banco de dados - gera scripts em log interno");
            sw.WriteLine($"{Tabs(t)}///</summary>");
            sw.WriteLine($"{Tabs(t)}/// <returns></returns>");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}public GenericResult ValidaBD()");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}GenericResult result = new GenericResult();");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}#region Update/Valida Esquema");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}bool validaEsquema = true;");
            sw.WriteLine($"{Tabs(t)}bool updateSchema = true;");
            sw.WriteLine($"{Tabs(t)}");

            #region Gera Update Esquema
            sw.WriteLine($"{Tabs(t)}#region Update Esquema");
            sw.WriteLine($"{Tabs(t)}if (updateSchema)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}try");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}NHibernateHelper.ExportaSchema(@\"ScriptSQL_Create_{siglaApi}.sql\");");
            sw.WriteLine($"{Tabs(t)}StringBuilder sb = NHibernateHelper.UpdateSchema(@\"ScriptSQL_Update_{siglaApi}.sql\");");
            sw.WriteLine($"{Tabs(t)}string[] sbs = sb.ToString().Split(\"\\n; \");");
            sw.WriteLine($"{Tabs(t)}if (sbs != null && sbs.Length > 0)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}result.AddMensagem(\"\", \"Script SQL\", false);");
            sw.WriteLine($"{Tabs(t++)}foreach (string str in sbs)");
            sw.WriteLine($"{Tabs(t--)}result.AddMensagem(\"\", str, false);");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}catch (Exception e)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}log.Error(\"Update Schema não realizado, provável falta de suporte.\", e);");
            sw.WriteLine($"{Tabs(t)}result.AddMensagem(\"\", \"Update Schema não realizado, provável falta de suporte.\", true);");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}#endregion");
            sw.WriteLine($"{Tabs(t)}");
            #endregion

            #region Gera Valida Esquema
            sw.WriteLine($"{Tabs(t)}#region Valida Esquema");
            sw.WriteLine($"{Tabs(t)}if (validaEsquema)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}NHibernate.Tool.hbm2ddl.SchemaValidator validator = new NHibernate.Tool.hbm2ddl.SchemaValidator(NHibernateHelper.configuration);");
            sw.WriteLine($"{Tabs(t)}try");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}validator.Validate();");
            sw.WriteLine($"{Tabs(t)}validator = null;");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}catch (NHibernate.SchemaValidationException ex)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}int difVarchar = 0;");
            sw.WriteLine($"{Tabs(t)}int difNumeric = 0;");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}if (ex.ValidationErrors != null)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}bool msg = false;");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}foreach (string s in ex.ValidationErrors)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}//Ignora os erros 19,5 e varchar/nvarchar e numeric/Decimal");
            sw.WriteLine($"{Tabs(t)}if (!s.Contains(\"Expected DECIMAL(19, 5)\"))");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}if (s.Contains(\"Found: varchar, Expected NVARCHAR\"))");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}difVarchar++;");
            sw.WriteLine($"{Tabs(t)}continue;");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}");

            sw.WriteLine($"{Tabs(t)}if (s.Contains(\"Found: numeric, Expected DECIMAL(\"))");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}difNumeric++;");
            sw.WriteLine($"{Tabs(t)}continue;");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}");

            sw.WriteLine($"{Tabs(t)}if (!msg)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}log.Error(\"Possíveis erros de validação Schema:\");");
            sw.WriteLine($"{Tabs(t)}msg = true;");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}log.Error(s);");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}msg = false;");
            sw.WriteLine($"{Tabs(t)}foreach (string s in ex.ValidationErrors)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}//Ignora os erros 19,5?");
            sw.WriteLine($"{Tabs(t)}if (s.Contains(\"Expected DECIMAL(19, 5)\"))");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}if (!msg)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}log.Error(\"Possíveis erros de validação Schema - ID não declarado corretamente ?:\");");
            sw.WriteLine($"{Tabs(t)}msg = true;");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}log.Error(s);");
            sw.WriteLine($"{Tabs(t)}result.AddMensagem(\"Validação Campos\", s, true);");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}catch (Exception ex)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}log.Error(\"Possui Erros de validação Schema:\", ex);");
            sw.WriteLine($"{Tabs(t)}result.AddMensagem(\"Validação Campos\", \"Possui Erros de validação Schema:\", true);");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}finally");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}validator = null;");
            sw.WriteLine($"{Tabs(--t)}}}");

            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}#endregion");
            sw.WriteLine($"{Tabs(t)}");//
            sw.WriteLine($"{Tabs(t)}return result;");

            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}#endregion");

            sw.WriteLine($"{Tabs(t)}#region ParametrosQry");
            sw.WriteLine($"{Tabs(t)}/// <summary>");
            sw.WriteLine($"{Tabs(t)}/// Converte parametros CAMPO,VALOR,TIPO em CAMPO,VALOR já com cast");
            sw.WriteLine($"{Tabs(t)}/// </summary>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"parametrosqry\"></param>");
            sw.WriteLine($"{Tabs(t)}/// <returns></returns>");
            sw.WriteLine($"{Tabs(t)}public object[] ParametrosQry(string[] parametrosqry)");
            sw.WriteLine($"{Tabs(t++)}{{");

            sw.WriteLine($"{Tabs(t)}object[] parametros = new object[2*(long)Math.Ceiling(parametrosqry?.Length / 3.0 ?? 0)];");

            sw.WriteLine($"{Tabs(t)}for (int idx = 0; idx < (parametrosqry?.Length ?? 0); idx += 3)");

            sw.WriteLine($"{Tabs(t++)}{{"); 
            sw.WriteLine($"{Tabs(t)}parametros[idx / 3] = parametrosqry[idx];");
            sw.WriteLine($"{Tabs(t)}string tipo = \"string\";");
            sw.WriteLine($"{Tabs(t)}if (parametrosqry?.Length > (idx + 2))");
            sw.WriteLine($"{Tabs(t++)}{{"); 
            sw.WriteLine($"{Tabs(t)} tipo = parametrosqry[idx + 2] ?? \"string\";");
            sw.WriteLine($"{Tabs(--t)}}}");
            
            sw.WriteLine($"{Tabs(t)}if (parametrosqry?.Length > (idx + 1))");
            sw.WriteLine($"{Tabs(t++)}{{");
            
            sw.WriteLine($"{Tabs(t)}switch (tipo)");

            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}case \"decimal\":");
            sw.WriteLine($"{Tabs(t++)}case \"numeric\":");
			sw.WriteLine($"{Tabs(t)}try");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}parametros[idx / 3 + 1] = Convert.ToDecimal(parametrosqry[idx + 1]);");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}catch (Exception e)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}parametros[idx / 3 + 1] = 0m;");
            sw.WriteLine($"{Tabs(t)}log.Error(\"Convert.ToDecimal(parametrosqry)\", e);");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t--)}break;");
            sw.WriteLine($"{Tabs(t++)}default:");
            sw.WriteLine($"{Tabs(t)}parametros[idx / 3 + 1] = parametrosqry[idx + 1];");
            sw.WriteLine($"{Tabs(t--)}break;");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}else parametros[idx / 3 + 1] = null;");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}return parametros;");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}#endregion");

            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}#endregion");
            */
            #endregion

            sw.WriteLine($"{Tabs(--t)}}}");

            sw.WriteLine($"{Tabs(--t)}}}");
            //sw.WriteLine($"{Tabs(t)}#endregion");
            sw.WriteLine("");


            sw.Close();

            #endregion

            #region BO_Base
            StreamReader sr;
            try
            {
                string workdir = Path.Combine(diretorioApi, "Negocio");
                if (!Directory.Exists(workdir))
                    Directory.CreateDirectory(workdir);

                sw = new StreamWriter(File.Open($"{workdir}{Path.DirectorySeparatorChar}Negocio{nomeApi}BO_Base.cs", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error($"Exception {this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}", ex);
                return false;
            }

            sr = File.OpenText($"Templates{Path.DirectorySeparatorChar}NegocioBase_cs.txt");

            string s = sr.ReadToEnd().Replace("[nomeAPI]", nomeApi).Replace("[siglaAPI]", siglaApi);

            sw.Write(s);
            sw.Close();
            #endregion

            #region Varre DLLs
            ok = true;
            foreach (string nHibernateDll in nHibernateDlls)
            {
                dll = Assembly.LoadFrom(nHibernateDll);

                //int idx = nHibernateDll.LastIndexOf(Path.DirectorySeparatorChar);
                //string nucleo_hibernate = nHibernateDll.Substring(idx + 1);
                //int idxExt = nucleo_hibernate.LastIndexOf(".");
                //nucleo_hibernate = nucleo_hibernate.Substring(0, idxExt);

                IList<string> entityNames = GetEntityNames();

                #region Classes Negocio por Entidade
                foreach (string className in entityNames)
                {
                    if (!GeraClasseNegocioPorEntidade(className))
                        ok = false;
                }
                #endregion
            }
            #endregion

            return ok;
        }
        #endregion

        #region Gera Classes Result
        /// <summary>
        /// Gera Classe Negocio
        /// </summary>

        public bool GeraClassesResult()
        {
            log.Info("GeraClassesResult...");

            StreamWriter sw;
            StreamReader sr;

            #region GenericResult
            try
            {
                string workdir = Path.Combine(diretorioApi, "Model");
                if (!Directory.Exists(workdir))
                    Directory.CreateDirectory(workdir);

                sw = new StreamWriter(File.Open($"{workdir}{Path.DirectorySeparatorChar}GenericResult.cs", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error($"Exception {this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}", ex);
                return false;
            }

            sr = File.OpenText($"Templates{Path.DirectorySeparatorChar}GenericResult_cs.txt");

            string s = sr.ReadToEnd().Replace("[nomeAPI]", nomeApi).Replace("[siglaAPI]", siglaApi);

            sw.Write(s);
            sw.Close();
            #endregion

            foreach (string nHibernateDll in nHibernateDlls)
            {
                dll = Assembly.LoadFrom(nHibernateDll);

                int idx = nHibernateDll.LastIndexOf(Path.DirectorySeparatorChar);
                string nucleo_hibernate = nHibernateDll.Substring(idx + 1);
                int idxExt = nucleo_hibernate.LastIndexOf(".");
                nucleo_hibernate = nucleo_hibernate.Substring(0, idxExt);

                try
                {
                    string workdir = Path.Combine(diretorioApi, "Model");
                    if (!Directory.Exists(workdir))
                        Directory.CreateDirectory(workdir);

                    sw = new StreamWriter(File.Open($"{workdir}{Path.DirectorySeparatorChar}{nomeApi}Results_{nucleo_hibernate}.cs", FileMode.Create), Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    log.Error("Exception GeraClassesResult:", ex);
                    return false;
                }

                int t = 0;



                sw.WriteLine($"{Tabs(t)}using Newtonsoft.Json;");
                sw.WriteLine($"{Tabs(t)}using {nucleo_hibernate};");
                sw.WriteLine($"{Tabs(t)}using System;");
                sw.WriteLine($"{Tabs(t)}using System.Collections.Generic;");
                sw.WriteLine($"{Tabs(t)}using System.Text;");

                sw.WriteLine($"{Tabs(t)}");
                sw.WriteLine($"{Tabs(t)}namespace {nomeApi}.Model");
                sw.WriteLine($"{Tabs(t++)}{{");

                #region Gera Results
                sw.WriteLine($"{Tabs(t)}#region Results");
                sw.WriteLine($"{Tabs(t)}// *Criada pelo GeradorCoreAPI, não editar! Inclua em outro arquivo class partial para evitar sobreposição");

                sw.WriteLine($"{Tabs(t)}");

                string objName;

                IList<string> entityNames = GetEntityNames();
                foreach (string className in entityNames)
                {

                    log.Info($"GeraClassesResult: {className}...");

                    string titEntidade = className.Substring(0, 1).ToUpper() + className.Substring(1);

                    objName = className.Substring(0, 1).ToLower() + className.Substring(1);

                    bool propOk = GetNHibernateProps(className, null);

                    sw.WriteLine($"{Tabs(t)}#region {className}");
                    sw.WriteLine($"{Tabs(t)}///<summary>");
                    sw.WriteLine($"{Tabs(t)}///{titEntidade}sResult - Retorno de Lista de {className}");
                    sw.WriteLine($"{Tabs(t)}///</summary>");
                    sw.WriteLine($"{Tabs(t)}");
                    sw.WriteLine($"{Tabs(t)}public class {titEntidade}sResult : GenericResult");
                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t)}[JsonProperty(\"{objName}s\")]");
                    sw.WriteLine($"{Tabs(t)}public IList<{className}> {className}s {{ get; set; }}");
                    sw.WriteLine($"{Tabs(--t)}}}");
                    sw.WriteLine($"{Tabs(t)}");
                    sw.WriteLine($"{Tabs(t)}///<summary>");
                    sw.WriteLine($"{Tabs(t)}///{titEntidade}Result - Retorno de {className}");
                    sw.WriteLine($"{Tabs(t)}///</summary>");
                    sw.WriteLine($"{Tabs(t)}");
                    sw.WriteLine($"{Tabs(t)}public class {titEntidade}Result : GenericResult");
                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t)}[JsonProperty(\"{objName}\")]");
                    sw.WriteLine($"{Tabs(t)}public {className} {className} {{ get; set; }}");
                    sw.WriteLine($"{Tabs(--t)}}}");
                    sw.WriteLine($"{Tabs(t)}");
                    sw.WriteLine($"{Tabs(t)}#endregion");
                    sw.WriteLine($"{Tabs(t)}");
                }

                sw.WriteLine($"{Tabs(t)}#endregion");
                sw.WriteLine($"{Tabs(t)}");

                #endregion

                sw.WriteLine($"{Tabs(--t)}}}");
                sw.WriteLine("");

                sw.Close();
            }

            return true;
        }
        #endregion





        #region Gera Classe Negocio Por Entidade
        /// <summary>
        /// Gera Classe Negocio
        /// </summary>

        public bool GeraClasseNegocioPorEntidade(string Entidade)
        {
            log.Info($"GeraClasseNegocioPorEntidade: {Entidade}...");

            string titEntidade = Entidade.Substring(0, 1).ToUpper() + Entidade.Substring(1);

            StreamWriter sw;

            try
            {
                string workdir = Path.Combine(diretorioApi, "Negocio");
                if (!Directory.Exists(workdir))
                    Directory.CreateDirectory(workdir);

                sw = new StreamWriter(File.Open($"{workdir}{Path.DirectorySeparatorChar}Negocio{nomeApi}BO_{titEntidade}.cs", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error("Exception ao gerar ClasseNegocio:", ex);
                return false;
            }

            int t = 0;

            bool propOk = GetNHibernateProps(Entidade, null);


            sw.WriteLine($"{Tabs(t)}using {nomeApi}.Model;");
            sw.WriteLine($"{Tabs(t)}using log4net;");
            sw.WriteLine($"{Tabs(t)}using Newtonsoft.Json;");
            sw.WriteLine($"{Tabs(t)}using NHibernate;");
            sw.WriteLine($"{Tabs(t)}using Nucleo_Geral_Core;");
            foreach (string nucleo_hibernate in nucleos_hibernate)
            {
                sw.WriteLine($"{Tabs(t)}using {nucleo_hibernate};");
                //sw.WriteLine($"{Tabs(t)}using Nucleo_Hibernate_{siglaApi};");
            }
            sw.WriteLine($"{Tabs(t)}using System;");
            sw.WriteLine($"{Tabs(t)}using System.Collections.Generic;");
            sw.WriteLine($"{Tabs(t)}using System.Linq;");
            sw.WriteLine($"{Tabs(t)}using System.Text;");
            sw.WriteLine($"{Tabs(t)}using System.Threading.Tasks;");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}namespace {nomeApi}.Negocio");
            sw.WriteLine($"{Tabs(t++)}{{");

            sw.WriteLine($"{Tabs(t)}/// <summary>");
            sw.WriteLine($"{Tabs(t)}/// Classe negocio {siglaApi}");
            sw.WriteLine($"{Tabs(t)}/// *Criada pelo GeradorCoreAPI, não editar! Inclua em outro arquivo class partial para evitar sobreposição");
            sw.WriteLine($"{Tabs(t)}/// </summary>");
            sw.WriteLine($"{Tabs(t)}public partial class Negocio{siglaApi}BO");
            sw.WriteLine($"{Tabs(t++)}{{");

            #region Get Lista
            try
            {
                sw.WriteLine($"{Tabs(t)}#region Get{titEntidade}s");
                sw.WriteLine($"{Tabs(t)}/// <summary>");
                sw.WriteLine($"{Tabs(t)}/// Retorna lista de {Entidade} - Opcional: Número inicial (parte de 0), número de registros máximo a retornar, Ordenação e parâmetros filtro.");
                sw.WriteLine($"{Tabs(t)}/// </summary>");
                sw.WriteLine($"{Tabs(t)}/// <param name=\"inicio\">Opcional: Registro inicial, 0=primeiro</param>");
                sw.WriteLine($"{Tabs(t)}/// <param name=\"numeroRegistros\">Opcional: Número máximo de registros a retornar, 0=todos</param>");
                sw.WriteLine($"{Tabs(t)}/// <param name=\"orderBy\">Opcional: Campos para Ordenação - Ex. 'Descricao desc, Id'</param>");
                sw.WriteLine($"{Tabs(t)}/// <param name=\"parametrosQry\">Array strings CAMPO,VALOR,TIPO - tipo determina a conversão Decimal se for o caso</param>");

                sw.WriteLine($"{Tabs(t)}/// <returns>{titEntidade}sResult</returns>");
                sw.WriteLine($"{Tabs(t)}public {titEntidade}sResult Get{titEntidade}s(int inicio=0, int numeroRegistros=0, string orderBy=\"\", params string[] parametrosQry)");
                sw.WriteLine($"{Tabs(t++)}{{");

                //20.08.2021
                sw.WriteLine($"{Tabs(t)}DateTime logInicio = DateTime.Now;");
                sw.WriteLine($"{Tabs(t)}DateTime logFim;");
                sw.WriteLine($"{Tabs(t)}");
                sw.WriteLine($"{Tabs(t)}logAtv.Info($\"Get{titEntidade}s:Reg.{{inicio}} Num:{{numeroRegistros}} Order:'{{orderBy}}' Param:{{parametrosQry?.ToString() ?? \"\"}})\");");


                sw.WriteLine($"{Tabs(t)}object[] parametros = ParametrosQry(parametrosQry);");

                sw.WriteLine($"{Tabs(t)}{titEntidade}sResult result = new {titEntidade}sResult");
                sw.WriteLine($"{Tabs(t++)}{{");
                sw.WriteLine($"{Tabs(t)}Status = \"OK\"");
                sw.WriteLine($"{Tabs(--t)}}};");
                sw.WriteLine($"{Tabs(t)}");

                sw.WriteLine($"{Tabs(t)}try");
                sw.WriteLine($"{Tabs(t++)}{{");

                sw.WriteLine($"{Tabs(t)}string lastOrderBy = string.Empty;");
                sw.WriteLine($"{Tabs(t)}if(!string.IsNullOrEmpty(orderBy))");
                sw.WriteLine($"{Tabs(t++)}{{");
                sw.WriteLine($"{Tabs(t)}lastOrderBy = {titEntidade}Rep.GetOrderBy();");
                sw.WriteLine($"{Tabs(t)}{titEntidade}Rep.SetOrderBy(orderBy);");
                sw.WriteLine($"{Tabs(--t)}}}");

                sw.WriteLine($"{Tabs(t)}if(inicio == 0 && numeroRegistros == 0)");
                sw.WriteLine($"{Tabs(t++)}{{");
                sw.WriteLine($"{Tabs(t)}result.{Entidade}s = {titEntidade}Rep.GetAllByField(parametros);");
                sw.WriteLine($"{Tabs(t)}result.Itens = result.{Entidade}s.Cast<object>().ToList();");
                sw.WriteLine($"{Tabs(--t)}}}");
                sw.WriteLine($"{Tabs(t)}else");
                sw.WriteLine($"{Tabs(t++)}{{");

                sw.WriteLine($"{Tabs(t)}result.PaginationCount = {titEntidade}Rep.CountByField(parametros);");
                sw.WriteLine($"{Tabs(t)}result.PaginationLimit = numeroRegistros;");
                sw.WriteLine($"{Tabs(t)}result.PaginationPage = (inicio == 0 ? 1 : (inicio / numeroRegistros)+1);");
                sw.WriteLine($"{Tabs(t)}result.{Entidade}s = {titEntidade}Rep.GetByFieldPaginado(inicio,numeroRegistros,parametros);");
                sw.WriteLine($"{Tabs(t)}result.Itens = result.{Entidade}s.Cast<object>().ToList();");
                sw.WriteLine($"{Tabs(--t)}}}");

                sw.WriteLine($"{Tabs(t)}if(!string.IsNullOrEmpty(lastOrderBy))");
                sw.WriteLine($"{Tabs(t++)}{{");
                sw.WriteLine($"{Tabs(t)}{titEntidade}Rep.SetOrderBy(lastOrderBy);");
                sw.WriteLine($"{Tabs(--t)}}}");

                sw.WriteLine($"{Tabs(t)}if(result.{Entidade}s == null || result.{Entidade}s.Count == 0)");
                sw.WriteLine($"{Tabs(t++)}{{");
                sw.WriteLine($"{Tabs(t)}if (!string.IsNullOrEmpty({titEntidade}Rep.GetMessage()))");
                sw.WriteLine($"{Tabs(t++)}{{");
                sw.WriteLine($"{Tabs(t)}result.AddMensagem(\"\",$\"{{{titEntidade}Rep.GetMessage()}}.\",true);");
                sw.WriteLine($"{Tabs(t)}result.Status = \"ERRO\";");
                sw.WriteLine($"{Tabs(--t)}}}");
                sw.WriteLine($"{Tabs(t)}else");
                sw.WriteLine($"{Tabs(t++)}{{");
                sw.WriteLine($"{Tabs(t)}result.AddMensagem(\"\",$\"Nenhum(a) {Entidade} localizado(a).\",true);");
                sw.WriteLine($"{Tabs(--t)}}}");
                sw.WriteLine($"{Tabs(--t)}}}");


                sw.WriteLine($"{Tabs(--t)}}} catch(Exception e)");
                sw.WriteLine($"{Tabs(t++)}{{");
                sw.WriteLine($"{Tabs(t)}result.AddMensagem(\"\",$\"Erro ao buscar {Entidade}s.\",true);");
                sw.WriteLine($"{Tabs(t)}result.Status = \"ERRO\";");
                sw.WriteLine($"{Tabs(t)}log.Error($\"Erro ao buscar {Entidade}s.\",e);");
                sw.WriteLine($"{Tabs(--t)}}}");

                //20.08.2021
                sw.WriteLine($"{Tabs(t)}logFim = DateTime.Now;");
                sw.WriteLine($"{Tabs(t)}logResumo.Info($\"Resumo:Get{titEntidade}s Início:{{logInicio.ToString(\"yyyy/MM/dd HH:mm:ss.fff\")}} Fim:{{logFim.ToString(\"yyyy/MM/dd HH:mm:ss.fff\")}} Tempo:{{(logFim - logInicio).TotalMilliseconds}} Result:{{result.Status}}\");");

                sw.WriteLine($"{Tabs(t)}");
                sw.WriteLine($"{Tabs(t)}return result;");
                sw.WriteLine($"{Tabs(--t)}}}");
                sw.WriteLine($"{Tabs(t)}#endregion");
            }
            catch (Exception ex)
            {
                log.Error("Exception ao gerar Get Lista:", ex);
                return false;
            }
            #endregion

            #region Get Objeto
            try
            {
                sw.WriteLine($"{Tabs(t)}#region Get{titEntidade}");
                if (!propOk || (string.IsNullOrEmpty(idNameNH) && compositekeysNH.Count == 0))
                {
                    sw.WriteLine($"{Tabs(t)}//public {titEntidade}Result Get{titEntidade}(?) - Não gerado, não foi possível identificar chave única");
                }
                else
                {
                    if (string.IsNullOrEmpty(idTpNH))
                        idTpNH = "decimal";

                    int idx = idTpNH.IndexOf("(");

                    if (idx > 0)
                        idTpNH = idTpNH.Substring(0, idx);


                    sw.WriteLine($"{Tabs(t)}/// <summary>");
                    sw.WriteLine($"{Tabs(t)}/// Get{titEntidade}");
                    sw.WriteLine($"{Tabs(t)}/// </summary>");
                    if (compositekeysNH.Count > 0)
                    {
                        foreach (string compositeKey in compositekeysNH)
                        {
                            sw.WriteLine($"{Tabs(t)}/// <param name=\"{compositeKey}\">{compositeKey} de {Entidade}</param>");
                        }
                    }
                    else
                    {

                        sw.WriteLine($"{Tabs(t)}/// <param name=\"{idNameNH}\">{idNameNH} de {Entidade}</param>");
                    }
                    sw.WriteLine($"{Tabs(t)}/// <returns>{titEntidade}Result</returns>");



                    sw.Write($"{Tabs(t)}public {titEntidade}Result Get{titEntidade}(");

                    if (compositekeysNH.Count > 0)
                    {
                        int cnt = 0;
                        foreach (string compositeKey in compositekeysNH)
                        {
                            if (cnt > 0) sw.Write($",");
                            GetNHibernateProps(Entidade, compositeKey);
                            sw.Write($"{tpNH} {compositeKey}");
                            cnt++;
                        }
                        //Le novamente todos
                        propOk = GetNHibernateProps(Entidade, null);
                    }
                    else
                    {
                        sw.Write($"{idTpNH} {idNameNH}");
                    }


                    sw.WriteLine($")");


                    sw.WriteLine($"{Tabs(t++)}{{");

                    //20.08.2021
                    sw.WriteLine($"{Tabs(t)}DateTime logInicio = DateTime.Now;");
                    sw.WriteLine($"{Tabs(t)}DateTime logFim;");
                    sw.WriteLine($"{Tabs(t)}");

                    StringBuilder infoCampos = new StringBuilder();

                    if (compositekeysNH.Count > 0)
                    {
                        
                        foreach (string compositeKey in compositekeysNH)
                        {
                            GetNHibernateProps(Entidade, compositeKey);
                            if (tpNH?.Equals("string", StringComparison.InvariantCultureIgnoreCase) ?? false)
                            {
                                infoCampos.Append($" {{{compositeKey}}}");
                            }
                            else
                            {
                                infoCampos.Append($" {{{compositeKey}.ToString()}}");
                            }
                        }
                        
                        propOk = GetNHibernateProps(Entidade, null);
                    }
                    else
                    {
                        infoCampos.Append($"Id {{{idNameNH}.ToString()}}");
                    }
                    sw.WriteLine($"{Tabs(t)}logAtv.Info($\"Get{titEntidade}:{infoCampos}\");");

                    sw.WriteLine($"{Tabs(t)}{titEntidade}Result result = new {titEntidade}Result");
                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t)}Status = \"OK\"");
                    sw.WriteLine($"{Tabs(--t)}}};");
                    sw.WriteLine($"{Tabs(t)}");
                    sw.WriteLine($"{Tabs(t)}try");
                    sw.WriteLine($"{Tabs(t++)}{{");

                    if (compositekeysNH.Count > 0)
                    {
                        sw.Write($"{Tabs(t)}result.{Entidade} = {titEntidade}Rep.GetByField(");
                        int cnt = 0;
                        foreach (string compositeKey in compositekeysNH)
                        {
                            if (cnt > 0) sw.Write($",");
                            sw.Write($"\"{compositeKey}\",{compositeKey}");
                            cnt++;
                        }
                        sw.WriteLine(");");
                    }
                    else
                    {
                        sw.WriteLine($"{Tabs(t)}result.{Entidade} = {titEntidade}Rep.GetById({idNameNH});");
                    }
                    sw.WriteLine($"{Tabs(t)}result.Item = result.{Entidade} as object;");

                    sw.WriteLine($"{Tabs(t)}if(result.{Entidade} == null)");
                    sw.WriteLine($"{Tabs(t++)}{{");

                    
                    sw.WriteLine($"{Tabs(t)}result.AddMensagem(\"\",$\"{Entidade} {infoCampos} não localizado(a).\",true);");
                    sw.WriteLine($"{Tabs(t)}result.Status = \"ERRO\";");
                    sw.WriteLine($"{Tabs(--t)}}}");
                    sw.WriteLine($"{Tabs(--t)}}} catch(Exception e)");
                    sw.WriteLine($"{Tabs(t++)}{{");
                    

                    sw.WriteLine($"{Tabs(t)}result.AddMensagem(\"\",$\"Erro ao buscar {Entidade} {infoCampos}.\",true);");


                    sw.WriteLine($"{Tabs(t)}result.Status = \"ERRO\";");
                    sw.WriteLine($"{Tabs(t)}log.Error($\"Erro ao buscar {Entidade} {infoCampos}.\",e);");
                    sw.WriteLine($"{Tabs(--t)}}}");

                    //20.08.2021
                    sw.WriteLine($"{Tabs(t)}logFim = DateTime.Now;");
                    sw.WriteLine($"{Tabs(t)}logResumo.Info($\"Resumo:Get{titEntidade} Início:{{logInicio.ToString(\"yyyy/MM/dd HH:mm:ss.fff\")}} Fim:{{logFim.ToString(\"yyyy/MM/dd HH:mm:ss.fff\")}} Tempo:{{(logFim - logInicio).TotalMilliseconds}} Result:{{result.Status}}\");");


                    sw.WriteLine($"{Tabs(t)}");
                    sw.WriteLine($"{Tabs(t)}return result;");
                    sw.WriteLine($"{Tabs(--t)}}}");
                }
                sw.WriteLine($"{Tabs(t)}#endregion");
            }
            catch (Exception ex)
            {
                log.Error("Exception ao gerar Get Objeto:", ex);
                return false;
            }

            #endregion

            #region Post Objeto
            try
            {
                sw.WriteLine($"{Tabs(t)}#region Post{Entidade}");
                if (!propOk)
                {
                    sw.WriteLine($"{Tabs(t)}//public {titEntidade}Result Post{titEntidade}(?) - Não gerado, problema ao buscar propriedades NHibernate");
                }
                else
                {
                    sw.WriteLine($"{Tabs(t)}/// <summary>");
                    sw.WriteLine($"{Tabs(t)}/// Post{titEntidade}");
                    sw.WriteLine($"{Tabs(t)}/// </summary>");
                    sw.WriteLine($"{Tabs(t)}/// <param name=\"{Entidade}\">{Entidade}</param>");
                    sw.WriteLine($"{Tabs(t)}/// <param name=\"novo\">Novo</param>");
                    sw.WriteLine($"{Tabs(t)}/// <returns>{titEntidade}Result</returns>");
                    sw.WriteLine($"{Tabs(t)}public {titEntidade}Result Post{titEntidade}({Entidade} {Entidade}, bool novo = true)");
                    sw.WriteLine($"{Tabs(t++)}{{");


                    //20.08.2021
                    sw.WriteLine($"{Tabs(t)}DateTime logInicio = DateTime.Now;");
                    sw.WriteLine($"{Tabs(t)}DateTime logFim;");
                    sw.WriteLine($"{Tabs(t)}");
                    sw.WriteLine($"{Tabs(t)}logAtv.Info($\"Post{titEntidade}:{{JsonConvert.SerializeObject({Entidade})}}\");");


                    sw.WriteLine($"{Tabs(t)}{titEntidade}Result result = new {titEntidade}Result");
                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t)}Status = \"OK\",");
                    sw.WriteLine($"{Tabs(--t)}}};");
                    sw.WriteLine($"{Tabs(t)}");
                    //Valida Campos
                    sw.WriteLine($"{Tabs(t)}if ({Entidade} == null)");
                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t)}result.Status = \"ERRO\";");
                    sw.WriteLine($"{Tabs(t)}result.AddMensagem(\"\", $\"{Entidade} não pode ser vazio/nulo.\", true);");
                    sw.WriteLine($"{Tabs(--t)}}}");
                    sw.WriteLine($"{Tabs(t)}else");
                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t)}ValidaCampos({Entidade}, result);");
                    sw.WriteLine($"{Tabs(--t)}}}");
                    sw.WriteLine($"{Tabs(t)}");
                    sw.WriteLine($"{Tabs(t)}if (!result.Status.Equals(\"OK\",StringComparison.InvariantCulture))");
                    sw.WriteLine($"{Tabs(t++)}{{");

                    //20.08.2021
                    sw.WriteLine($"{Tabs(t)}logFim = DateTime.Now;");
                    sw.WriteLine($"{Tabs(t)}logResumo.Info($\"Resumo:Post{titEntidade}s Início:{{logInicio.ToString(\"yyyy/MM/dd HH:mm:ss.fff\")}} Fim:{{logFim.ToString(\"yyyy/MM/dd HH:mm:ss.fff\")}} Tempo:{{(logFim - logInicio).TotalMilliseconds}} Result:{{result.Status}}\");");

                    sw.WriteLine($"{Tabs(t)}return result;");

                    sw.WriteLine($"{Tabs(--t)}}};");
                    sw.WriteLine($"{Tabs(t)}");

                    string campoId;
                    if (propOk)
                    {
                        campoId = $"{Entidade}.{idNameNH}";
                    }
                    else
                    {
                        campoId = $"{Entidade}.id{Entidade}";
                    }


                    if (propOk)
                    {
                        if (compositekeysNH.Count > 0)
                        {
                            sw.Write($"{Tabs(t)}if (!novo");

                            foreach (string compositeKey in compositekeysNH)
                            {
                                bool isString = false;

                                GetNHibernateProps(Entidade, compositeKey);

                                isString = tpNH?.Equals("string", StringComparison.InvariantCultureIgnoreCase) ?? false;

                                if (isString)
                                {
                                    sw.Write($" && !string.IsNullOrEmpty({Entidade}.{compositeKey})");
                                }
                                else
                                {
                                    sw.Write($" && {Entidade}.{compositeKey} > 0");
                                }
                                propOk = GetNHibernateProps(Entidade, null);
                            }
                            sw.WriteLine($"{Tabs(t)})");

                        }
                        else
                        {



                            //Ou é string ou numerico > 0 - ver se precisa tratar mais completo
                            if (idTpNH?.Equals("string", StringComparison.OrdinalIgnoreCase) ?? false)
                                sw.WriteLine($"{Tabs(t)}if (!novo && !string.IsNullOrEmpty({campoId}))");
                            else
                                sw.WriteLine($"{Tabs(t)}if (!novo && {campoId} > 0)");
                        }
                    }
                    else
                        sw.WriteLine($"{Tabs(t)}if (!novo && {campoId} > 0)");


                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t)}try");
                    sw.WriteLine($"{Tabs(t++)}{{");

                    if (compositekeysNH.Count > 0)
                    {

                        sw.Write($"{Tabs(t)}{Entidade} local{titEntidade} = {titEntidade}Rep.GetByField(");
                        int cnt = 0;
                        foreach (string compositeKey in compositekeysNH)
                        {
                            if (cnt > 0) sw.Write($",");
                            sw.Write($"\"{compositeKey}\",{Entidade}.{compositeKey}");
                            cnt++;
                        }
                        sw.WriteLine(");");

                        //sw.WriteLine($"{Tabs(t)}{Entidade} local{titEntidade} = {titEntidade}Rep.GetById({campoId});");//
                    }
                    else
                    {
                        sw.WriteLine($"{Tabs(t)}{Entidade} local{titEntidade} = {titEntidade}Rep.GetById({campoId});");
                    }

                    sw.WriteLine($"{Tabs(t)}");
                    sw.WriteLine($"{Tabs(t)}if (local{titEntidade} == null)");
                    sw.WriteLine($"{Tabs(t++)}{{");

                    if (compositekeysNH.Count > 0)
                    {
                        sw.Write($"{Tabs(t)}result.AddMensagem(\"\", $\"{Entidade} ");
                        int cnt = 0;
                        foreach (string compositeKey in compositekeysNH)
                        {
                            if (cnt > 0) sw.Write($",");
                            sw.Write($"{compositeKey}:{{{Entidade}.{compositeKey}}}");
                            cnt++;
                        }
                        sw.WriteLine(" não localizado(a) para atualização.\", true);");
                    }
                    else
                    {
                        sw.WriteLine($"{Tabs(t)}result.AddMensagem(\"\", $\"{Entidade} {campoId}:{{{campoId}}} não localizado(a) para atualização.\", true);");
                    }

                    //20.08.2021
                    sw.WriteLine($"{Tabs(t)}logFim = DateTime.Now;");
                    sw.WriteLine($"{Tabs(t)}logResumo.Info($\"Resumo:Post{titEntidade}s Início:{{logInicio.ToString(\"yyyy/MM/dd HH:mm:ss.fff\")}} Fim:{{logFim.ToString(\"yyyy/MM/dd HH:mm:ss.fff\")}} Tempo:{{(logFim - logInicio).TotalMilliseconds}} Result:{{result.Status}}\");");

                    sw.WriteLine($"{Tabs(t)}return result;");
                    sw.WriteLine($"{Tabs(--t)}}}");

                    sw.WriteLine($"{Tabs(t)}else");

                    sw.WriteLine($"{Tabs(t++)}{{");

                    //NHibernateHelperStatic.CopiaCampos(ae009a, localae009a, new string[] { "a009codlyt" });

                    if (compositekeysNH.Count > 0)
                    {
                        sw.Write($"{Tabs(t)}NHibernateHelperStatic.CopiaCampos({Entidade}, local{titEntidade}, new string[] {{");
                        int cnt = 0;
                        foreach (string compositeKey in compositekeysNH)
                        {
                            if (cnt > 0) sw.Write($",");
                            sw.Write($"\"{compositeKey}\"");
                            cnt++;
                        }
                        sw.WriteLine($"}}); ");
                    }
                    else
                    {
                        if (campoId.Contains("."))
                        {
                            int pos = campoId.LastIndexOf('.');

                            sw.WriteLine($"{Tabs(t)}NHibernateHelperStatic.CopiaCampos({Entidade}, local{titEntidade}, new string[] {{\"{campoId.Substring(pos + 1)}\"}});");
                        }
                        else
                            sw.WriteLine($"{Tabs(t)}NHibernateHelperStatic.CopiaCampos({Entidade}, local{titEntidade}, new string[] {{\"{campoId}\"}});");
                    }

                    //sw.WriteLine($"{Tabs(t)}local{titEntidade}.CopiaCampos({Entidade});");
                    sw.WriteLine($"{Tabs(t)}{titEntidade}Rep.SaveOrUpdate(local{titEntidade});");
                    sw.WriteLine($"{Tabs(t)}result.AddMensagem(\"\", $\"{Entidade} atualizado com sucesso.\", false);");
                    sw.WriteLine($"{Tabs(t)}result.{Entidade} = local{titEntidade};");
                    sw.WriteLine($"{Tabs(t)}result.Item = result.{Entidade} as object;");
                    sw.WriteLine($"{Tabs(--t)}}}");
                    sw.WriteLine($"{Tabs(--t)}}}");
                    sw.WriteLine($"{Tabs(t)}catch(Exception e)");
                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t)}result.AddMensagem(\"\", $\"Erro ao atualizar {Entidade}.\", true);");
                    sw.WriteLine($"{Tabs(t)}log.Error($\"Erro ao atualizar {Entidade}.\", e);");
                    sw.WriteLine($"{Tabs(t)}result.Status = \"ERRO\";");
                    sw.WriteLine($"{Tabs(--t)}}}");
                    sw.WriteLine($"{Tabs(--t)}}}");
                    sw.WriteLine($"{Tabs(t)}else");
                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t)}try");
                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t)}{titEntidade}Rep.SaveOrUpdate({Entidade});");
                    sw.WriteLine($"{Tabs(t)}result.AddMensagem(\"\", $\"{Entidade} criado com sucesso.\", false);");
                    sw.WriteLine($"{Tabs(t)}result.{Entidade} = {Entidade};");
                    sw.WriteLine($"{Tabs(t)}result.Item = result.{Entidade} as object;");
                    sw.WriteLine($"{Tabs(--t)}}}");
                    sw.WriteLine($"{Tabs(t)}catch(Exception e)");
                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t)}result.AddMensagem(\"\", $\"Erro ao criar {Entidade}.\", true);");
                    sw.WriteLine($"{Tabs(t)}log.Error($\"Erro ao criar {Entidade}.\", e);");
                    sw.WriteLine($"{Tabs(--t)}}}");
                    sw.WriteLine($"{Tabs(--t)}}}");

                    //20.08.2021
                    sw.WriteLine($"{Tabs(t)}logFim = DateTime.Now;");
                    sw.WriteLine($"{Tabs(t)}logResumo.Info($\"Resumo:Post{titEntidade}s Início:{{logInicio.ToString(\"yyyy/MM/dd HH:mm:ss.fff\")}} Fim:{{logFim.ToString(\"yyyy/MM/dd HH:mm:ss.fff\")}} Tempo:{{(logFim - logInicio).TotalMilliseconds}} Result:{{result.Status}}\");");


                    sw.WriteLine($"{Tabs(t)}return result;");
                    sw.WriteLine($"{Tabs(--t)}}}");
                }
                sw.WriteLine($"{Tabs(t)}#endregion");
            }
            catch (Exception ex)
            {
                log.Error("Exception ao gerar Post Objeto:", ex);
                return false;
            }
            #endregion

            #region Put Objeto
            try
            {
                sw.WriteLine($"{Tabs(t)}#region Put{Entidade}");
                if (!propOk || (string.IsNullOrEmpty(idNameNH) && compositekeysNH.Count == 0))
                {
                    sw.WriteLine($"{Tabs(t)}//public {titEntidade}Result Put{titEntidade}(?) - Não gerado, não foi possível identificar chave única");
                }
                else
                {
                    sw.WriteLine($"{Tabs(t)}/// <summary>");
                    sw.WriteLine($"{Tabs(t)}/// Put{titEntidade}");
                    sw.WriteLine($"{Tabs(t)}/// </summary>");
                    sw.WriteLine($"{Tabs(t)}/// <param name=\"{Entidade}\">{Entidade}</param>");
                    sw.WriteLine($"{Tabs(t)}/// <returns>{titEntidade}Result</returns>");
                    sw.WriteLine($"{Tabs(t)}public {titEntidade}Result Put{titEntidade}({Entidade} {Entidade})");
                    sw.WriteLine($"{Tabs(t++)}{{");

                    //20.08.2021
                    sw.WriteLine($"{Tabs(t)}DateTime logInicio = DateTime.Now;");
                    sw.WriteLine($"{Tabs(t)}DateTime logFim;");
                    sw.WriteLine($"{Tabs(t)}");
                    sw.WriteLine($"{Tabs(t)}logAtv.Info($\"Put{titEntidade}:{{JsonConvert.SerializeObject({Entidade})}}\");");

                    sw.WriteLine($"{Tabs(t)}{titEntidade}Result result = new {titEntidade}Result");
                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t)}Status = \"OK\",");
                    sw.WriteLine($"{Tabs(--t)}}};");
                    sw.WriteLine($"{Tabs(t)}");


                    //01.07.2021
                    if (!string.IsNullOrEmpty(idNameNH))
                    {
                        if (idTpNH?.Equals("decimal", StringComparison.InvariantCultureIgnoreCase) ?? false)
                            sw.WriteLine($"{Tabs(t)}if ({Entidade} == null || {Entidade}.{idNameNH} == decimal.Zero)");
                        else
                            if (idTpNH?.Equals("string", StringComparison.InvariantCultureIgnoreCase) ?? false)
                            sw.WriteLine($"{Tabs(t)}if ({Entidade} == null || {Entidade}.{idNameNH}.Equals(\"\"))");
                        else
                            sw.WriteLine($"{Tabs(t)}if ({Entidade} == null || {Entidade}.{idNameNH} == decimal.Zero)");
                    }
                    else
                    {
                        //Chave composta?

                        if (compositekeysNH.Count > 0)
                        {
                            
                            sw.Write($"{Tabs(t)}if ({Entidade} == null || (");
                            int cnt = 0;
                            foreach (string compositeKey in compositekeysNH)
                            {
                                GetNHibernateProps(Entidade, compositeKey);

                                if (cnt > 0) 
                                    sw.Write(" && ");

                                if (tpNH.Equals("string", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    sw.Write($"string.IsNullOrEmpty({Entidade}.{compositeKey})");
                                }
                                else
                                {
                                    if (tpNH.Equals("decimal", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        sw.Write($"{Entidade}.{compositeKey} == 0");
                                    } else
                                        sw.Write($"{compositeKey} == null ");
                                }

                                cnt++;
                            }
                            sw.WriteLine($"))");

                            //Le novamente todos
                            propOk = GetNHibernateProps(Entidade, null);

                            //sw.WriteLine($"{Tabs(t)}if ({Entidade} == null)");
                        }
                        else
                        {
                            sw.WriteLine($"{Tabs(t)}if ({Entidade} == null || {Entidade}.Id == decimal.Zero)");
                        }
                    }

                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t)}MensagemModel msg = new MensagemModel");
                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t)}Campo = \"\",");
                    sw.WriteLine($"{Tabs(t)}Mensagem = ({Entidade} == null ? \"Item não pode ser nulo\" : \"Para atualização é necessário informar o identificador.\"),");
                    sw.WriteLine($"{Tabs(t)}Erro = true");
                    sw.WriteLine($"{Tabs(--t)}}};");
                    sw.WriteLine($"{Tabs(t)}result.Status = \"ERRO\";");
                    sw.WriteLine($"{Tabs(t)}result.Mensagens = new List<MensagemModel>();");
                    sw.WriteLine($"{Tabs(t)}result.Mensagens.Add(msg);");

                    //20.08.2021
                    sw.WriteLine($"{Tabs(t)}logFim = DateTime.Now;");
                    sw.WriteLine($"{Tabs(t)}logResumo.Info($\"Resumo:Put{titEntidade}s Início:{{logInicio.ToString(\"yyyy/MM/dd HH:mm:ss.fff\")}} Fim:{{logFim.ToString(\"yyyy/MM/dd HH:mm:ss.fff\")}} Tempo:{{(logFim - logInicio).TotalMilliseconds}} Result:{{result.Status}}\");");

                    sw.WriteLine($"{Tabs(t)}return result;");
                    sw.WriteLine($"{Tabs(--t)}}}");
                    sw.WriteLine($"{Tabs(t)}return Post{titEntidade} ({Entidade},false);");
                    sw.WriteLine($"{Tabs(--t)}}}");
                }
                sw.WriteLine($"{Tabs(t)}#endregion");
            }
            catch (Exception ex)
            {
                log.Error("Exception ao gerar Put Objeto:", ex);
                return false;
            }
            #endregion

            #region Delete Objeto
            try
            {
                sw.WriteLine($"{Tabs(t)}#region Delete{titEntidade}");
                if (!propOk || (string.IsNullOrEmpty(idNameNH) && compositekeysNH.Count == 0))
                {
                    sw.WriteLine($"{Tabs(t)}//public {titEntidade}Result Delete{titEntidade}(?) - Não gerado, não foi possível identificar chave única");
                }
                else
                {
                    if (string.IsNullOrEmpty(idTpNH))
                        idTpNH = "decimal";

                    int idx = idTpNH.IndexOf("(");

                    if (idx > 0)
                        idTpNH = idTpNH.Substring(0, idx);


                    sw.WriteLine($"{Tabs(t)}/// <summary>");
                    sw.WriteLine($"{Tabs(t)}/// Delete{titEntidade}");
                    sw.WriteLine($"{Tabs(t)}/// </summary>");

                    if (compositekeysNH.Count > 0)
                    {
                        foreach (string compositeKey in compositekeysNH)
                        {
                            sw.WriteLine($"{Tabs(t)}/// <param name=\"{compositeKey}\">{compositeKey} de {Entidade}</param>");
                        }
                    }
                    else
                    {

                        sw.WriteLine($"{Tabs(t)}/// <param name=\"{idNameNH}\">{idNameNH} de {Entidade}</param>");
                    }

                    sw.WriteLine($"{Tabs(t)}/// <returns>{titEntidade}Result</returns>");

                    sw.Write($"{Tabs(t)}public {titEntidade}Result Delete{titEntidade}(");

                    if (compositekeysNH.Count > 0)
                    {
                        int cnt = 0;
                        foreach (string compositeKey in compositekeysNH)
                        {
                            if (cnt > 0) sw.Write($",");
                            GetNHibernateProps(Entidade, compositeKey);
                            sw.Write($"{tpNH} {compositeKey}");
                            cnt++;
                        }
                        //Le novamente todos
                        propOk = GetNHibernateProps(Entidade, null);
                    }
                    else
                    {
                        sw.Write($"{idTpNH} {idNameNH}");
                    }
                    sw.WriteLine($")");


                    sw.WriteLine($"{Tabs(t++)}{{");

                    //20.08.2021
                    sw.WriteLine($"{Tabs(t)}DateTime logInicio = DateTime.Now;");
                    sw.WriteLine($"{Tabs(t)}DateTime logFim;");
                    sw.WriteLine($"{Tabs(t)}");

                    StringBuilder infoCampos = new StringBuilder();

                    if (compositekeysNH.Count > 0)
                    {

                        foreach (string compositeKey in compositekeysNH)
                        {
                            GetNHibernateProps(Entidade, compositeKey);
                            if (tpNH?.Equals("string", StringComparison.InvariantCultureIgnoreCase) ?? false)
                            {
                                infoCampos.Append($" {{{compositeKey}}}");
                            }
                            else
                            {
                                infoCampos.Append($" {{{compositeKey}.ToString()}}");
                            }
                        }

                        propOk = GetNHibernateProps(Entidade, null);
                    }
                    else
                    {
                        infoCampos.Append($"Id {{{idNameNH}.ToString()}}");
                    }

                    
                    sw.WriteLine($"{Tabs(t)}logAtv.Info($\"Delete{titEntidade}:{infoCampos}\");");

                    sw.WriteLine($"{Tabs(t)}{titEntidade}Result result = new {titEntidade}Result");
                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t)}Status = \"OK\"");
                    sw.WriteLine($"{Tabs(--t)}}};");
                    sw.WriteLine($"{Tabs(t)}");
                    sw.WriteLine($"{Tabs(t)}try");
                    sw.WriteLine($"{Tabs(t++)}{{");

                    if (compositekeysNH.Count > 0)
                    {
                        sw.Write($"{Tabs(t)}result.{Entidade} = {titEntidade}Rep.GetByField(");
                        int cnt = 0;
                        foreach (string compositeKey in compositekeysNH)
                        {
                            if (cnt > 0) sw.Write($",");
                            sw.Write($"\"{compositeKey}\",{compositeKey}");
                            cnt++;
                        }
                        sw.WriteLine(");");
                    }
                    else
                    {
                        sw.WriteLine($"{Tabs(t)}result.{Entidade} = {titEntidade}Rep.GetById({idNameNH});");
                    }

                    sw.WriteLine($"{Tabs(t)}if(result.{Entidade} != null)");
                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t)}{titEntidade}Rep.Delete(result.{Entidade});");
                    sw.WriteLine($"{Tabs(t)}result.{Entidade} = null;");

                    sw.WriteLine($"{Tabs(t)}result.AddMensagem(\"\",$\"{Entidade} {infoCampos} excluído(a) com sucesso.\",false);");
                    sw.WriteLine($"{Tabs(--t)}}}");
                    sw.WriteLine($"{Tabs(t)}else");
                    sw.WriteLine($"{Tabs(t++)}{{");

                    sw.WriteLine($"{Tabs(t)}result.AddMensagem(\"\",$\"{Entidade} {infoCampos} não localizado(a) para exclusão.\",true);");
                    sw.WriteLine($"{Tabs(t)}result.Status = \"ERRO\";");
                    sw.WriteLine($"{Tabs(--t)}}}");
                    sw.WriteLine($"{Tabs(--t)}}} catch(Exception e)");
                    sw.WriteLine($"{Tabs(t++)}{{");
                    sw.WriteLine($"{Tabs(t)}result.AddMensagem(\"\",$\"Erro ao excluir {Entidade} {infoCampos}.\",true);");
                    sw.WriteLine($"{Tabs(t)}result.Status = \"ERRO\";");
                    sw.WriteLine($"{Tabs(t)}log.Error($\"Erro ao excluir {Entidade} {infoCampos}.\",e);");
                    sw.WriteLine($"{Tabs(--t)}}}");

                    sw.WriteLine($"{Tabs(t)}");

                    //20.08.2021
                    sw.WriteLine($"{Tabs(t)}logFim = DateTime.Now;");
                    sw.WriteLine($"{Tabs(t)}logResumo.Info($\"Resumo:Delete{titEntidade}s Início:{{logInicio.ToString(\"yyyy/MM/dd HH:mm:ss.fff\")}} Fim:{{logFim.ToString(\"yyyy/MM/dd HH:mm:ss.fff\")}} Tempo:{{(logFim - logInicio).TotalMilliseconds}} Result:{{result.Status}}\");");

                    sw.WriteLine($"{Tabs(t)}return result;");
                    sw.WriteLine($"{Tabs(--t)}}}");
                }
                sw.WriteLine($"{Tabs(t)}#endregion");
            }
            catch (Exception ex)
            {
                log.Error("Exception ao gerar Delete Objeto:", ex);
                return false;
            }

            #endregion

            #region ValidaCampos
            try
            {
                sw.WriteLine($"{Tabs(t)}#region ValidaCampos");
                sw.WriteLine($"{Tabs(t)}/// <summary>");
                sw.WriteLine($"{Tabs(t)}/// ValidaCampos - Valida campos");
                sw.WriteLine($"{Tabs(t)}/// </summary>");
                sw.WriteLine($"{Tabs(t)}/// <param name=\"{Entidade}\">{Entidade}</param>");
                sw.WriteLine($"{Tabs(t)}/// <param name=\"result\">result</param>");
                sw.WriteLine($"{Tabs(t)}/// <returns></returns>");
                sw.WriteLine($"{Tabs(t)}public virtual bool ValidaCampos({Entidade} {Entidade}, {titEntidade}Result result)");
                sw.WriteLine($"{Tabs(t++)}{{");
                sw.WriteLine($"{Tabs(t)}return true;");
                sw.WriteLine($"{Tabs(--t)}}}");
                sw.WriteLine($"{Tabs(t)}#endregion");
            }
            catch (Exception ex)
            {
                log.Error("Exception ao gerar Valida Campos:", ex);
                return false;
            }
            #endregion

            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}");

            sw.Close();

            return true;
        }
        #endregion

        #region Gera Classe Negocio Por Entidade
        /// <summary>
        /// Gera Classe Negocio
        /// </summary>

        public bool GeraInterfaceNegocioPorEntidade(string Entidade, StreamWriter sw)
        {
            log.Info($"GeraClasseNegocioPorEntidade: {Entidade}...");



            string titEntidade = Entidade.Substring(0, 1).ToUpper() + Entidade.Substring(1);

            int t = 2;

            bool propOk = GetNHibernateProps(Entidade, null);

            sw.WriteLine($"{Tabs(t)}#region {titEntidade}");
            #region GetEntidades

            sw.WriteLine($"{Tabs(t)}/// <summary>");
            sw.WriteLine($"{Tabs(t)}/// Get{titEntidade}s");
            sw.WriteLine($"{Tabs(t)}/// </summary>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"inicio\"></param>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"numeroRegistros\"></param>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"orderBy\"></param>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"parametrosQry\"></param>");

            sw.WriteLine($"{Tabs(t)}{titEntidade}sResult Get{titEntidade}s(int inicio=0, int numeroRegistros=0, string orderBy=\"\", params string[] parametrosQry);");
            #endregion
            #region GetEntidade
            if (!propOk || (string.IsNullOrEmpty(idNameNH) && compositekeysNH.Count == 0))
            {
                sw.WriteLine($"{Tabs(t)}//{titEntidade}Result Get{titEntidade}(?) - Não gerado, não foi possível identificar chave única");
            }
            else
            {
                if (string.IsNullOrEmpty(idTpNH))
                    idTpNH = "decimal";

                int idx = idTpNH.IndexOf("(");

                if (idx > 0)
                    idTpNH = idTpNH.Substring(0, idx);

                sw.WriteLine($"{Tabs(t)}/// <summary>");
                sw.WriteLine($"{Tabs(t)}/// Get{titEntidade}");
                sw.WriteLine($"{Tabs(t)}/// </summary>");
                if (compositekeysNH.Count > 0)
                {
                    foreach (string compositeKey in compositekeysNH)
                    {
                        sw.WriteLine($"{Tabs(t)}/// <param name=\"{compositeKey}\">{compositeKey} de {Entidade}</param>");
                    }
                }
                else
                {

                    sw.WriteLine($"{Tabs(t)}/// <param name=\"{idNameNH}\">{idNameNH} de {Entidade}</param>");
                }

                sw.Write    ($"{Tabs(t)}{titEntidade}Result Get{titEntidade}(");

                if (compositekeysNH.Count > 0)
                {
                    int cnt = 0;
                    foreach (string compositeKey in compositekeysNH)
                    {
                        if (cnt > 0) sw.Write($",");
                        GetNHibernateProps(Entidade, compositeKey);
                        sw.Write($"{tpNH} {compositeKey}");
                        cnt++;
                    }
                    //Le novamente todos
                    propOk = GetNHibernateProps(Entidade, null);
                }
                else
                {
                    sw.Write($"{idTpNH} {idNameNH}");
                }
                sw.WriteLine($");");

            }
            #endregion
            #region Post Objeto

            if (!propOk)
            {
                sw.WriteLine($"{Tabs(t)}//{titEntidade}Result Post{titEntidade}(?) - Não gerado, problema ao buscar propriedades NHibernate");
            }
            else
            {
                sw.WriteLine($"{Tabs(t)}/// <summary>");
                sw.WriteLine($"{Tabs(t)}/// Post{titEntidade} - Insere {titEntidade}");
                sw.WriteLine($"{Tabs(t)}/// </summary>");
                sw.WriteLine($"{Tabs(t)}/// <param name=\"{Entidade}\"></param>");
                sw.WriteLine($"{Tabs(t)}/// <param name=\"novo\"></param>");
                sw.WriteLine($"{Tabs(t)}{titEntidade}Result Post{titEntidade}({Entidade} {Entidade}, bool novo = true);");
            }
            #endregion
            #region Put Objeto

            if (!propOk || (string.IsNullOrEmpty(idNameNH) && compositekeysNH.Count == 0))
            {
                sw.WriteLine($"{Tabs(t)}//{titEntidade}Result Put{titEntidade}(?) - Não gerado, não foi possível identificar chave única");
            }
            else
            {
                sw.WriteLine($"{Tabs(t)}/// <summary>");
                sw.WriteLine($"{Tabs(t)}/// Put{titEntidade} - Atualiza {titEntidade}");
                sw.WriteLine($"{Tabs(t)}/// </summary>");
                sw.WriteLine($"{Tabs(t)}/// <param name=\"{Entidade}\"></param>");
                sw.WriteLine($"{Tabs(t)}{titEntidade}Result Put{titEntidade}({Entidade} {Entidade});");
            }
            #endregion

            #region Delete Objeto
            if (!propOk || (string.IsNullOrEmpty(idNameNH) && compositekeysNH.Count == 0))
            {
                sw.WriteLine($"{Tabs(t)}//{titEntidade}Result Delete{titEntidade}(?) - Não gerado, não foi possível identificar chave única");
            }
            else
            {
                if (string.IsNullOrEmpty(idTpNH))
                    idTpNH = "decimal";

                int idx = idTpNH.IndexOf("(");

                if (idx > 0)
                    idTpNH = idTpNH.Substring(0, idx);

                sw.WriteLine($"{Tabs(t)}/// <summary>");
                sw.WriteLine($"{Tabs(t)}/// Delete{titEntidade} - Deleta {titEntidade}");
                sw.WriteLine($"{Tabs(t)}/// </summary>");
                
                if (compositekeysNH.Count > 0)
                {
                    foreach (string compositeKey in compositekeysNH)
                    {
                        sw.WriteLine($"{Tabs(t)}/// <param name=\"{compositeKey}\">{compositeKey} de {Entidade}</param>");
                    }
                }
                else
                {

                    sw.WriteLine($"{Tabs(t)}/// <param name=\"{idNameNH}\">{idNameNH} de {Entidade}</param>");
                }


                sw.Write($"{Tabs(t)}{titEntidade}Result Delete{titEntidade}(");

                if (compositekeysNH.Count > 0)
                {
                    int cnt = 0;
                    foreach (string compositeKey in compositekeysNH)
                    {
                        if (cnt > 0) sw.Write($",");
                        GetNHibernateProps(Entidade, compositeKey);
                        sw.Write($"{tpNH} {compositeKey}");
                        cnt++;
                    }
                    //Le novamente todos
                    propOk = GetNHibernateProps(Entidade, null);
                }
                else
                {
                    sw.Write($"{idTpNH} {idNameNH}");
                }
                sw.WriteLine($");");
            }
            #endregion
            #region ValidaCampos
            sw.WriteLine($"{Tabs(t)}/// <summary>");
            sw.WriteLine($"{Tabs(t)}/// ValidaCampos");
            sw.WriteLine($"{Tabs(t)}/// </summary>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"{Entidade}\"></param>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"result\"></param>");
            sw.WriteLine($"{Tabs(t)}bool ValidaCampos({Entidade} {Entidade}, {titEntidade}Result result);");
            #endregion
            sw.WriteLine($"{Tabs(t)}#endregion {titEntidade}");
            sw.WriteLine($"{Tabs(t)}");
            return true;
        }
        #endregion

    }
}
