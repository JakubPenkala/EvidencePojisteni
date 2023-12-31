namespace insurance.Models
{
    public class InsuredDetailsViewModel
    {
        public InsuredViewModel Insured { get; set; }
        public InsuranceViewModel Insurance { get; set; }
        public IEnumerable<InsuredViewModel> GetInsured { get; set; }
        public IEnumerable<InsuranceViewModel> GetInsurance { get; set; }
        public InsuredDetailsViewModel() 
        {
            Insured = new InsuredViewModel();
            Insurance = new InsuranceViewModel();
            GetInsured = new List<InsuredViewModel>();
            GetInsurance = new List<InsuranceViewModel>();
        }
    }
}
