using Microsoft.AspNetCore.Mvc.Rendering;
using insurance.Models;

namespace insurance.Classes
{
    public class InsuranceTypes
    {
        public InsuranceViewModel GetInsuranceTypes()
        {

            var insuranceTypes = new InsuranceViewModel
            {
                InsuranceList = new List<SelectListItem>
                {
                    new SelectListItem { Text = "životní pojištění", Value = "životní pojištění" },
                    new SelectListItem { Text = "cestovní pojištění", Value = "cestovní pojištění" },
                    new SelectListItem { Text = "úrazové pojištění", Value = "úrazové pojištění" },
                    new SelectListItem { Text = "pojištění majetku", Value = "pojištění majetku" }
                }
            };
            return insuranceTypes;
        }
    }
}
