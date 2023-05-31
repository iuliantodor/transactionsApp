using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TransactionApp.Application.Common.Constants;
using TransactionApp.Application.Articles.Commands;
using TransactionApp.Application.Articles.Queries;

namespace TransactionApp.Controllers
{
    [Route("[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ArticlesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IResult> GetArticlesWithPagination(
            int pageSize = PagedConstants.PageSize,
            int pageNumber = PagedConstants.PageNumber)
        {
            var result = await _mediator.Send(new GetArticlesWithPaginationQuery { PageNumber = pageNumber, PageSize = pageSize });
            return result is not null ? Results.Ok(result) : Results.NoContent();
        }

        [HttpGet("GetArticleById")]
        public async Task<IResult> GetArticlesById([FromQuery]Guid Id)
        {
            var result = await _mediator.Send(new GetArticlesByIdQuery() { Id = Id });
            return result is not null ? Results.Ok(result) : Results.NoContent();
        }

        [HttpPost]
        public async Task<IResult> AddArticle([FromBody] AddArticleCommand command)
        {
            if (!ModelState.IsValid)
            {
                throw new ValidationException();
            }
            var result = await _mediator.Send(command);
            return result is not null ? Results.Ok(result) : Results.BadRequest();
        }

        [HttpPut]
        public async Task<IResult> UpdateArticle([FromBody] UpdateArticleCommand command)
        {
            if (!ModelState.IsValid)
            {
                throw new ValidationException();
            }
            var result = await _mediator.Send(command);
            return result is not null ? Results.Ok(result) : Results.NoContent();
        }

        [HttpDelete]
        public async Task<IResult> DeleteArticle([FromQuery] Guid Id)
        {
            await _mediator.Send(new DeleteArticleCommand(Id));
            return Results.Ok();
        }
    }
}

