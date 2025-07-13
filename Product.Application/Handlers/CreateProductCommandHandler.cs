using Product.Domain.Models;
using Product.Domain.Interfaces; 
using Product.Application.Commands;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;


namespace Product.Application.Handlers
{
    public class CreateProductCommandHandler
    {
        private readonly IProductRepository _productRepository;

        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Products> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Products>(command);

            _productRepository.AddAsync(product);
            return product;
        }
    }
}
