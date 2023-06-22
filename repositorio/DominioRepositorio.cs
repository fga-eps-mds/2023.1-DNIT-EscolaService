﻿using Dapper;
using dominio.Dominio;
using dominio.Enums;
using repositorio.Contexto;
using repositorio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static repositorio.Contexto.ResolverContexto;

namespace repositorio
{
    public class DominioRepositorio : IDominioRepositorio
    {
        private readonly IContexto contexto;

        public DominioRepositorio(ResolverContextoDelegate resolverContexto)
        {
            contexto = resolverContexto(ContextoBancoDeDados.Postgresql);
        }
        public IEnumerable<UnidadeFederativa> ObterUnidadeFederativa()
        {
            var sql = @"SELECT id as Id, descricao as Nome FROM public.unidade_federativa";

            var unidadesFederativas = contexto?.Conexao.Query<UnidadeFederativa>(sql);

            return unidadesFederativas ?? Enumerable.Empty<UnidadeFederativa>();
        }
        public IEnumerable<EtapasdeEnsino> ObterEtapasdeEnsino()
        {
            var sql = @"SELECT descricao_etapas_de_ensino as Descricao, id_etapas_de_ensino as Id FROM public.etapas_de_ensino";

            var EtapasdeEnsino = contexto?.Conexao.Query<EtapasdeEnsino>(sql);

            return EtapasdeEnsino ?? Enumerable.Empty<EtapasdeEnsino>();
        }
        public IEnumerable<Municipio> ObterMunicipio()
        {
            var sql = @"SELECT id_municipio as Id, nome as Nome FROM public.municipio";

            var Municipio = contexto?.Conexao.Query<Municipio>(sql);

            return Municipio ?? Enumerable.Empty<Municipio>();
        }
        public IEnumerable<Situacao> ObterSituacao()
        {
            var sql = @"SELECT id_situacao as Id, descricao_situacao as Descricao FROM public.situacao";

            var Situacao = contexto?.Conexao.Query<Situacao>(sql);

            return Situacao ?? Enumerable.Empty<Situacao>();
        }

    }
}