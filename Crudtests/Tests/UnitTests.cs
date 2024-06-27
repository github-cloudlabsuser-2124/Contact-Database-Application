using NUnit.Framework;
using CRUD_application_2.Controllers;
using CRUD_application_2.Models;
using System.Web.Mvc;
using System.Linq;

namespace CRUD_Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private UserController controller;

        [SetUp]
        public void Setup()
        {
            // Initialize UserController and any necessary data before each test
            controller = new UserController();
            UserController.userlist.Clear(); // Ensure userlist is empty before each test
            UserController.userlist.Add(new User { Id = 1, Name = "Test User", Email = "test@example.com" }); // Add a test user
        }
        [TearDown]
        public void TearDown()
        {
            // Dispose the controller after each test
            controller.Dispose();
        }

        [Test]
        public void Index_ReturnsView_WithAllUsers()
        {
            var result = controller.Index() as ViewResult;
            var model = result.Model as System.Collections.Generic.List<User>;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, model.Count); // Assuming there's only one user added in Setup
        }

        [Test]
        public void Details_WithValidId_ReturnsUser()
        {
            var result = controller.Details(1) as ViewResult;
            var model = result.Model as User;

            Assert.IsNotNull(result);
            Assert.AreEqual("Test User", model.Name);
        }

        [Test]
        public void Details_WithInvalidId_ReturnsHttpNotFound()
        {
            var result = controller.Details(99);

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void Create_Get_ReturnsView()
        {
            var result = controller.Create() as ViewResult;

            Assert.IsNotNull(result);
        }

        [Test]
        public void Create_Post_ValidUser_RedirectsToIndex()
        {
            var newUser = new User { Id = 2, Name = "New User", Email = "new@example.com" };
            var result = controller.Create(newUser) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(2, UserController.userlist.Count); // Assuming one user was added in Setup
        }

        [Test]
        public void Create_Post_InvalidUser_ReturnsView()
        {
            controller.ModelState.AddModelError("Name", "Name is required"); // Simulate model validation failure
            var newUser = new User { Id = 2, Email = "new@example.com" };
            var result = controller.Create(newUser) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(newUser, result.Model); // Ensure the same model is returned for correction
        }

        [Test]
        public void Edit_Get_WithValidId_ReturnsUser()
        {
            var result = controller.Edit(1) as ViewResult;
            var model = result.Model as User;

            Assert.IsNotNull(result);
            Assert.AreEqual("Test User", model.Name);
        }

        [Test]
        public void Edit_Get_WithInvalidId_ReturnsHttpNotFound()
        {
            var result = controller.Edit(99);

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void Edit_Post_WithValidData_RedirectsToIndex()
        {
            var updatedUser = new User { Id = 1, Name = "Updated User", Email = "update@example.com" };
            var result = controller.Edit(1, updatedUser) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Updated User", UserController.userlist.First().Name);
        }

        [Test]
        public void Delete_Get_WithValidId_ReturnsUser()
        {
            var result = controller.Delete(1) as ViewResult;
            var model = result.Model as User;

            Assert.IsNotNull(result);
            Assert.AreEqual("Test User", model.Name);
        }

        [Test]
        public void Delete_Get_WithInvalidId_ReturnsHttpNotFound()
        {
            var result = controller.Delete(99);

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void Delete_Post_WithValidId_RedirectsToIndex()
        {
            var result = controller.Delete(1, null) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(0, UserController.userlist.Count); // Assuming the user was deleted
        }
    }
}
