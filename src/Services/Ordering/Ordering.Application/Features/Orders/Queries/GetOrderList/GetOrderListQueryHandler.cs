using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList;
public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, List<OrderVm>>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;

    public GetOrderListQueryHandler(IOrderRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<OrderVm>> Handle(GetOrderListQuery request,
        CancellationToken cancellationToken)
        => _mapper.Map<List<OrderVm>>
                (await _repository.GetOrdersByUserName(request.UserName));

}

