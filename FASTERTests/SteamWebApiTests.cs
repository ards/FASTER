using NUnit.Framework;
using System.Threading.Tasks;
// ReSharper disable CheckNamespace
// ReSharper disable PossibleNullReferenceException

namespace FASTER.Models.Tests
{
    [TestFixture()]
    public class SteamWebApiTests
    {
        [Test()]
        public async Task GetSingleFileDetailsTestAsync()
        {
            var res = await SteamWebApi.GetSingleFileDetailsAsync(463939057);
            Assert.IsNotNull(res);
            Assert.AreEqual("463939057", res["publishedfileid"].ToString());
            Assert.AreEqual("76561198194647182", res["creator"].ToString());
            Assert.AreEqual("ace", res["title"].ToString());
            Assert.AreEqual("107410", res["creator_appid"].ToString());
        }
    }
}