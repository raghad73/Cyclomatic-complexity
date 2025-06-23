namespace LoanApp
{
    public class LoanEvaluator
    {
        public string GetLoanEligibility(int income, bool hasJob, int creditScore, int dependents, bool ownsHouse)
        {
            if (IsLowIncome(income))
                return "Not Eligible";

            return hasJob
                ? EvaluateEmployedApplicant(creditScore, dependents, ownsHouse)
                : EvaluateUnemployedApplicant(income, creditScore, dependents, ownsHouse);
        }

        public bool IsLowIncome(int income) => income < 2000;

        public string EvaluateEmployedApplicant(int creditScore, int dependents, bool ownsHouse)
        {
            if (HasExcellentCredit(creditScore))
                return EvaluateDependents(dependents);

            if (HasGoodCredit(creditScore))
                return ownsHouse ? "Review Manually" : "Not Eligible";

            return "Not Eligible";
        }

        public string EvaluateUnemployedApplicant(int income, int creditScore, int dependents, bool ownsHouse)
        {
            if (IsHighRiskUnemployed(creditScore, income, ownsHouse))
                return "Eligible";

            if (IsMediumRiskUnemployed(creditScore, dependents))
                return "Review Manually";

            return "Not Eligible";
        }

        public bool HasExcellentCredit(int creditScore) => creditScore >= 700;
        public bool HasGoodCredit(int creditScore) => creditScore >= 600;

        public string EvaluateDependents(int dependents)
        {
            if (dependents == 0) return "Eligible";
            if (dependents <= 2) return "Review Manually";
            return "Not Eligible";
        }

        public bool IsHighRiskUnemployed(int creditScore, int income, bool ownsHouse)
            => creditScore >= 750 && income > 5000 && ownsHouse;

        public bool IsMediumRiskUnemployed(int creditScore, int dependents)
            => creditScore >= 650 && dependents == 0;
    }
}
