using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransactionApp.Application.Common.Constants;
using TransactionApp.Application.Common.Exceptions;
using TransactionApp.Application.Customers.Commands;
using TransactionApp.Application.Customers.Queries;

namespace TransactionApp.Controllers
{
    [Route("[controller]")]
    public class CustomersController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IResult> GetCustomersWithPagination(
            int pageSize = PagedConstants.PageSize,
            int pageNumber = PagedConstants.PageNumber)
        {
            var result = await _mediator.Send(new GetCustomerWithPaginationQuery { PageNumber = pageNumber, PageSize = pageSize });
            return result is not null ? Results.Ok(result) : Results.NoContent();
        }

        [HttpGet("GetCustomersById")]
        public async Task<IResult> GetCustomersById([FromQuery] Guid id)
        {
            var result = await _mediator.Send(new GetCustomersByIdQuery() { Id = id });
            return result is not null ? Results.Ok(result) : Results.NoContent();
        }

        [HttpPost]
        public async Task<IResult> AddCustomer([FromBody] AddCustomerCommand command)
        {
            if (!ModelState.IsValid)
            {
                throw new ValidationException();
            }
            var result = await _mediator.Send(command);
            return result is not null ? Results.Ok(result) : Results.BadRequest();
        }

        [HttpPut]
        public async Task<IResult> UpdateCustomer([FromBody] UpdateCustomerCommand command)
        {
            if (!ModelState.IsValid)
            {
                throw new ValidationException();
            }
            var result = await _mediator.Send(command);
            return result is not null ? Results.Ok(result) : Results.NoContent();
        }

        [HttpDelete]
        public async Task<IResult> DeleteCustomer([FromQuery] Guid Id)
        {
            await _mediator.Send(new DeleteCustomerCommand(Id));
            return Results.Ok();
        }
    }
}
