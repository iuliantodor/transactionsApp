using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionApp.Application.Common.Exceptions;
using TransactionApp.Application.Common.Interfaces;
using TransactionApp.Application.Transactions.Dtos;

namespace TransactionApp.Application.Transactions.Queries
{
    public class GetTransactionsByIdQuery : IRequest<TransactionsDto>
    {
        public Guid Id { get; set; }
    }

    public class GetTransactionsByIdQueryHandler : IRequestHandler<GetTransactionsByIdQuery, TransactionsDto>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetTransactionsByIdQueryHandler(
           IRepository repository,
           IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<TransactionsDto> Handle(GetTransactionsByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                throw new Common.Exceptions.ApplicationException("Invalid Id");
            }
            var transaction = await _repository.GetByIdAsync<Domain.Entities.Transactions>(request.Id);

            return transaction != null ? _mapper.Map<TransactionsDto>(transaction) : throw new NotFoundException(nameof(Domain.Entities.Transactions), request.Id);
        }
    }
}
