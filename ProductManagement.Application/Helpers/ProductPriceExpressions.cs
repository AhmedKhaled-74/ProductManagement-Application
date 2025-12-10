using ProductManagement.Domain.Entities;
using System.Linq.Expressions;

namespace ProductManagement.Application.Helpers
{
    public static class ProductPriceExpressions
    {
        public static Expression<Func<Product, decimal>> GetPriceExpression(PriceConstsSetup priceConsts)
        {
            return p =>
                Math.Round(
                    (
                        priceConsts.PoeRegion *
                        (p.ProductPrice + (p.ProductMass * priceConsts.CMass) + (p.ProductVolume * priceConsts.CVol))
                        - (p.ProductPrice * p.Discount)
                    ) * priceConsts.CTax, 3
                );
        }
    }
}
