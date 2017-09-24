using AutoMapper;
using packt_asp_net_core_and_angular2.Data.Items;
using packt_asp_net_core_and_angular2.ViewModels;

namespace packt_asp_net_core_and_angular2
{
  public class AutoMapperProfileConfiguration : Profile
  {
    public AutoMapperProfileConfiguration()
    : this("MyProfile")
    {
    }
    protected AutoMapperProfileConfiguration(string profileName)
    : base(profileName)
    {
        CreateMap<Item, ItemViewModel>();
    }
  }
}