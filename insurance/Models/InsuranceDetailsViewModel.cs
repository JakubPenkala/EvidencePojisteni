namespace insurance.Models
{
    public class InsuranceDetailsViewModel
    {
        public InsuranceViewModel Insurance { get; set; }
        public InsuranceEventsViewModel InsuranceEvents { get; set; }
        public IEnumerable<InsuranceViewModel> GetInsurance { get; set; }
        public IEnumerable<InsuranceEventsViewModel> GetInsuranceEvents { get; set; }
        public InsuranceDetailsViewModel()
        {
            Insurance = new InsuranceViewModel();
            InsuranceEvents = new InsuranceEventsViewModel();
            GetInsurance = new List<InsuranceViewModel>();
            GetInsuranceEvents = new List<InsuranceEventsViewModel>();
        }
    }
}
