using BFme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFme.ViewModels
{
    public class _LotViewModel
    {   
        public _LotViewModel(Lot Lot, ActionsEnum Action, string Message = "")
        {
            this.Lot = Lot;
            InvestConcepts = Lot.InvestConcepts;
            Files = Lot.Files;

            this.Action = Action;
            this.Message = Message;
        }

        public Lot Lot { get; private set; }
        public List<InvestConcept> InvestConcepts { get; private set; } = new List<InvestConcept>();
        public List<LotFile> Files { get; private set; } = new List<LotFile>();

        public ActionsEnum Action { get; private set; }
        public string Message { get; private set; }
    }
}
