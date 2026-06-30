using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sehatak.Application.DTOs.AssignFeaturesWithPlan;
using Sehatak.Application.Interfaces.AssignFeatursToPlan;
using Sehatak.Infrastructure.Services.AssignFeatureToPlan;

namespace Sehatak.API.Controllers
{
    [ApiController]
    [Route("api/admin/plans/{planId}/features")]
    public class PlanFeatureController : ControllerBase
    {
        

    }
}
