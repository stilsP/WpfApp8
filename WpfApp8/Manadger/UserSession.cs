using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp8.Entities;

namespace WpfApp8.Manadger
{
    public static class UserSession
    {
        public static string CurrentUserFullName { get; set; }
        public static int CurrentUserId { get; set; }
    }
}
