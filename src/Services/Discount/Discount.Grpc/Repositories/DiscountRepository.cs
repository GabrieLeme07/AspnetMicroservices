using Dapper;
using Discount.Grpc.Entities;
using Npgsql;

namespace Discount.Grpc.Repositories;
public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;
    private string _connection;

    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connection = _configuration
            .GetValue<string>("DatabaseSettings:ConnectionString");
    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        using var connection = new NpgsqlConnection(_connection);

        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
            "SELECT * FROM Coupon WHERE ProductName = @ProductName",
            new { ProductName = productName });

        if (coupon == null)
            return new Coupon
            {
                ProductName = "No Discoiunt",
                Amount = 0,
                Description = "No Discoiunt"
            };

        return coupon;
    }

    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        using var connection = new NpgsqlConnection(_connection);

        var affected =
            await connection.ExecuteAsync
                (@"INSERT INTO Coupon 
                   (ProductName, Description, Amount)
                   VALUES (@ProductName, @Description, @Amount)",
                   new
                   {
                       coupon.ProductName,
                       coupon.Description,
                       coupon.Amount
                   });

        return affected != 0;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        using var connection = new NpgsqlConnection(_connection);

        var affected =
            await connection.ExecuteAsync
                (@"DELETE FROM Coupon WHERE ProductName = @ProductName",
                   new { ProductName = productName });

        return affected != 0;
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        using var connection = new NpgsqlConnection(_connection);

        var affected =
            await connection.ExecuteAsync
                (@"UPDATE Coupon SET
                   ProductName=@ProductName,
                   Description=@Description,
                   Amount=@Amount
                   WHERE Id = @Id",
                   new
                   {
                       coupon.ProductName,
                       coupon.Description,
                       coupon.Amount,
                       coupon.Id
                   });

        return affected != 0;
    }
}

