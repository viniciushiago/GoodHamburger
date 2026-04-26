using GoodHamburger.Application.Pedidos.Command.AtualizarPedido;
using GoodHamburger.Application.Pedidos.Command.CriarPedido;
using GoodHamburger.Application.Pedidos.Command.DeletarPedido;
using GoodHamburger.Application.Pedidos.Queries.ObterPedidoPorId;
using GoodHamburger.Application.Pedidos.Queries.ObterTodosPedidos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PedidoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(CriarPedidoCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(BuscarPorId), new { id = response.Id }, response);
        }

        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new ObterTodosPedidosQuery(), cancellationToken);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(long id, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new ObterPedidoPorIdQuery(id), cancellationToken);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(long id, AtualizarPedidoCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(long id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeletarPedidoCommand(id), cancellationToken);
            return NoContent();
        }
    }
}