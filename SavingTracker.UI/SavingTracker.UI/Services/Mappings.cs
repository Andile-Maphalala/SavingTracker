using Mapster;
using SavingTracker.ApiClient;
using SavingTracker.UI.Models.CRUDs;
using SavingTracker.UI.Models.Summary;

namespace SavingTracker.UI.Services
{
    public class Mappings : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ContributionModel, ContributionDto>()
                .TwoWays();
            config.NewConfig<ContributionTypeModel, ContributionTypeDto>()
                .TwoWays();
            config.NewConfig<MemberModel, MemberDto>()
                .TwoWays();
            config.NewConfig<SavingsPlanModel, SavingsPlanDto>()
                .TwoWays();
            config.NewConfig<ContributionSummaryModel, ContributionSummaryDto>()
                .TwoWays();
            config.NewConfig<DashboardSummaryModel, DashboardSummaryDto>()
                .TwoWays();
            config.NewConfig<MemberDashboardSummaryModel, MemberDashboardSummaryDto>()
                .TwoWays();
        }
    }
}
