namespace TestApp.Models
{
    public class RiskProfile
    {
        public int RiskClass { get; set; }
        public RiskClause[] RiskClauses { get; set; }

        public string Print()
        {
            return $"Class {RiskClass}, Clauses: {string.Join(",", RiskClauses.Select(s => s.ToString()).ToArray())}";
        }
    }
}
