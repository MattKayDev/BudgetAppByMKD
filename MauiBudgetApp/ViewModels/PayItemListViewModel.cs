using MauiBudgetApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBudgetApp.ViewModels
{
    public class PayItemListViewModel : BaseViewModel
    {

        public PayItemListViewModel(PayItemService payItemService)
        {
            this.Title = "";
            this.PayItemService = payItemService;
        }


    }
}
