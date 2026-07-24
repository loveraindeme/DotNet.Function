using DotNet.EFCore.Dtos;
using DotNet.EFCore.Entities;
using EFCore;
using EFCore.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet.EFCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IEFCoreRepository<User, Guid> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IEFCoreRepository<User, Guid> userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<List<UserDto>> GetListAsync([FromQuery] UserQueryDto input, CancellationToken cancellationToken)
        {
            // 分页信息和排序方式可以通过参数传递，这里只是示例，默认排序方式为按Name降序，默认分页大小为10，分页序号为1
            var totalNumber = new RefAsync<int>();
            var totalPage = new RefAsync<int>();
            var query = _userRepository.AsQueryable();
            if (!string.IsNullOrWhiteSpace(input.Name))
            {
                query = query.Where(user => user.Name.Contains(input.Name));
            }
            var userList = await query
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Age = user.Age,
                    Email = user.Email,
                    Phone = user.Phone
                })
                .OrderByDescending(user => user.Name)
                .ToPageListAsync(1, 10, totalNumber, totalPage, cancellationToken);
            return userList;
        }

        [HttpGet("{id}")]
        public async Task<UserDto> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(id, cancellationToken) ?? throw new Exception("User not found");
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Age = user.Age,
                Email = user.Email,
                Phone = user.Phone
            };
        }

        [HttpPost]
        public async Task CreateAsync(UserCreateDto input, CancellationToken cancellationToken)
        {
            var existUser = await _userRepository.GetAsync(user => user.Name == input.Name, cancellationToken);
            if (existUser != null)
            {
                throw new Exception("User with the same name already exists");
            }
            var user = new User
            {
                Name = input.Name,
                Age = input.Age,
                Email = input.Email,
                Phone = input.Phone
            };
            await _userRepository.AddAsync(user, cancellationToken: cancellationToken);
        }

        [HttpPut]
        public async Task UpdateAsync(UserUpdateDto input, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(input.Id, cancellationToken) ?? throw new Exception("User not found");
            user.Age = input.Age;
            user.Email = input.Email;
            user.Phone = input.Phone;
            await _userRepository.UpdateAsync(user, cancellationToken: cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _userRepository.RemoveAsync(id, cancellationToken: cancellationToken);
        }

        [HttpDelete]
        public async Task BatchDeleteAsync([FromBody] List<Guid> ids, CancellationToken cancellationToken)
        {
            await _userRepository.RemoveAsync(ids, cancellationToken: cancellationToken);
        }

        [HttpGet("deleted")]
        public async Task<List<UserDto>> GetDeletedListAsync(CancellationToken cancellationToken)
        {
            var query = _userRepository.DbSet.IgnoreQueryFilters().Where(user => user.IsDeleted);
            return await query.Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Age = user.Age,
                Email = user.Email,
                Phone = user.Phone
            }).ToListAsync(cancellationToken);
        }

        [HttpPost("multiple")]
        public async Task CreateMultipleUserAsync(CancellationToken cancellationToken)
        {
            await _unitOfWork.ExecuteInTransactionAsync(async (CancellationToken cancellationToken) =>
            {
                var user1 = new User
                {
                    Name = "unknow1",
                    Age = 18,
                    Email = "",
                    Phone = ""
                };
                await _userRepository.AddAsync(user1, cancellationToken: cancellationToken);

                var user2 = new User
                {
                    Name = "unknow2-cbeb0432-8688-11f1-832b-000c29afa74a-d4a71c7d-8688-11f1-832b-000c29afa74a",
                    Age = 20,
                    Email = "",
                    Phone = ""
                };
                await _userRepository.AddAsync(user2, cancellationToken: cancellationToken);
            }, cancellationToken: cancellationToken);
        }
    }
}
