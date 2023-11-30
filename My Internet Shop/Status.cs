using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyInternetShop
{
    public static class Status
    {
        public const string Confirmed = "Подтвержден";
        public const string Waiting = "Ожидает потверждения";
        public const string New = "Новый";
        public const string Cancelled = "Отменен";
        public const string CancelledByUser = "Отменен Пользователем";
    }
}
