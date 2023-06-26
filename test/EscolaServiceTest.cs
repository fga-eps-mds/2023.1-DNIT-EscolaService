﻿using dominio;
using Moq;
using repositorio.Interfaces;
using service;
using service.Interfaces;

namespace Test
{
    public class EscolaServiceTest
    {
        [Fact]
        public void ExcluirEscola_QuandoForChamado_DeveChamarExcluirEscolaDoRepositorio()
        {
            Mock<IEscolaRepositorio> escolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(escolaRepositorio.Object);
            int idEscolaTest = 41;

            escolaService.ExcluirEscola(idEscolaTest);
            escolaRepositorio.Verify(x => x.ExcluirEscola(idEscolaTest), Times.Once);
        }
        [Fact]
        public void CadastrarEscola_QuandoForChamado_DeveChamarORepositorioUmaVez()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            CadastroEscolaDTO cadastroEscolaDTO = new();

            escolaService.CadastrarEscola(cadastroEscolaDTO);
            mockEscolaRepositorio.Verify(x => x.CadastrarEscola(cadastroEscolaDTO), Times.Once);
        }
        [Fact]
        public void Obter_QuandoForChamado_DeveChamarORepositorioUmaVez()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            PesquisaEscolaFiltro pesquisaEscolaFiltro = new() { Pagina = 1, TamanhoPagina = 2 };

            escolaService.Obter(pesquisaEscolaFiltro);
            mockEscolaRepositorio.Verify(x => x.ObterEscolas(pesquisaEscolaFiltro), Times.Once);
        }
        [Fact]
        public void AdicionarSituacaoEscola_QuandoForChamado_DeveChamarORepositorioUmaVez()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            AtualizarSituacaoDTO atualizarSituacaoDto = new() { IdSituacao = 1, IdEscola = 2 };

            escolaService.AdicionarSituacao(atualizarSituacaoDto);
            mockEscolaRepositorio.Verify(x => x.AdicionarSituacao(atualizarSituacaoDto.IdSituacao, atualizarSituacaoDto.IdEscola), Times.Once);
        }

        [Fact]
        public void RemoverSituacaoEscola_QuandoForChamado_DeveChamarORepositorioUmaVez()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            var IdEscola = 5;

            escolaService.RemoverSituacaoEscola(IdEscola);
            mockEscolaRepositorio.Verify(x => x.RemoverSituacaoEscola(IdEscola), Times.Once);
        }

        [Fact]
        public void Listar_QuandoForChamado_DeveChamarORepositorioUmaVez()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            var IdEscola = 22;

            escolaService.Listar(IdEscola);
            mockEscolaRepositorio.Verify(x => x.Obter(IdEscola), Times.Once);
        }

        [Fact]
        public void RemoverSituacaoEscola_QuandoOIdForInexistente_DeveRetornarErro()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            var IdEscola = 8;
            var IdInexistente = 999;

            escolaService.RemoverSituacaoEscola(IdInexistente);
            mockEscolaRepositorio.Verify(x => x.RemoverSituacaoEscola(IdInexistente), Times.Once);
        }

        [Fact]
        public void AdicionarSituacaoEscola_QuandoOIdForInexistente_DeveRetornarErro()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            AtualizarSituacaoDTO atualizarSituacaoDto = new() { IdSituacao = 7, IdEscola = 4 };
            int IdInexistente = 3;

            escolaService.AdicionarSituacao(atualizarSituacaoDto);
            mockEscolaRepositorio.Verify(x => x.AdicionarSituacao(atualizarSituacaoDto.IdSituacao, IdInexistente), Times.Never);
        }

        [Fact]
        public void Listar_QuandoOIdInexistenteForChamado_DeveRetornarErro()
        {
            Mock<IEscolaRepositorio> mockEscolaRepositorio = new();
            IEscolaService escolaService = new EscolaService(mockEscolaRepositorio.Object);
            var IdEscola = 22;
            var IdInexistente = 15;

            escolaService.Listar(IdEscola);
            mockEscolaRepositorio.Verify(x => x.Obter(IdInexistente), Times.Never);
        }
    }
}
