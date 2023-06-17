﻿using dominio;
using Microsoft.AspNetCore.Mvc;
using service;
using service.Interfaces;
using System.Collections;
using service.Interfaces;


namespace app.Controllers
{
    [ApiController]
    [Route("api/escolas")]
    public class EscolaController : ControllerBase
    {
        private readonly IEscolaService escolaService;

        public EscolaController(IEscolaService escolaService)
        {
            this.escolaService = escolaService;
        }

        [HttpDelete ("excluir")]
        public IActionResult Excluir(ExclusaoEscola exclusaoEscola)
        {
            escolaService.Excluir(exclusaoEscola);
            return Ok();

    }


        [HttpGet("listarEscolas")]
        public IEnumerable<Escola> Listar()
        {
            IEnumerable<Escola> escolas = escolaService.Listar();
            return escolas;
        }

        [HttpGet("listarInformacoesEscola")]
        public IActionResult ListarInformacoesEscola([FromQuery] int idEscola)
        {
            Escola escola = escolaService.Listar(idEscola);
            return Ok(escola);
        }

        [HttpPost("adicionarSituacao")]
        public IActionResult AdicionarSituacao([FromBody] AtualizarSituacaoDTO atualizarSituacaoDTO)
        {
            escolaService.AdicionarSituacao(atualizarSituacaoDTO);
            return Ok();
        }

    }
}
