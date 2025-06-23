using Xunit;
using LoanApp;

namespace LoanApp.Tests
{
    public class RefactoredLoanEvaluatorTests
    {
        private readonly LoanEvaluator _evaluator;

        public RefactoredLoanEvaluatorTests()
        {
            _evaluator = new LoanEvaluator();
        }

        // ============ اختبارات الدالة الرئيسية ============
        [Fact]
        public void GetLoanEligibility_Should_Return_NotEligible_When_Income_Low()
        {
            var result = _evaluator.GetLoanEligibility(1500, true, 800, 0, true);
            Assert.Equal("Not Eligible", result);
        }

        [Fact]
        public void GetLoanEligibility_Should_Process_Employed_Applicants()
        {
            var result = _evaluator.GetLoanEligibility(2500, true, 720, 1, false);
            Assert.NotEqual("Not Eligible", result);
        }

        // ============ اختبارات التوابع المساعدة ============

        [Theory]
        [InlineData(1999, true)]
        [InlineData(2000, false)]
        [InlineData(3000, false)]
        public void IsLowIncome_Should_Validate_Correctly(int income, bool expected)
        {
            Assert.Equal(expected, _evaluator.IsLowIncome(income));
        }


        [Theory]
        [InlineData(700, 0, false, "Eligible")]
        [InlineData(700, 2, false, "Review Manually")]
        [InlineData(600, 0, true, "Review Manually")]
        [InlineData(500, 0, false, "Not Eligible")]
        public void EvaluateEmployedApplicant_Should_Return_Correct_Results(
            int creditScore, int dependents, bool ownsHouse, string expected)
        {
            var result = _evaluator.EvaluateEmployedApplicant(creditScore, dependents, ownsHouse);
            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData(760, 6000, true, 0, "Eligible")]
        [InlineData(660, 3000, false, 0, "Review Manually")]
        [InlineData(600, 2000, false, 1, "Not Eligible")]
        public void EvaluateUnemployedApplicant_Should_Return_Correct_Results(
            int creditScore, int income, bool ownsHouse, int dependents, string expected)
        {
            var result = _evaluator.EvaluateUnemployedApplicant(income, creditScore, dependents, ownsHouse);
            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData(0, "Eligible")]
        [InlineData(1, "Review Manually")]
        [InlineData(3, "Not Eligible")]
        public void EvaluateDependents_Should_Classify_Correctly(int dependents, string expected)
        {
            var result = _evaluator.EvaluateDependents(dependents);
            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData(700, true)]
        [InlineData(699, false)]
        public void HasExcellentCredit_Should_Validate_Correctly(int creditScore, bool expected)
        {
            Assert.Equal(expected, _evaluator.HasExcellentCredit(creditScore));
        }

        [Theory]
        [InlineData(600, true)]
        [InlineData(599, false)]
        public void HasGoodCredit_Should_Validate_Correctly(int creditScore, bool expected)
        {
            Assert.Equal(expected, _evaluator.HasGoodCredit(creditScore));
        }

        [Theory]
        [InlineData(800, 6000, true, true)]
        [InlineData(700, 4000, true, false)]
        public void IsHighRiskUnemployed_Should_Validate_Correctly(
            int creditScore, int income, bool ownsHouse, bool expected)
        {
            Assert.Equal(expected,
                _evaluator.IsHighRiskUnemployed(creditScore, income, ownsHouse));
        }

        [Theory]
        [InlineData(660, 0, true)]
        [InlineData(660, 1, false)]
        public void IsMediumRiskUnemployed_Should_Validate_Correctly(
            int creditScore, int dependents, bool expected)
        {
            Assert.Equal(expected,
                _evaluator.IsMediumRiskUnemployed(creditScore, dependents));
        }
    }
}

