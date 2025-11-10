using ChamedoonWebUI.Areas.Admin.Models;

namespace ChamedoonWebUI.Areas.Admin.ViewModels;

public class DashboardViewModel
{
    public DashboardSummary Summary { get; set; } = new();
    public IReadOnlyDictionary<Guid, string> RoleLookup { get; set; } = new Dictionary<Guid, string>();
}
