using FlowerSpotCore.InterfacesRepository;
using FlowerSpotCore.ModelsEndpoints.Users;
using FlowerSpotCore.ModelsRepository;
using FlowerSpotCore.ModelsServices;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq.Expressions;

namespace FlowerSpotLogic.Tests
{
    [TestClass()]
    public class UsersServiceTests
    {
        private Mock<IAllRepositories> _allRepositories;
        private Mock<IUsersRepository> _usersRepository;
        private UsersService _usersService;
        private List<UserModel> _users;

        public UsersServiceTests()
        {
            _allRepositories = new Mock<IAllRepositories>();
            _usersRepository = new Mock<IUsersRepository>();
            _usersService = new UsersService(_allRepositories.Object);
            _users = new List<UserModel>();

            _usersRepository.Setup(x => x.Create(It.IsAny<UserModel>())).Callback<UserModel>((user) => _users.Add(user));
            _usersRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UserModel, bool>>>()))
                .Returns<Expression<Func<UserModel, bool>>>(predicate => _users.Where(predicate.Compile()).AsQueryable());

            _allRepositories.Setup(x => x.Users).Returns(_usersRepository.Object);
        }

        [TestMethod()]
        public void AddUser()
        {
            RegisterUserModel testingUser = new RegisterUserModel();
            testingUser.UserName = "testingUser";
            testingUser.Password = "testPassword";
            testingUser.Email = "testemail@a.si";

            _users = new List<UserModel>() { new UserModel("user1", "pass1", "salt1", "user1@test.si"), new UserModel("user2", "pass2", "salt2", "user2@test.si") };

            _allRepositories.Setup(x => x.Save()).Returns(() => (_users.Any(x => x.UserName.Equals(testingUser.UserName) && x.Email.Equals(testingUser.Email))) ? 1 : 0);

            _usersService = new UsersService(_allRepositories.Object);
            _usersService.AddUser(testingUser.UserName, testingUser.Password, testingUser.Email).Should().BeTrue();

        }

        [TestMethod()]
        public void LoginUser_Success()
        {
            //initialize user
            RegisterUserModel testingUser = new RegisterUserModel();
            testingUser.UserName = "testingUser";
            testingUser.Password = "testPassword";
            testingUser.Email = "testemail@a.si";

            _users = new List<UserModel>() { new UserModel("user1", "pass1", "salt1", "user1@test.si"), new UserModel("user2", "pass2", "salt2", "user2@test.si") };

            _allRepositories.Setup(x => x.Save()).Returns(() => _users.Any(x => x.UserName.Equals(testingUser.UserName) && x.Email.Equals(testingUser.Email)) ? 1 : 0);

            _usersService = new UsersService(_allRepositories.Object);
            _usersService.AddUser(testingUser.UserName, testingUser.Password, testingUser.Email);

            //test
            _usersService.LoginUser(testingUser.UserName, testingUser.Password).Should().NotBeEmpty();

        }

        [TestMethod()]
        public void LoginUser_Fails()
        {
            //initialize user
            RegisterUserModel testingUser = new RegisterUserModel();
            testingUser.UserName = "testingUser";
            testingUser.Password = "testPassword";
            testingUser.Email = "testemail@a.si";

            _users = new List<UserModel>() { new UserModel("user1", "pass1", "salt1", "user1@test.si"), new UserModel("user2", "pass2", "salt2", "user2@test.si") };

            _allRepositories.Setup(x => x.Save()).Returns(() => (_users.Any(x => x.UserName.Equals(testingUser.UserName) && x.Email.Equals(testingUser.Email))) ? 1 : 0);

            _usersService = new UsersService(_allRepositories.Object);
            _usersService.AddUser(testingUser.UserName, testingUser.Password, testingUser.Email);

            //test
            string wrongPassword = "tstPassword123";
            _usersService.LoginUser(testingUser.UserName, wrongPassword).Should().BeEmpty();

        }



        [TestMethod()]
        public void CheckIfTokenValidAndCorruptedBecomeInvalid()
        {
            //initialize user
            RegisterUserModel testingUser = new RegisterUserModel();
            testingUser.UserName = "testingUser";
            testingUser.Password = "testPassword";
            testingUser.Email = "testemail@a.si";

            _users = new List<UserModel>() { new UserModel("user1", "pass1", "salt1", "user1@test.si"), new UserModel("user2", "pass2", "salt2", "user2@test.si") };

            _allRepositories.Setup(x => x.Save()).Returns(() => _users.Any(x => x.UserName.Equals(testingUser.UserName) && x.Email.Equals(testingUser.Email)) ? 1 : 0);

            _usersService = new UsersService(_allRepositories.Object);
            _usersService.AddUser(testingUser.UserName, testingUser.Password, testingUser.Email);

            //get valid token
            string validToken = _usersService.LoginUser(testingUser.UserName, testingUser.Password);

            //test valid token
            JwtTokenModel? validTokenData = _usersService.ValidateTokenAndGetData(validToken);
            bool validLogin = validTokenData != null ? true : false;

            //find data section
            int dataStart = validToken.IndexOf('.') + 1;
            int dataEnd = validToken.IndexOf('.', dataStart);

            //test corrupted token
            bool invalidLogin = false;
            char[] invalidTokenArray;
            JwtTokenModel? invalidTokenData;
            char[] validTokenArray = validToken.ToCharArray();

            for (int i = dataStart; i < dataEnd; i++)
            {
                invalidTokenArray = (char[])validTokenArray.Clone();
                invalidTokenArray[i]++;

                invalidTokenData = _usersService.ValidateTokenAndGetData(new string(invalidTokenArray));

                if (invalidTokenData != null)
                {
                    invalidLogin = true;
                }
            }

            validLogin.Should().BeTrue();
            invalidLogin.Should().BeFalse();
        }
    }
}