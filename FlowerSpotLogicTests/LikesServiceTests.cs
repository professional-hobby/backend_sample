using FlowerSpotCore.InterfacesRepository;
using FlowerSpotCore.ModelsEndpoints.Likes;
using FlowerSpotCore.ModelsRepository;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq.Expressions;

namespace FlowerSpotLogic.Tests
{
    [TestClass()]
    public class LikesServiceTests
    {
        private Mock<IAllRepositories> _allRepositories;
        private Mock<IUsersRepository> _usersRepository;
        private Mock<ILikesRepository> _likesRepository;
        private UsersService _usersService;
        private LikesService _likesService;
        private List<UserModel> _users;
        private List<LikeModel> _likes;

        public LikesServiceTests()
        {
            _allRepositories = new Mock<IAllRepositories>();
            _usersRepository = new Mock<IUsersRepository>();
            _likesRepository = new Mock<ILikesRepository>();
            _usersService = new UsersService(_allRepositories.Object);
            _likesService = new LikesService(_allRepositories.Object, _usersService);
            _users = new List<UserModel>();
            _likes = new List<LikeModel>();

            _usersRepository.Setup(x => x.Create(It.IsAny<UserModel>())).Callback<UserModel>((user) => _users.Add(user));
            _usersRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<UserModel, bool>>>()))
                .Returns<Expression<Func<UserModel, bool>>>(predicate => _users.Where(predicate.Compile()).AsQueryable());

            _likesRepository.Setup(x => x.Delete(It.IsAny<LikeModel>())).Callback<LikeModel>((like) => _likes.Remove(like));
            _likesRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<LikeModel, bool>>>()))
                .Returns<Expression<Func<LikeModel, bool>>>(predicate => _likes.Where(predicate.Compile()).AsQueryable());


            _allRepositories.Setup(x => x.Users).Returns(_usersRepository.Object);
            _allRepositories.Setup(x => x.Likes).Returns(_likesRepository.Object);
        }

        [TestMethod()]
        public void DeleteLikeTest()
        {
            int deletingUserId = -1;
            int deletetingSightingId = -1;
            bool internResult = false;

            //prepare test
            setupTest();
            _allRepositories.Setup(x => x.Save()).Callback(() => internResult = !_likes.Any(x => x.UserId.Equals(deletingUserId) && x.SightingId.Equals(deletetingSightingId))).Returns(() => internResult ? 1 : 0);

            //test
            //as setupTest function shows, a user with UserId 1 is an author of SightingId 1 like, but not SightingId 2.
            //so first delete request should be successful, but the second should not be.

            deletingUserId = 1;
            deletetingSightingId = 1;
            bool result1 = _likesService.DeleteLike(deletingUserId, new DeleteLikeModel() { SightingId = deletetingSightingId });
            deletetingSightingId = 2;
            bool result2 = _likesService.DeleteLike(deletingUserId, new DeleteLikeModel() { SightingId = deletetingSightingId });

            result1.Should().BeTrue();
            result2.Should().BeFalse();
            _likes.Count.Should().Be(1);
            _likes.Exists(x => x.SightingId == 2).Should().BeTrue();
        }

        private void setupTest()
        {
            _likes.Clear();
            _likes.Add(new LikeModel(1, 1));
            _likes.Add(new LikeModel(2, 2));
        }
    }
}