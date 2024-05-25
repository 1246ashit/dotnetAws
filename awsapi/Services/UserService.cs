using System.Linq.Expressions;
using awsapi.Entities;
using Dapper;

namespace awsapi.Services
{
    public interface IUserService
    {
        Task<int> CheckedUser(LoginEntity loginEntity);
        Task<int> CreateUser(UserEntity userEntity);
    }
    public class UserService : IUserService
    {
        private readonly IDbService _DbService;
        public UserService(IDbService DbService)
        {
            _DbService = DbService;
        }
        public async Task<int> CheckedUser(LoginEntity loginEntity)
        {
            using var db = await _DbService.GetConnectionAsync();
            string sql = "select id, password from users where username=@username";
            var user = await db.QueryFirstOrDefaultAsync<dynamic>(sql, new { username = loginEntity.username });
            if (user == null)
            {
                return 0; // 用戶不存在
            }
            // 驗證密碼
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginEntity.password, user.password);
            if (!isPasswordValid)
            {
                return 0; // 密碼不正確
            }

            return user.id; // 返回用戶ID
        }
        public async Task<int> CreateUser(UserEntity userEntity)
        {
            using var db = await _DbService.GetConnectionAsync();
            LoginEntity test = new LoginEntity
            {
                username = userEntity.username,
                password = userEntity.password,
            };
            if (await CheckedUser(test) != 0)
            {
                return 0;//已存在
            }
            string sql = "insert into users (username, password, email, birthday) " +
            "values (@username, @password, @email, @birthday) returning id";
            try
            {
                var id = await db.QueryFirstOrDefaultAsync<int>(sql, userEntity);
                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;//註冊失效
            }


        }
    }
}