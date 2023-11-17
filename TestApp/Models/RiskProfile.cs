namespace TestApp.Models
{
    public class RiskProfile
    {
        public int RiskClass { get; set; }
        public RiskClause[] RiskClauses { get; set; }
    }
}
