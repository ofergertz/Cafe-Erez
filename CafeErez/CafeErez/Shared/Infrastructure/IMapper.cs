using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.Infrastructure
{
    public interface IMapper<in source, out Tout>
    {
        Tout Map(source source);
    }
}
