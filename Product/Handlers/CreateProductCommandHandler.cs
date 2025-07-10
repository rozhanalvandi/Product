using Product.Commands;
using Product.Data;
using Product.Models;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;

namespace Product.Handlers
{
    public class CreateProductCommandHandler
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Products> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Products>(command);

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return product;
        }
    }
}
