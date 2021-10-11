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
        #region Gera Classe Controller
        /// <summary>
        /// Gera Classe Negocio
        /// </summary>

        public bool GeraClassesController()
        {
            log.Info($"GeraClasseController...");
            int t = 0;
            StreamWriter sw;

            #region Cria arquivo ClasseControllerBase.cs

            try
            {
                string workdir = Path.Combine(diretorioApi, "Controllers");
                if (!Directory.Exists(workdir))
                    Directory.CreateDirectory(workdir);
                sw = new StreamWriter(File.Open($"{workdir}{Path.DirectorySeparatorChar}{nomeApi}ControllerBase.cs", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error("Exception ao gerar ClasseControllerBase:", ex);
                return false;
            }
            #endregion

            #region Class Controller Base 
            sw.WriteLine($"{Tabs(t)}using {nomeApi}.Helpers;");
            sw.WriteLine($"{Tabs(t)}using {nomeApi}.Negocio;");
            sw.WriteLine($"{Tabs(t)}using {nomeApi}.Services;");
            sw.WriteLine($"{Tabs(t)}using Microsoft.AspNetCore.Authorization;");
            sw.WriteLine($"{Tabs(t)}using Microsoft.AspNetCore.Mvc;");
            sw.WriteLine($"{Tabs(t)}using Microsoft.Extensions.Logging;");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}namespace {nomeApi}.Controllers");
            sw.WriteLine($"{Tabs(t++)}{{");

            sw.WriteLine($"{Tabs(t)}/// <summary>");
            sw.WriteLine($"{Tabs(t)}/// Classe Controller {nomeApi}");
            sw.WriteLine($"{Tabs(t)}/// *Criada pelo GeradorCoreAPI, não editar! Inclua em outro arquivo class partial para evitar sobreposição");
            sw.WriteLine($"{Tabs(t)}/// </summary>");
            sw.WriteLine($"{Tabs(t)}[Authorize]");
            sw.WriteLine($"{Tabs(t)}public class {nomeApi}ControllerBase : ControllerBase");
            sw.WriteLine($"{Tabs(t++)}{{");
            #endregion

            sw.WriteLine($"{Tabs(t)}#region Atributos diversos");
            sw.WriteLine($"{Tabs(t)}protected ILogger<{nomeApi}Controller> logger {{ get; set; }}");
            sw.WriteLine($"{Tabs(t)}protected IUserService userService {{ get; set; }}");
            sw.WriteLine($"{Tabs(t)}protected AppSettings appSettings {{ get; set; }}");
            sw.WriteLine($"{Tabs(t)}protected INegocio{siglaApi}BO negocio {{ get; set; }}");
            sw.WriteLine($"{Tabs(t)}#endregion");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}#region Construtor");
            sw.WriteLine($"{Tabs(t)}/// <summary>");
            sw.WriteLine($"{Tabs(t)}/// Construtor");
            sw.WriteLine($"{Tabs(t)}/// </summary>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"userService\"></param>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"logger\"></param>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"negocio\"></param>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"options\"></param>");

            //	public AjudaController(IUserService userService, ILogger<ApiMOVController> logger, INegocioMOVBO negocio, IWritableOptions<AppSettings> options) : base(userService, logger, negocio, options)

            sw.WriteLine($"{Tabs(t)}public {nomeApi}ControllerBase(IUserService userService, ILogger<{nomeApi}Controller> logger, INegocio{siglaApi}BO negocio, IWritableOptions<AppSettings> options)");

            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}this.logger = logger;");
            sw.WriteLine($"{Tabs(t)}this.userService = userService;");
            sw.WriteLine($"{Tabs(t)}this.negocio = negocio;");
            sw.WriteLine($"{Tabs(t)}this.appSettings = options.Get(\"\");");

            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}#endregion");
            
            #region Finaliza arquivo ClasseControllerBase.cs
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine("");
            sw.Close();

            bool ok = true;
            #endregion

            #region Cria arquivo ClasseController.cs

            try
            {
                string workdir = Path.Combine(diretorioApi, "Controllers");
                if (!Directory.Exists(workdir))
                    Directory.CreateDirectory(workdir);
                sw = new StreamWriter(File.Open($"{workdir}{Path.DirectorySeparatorChar}{nomeApi}Controller.cs", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error("Exception ao gerar ClasseController:", ex);
                return false;
            }
            #endregion

            #region Class Controller 

            sw.WriteLine($"{Tabs(t)}using System;");
            sw.WriteLine($"{Tabs(t)}using System.Collections.Generic;");
            sw.WriteLine($"{Tabs(t)}using System.IdentityModel.Tokens.Jwt;");
            sw.WriteLine($"{Tabs(t)}using System.Security.Claims;");
            sw.WriteLine($"{Tabs(t)}using System.Text;");
            sw.WriteLine($"{Tabs(t)}using System.Threading.Tasks;");

            sw.WriteLine($"{Tabs(t)}using {nomeApi}.Helpers;");
            sw.WriteLine($"{Tabs(t)}using {nomeApi}.Model;");
            sw.WriteLine($"{Tabs(t)}using {nomeApi}.Negocio;");
            sw.WriteLine($"{Tabs(t)}using {nomeApi}.Services;");
            sw.WriteLine($"{Tabs(t)}using log4net;");
            
            sw.WriteLine($"{Tabs(t)}using Microsoft.AspNetCore.Authorization;");
            sw.WriteLine($"{Tabs(t)}using Microsoft.AspNetCore.Mvc;");
            sw.WriteLine($"{Tabs(t)}using Microsoft.Extensions.Logging;");
            sw.WriteLine($"{Tabs(t)}using Microsoft.IdentityModel.Tokens;");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}namespace {nomeApi}.Controllers");
            sw.WriteLine($"{Tabs(t++)}{{");

            sw.WriteLine($"{Tabs(t)}/// <summary>");
            sw.WriteLine($"{Tabs(t)}/// Classe Controller {nomeApi}");
            sw.WriteLine($"{Tabs(t)}/// *Criada pelo GeradorCoreAPI, não editar! Inclua em outro arquivo class partial para evitar sobreposição");
            sw.WriteLine($"{Tabs(t)}/// </summary>");
            sw.WriteLine($"{Tabs(t)}[Authorize]");
            sw.WriteLine($"{Tabs(t)}[ApiController]");
            sw.WriteLine($"{Tabs(t)}[Route(\"[controller]\")]");
            sw.WriteLine($"{Tabs(t)}public partial class {nomeApi}Controller: {nomeApi}ControllerBase");
            sw.WriteLine($"{Tabs(t++)}{{");
            #endregion

            #region Gera Atributos Diversos
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}#region Atributos diversos");
            sw.WriteLine($"{Tabs(t)}/// <summary>");
            sw.WriteLine($"{Tabs(t)}/// log4net");
            sw.WriteLine($"{Tabs(t)}/// </summary>");

            sw.WriteLine($"{Tabs(t)}private static readonly ILog log = LogManager.GetLogger(typeof({nomeApi}Controller));");
            sw.WriteLine($"{Tabs(t)}");
            //sw.WriteLine($"{Tabs(t)}private readonly ILogger<{nomeApi}Controller> _logger;"); Declarados na classe base
            //sw.WriteLine($"{Tabs(t)}private IUserService _userService;");
            //sw.WriteLine($"{Tabs(t)}private Negocio{siglaApi}BO neg = null;");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}#endregion");
            sw.WriteLine($"{Tabs(t)}");
            #endregion

            #region Gera Construtor
            sw.WriteLine($"{Tabs(t)}#region Construtor");
            sw.WriteLine($"{Tabs(t)}/// <summary>");
            sw.WriteLine($"{Tabs(t)}/// Construtor");
            sw.WriteLine($"{Tabs(t)}/// </summary>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"userService\"></param>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"logger\"></param>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"negocio\"></param>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"options\"></param>");
           
            sw.WriteLine($"{Tabs(t)}public {nomeApi}Controller(IUserService userService, ILogger<{nomeApi}Controller> logger, INegocio{siglaApi}BO negocio,IWritableOptions<AppSettings> options) : base(userService, logger, negocio, options)");
            sw.WriteLine($"{Tabs(t++)}{{");
           
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}#endregion");
            sw.WriteLine($"{Tabs(t)}");
            #endregion

            #region Gera Authenticate
            sw.WriteLine($"{Tabs(t)}#region Authenticate");
            sw.WriteLine($"{Tabs(t)}/// <summary>");
            sw.WriteLine($"{Tabs(t)}/// Faz autenticação para acesso Endpoints API");
            sw.WriteLine($"{Tabs(t)}/// </summary>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"model\">Parametros Username e Password</param>");
            sw.WriteLine($"{Tabs(t)}/// <returns></returns>");
            sw.WriteLine($"{Tabs(t)}[AllowAnonymous]");
            sw.WriteLine($"{Tabs(t)}[HttpPost(\"authenticate\")]");
            sw.WriteLine($"{Tabs(t)}public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel model)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}Services.UsuarioAPI user = await userService.Authenticate(model.usuario, model.senha);");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t++)}if (user == null)");
            sw.WriteLine($"{Tabs(t--)}return BadRequest(new {{ message = \"Usuário ou senha incorretos.\" }});");
            sw.WriteLine($"{Tabs(t)}");

            sw.WriteLine($"{Tabs(t)}var tokenHandler = new JwtSecurityTokenHandler();");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}var key = Encoding.ASCII.GetBytes(appSettings.webapi_segredo);");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}var claims = new List<Claim>();");
            sw.WriteLine($"{Tabs(t)}claims.Add(new Claim(ClaimTypes.Name, user.Id.ToString()));");
            sw.WriteLine($"{Tabs(t)}if (user.AddicionalProperties != null)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}foreach (Dictionary<string, string> dicts in user.AddicionalProperties)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}if (dicts == null)");
            sw.WriteLine($"{Tabs(t)}    continue;");
            sw.WriteLine($"{Tabs(t)}foreach (var dict in dicts)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}claims.Add(new Claim(dict.Key, dict.Value));");
            sw.WriteLine($"{Tabs(t--)}}};");
            sw.WriteLine($"{Tabs(t--)}}};");
            sw.WriteLine($"{Tabs(t--)}}};");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}var tokenDescriptor = new SecurityTokenDescriptor");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}Subject = new ClaimsIdentity(claims),");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}//Expires = DateTime.UtcNow.AddDays(7),");
            sw.WriteLine($"{Tabs(t)}Expires = DateTime.Now.AddYears(10),");
            sw.WriteLine($"{Tabs(t)}SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)");
            sw.WriteLine($"{Tabs(t--)}}};");
            sw.WriteLine($"{Tabs(t)}var token = tokenHandler.CreateToken(tokenDescriptor);");
            sw.WriteLine($"{Tabs(t)}var tokenString = tokenHandler.WriteToken(token);");

            sw.WriteLine($"{Tabs(t)}//    string accessToken = User.Claims.FirstOrDefault(c => c.Type == \"access_token\")?.Value;");

            sw.WriteLine($"{Tabs(t)}// Retorna informaçoes basicas do usuario e token para ser armazenado");
            sw.WriteLine($"{Tabs(t)}return Ok(new");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}Id = user.Id,");
            sw.WriteLine($"{Tabs(t)}Username = user.Username,");
            sw.WriteLine($"{Tabs(t)}Token = tokenString,");
            sw.WriteLine($"{Tabs(t)}AddicionalProperties = user.AddicionalProperties");

            sw.WriteLine($"{Tabs(t--)}}});");

            //sw.WriteLine($"{Tabs(t)}return Ok(user);");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}#endregion");
            sw.WriteLine($"{Tabs(t)}");
            #endregion

            #region Gera Valida BD
            sw.WriteLine($"{Tabs(t)}#region Valida BD");
            sw.WriteLine($"{Tabs(t)}///<summary>");
            sw.WriteLine($"{Tabs(t)}/// Valida Banco de dados - retorna Script / divergências (uso interno)");
            sw.WriteLine($"{Tabs(t)}///</summary>");
            sw.WriteLine($"{Tabs(t)}/// <returns></returns>");
            sw.WriteLine($"{Tabs(t)}[HttpGet(\"validaBD\")]");
            sw.WriteLine($"{Tabs(t)}[AllowAnonymous]");
            sw.WriteLine($"{Tabs(t)}public GenericResult ValidaBD()");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}return negocio.ValidaBD();");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}#endregion");
            sw.WriteLine($"{Tabs(t)}");
            #endregion

            #region Finaliza arquivo ClasseController.cs
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine("");
            sw.Close();

            ok = true;
            #endregion

            #region Controller__Generico
            StreamReader sr;
            try
            {
                string workdir = Path.Combine(diretorioApi, "Controllers");
                if (!Directory.Exists(workdir))
                    Directory.CreateDirectory(workdir);

                sw = new StreamWriter(File.Open($"{workdir}{Path.DirectorySeparatorChar}{nomeApi}Controller__Generico.cs", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error($"Exception {this.GetType().Name}:{MethodBase.GetCurrentMethod().Name}", ex);
                return false;
            }

            sr = File.OpenText($"Templates{Path.DirectorySeparatorChar}ControllerGenerico_cs.txt");

            string s = sr.ReadToEnd().Replace("[nomeAPI]", nomeApi).Replace("[siglaAPI]", siglaApi);

            sw.Write(s);
            sw.Close();
            #endregion

            #region Classes Controller por Entidade

            foreach (string nHibernateDll in nHibernateDlls)
            {
                dll = Assembly.LoadFrom(nHibernateDll);

                IList<string> entityNames = GetEntityNames();

                foreach (string className in entityNames)
                {
                    if (!GeraClasseControllerPorEntidade(className))
                        ok = false;
                }
            }
            #endregion

            return ok;
        }
        #endregion

        #region Gera Classe Controller Por Entidade
        /// <summary>
        /// Gera Classe Controller
        /// </summary>

        public bool GeraClasseControllerPorEntidade(string Entidade)
        {
            log.Info($"GeraClasseControllerPorEntidade: {Entidade}...");

            string titEntidade = Entidade.Substring(0, 1).ToUpper() + Entidade.Substring(1);

            //TODO tratar melhor plural?
            string apiEntidade;

            try
            {
                bool Ok = GetNHibernateProps(Entidade, null);

                object instance = Activator.CreateInstance(type);
                FieldInfo myFieldInfo = type.GetField("classePlural");

                string classePlural = (string)(myFieldInfo.GetValue(instance));
                apiEntidade = classePlural.Substring(0, 1).ToLower() + classePlural.Substring(1);
            }
            catch (Exception ex)
            {
                apiEntidade = Entidade.Substring(0, 1).ToLower() + Entidade.Substring(1) + "s";
                log.Error("Exceção ao definir classePlural/Api entidade: utilizado default", ex);
            }


            StreamWriter sw;

            try
            {
                string workdir = Path.Combine(diretorioApi, "Controllers");
                if (!Directory.Exists(workdir))
                    Directory.CreateDirectory(workdir);
                
                sw = new StreamWriter(File.Open($"{workdir}{Path.DirectorySeparatorChar}{titEntidade}Controller.cs", FileMode.Create), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                log.Error($"Exception ao gerar {titEntidade}Controller:", ex);
                return false;
            }

            int t = 0;

            bool propOk = GetNHibernateProps(Entidade, null);

            #region Classe Controller
            sw.WriteLine($"{Tabs(t)}using {nomeApi}.Helpers;");
            sw.WriteLine($"{Tabs(t)}using {nomeApi}.Model;");
            sw.WriteLine($"{Tabs(t)}using {nomeApi}.Negocio;");
            sw.WriteLine($"{Tabs(t)}using {nomeApi}.Services;");
            sw.WriteLine($"{Tabs(t)}using log4net;");
            sw.WriteLine($"{Tabs(t)}using Microsoft.AspNetCore.Mvc;");
            sw.WriteLine($"{Tabs(t)}using Microsoft.AspNetCore.Authorization;");
            sw.WriteLine($"{Tabs(t)}using Microsoft.Extensions.Logging;");
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
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}namespace {nomeApi}.Controllers");
            sw.WriteLine($"{Tabs(t++)}{{");

            sw.WriteLine($"{Tabs(t)}/// <summary>");
            sw.WriteLine($"{Tabs(t)}/// Classe Controller {siglaApi} {Entidade}");
            sw.WriteLine($"{Tabs(t)}/// *Criada pelo GeradorCoreAPI, não editar! Inclua em outro arquivo class partial para evitar sobreposição");
            sw.WriteLine($"{Tabs(t)}/// </summary>");
            sw.WriteLine($"{Tabs(t)}");
            sw.WriteLine($"{Tabs(t)}[Authorize]");
            sw.WriteLine($"{Tabs(t)}public partial class {titEntidade}Controller : {nomeApi}ControllerBase");
            sw.WriteLine($"{Tabs(t++)}{{");
            #endregion

            #region Construtor
            sw.WriteLine($"{Tabs(t)}#region Construtor");
            sw.WriteLine($"{Tabs(t)}/// <summary>");
            sw.WriteLine($"{Tabs(t)}/// Construtor");
            sw.WriteLine($"{Tabs(t)}/// </summary>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"userService\"></param>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"logger\"></param>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"negocio\"></param>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"options\"></param>");

            sw.WriteLine($"{Tabs(t)}public {titEntidade}Controller(IUserService userService, ILogger<{nomeApi}Controller> logger, INegocio{siglaApi}BO negocio,IWritableOptions<AppSettings> options) : base(userService, logger, negocio, options)");

            sw.WriteLine($"{Tabs(t++)}{{");


            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}#endregion Construtor");
            #endregion

            #region Get Lista
            sw.WriteLine($"{Tabs(t)}#region Get{titEntidade}s");
            sw.WriteLine($"{Tabs(t)}/// <summary>");
            sw.WriteLine($"{Tabs(t)}/// Retorna lista de {Entidade} - Opcional: Número inicial (parte de 0), número de registros máximo a retornar, Ordenação e parâmetros filtro.");
            sw.WriteLine($"{Tabs(t)}/// </summary>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"inicio\">Opcional: Registro inicial, 0=primeiro</param>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"numeroRegistros\">Opcional: Número máximo de registros a retornar, 0=todos</param>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"orderBy\">Opcional: Campos para Ordenação - Ex. 'Descricao desc, Id' Obs. Campos conforme mapeamento Classe.</param>");
            sw.WriteLine($"{Tabs(t)}/// <param name=\"parametrosQry\">Opcional: Array de CAMPO,VALOR,TIPO para filtro - Ex. ID,10,decimal,DESCRICAO,TESTE,string </param>");
            sw.WriteLine($"{Tabs(t)}/// <returns>{titEntidade}sResult</returns>");

            //sw.WriteLine($"{Tabs(t)}[HttpGet(\"get{titEntidade}s\")]");
            sw.WriteLine($"{Tabs(t)}[HttpGet]");
            sw.WriteLine($"{Tabs(t)}[Route(\"api/{apiEntidade}/\")]");
            if (comAutenticacao) sw.WriteLine($"{Tabs(t)}#if DEBUG");
            sw.WriteLine($"{Tabs(t)}[AllowAnonymous]");
            if (comAutenticacao) sw.WriteLine($"{Tabs(t)}#endif");
            
            sw.WriteLine($"{Tabs(t)}public {titEntidade}sResult Get{titEntidade}s(int inicio=0, int numeroRegistros=0, string orderBy=\"\",[FromQuery] params string[] parametrosQry)");
            sw.WriteLine($"{Tabs(t++)}{{");
            sw.WriteLine($"{Tabs(t)}return negocio.Get{titEntidade}s(inicio, numeroRegistros, orderBy, parametrosQry);");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}#endregion");

            #endregion

            #region Get Objeto
            try
            {
                sw.WriteLine($"{Tabs(t)}#region Get{titEntidade}");
                if (!propOk)
                {
                    sw.WriteLine($"{Tabs(t)}//public {titEntidade}Result Get{titEntidade}(?) - Não gerado, problemas na leitura das propriedades NHibernate");
                }
                else
                {
                    if (string.IsNullOrEmpty(idTpNH))
                        idTpNH = "decimal";

                    int idx = idTpNH.IndexOf("(");

                    if (idx > 0)
                        idTpNH = idTpNH.Substring(0, idx);

                    #region Coments

                    sw.WriteLine($"{Tabs(t)}/// <summary>");
                    

                    if (compositekeysNH.Count > 0)
                    {
                        sw.Write($"{Tabs(t)}/// Retorna {Entidade} por ");
                        int cnt = 0;
                        foreach (string compositeKey in compositekeysNH)
                        {
                            if (cnt > 0) sw.Write($", ");
                            sw.Write($"{compositeKey}");
                            cnt++;
                        }
                        sw.WriteLine($".");
                    }
                    else
                    {
                        sw.WriteLine($"{Tabs(t)}/// Retorna {Entidade} por {idNameNH}.");
                    }



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
                    #endregion

                    sw.WriteLine($"{Tabs(t)}[HttpGet]");
                    sw.Write($"{Tabs(t)}[Route(\"api/{apiEntidade}");

                    if (compositekeysNH.Count > 0)
                    {
                        foreach (string compositeKey in compositekeysNH)
                        {
                            sw.Write($"/{{{compositeKey}}}");
                        }
                    }
                    else
                    {
                        sw.Write($"/{{{idNameNH}}}");
                    }
                    sw.WriteLine($"\")]");


                    if (comAutenticacao) sw.WriteLine($"{Tabs(t)}#if DEBUG");
                    sw.WriteLine($"{Tabs(t)}[AllowAnonymous]");
                    if (comAutenticacao) sw.WriteLine($"{Tabs(t)}#endif");


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
                    sw.Write($"{Tabs(t)}return negocio.Get{titEntidade}(");
                    if (compositekeysNH.Count > 0)
                    {
                        int cnt = 0;
                        foreach (string compositeKey in compositekeysNH)
                        {
                            if (cnt > 0) sw.Write($",");
                            sw.Write($"{compositeKey}");
                            cnt++;
                        }
                    }
                    else
                    {
                        sw.Write($"{idNameNH}");
                    }

                    sw.WriteLine($");");
                    sw.WriteLine($"{Tabs(--t)}}}");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception ao gerar Get Objeto:", ex);
                return false;
            }
            sw.WriteLine($"{Tabs(t)}#endregion");

            #endregion

            #region Post Objeto
            try
            { 
            sw.WriteLine($"{Tabs(t)}#region Post{titEntidade}");
            if (!propOk)
            {
                sw.WriteLine($"{Tabs(t)}//public {titEntidade}Result Post{titEntidade}(?) - Não gerado, problemas na leitura das propriedades NHibernate");
            }
            else
            {
                sw.WriteLine($"{Tabs(t)}/// <summary>");
                sw.WriteLine($"{Tabs(t)}/// Insere {Entidade}.");
                sw.WriteLine($"{Tabs(t)}/// </summary>");
                sw.WriteLine($"{Tabs(t)}/// <param name=\"{Entidade}\">{Entidade}</param>");
                sw.WriteLine($"{Tabs(t)}/// <returns>{titEntidade}Result</returns>");
                sw.WriteLine($"{Tabs(t)}[HttpPost]");
                sw.WriteLine($"{Tabs(t)}[Route(\"api/{apiEntidade}/\")]");

                if (comAutenticacao) sw.WriteLine($"{Tabs(t)}#if DEBUG");
                sw.WriteLine($"{Tabs(t)}[AllowAnonymous]");
                if (comAutenticacao) sw.WriteLine($"{Tabs(t)}#endif");

                sw.WriteLine($"{Tabs(t)}public {titEntidade}Result Post{titEntidade}([FromBody] {Entidade} {Entidade})");
                sw.WriteLine($"{Tabs(t++)}{{");
                sw.WriteLine($"{Tabs(t)}return negocio.Post{titEntidade}({Entidade});");
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
            try { 
            sw.WriteLine($"{Tabs(t)}#region Put{titEntidade}");
            if (!propOk )
            {
                sw.WriteLine($"{Tabs(t)}//public {titEntidade}Result Put{titEntidade}(?) - Não gerado, problemas na leitura das propriedades NHibernate");
            }
            else
            {
                sw.WriteLine($"{Tabs(t)}/// <summary>");
                sw.WriteLine($"{Tabs(t)}/// Atualiza {Entidade}.");
                sw.WriteLine($"{Tabs(t)}/// </summary>");
                sw.WriteLine($"{Tabs(t)}/// <param name=\"{Entidade}\">{Entidade}</param>");
                sw.WriteLine($"{Tabs(t)}/// <returns>{titEntidade}Result</returns>");
                
                sw.WriteLine($"{Tabs(t)}[HttpPut]");
                sw.WriteLine($"{Tabs(t)}[Route(\"api/{apiEntidade}/\")]");

                if (comAutenticacao) sw.WriteLine($"{Tabs(t)}#if DEBUG");
                sw.WriteLine($"{Tabs(t)}[AllowAnonymous]");
                if (comAutenticacao) sw.WriteLine($"{Tabs(t)}#endif");

                sw.WriteLine($"{Tabs(t)}public {titEntidade}Result Put{titEntidade}([FromBody] {Entidade} {Entidade})");
                sw.WriteLine($"{Tabs(t++)}{{");
                sw.WriteLine($"{Tabs(t)}return negocio.Put{titEntidade}({Entidade});");
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
            sw.WriteLine($"{Tabs(t)}#region Delete{titEntidade}");
            if (!propOk )
            {
                sw.WriteLine($"{Tabs(t)}//public {titEntidade}Result Delete{titEntidade}(?) - Não gerado, problemas na leitura das propriedades NHibernate");
            }
            else
            {
                if (string.IsNullOrEmpty(idTpNH))
                    idTpNH = "decimal";

                int idx = idTpNH.IndexOf("(");

                if (idx > 0)
                    idTpNH = idTpNH.Substring(0, idx);

                sw.WriteLine($"{Tabs(t)}/// <summary>");

                if (compositekeysNH.Count > 0)
                {
                    sw.Write($"{Tabs(t)}/// Deleta {Entidade} por ");
                    int cnt = 0;
                    foreach (string compositeKey in compositekeysNH)
                    {
                        if (cnt > 0) sw.Write($", ");
                        sw.Write($"{compositeKey}");
                        cnt++;
                    }
                    sw.WriteLine($".");
                }
                else
                {
                    sw.WriteLine($"{Tabs(t)}/// Deleta {Entidade} por {idNameNH}.");
                }

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
                sw.WriteLine($"{Tabs(t)}/// <returns></returns>");
                
                sw.WriteLine($"{Tabs(t)}[HttpDelete]");

                sw.Write($"{Tabs(t)}[Route(\"api/{apiEntidade}");

                if (compositekeysNH.Count > 0)
                {
                    foreach (string compositeKey in compositekeysNH)
                    {
                        sw.Write($"/{{{compositeKey}}}");
                    }
                }
                else
                {
                    sw.Write($"/{{{idNameNH}}}");
                }
                sw.WriteLine($"\")]");


                if (comAutenticacao) sw.WriteLine($"{Tabs(t)}#if DEBUG");
                sw.WriteLine($"{Tabs(t)}[AllowAnonymous]");
                if (comAutenticacao) sw.WriteLine($"{Tabs(t)}#endif");
                                
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
                sw.Write($"{Tabs(t)}return negocio.Delete{titEntidade}(");
                if (compositekeysNH.Count > 0)
                {
                    int cnt = 0;
                    foreach (string compositeKey in compositekeysNH)
                    {
                        if (cnt > 0) sw.Write($",");
                        sw.Write($"{compositeKey}");
                        cnt++;
                    }
                }
                else
                {
                    sw.Write($"{idNameNH}");
                }

                sw.WriteLine($");");
                sw.WriteLine($"{Tabs(--t)}}}");
            }
            sw.WriteLine($"{Tabs(t)}#endregion");

            #endregion

            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(--t)}}}");
            sw.WriteLine($"{Tabs(t)}");
            

            sw.Close();

            return true;
        }
        #endregion
    }
}
