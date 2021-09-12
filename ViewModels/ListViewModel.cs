using BFme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFme.ViewModels
{
    public class ListViewModel
    {
        public ListViewModel( List<Lot> Lots, int CurrentPage = 1, string Message = "")
        {
            foreach (Lot lot in Lots) this.Lots.Add(new LotRow(lot));

            this.Message = Message;
            this.CurrentPage = CurrentPage;
        }

        public List<LotRow> Lots { get; private set; } = new List<LotRow>();
        public string Message { get; private set; }
        public int CurrentPage { get; private set; }
    }

    public struct LotRow
    {
        public LotRow(Lot lot)
        {
            Id = lot.Id;
            Name = lot.Name;
            Description = lot.Description;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
    }
}
