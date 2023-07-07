using System.IO;
using AutoMapper;
using dominio;
using repositorio.Interfaces;
using service.Interfaces;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using dominio.Dominio;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;

namespace service
{
    public class EscolaService : IEscolaService
    {
        private readonly IEscolaRepositorio escolaRepositorio;
        public EscolaService(IEscolaRepositorio escolaRepositorio)
        {
            this.escolaRepositorio = escolaRepositorio;
        }

        public bool SuperaTamanhoMaximo(MemoryStream planilha)
        { 
            using (var reader = new StreamReader(planilha))
            {
                int tamanho_max = 5000;
                int quantidade_escolas = -1;

                while (reader.ReadLine() != null) { quantidade_escolas++; }

                return quantidade_escolas > tamanho_max;
            }
        }

        public List<string> CadastrarEscolaViaPlanilha(MemoryStream planilha)
        {
            List<string> escolasNovas = new List<string>();
            using (var reader = new StreamReader(planilha))
            {
                using (var parser = new TextFieldParser(reader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(";");

                    bool primeiralinha = false;

                    while (!parser.EndOfData)
                    {
                        try
                        {
                            string[] linha = parser.ReadFields();
                            if (!primeiralinha)
                            {
                                primeiralinha = true;
                                continue;
                            }
                            Escola escola = new Escola();
                            escola.NomeEscola = linha[0];
                            escola.CodigoEscola = int.Parse(linha[1]);
                            escola.Cep = linha[2];
                            escola.Endereco = linha[3];
                            escola.Latitude = linha[4];
                            escola.Longitude = linha[5];
                            escola.NumeroTotalDeAlunos = int.Parse(linha[6]);
                            escola.Telefone = linha[7];
                            escola.NumeroTotalDeDocentes = int.Parse(linha[8]);
                            escola.IdEscola = int.Parse(linha[9]);
                            escola.IdRede = int.Parse(linha[10]);
                            escola.IdUf = ObterEstadoPelaSigla(linha[11]);

                            if (escola.IdUf == 0)
                            {
                                throw new Exception("Erro. A leitura do arquivo parou na escola: " + escola.NomeEscola + ", UF inv�lida!");
                            }

                            escola.IdLocalizacao = int.Parse(linha[12]);

                            string municipio = ObterCodigoMunicipioPorCEP(escola.Cep).GetAwaiter().GetResult();

                            if (municipio == null) 
                            { 
                                throw new Exception("Erro. A leitura do arquivo parou na escola: "+escola.NomeEscola+", CEP inv�lido!"); 
                            } 
                            else 
                            { 
                                escola.IdMunicipio = int.Parse(municipio); 
                            }

                            //escola.IdEtapasDeEnsino = int.Parse(linha[14]);
                            List<int> teste = EtapasParaIds(linha[14]);
                            escola.IdPorte = ObterPortePeloId(linha[15]);

                            if (escola.IdPorte == 0)
                            {
                                throw new Exception("Erro. A leitura do arquivo parou na escola: " + escola.NomeEscola + ", descri��o do porte inv�lida!");
                            }

                            escola.IdSituacao = int.Parse(linha[16]);

                            if (escolaRepositorio.EscolaJaExiste(escola.CodigoEscola))
                            {
                                escolaRepositorio.AtualizarDadosPlanilha(escola);
                                continue;
                            }

                            escolasNovas.Add(escola.NomeEscola);
                            escolaRepositorio.CadastrarEscola(escola);
                        }
                        catch (FormatException ex)
                        {
                            throw new Exception("Planilha com formato incompat�vel.");
                        }
                    }
                }
            }
            return escolasNovas;
        }

        public void ExcluirEscola(int id)
        {
            escolaRepositorio.ExcluirEscola(id);

        }
        public Escola Listar(int idEscola)
        {
            Escola escola = escolaRepositorio.Obter(idEscola);

            return escola;
        }

        public void AdicionarSituacao(AtualizarSituacaoDTO atualizarSituacaoDTO)
        {
            escolaRepositorio.AdicionarSituacao(atualizarSituacaoDTO.IdSituacao, atualizarSituacaoDTO.IdEscola);
        }

        public void CadastrarEscola(CadastroEscolaDTO cadastroEscolaDTO)
        {
            escolaRepositorio.CadastrarEscola(cadastroEscolaDTO);
        }

        public void RemoverSituacaoEscola(int idEscola)
        {
            escolaRepositorio.RemoverSituacaoEscola(idEscola);
        }

        public ListaPaginada<Escola> Obter(PesquisaEscolaFiltro pesquisaEscolaFiltro)
        {
            return escolaRepositorio.ObterEscolas(pesquisaEscolaFiltro);
        }

        public async Task<string> ObterCodigoMunicipioPorCEP(string cep)
        {
            string url = $"https://viacep.com.br/ws/{cep}/json/";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var conteudo = await response.Content.ReadAsStringAsync();
                        dynamic resultado = JObject.Parse(conteudo);

                        return resultado.ibge;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public int ObterEstadoPelaSigla(string UF)
        {
            Dictionary<int, string> estados = new Dictionary<int, string>()
            {
                { 1, "AC"},
                { 2, "AL"},
                { 3, "AP"},
                { 4, "AM"},
                { 5, "BA"},
                { 6, "CE"},
                { 7, "ES"},
                { 8, "GO"},
                { 9, "MA"},
                { 10, "MT"},
                { 11, "MS"},
                { 12, "MG"},
                { 13, "PA"},
                { 14, "PB"},
                { 15, "PR"},
                { 16, "PR"},
                { 17, "PI"},
                { 18, "RJ"},
                { 19, "RN"},
                { 20, "RS"},
                { 21, "RO"},
                { 22, "RR"},
                { 23, "SC"},
                { 24, "SP"},
                { 25, "SE"},
                { 26, "TO"},
                { 27, "DF"}
            };

            foreach (var estado in estados)
            {
                if(estado.Value == UF.ToUpper()) return estado.Key;
            }
            return 0;
        }

        public int ObterPortePeloId(string Porte)
        {
            Dictionary<int, string> porte = new Dictionary<int, string>()
            {
                { 1, "At� 50 matr�culas de escolariza��o"},
                { 2, "Entre 201 e 500 matr�culas de escolariza��o"},
                { 3, "Entre 501 e 1000 matr�culas de escolariza��o"},
                { 4, "Entre 51 e 200 matr�culas de escolariza��o"},
                { 5, "Mais de 1000 matr�culas de escolariza��o"},
            };

            foreach (var descricao in porte)
            {
                if (descricao.Value == Porte) return descricao.Key;
            }
            return 0;
        }

        public List<int> EtapasParaIds(string etapas)
        {
            List<int> ids = new List<int>();

            //char[] delimitadores = { ',', ' ' };
            List<string> etapas_separadas = etapas.Split(',').Select(item => item.Trim()).ToList();

            Dictionary<int, string> descricao_etapas = new Dictionary<int, string>()
            {
                { 1, "Educa��o Infantil"},
                { 2, "Ensino Fundamental"},
                { 3, "Ensino M�dio"},
                { 4, "Educa��o de Jovens Adultos"},
                { 5, "Educa��o Profissional"},
            };

            foreach (var nome in etapas_separadas)
            {
                foreach (var etapa in descricao_etapas)
                {
                    if(etapa.Value == nome)
                    {
                        ids.Add(etapa.Key); 
                        break;
                    }
                }
            }
            return ids;
        }
    }
}


