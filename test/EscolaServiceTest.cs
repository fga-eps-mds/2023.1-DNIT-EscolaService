using Xunit;
using Moq;
using repositorio.Interfaces;
using service;
using dominio;
using service.Interfaces;

namespace test
{
    public class EscolaServiceTest
    {
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

    }
}



