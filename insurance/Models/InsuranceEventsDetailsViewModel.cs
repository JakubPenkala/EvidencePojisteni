namespace insurance.Models
{
    public class InsuranceEventsDetailsViewModel
    {
        public InsuranceViewModel Insurance { get; set; }
        public InsuranceEventsViewModel InsuranceEvents { get; set; }        
        public IEnumerable<InsuranceEventsViewModel> GetInsuranceEvents { get; set; }
        public InsuranceEventsDetailsViewModel() 
        { 
            Insurance = new InsuranceViewModel();
            InsuranceEvents = new InsuranceEventsViewModel();
            GetInsuranceEvents = new List<InsuranceEventsViewModel>();
        }

    }
}
