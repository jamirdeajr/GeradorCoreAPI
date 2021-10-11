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

        #region Leitura Propriedades da DLL NHibernate
        /// <summary>
        /// Propriedades lidas do XML
        /// </summary>
        /// <param name="hbm"></param>
        /// <param name="nomeColunaAtual"></param>
        /// <returns></returns>
        public bool GetNHibernateProps(string hbm, string nomeColunaAtual)
        {
            string lastName = "";
            Boolean ok = false;

            XmlDocument doc = new XmlDocument();

            string[] resnames = dll.GetManifestResourceNames();

            string resName = "";

            Stream stream = null;

            foreach (string resname in resnames)
            {
                if (resname.EndsWith("."+ hbm + ".hbm.xml"))
                {
                    stream = dll.GetManifestResourceStream(resname);
                    resName = resname;
                    break;
                }
            }

            if (string.IsNullOrEmpty(nomeColunaAtual))
                LimpaNH(true);
            else
                LimpaNH(false);

            XmlReader xmlReader = null;

            if (stream == null)
                return false;




            // Leitura de atributos da classe toda
            if (string.IsNullOrEmpty(nomeColunaAtual))
            {
                uniquekeysNH = new List<string>();
                compositekeysNH = new List<string>();
                xmlReader = new XmlTextReader(stream);
                while (xmlReader.Read())
                {

                    if (xmlReader.Name.ToLower().Equals("bag")
                        || xmlReader.Name.ToLower().Equals("many-to-one")
                        )
                    {

                        lastName = xmlReader.Name;
                        continue;
                    }

                    if (xmlReader.Name.ToLower() == "composite-id")
                    {
                        compositeNH = true;
                    }

                    if (xmlReader.Name == "generator")
                    {
                        try
                        {
                            generatorClassNH = xmlReader.GetAttribute("class");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exceção:" + ex.StackTrace);
                        }
                    }

                    if (xmlReader.Name.ToLower().Equals("property")
                        || xmlReader.Name.ToLower().Equals("key-property")
                        || xmlReader.Name.ToLower().Equals("id")
                        )
                    {
                        try
                        {
                            if (xmlReader.GetAttribute("name") != null)
                            {

                                nameNH = xmlReader.GetAttribute("name");

                                if (xmlReader.Name == "id")
                                {
                                    idNameNH = nameNH;

                                    try
                                    {
                                        idTpNH = xmlReader.GetAttribute("type");
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error("Exceção:", ex);
                                    }
                                }

                                if (xmlReader.Name.ToLower().Equals("key-property") && compositeNH)
                                {
                                    compositekeysNH.Add(nameNH);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Exceção (6):", ex);
                        }
                        lastName = xmlReader.Name;

                    }

                    if (xmlReader.Name.Equals("column") && (lastName.Equals("property")
                        ))
                    {
                        try
                        {

                            columnNH = xmlReader.GetAttribute("name");

                            if (lastName.Equals("property"))
                            {
                                Object idx = xmlReader.GetAttribute("unique-key");

                                if (idx != null)
                                {
                                    uniquekeysNH.Add(idx.ToString());
                                    uniquekeysNH.Add(columnNH);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Exceção (7):", ex);
                        }
                    }
                }

                stream.Close();

                type = dll.GetType(dll.GetName().Name + "." + hbm);

                if (type != null)
                {

                    Object instance = (Object)Activator.CreateInstance(type);

                    try
                    {
                        FieldInfo myFieldInfo = type.GetField("classePlural");
                        nomeClassePlural = (string)myFieldInfo.GetValue(instance);
                    }
                    catch (Exception )
                    {
                        nomeClassePlural = hbm.Substring(0,1).ToLower()+hbm.Substring(1)+"s";
                    }

                }

                return true;
            }
            else
            {
                xmlReader = new XmlTextReader(stream);

                while (xmlReader.Read())
                {

                    if (xmlReader.Name.ToLower().Equals("property")
                        || xmlReader.Name.ToLower().Equals("key-property")
                        || xmlReader.Name.ToLower().Equals("id")
                        || xmlReader.Name.ToLower().Equals("many-to-one")
                        || xmlReader.Name.ToLower().Equals("many-to-many")
                        || xmlReader.Name.ToLower().Equals("one-to-one")
                        || xmlReader.Name.ToLower().Equals("one-to-many")
                        || xmlReader.Name.ToLower().Equals("bag")
                        )
                    {
                        try
                        {
                            Object name = xmlReader.GetAttribute("name");

                            if (name != null)
                                nameNH = name.ToString();


                            if (name != null && nameNH.Equals(nomeColunaAtual))
                            {

                                if (xmlReader.Name == "many-to-one")
                                {
                                    manytooneNH = true;
                                }

                                if (xmlReader.Name == "one-to-many")
                                {
                                    onetomanyNH = true;
                                }

                                if (xmlReader.Name == "many-to-many")
                                {
                                    //manytomanyNH = true;
                                }

                                if (xmlReader.Name == "one-to-one")
                                {
                                    onetooneNH = true;
                                }

                                if (xmlReader.Name == "bag")
                                {
                                    bagNH = true;
                                }

                                try
                                {
                                    if (xmlReader.Name.ToLower().Equals("id"))
                                        columnNH = nameNH;
                                    else
                                        columnNH = xmlReader.GetAttribute("column");
                                }
                                catch (Exception ex)
                                {
                                    log.Error("Exceção:", ex);
                                }


                                //<property name="U002INSMUN" type="string" length="20" not-null="true" />
                                //<property name="U002CODEAN" type="Decimal" precision="15" not-null="true" />
                                //<property name="U002ALIISS" type="Decimal(4,2)" not-null="true" />
                                try
                                {
                                    tpNH = xmlReader.GetAttribute("type");
                                }
                                catch (Exception ex)
                                {
                                    log.Error("Exceção:", ex);
                                }
                                try
                                {
                                    lenNH = xmlReader.GetAttribute("length");
                                }
                                catch (Exception ex)
                                {
                                    if (string.IsNullOrEmpty(tpNH) || (!tpNH.Contains("(") && !tpNH.Contains("Boolean") && !tpNH.Contains("DateTime")))
                                        log.Error("Exceção:", ex);
                                }
                                try
                                {
                                    if (string.IsNullOrEmpty(tpNH) || (!tpNH.Contains("(") && !tpNH.Contains("Boolean") && !tpNH.Contains("DateTime")))
                                        precisionNH = xmlReader.GetAttribute("precision");
                                }
                                catch (Exception ex)
                                {
                                    log.Error("Exceção:", ex);
                                }

                                try
                                {
                                    string str = xmlReader.GetAttribute("insert");

                                    if (!string.IsNullOrEmpty(str))
                                    {
                                        if (str.Equals("true"))
                                            insertNH = true;
                                        else
                                            insertNH = false;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    log.Error("Exceção:", ex);
                                }


                                try
                                {
                                    string str = xmlReader.GetAttribute("inverse");

                                    if (!string.IsNullOrEmpty(str))
                                    {
                                        //if (str.Equals("true"))
                                        //    inverseNH = true;
                                        //else
                                        //    inverseNH = false;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    log.Error("Exceção:", ex);
                                }

                                if (tpNH != null)
                                {
                                    try
                                    {

                                        if (tpNH.Contains("("))
                                        {
                                            lenNH = "";
                                            decNH = "";
                                            string[] strs = tpNH.Split(new char[] { ',', '(', ')' });
                                            int cnt = 0;
                                            foreach (string s in strs)
                                            {
                                                switch (cnt)
                                                {
                                                    case 0: tpNH = s; break;
                                                    case 1: lenNH = s; break;
                                                    case 2: decNH = s; break;
                                                }
                                                cnt++;
                                            }
                                        }

                                        if (idNameNH.Equals(nameNH))
                                            idTpNH = tpNH;

                                        ok = true;

                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error("Exceção (8):",ex);

                                        ok = false;
                                    }
                                }
                                else
                                {
                                    log.Info("tpNH NULL para campo:" + nomeColuna);
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Exceção (9):", ex);
                        }
                    }

                    // Tag column depois de um property
                    if (nameNH.Equals(nomeColunaAtual) && xmlReader.Name.Equals("column") && lastName.Equals("property"))
                    {
                        try
                        {
                            lenNH = xmlReader.GetAttribute("length");
                        }
                        catch (Exception ex)
                        {
                            log.Error("Exceção:", ex);
                        }
                    }

                    // Tag column depois de um property
                    if (nameNH.Equals(nomeColunaAtual) && xmlReader.Name.Equals("column") && lastName.Equals("many-to-one"))
                    {
                        try
                        {
                            columnNH = xmlReader.GetAttribute("name");
                            columnNHs.Add(columnNH);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Exceção:", ex);
                        }
                    }

                    // Chave composta em many-to-one ?
                    if (nameNH.Equals(nomeColunaAtual) && xmlReader.Name.Equals("column") && lastName.Equals("column"))
                    {
                        try
                        {
                            columnNHs.Add(xmlReader.GetAttribute("name"));

                        }
                        catch (Exception ex)
                        {
                            log.Error("Exceção:", ex);
                        }
                    }


                    if (nameNH.Equals(nomeColunaAtual) && (lastName.Equals("bag") || lastName.Equals("key")))
                    {
                        if (xmlReader.Name.Equals("key"))
                        {

                            try
                            {
                                columnNH = xmlReader.GetAttribute("column");

                            }
                            catch (Exception ex)
                            {
                                log.Error("Exceção:", ex);
                            }
                        }

                        //one-to-many class="ComponenteCalculo" />
                        if (xmlReader.Name.Equals("one-to-many") || xmlReader.Name.Equals("many-to-many"))
                        {

                            try
                            {
                                classNH = xmlReader.GetAttribute("class");
                            }
                            catch (Exception ex)
                            {
                                log.Error("Exceção:", ex);
                            }
                        }


                    }

                    if (!string.IsNullOrEmpty(xmlReader.Name))
                        lastName = xmlReader.Name;
                }

                stream.Close();
                stream.Dispose();
                return ok;
            }
        }

        /// <summary>
        /// Lista Objetos ManyTo... para incluir como Select nos forms
        /// </summary>
        /// <returns></returns>
        public bool ListaObjetos()
        {
            listaObjetos = new List<string>();

            Type typ = null;
            Type typList = null;
            Type typRef = null;
            string tipo;
            string bagInstance = null;
            for (int i = 0; i < meusCamposGeral.Length; i++)
            {
                typ = meusCamposGeral[i].FieldType;
                nomeColuna = meusCamposGeral[i].Name.Replace("k__BackingField", "").Replace("<", "").Replace(">", "");

                if (nomeColuna.StartsWith("_u"))
                    nomeColuna = "U" + nomeColuna.Substring(2);
                if (nomeColuna.StartsWith("_a"))
                    nomeColuna = "A" + nomeColuna.Substring(2);
                if (nomeColuna.StartsWith("_b"))
                    nomeColuna = "B" + nomeColuna.Substring(2);
                if (nomeColuna.StartsWith("_c"))
                    nomeColuna = "C" + nomeColuna.Substring(2);

                if (typ.Name.Contains("IList") || typ.Name.Contains("Null") || typ.Name.Contains("PagedList"))
                {
                    tipo = (typ.GetGenericArguments()[0]).Name;
                    typList = (typ.GetGenericArguments()[0]);
                }
                else
                {
                    tipo = typ.Name;
                }

                typRef = meusCamposGeral[0].ReflectedType;


                if (GetNHibernateProps(typRef.Name, nomeColuna))
                {
                    if (manytooneNH)
                    {
                        listaObjetos.Add(tipo);
                        listaObjetos.Add(nomeColuna);
                        listaObjetos.Add(columnNH);
                        if (insertNH)
                            listaObjetos.Add("true");
                        else
                            listaObjetos.Add("false");
                        listaObjetos.Add("many-to-one");
                        listaObjetos.Add("");
                    }

                    if (bagNH)
                    {
                        listaObjetos.Add(tipo);
                        listaObjetos.Add(nomeColuna);
                        listaObjetos.Add(columnNH);
                        if (insertNH)
                            listaObjetos.Add("true");
                        else
                            listaObjetos.Add("false");
                        listaObjetos.Add("bag");

                        // Descobrir a instancia da classe do campo...
                        //if (GetNHibernateProps(tipo, null))
                        //{
                        FieldInfo[] meusCamposLista = typList.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);// | BindingFlags.Static);

                        if (meusCamposLista != null)
                        {
                            foreach (FieldInfo f in meusCamposLista)
                            {
                                if (f.FieldType.Name.Equals(typRef.Name))
                                {

                                    bagInstance = f.Name.Replace("k__BackingField", "").Replace("<", "").Replace(">", "");

                                    break;
                                }

                            }
                        }

                        if (bagInstance != null)
                            listaObjetos.Add(bagInstance);
                        else
                            listaObjetos.Add("");




                    }

                    if (onetomanyNH)
                    {
                        listaObjetos.Add(tipo);
                        listaObjetos.Add(nomeColuna);
                        listaObjetos.Add(columnNH);
                        if (insertNH)
                            listaObjetos.Add("true");
                        else
                            listaObjetos.Add("false");
                        listaObjetos.Add("one-to-many");
                        listaObjetos.Add("");
                    }

                    if (onetooneNH)
                    {
                        listaObjetos.Add(tipo);
                        listaObjetos.Add(nomeColuna);
                        listaObjetos.Add(columnNH);
                        if (insertNH)
                            listaObjetos.Add("true");
                        else
                            listaObjetos.Add("false");
                        listaObjetos.Add("one-to-one");
                        listaObjetos.Add("");


                    }
                }
            }
            return true;
        }

        public string GetDisplayName(object obj, string propertyName)
        {
            if (obj == null) return null;
            return GetDisplayName(obj.GetType(), propertyName);

        }

        public string GetDisplayName(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName);
            if (property == null)
            {
                //Retorna o a propriedade mesmo
                return propertyName;
            }

            return GetDisplayName(property);

        }

        public string GetDisplayName(PropertyInfo property)
        {
            var attrName = GetAttributeDisplayName(property);
            if (!string.IsNullOrEmpty(attrName))
                return attrName;

            var metaName = GetMetaDisplayName(property);
            if (!string.IsNullOrEmpty(metaName))
                return metaName;

            return property.Name.ToString();
        }

        private string GetAttributeDisplayName(PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(
                typeof(DisplayNameAttribute), true);
            if (atts.Length == 0)
                return null;
            return (atts[0] as DisplayNameAttribute).DisplayName;
        }

        private string GetMetaDisplayName(PropertyInfo property)
        {
            var atts = property.DeclaringType.GetCustomAttributes(
                typeof(MetadataTypeAttribute), true);
            if (atts.Length == 0)
                return null;

            var metaAttr = atts[0] as MetadataTypeAttribute;
            var metaProperty =
                metaAttr.MetadataClassType.GetProperty(property.Name);
            if (metaProperty == null)
                return null;
            return GetAttributeDisplayName(metaProperty);
        }

        private void LimpaNH(Boolean limpaID)
        {
            if (limpaID)
            {
                idTpNH = "";
                idNameNH = "";
            }
            compositeNH = false;
            nameNH = "";
            tpNH = "";
            lenNH = "";
            decNH = "";
            precisionNH = "";
            columnNH = "";
            columnNHs = new List<string>();
            classNH = "";
            insertNH = false;
            uniquekeysNH = new List<string>();
            compositeNH = false;
            manytooneNH = false;
            onetomanyNH = false;
            onetooneNH = false;
            //manytomanyNH = false;
            bagNH = false;
            nomeClasse = "";

        }

        

        #endregion

        #region Entity Names NHIbernate

        private IList<string> GetEntityNames()
        {

            IList<string> entityNames = new List<string>();

            try
            {

                string[] resnames = dll.GetManifestResourceNames();
                string resName;
                int pos;
                string className;

                // Varre todos os recursos procurando hbms

                if (resnames != null)
                {
                    foreach (string hbmFile in resnames)
                    {
                        // Ignora se não terminar em hbm...
                        if (hbmFile.EndsWith(".hbm.xml"))
                        {
                            resName = hbmFile.Replace(".hbm.xml", "");
                            pos = resName.LastIndexOf(".");

                            className = (pos >= 0 ? resName.Substring(pos + 1) : resName);

                            if (resName.Length <= 0)
                                continue;

                            entityNames.Add(className);

                        }
                    }
                }
            }
            catch (Exception )
            {

            }

            return entityNames;
        }
        #endregion
    }
}
