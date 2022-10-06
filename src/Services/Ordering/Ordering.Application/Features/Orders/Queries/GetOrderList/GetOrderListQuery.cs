using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList;
public class GetOrderListQuery : IRequest<List<OrderVm>>
{
    public GetOrderListQuery(string userName)
        => UserName = userName;

    public string UserName { get; set; } = string.Empty;

}

