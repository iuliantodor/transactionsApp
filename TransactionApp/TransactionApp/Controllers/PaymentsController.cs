using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TransactionApp.Application.Common.Constants;
using TransactionApp.Application.Payments.Commands;
using TransactionApp.Application.Payments.Queries;

namespace TransactionApp.Controllers
{
    [Route("[controller]")]
    public class PaymentsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetPaymentWithPagination")]
        public async Task<IResult> GetPaymentsWithPagination(
            int pageSize = PagedConstants.PageSize,
            int pageNumber = PagedConstants.PageNumber)
        {
            var result = await _mediator.Send(new GetPaymentsWithPaginationQuery { PageNumber = pageNumber, PageSize = pageSize });
            return result is not null ? Results.Ok(result) : Results.NoContent();
        }

        [HttpGet("GetPaymentById")]
        public async Task<IResult> GetPaymentById([FromQuery] Guid id)
        {
            var result = await _mediator.Send(new GetPaymentByIdQuery() { Id = id });
            return result is not null ? Results.Ok(result) : Results.NoContent();
        }

        [HttpPost]
        public async Task<IResult> AddPayment([FromBody] AddPaymentCommand command)
        {
            if (!ModelState.IsValid)
            {
                throw new ValidationException();
            }
            var result = await _mediator.Send(command);
            return result is not null ? Results.Ok(result) : Results.BadRequest();
        }

        [HttpPut]
        public async Task<IResult> UpdatePayment([FromBody] UpdatePaymentCommand command)
        {
            if (!ModelState.IsValid)
            {
                throw new ValidationException();
            }
            var result = await _mediator.Send(command);
            return result is not null ? Results.Ok(result) : Results.NoContent();
        }

        [HttpDelete]
        public async Task<IResult> DeletePayment([FromQuery] Guid Id)
        {
            await _mediator.Send(new DeletePaymentCommand(Id));
            return Results.Ok();
        }
    }
}
