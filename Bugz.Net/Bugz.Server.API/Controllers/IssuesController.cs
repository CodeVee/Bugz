using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace Bugz.Server.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly RepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public IssuesController(RepositoryWrapper repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }
    }
}