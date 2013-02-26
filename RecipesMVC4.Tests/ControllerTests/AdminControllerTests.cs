﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client.Embedded;
using Raven.Client.Linq;
using RecipesMVC4.Controllers;
using RecipesMVC4.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RecipesMVC4.Tests.ControllerTests
{

    [TestClass]
    public class AdminControllerTests
    {

        [TestMethod]
        public void Index_Action_Returns_List_Of_Recipes()
        {
            using (var documentStore = new EmbeddableDocumentStore
            {
                RunInMemory = true
            })
            {
                documentStore.Initialize();

                using (var session = documentStore.OpenSession())
                {
                    var recipe1 = new Recipe
                    {
                        Title = "Title1",
                        Description = "Description1",
                        ID = 1,
                        isIncorrect = false
                    };
                    session.Store(recipe1);

                    var recipe2 = new Recipe
                    {
                        Title = "Title2",
                        Description = "Description2",
                        ID = 2,
                        isIncorrect = true
                    };
                    session.Store(recipe2);

                    var recipe3 = new Recipe
                    {
                        Title = "Title3",
                        Description = "Description3",
                        ID = 3,
                        isIncorrect = false
                    };
                    session.Store(recipe3);

                    var recipe4 = new Recipe
                    {
                        Title = "Title4",
                        Description = "Description4",
                        ID = 4,
                        isIncorrect = true
                    };
                    session.Store(recipe4);

                    session.SaveChanges();

                    var adminController = new AdminController(session);

                    ViewResult result = adminController.Index() as ViewResult;

                    Assert.IsNotNull(result);

                    //next step
                    var actual = result.Model as List<Recipe>;
                    var expected = session.Query<Recipe>().OrderBy(x => x.ID).ToList();

                    CollectionAssert.AreEqual(actual, expected);
                    session.Dispose(); 
                }
            }
        }


        [TestMethod]
        public void Details_Action_Returns_A_Recipe()
        {
            using (var documentStore = new EmbeddableDocumentStore
            {
                RunInMemory = true
            })
            {
                documentStore.Initialize();

                using (var session = documentStore.OpenSession())
                {
                    var recipe1 = new Recipe
                    {
                        Title = "Title1",
                        Description = "Description1",
                        ID = 1,
                        isIncorrect = false
                    };
                    session.Store(recipe1);

                    var recipe2 = new Recipe
                    {
                        Title = "Title2",
                        Description = "Description2",
                        ID = 2,
                        isIncorrect = true
                    };
                    session.Store(recipe2);

                    var recipe3 = new Recipe
                    {
                        Title = "Title3",
                        Description = "Description3",
                        ID = 3,
                        isIncorrect = false
                    };
                    session.Store(recipe3);

                    var recipe4 = new Recipe
                    {
                        Title = "Title4",
                        Description = "Description4",
                        ID = 4,
                        isIncorrect = true
                    };
                    session.Store(recipe4);

                    session.SaveChanges();

                    var adminController = new AdminController(session);

                    var recipeCount = session.Query<Recipe>().Count();

                    ViewResult result = adminController.Details(recipeCount) as ViewResult;

                    Assert.IsNotNull(result);

                    var actual = result.Model as Recipe;
                    var expected = session.Query<Recipe>().OrderBy(x => x.ID).ToArray()[recipeCount - 1];

                    Assert.AreEqual(actual, expected);
                    session.Dispose(); 
                }
            }
        }


        [TestMethod]
        public void Edit_Action_Returns_A_Recipe_And_Can_Update_A_Recipe()
        {
            using (var documentStore = new EmbeddableDocumentStore
            {
                RunInMemory = true
            })
            {
                documentStore.Initialize();

                using (var session = documentStore.OpenSession())
                {
                    var recipe1 = new Recipe
                    {
                        Title = "Title1",
                        Description = "Description1",
                        ID = 1,
                        isIncorrect = false
                    };
                    session.Store(recipe1);

                    var recipe2 = new Recipe
                    {
                        Title = "Title2",
                        Description = "Description2",
                        ID = 2,
                        isIncorrect = true
                    };
                    session.Store(recipe2);

                    var recipe3 = new Recipe
                    {
                        Title = "Title3",
                        Description = "Description3",
                        ID = 3,
                        isIncorrect = false
                    };
                    session.Store(recipe3);

                    var recipe4 = new Recipe
                    {
                        Title = "Title4",
                        Description = "Description4",
                        ID = 4,
                        isIncorrect = true
                    };
                    session.Store(recipe4);

                    session.SaveChanges();

                    var adminController = new AdminController(session);

                    var recipeCount = session.Query<Recipe>().Count();

                    ViewResult result = adminController.Edit(recipeCount) as ViewResult;

                    var actual = result.Model as Recipe;
                    var expected = session.Query<Recipe>().OrderBy(x => x.ID).ToArray()[recipeCount - 1];

                    Assert.AreEqual(actual, expected);

                    //update step
                    //in admin-mode all fields cand be updated
                    var updatedRecipe = new Recipe
                    {
                        Title = "UpdatedTitle",
                        Description = "UpdatedDescription",
                        ID = 4,
                        isIncorrect = false
                    };

                    ViewResult resultOnUpdate = adminController.Edit(updatedRecipe) as ViewResult;
                    //  Assert.IsNotNull(resultOnUpdate);

                    var expectedAfterUpdate = updatedRecipe;
                    adminController.Edit(updatedRecipe.ID);
                    var actualAfterUpdate = session.Query<Recipe>().SingleOrDefault(recipe => recipe.ID == updatedRecipe.ID);
                    Assert.AreEqual(actualAfterUpdate, expectedAfterUpdate);
                    session.Dispose(); 
                }
            }
        }
    }
}