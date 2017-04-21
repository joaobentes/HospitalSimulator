using Microsoft.AspNetCore.Mvc;
using HospitalSimulator.Models;
using System.Collections.Generic;

namespace HospitalSimulator.Controllers {
    public class ConsultationController: Controller {
        
        [HttpGet]
        public IEnumerable<Consultation> GetAll()
        {
            return Repository.Instance.Consultations;
        }
    }
}