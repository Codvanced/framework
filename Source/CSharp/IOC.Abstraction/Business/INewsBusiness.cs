using IOC.Model;
using IOC.FW.Core.Abstraction.Business;

namespace IOC.Abstraction.Business
{
    public interface INewsBusiness
        : IBaseBusiness<News>
    {
        bool TitleAlreadyExists(string Title, int? id = null);
        bool Test(string Title);
    }
}
