﻿using Credit.Core.Application.UseCases.Clientes.ChangeCpf;
using Credit.Core.Application.UseCases.Clientes.Create;
using Credit.Core.Application.UseCases.Clientes.Edit;
using Credit.Core.Application.UseCases.Clientes.Remove;
using Microsoft.AspNetCore.Mvc;

namespace Credit.Presentation.BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ICreateClienteUseCase _createClienteUseCase;
        private readonly IEditClienteUseCase _editClienteUseCase;
        private readonly IChangeCpfClienteUseCase _changeCpfClienteUseCase;
        private readonly IRemoveClienteUseCase _removeClienteUseCase;

        public ClientesController(
            ICreateClienteUseCase createClienteUseCase,
            IEditClienteUseCase editClienteUseCase,
            IChangeCpfClienteUseCase changeCpfClienteUseCase,
            IRemoveClienteUseCase removeClienteUseCase)
        {
            _createClienteUseCase = createClienteUseCase ??
                throw new ArgumentNullException(nameof(createClienteUseCase));
            _editClienteUseCase = editClienteUseCase ??
                throw new ArgumentNullException(nameof(editClienteUseCase));
            _changeCpfClienteUseCase = changeCpfClienteUseCase ??
                throw new ArgumentNullException(nameof(changeCpfClienteUseCase));
            _removeClienteUseCase = removeClienteUseCase ??
                throw new ArgumentNullException(nameof(removeClienteUseCase));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateClienteOutput))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Create(CreateClienteInput input)
        {
            var createdCliente = await _createClienteUseCase.Execute(input);

            return Created(Request.Path, createdCliente);
        }

        [HttpPut("{clienteUid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Edit(string clienteUid, EditClienteInput input)
        {
            await _editClienteUseCase.Execute(clienteUid, input);

            return NoContent();
        }

        [HttpPatch("{clienteUid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> ChangeCpf(string clienteUid, ChangeCpfClienteInput input)
        {
            await _changeCpfClienteUseCase.Execute(clienteUid, input);

            return NoContent();
        }

        [HttpDelete("{clienteUid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Remove(string clienteUid)
        {
            await _removeClienteUseCase.Execute(clienteUid);

            return NoContent();
        }
    }
}
