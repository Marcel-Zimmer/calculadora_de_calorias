using CalculadoraCalorias.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCalorias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraficoController(
        IGraficoAppService _graficoAppService) : ApiBaseController
    {

        [HttpGet]
        [Route("dashboard-diario/{usuarioId:long}")]
        public async Task<IActionResult> GraficoDiario(long usuarioId)
        {
            return ProcessarResultado(await _graficoAppService.GraficoDiario(usuarioId));
        }

        [HttpGet]
        [Route("dashboard-semanal/{usuarioId:long}")]
        public async Task<IActionResult> GraficoSemanal(long usuarioId)
        {
            return ProcessarResultado(await _graficoAppService.GraficoSemanal(usuarioId));
        }

        [HttpGet]
        [Route("dashboard-mensal/{usuarioId:long}")]
        public async Task<IActionResult> GraficoMensal(long usuarioId)
        {
            return ProcessarResultado(await _graficoAppService.GraficoMensal(usuarioId));
        }
    }
}

    //metodos de crud basicos 
        //ajustar secrets do gemini

    //ajustar perfil biometrico para fazer os calculos de meta diaria de carboidratos, gorduras e etc..

     //adicionar um campo observação com os ingredientes
     //pedir para que o retorno inclua os açucares para eu conseguir estimar quanto estou consumindo por dia 
    //obter todos por dia para calcular quantas calorias foram ingeridas
    //adicionar Apelido
    //adicionar metodo que recebe só o codigo de uma refeição que já foi cadastrada, assim não preciso de llm 
        //nesse caso, preciso calcular as calorias com base na "refeição modelo"
    //fazer endpoins para subir configurações, por exemplo quantidade gasta por cada range de exercio, se ainda não estiver cadastrado posso pedir para uma llm fazer uma media e deixar como padrão
        //quero fazer o mesmo para gorduras, proteina e etc, fazer um grafico de um minimo/maximo que devo ingerir e quanto estou ingerindo 
