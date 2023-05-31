using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TransactionApp.Application.Common.Constants;
using TransactionApp.Application.Transactions.Commands;
using TransactionApp.Application.Transactions.Queries;

namespace TransactionApp.Controllers
{
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetTransactionsWithPagination")]
        public async Task<IResult> GetTransactionsWithPagination(
            int pageSize = PagedConstants.PageSize,
            int pageNumber = PagedConstants.PageNumber)
        {
            var result = await _mediator.Send(new GetTransactionsWithPaginationQuery { PageNumber = pageNumber, PageSize = pageSize });
            return result is not null ? Results.Ok(result) : Results.NoContent();
        }

        [HttpGet("GetTransactionsById")]
        public async Task<IResult> GetTransactionsById([FromQuery] Guid id)
        {
            var result = await _mediator.Send(new GetTransactionsByIdQuery() { Id = id });
            return result is not null ? Results.Ok(result) : Results.NoContent();
        }

        [HttpPost]
        public async Task<IResult> CreateTransaction([FromBody] CreateTransactionCommand command)
        {
            if (!ModelState.IsValid)
            {
                throw new ValidationException();
            }
            var result = await _mediator.Send(command);
            return result is not null ? Results.Ok(result) : Results.BadRequest();
        }
    }
}
