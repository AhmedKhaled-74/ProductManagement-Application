using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Domain.Entities
{
    public class CartProductCustomAttribute
    {
        public Guid CartProductId { get; set; }
        public Guid ProductCustomAttributeId { get; set; }
        // navigation props
        public virtual CartProduct CartProduct { get; set; } = null!;
        public virtual ProductCustomAttribute ProductCustomAttribute { get; set; } = null!;
    }
}
