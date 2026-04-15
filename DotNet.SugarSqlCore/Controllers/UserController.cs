using DotNet.SugarSqlCore.Dtos;
using DotNet.SugarSqlCore.Entities;
using DotNet.SugarSqlCore.Repositories;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace DotNet.SugarSqlCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<List<UserDto>> GetListAsync([FromQuery] UserQueryDto input)
        {
            // 分页信息和排序方式可以通过参数传递，这里只是示例，默认排序方式为按Name降序，默认分页大小为10，分页序号为1
            var sort = _userRepository.GenerateSortExpression("Name", "desc");
            var totalNumber = new RefAsync<int>();
            var totalPage = new RefAsync<int>();
            var userList = await _userRepository.AsQueryable()
                .WhereIF(!string.IsNullOrWhiteSpace(input.Name), user => user.Name.Contains(input.Name!))
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Age = user.Age,
                    Email = user.Email,
                    Phone = user.Phone
                })
                .OrderByIF(!string.IsNullOrWhiteSpace(sort), sort)
                .ToPageListAsync(1, 10, totalNumber, totalPage);
            return userList;
        }

        [HttpGet("{id}")]
        public async Task<UserDto> GetAsync(Guid id)
        {
            var user = await _userRepository.FindAsync(id) ?? throw new Exception("User not found");
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
        public async Task CreateAsync(UserCreateDto input)
        {
            var existUser = await _userRepository.GetAsync(user => user.Name == input.Name);
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
            await _userRepository.InsertAsync(user);
        }

        [HttpPut]
        public async Task UpdateAsync(UserUpdateDto input)
        {
            var user = await _userRepository.FindAsync(input.Id) ?? throw new Exception("User not found");
            user.Age = input.Age;
            user.Email = input.Email;
            user.Phone = input.Phone;
            await _userRepository.UpdateAsync(user);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            await _userRepository.DeleteByIdAsync(id);
        }

        [HttpDelete]
        public async Task BatchDeleteAsync([FromBody] List<Guid> ids)
        {
            await _userRepository.DeleteByIdAsync(ids);
        }
    }
}
